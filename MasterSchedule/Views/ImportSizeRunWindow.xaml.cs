using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using Microsoft.Win32;
using MasterSchedule.Models;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using MasterSchedule.Controllers;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ImportSizeRunWindow.xaml
    /// </summary>
    public partial class ImportSizeRunWindow : Window
    {
        string filePath;
        List<SizeRunModel> sizeRunList;
        BackgroundWorker bwLoad;
        BackgroundWorker bwImport;
        List<SizeRunModel> sizeRunToImportList;
        public ImportSizeRunWindow()
        {
            filePath = "";
            sizeRunList = new List<SizeRunModel>();
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwImport = new BackgroundWorker();
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);
            sizeRunToImportList = new List<SizeRunModel>();
            InitializeComponent();            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import Orders";
            openFileDialog.Filter = "EXCEL Files (*.xls, *.xlsx)|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                if (bwLoad.IsBusy == false)
                {
                    this.Cursor = Cursors.Wait;
                    sizeRunList.Clear();
                    lblStatus.Text = "Reading...";
                    bwLoad.RunWorkerAsync();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            Excel.Application excelApplication = new Excel.Application();
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(filePath);
            //excelApplication.Visible = true;
            Excel.Worksheet excelWorksheet;
            Excel.Range excelRange;
            try
            {
                excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets[1];
                excelRange = excelWorksheet.UsedRange;
                progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = excelRange.Rows.Count));
                for (int i = 2; i <= excelRange.Rows.Count; i++)
                {
                    var productNoValue = (excelRange.Cells[i, 4] as Excel.Range).Value2;
                    if (productNoValue != null)
                    {
                        string productNo = productNoValue.ToString();
                        for (int j = 17; j <= 65; j++)
                        {
                            var qtyValue = (excelRange.Cells[i, j] as Excel.Range).Value2;
                            if (qtyValue != null)
                            {
                                int qty = 0;
                                int.TryParse(qtyValue.ToString(), out qty);
                                string sizeNo = (excelRange.Cells[1, j] as Excel.Range).Value2.ToString();
                                if (qty > 0)
                                {
                                    SizeRunModel sizeRun = new SizeRunModel
                                    {
                                        ProductNo = productNo,
                                        SizeNo = sizeNo,
                                        Quantity = qty,
                                    };
                                    sizeRunList.Add(sizeRun);
                                }
                            }
                        }
                    }
                    progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = i));
                }
            }
            catch
            {
                sizeRunList.Clear();
            }
            finally
            {
                excelWorkbook.Close(false, Missing.Value, Missing.Value);
                excelApplication.Quit();
            }
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            this.Cursor = null;
            lblStatus.Text = "Load Completed!";
            if (sizeRunList.Count() > 0)
            {
                dgSizeRun.ItemsSource = sizeRunList;
                btnImport.IsEnabled = true;
                MessageBox.Show(string.Format("Read Completed. {0} Size Run!", sizeRunList.Count()), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Excel File Error. Try Again!", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }            
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (bwImport.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnImport.IsEnabled = false;
                sizeRunToImportList = dgSizeRun.Items.OfType<SizeRunModel>().ToList();
                progressBar.Value = 0;
                bwImport.RunWorkerAsync();
            }
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            // Insert SizeRun
            int i = 1;
            progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = sizeRunToImportList.Count));
            foreach (SizeRunModel sizeRun in sizeRunToImportList)
            {
                SizeRunController.Insert(sizeRun);
                dgSizeRun.Dispatcher.Invoke((Action)(() =>
                {
                    dgSizeRun.SelectedItem = sizeRun;
                    dgSizeRun.ScrollIntoView(sizeRun);
                }));
                Dispatcher.Invoke(new Action(() =>
                {
                    lblStatus.Text = "Importing SizeRun ...";
                    progressBar.Value = i;
                }));
            }

            // Update OutsoleSize, MidSoleSize
            var productNoList = sizeRunToImportList.Select(s => s.ProductNo).Distinct().ToList();
            var orderList = OrdersController.Select();
            int indexPO = 1;
            progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = 0));
            progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = productNoList.Count));
            foreach (var productNo in productNoList)
            {
                var orderPerPOModel = orderList.Where(w => w.ProductNo == productNo).FirstOrDefault();
                if (orderPerPOModel == null)
                    continue;
                string outsoleCode = orderPerPOModel.OutsoleCode;
                string articleNo = orderPerPOModel.ArticleNo.Contains("-") ? orderPerPOModel.ArticleNo.Split('-')[0].ToString() : orderPerPOModel.ArticleNo;

                var sizeRunPerArticleList = SizeRunController.SelectPerArticle(articleNo).Where(w => w.UpdateOutsoleSizeByArticle == true).ToList();
                var sizeRunPerOutsoleCodeList = SizeRunController.SelectPerOutsoleCode(outsoleCode).Where(w => w.UpdateOutsoleSizeByArticle == false).ToList();

                if (sizeRunPerOutsoleCodeList.Count == 0 && sizeRunPerArticleList.Count == 0)
                    continue;

                var sizeRunUpdateSizeMapList = sizeRunToImportList.Where(w => w.ProductNo == productNo).ToList();

                foreach (var sizeRun in sizeRunUpdateSizeMapList)
                {
                    string outsoleSize = "", midsoleSize = "";

                    var sizeRunInOutsoleCode = sizeRunPerOutsoleCodeList.Where(w => w.SizeNo == sizeRun.SizeNo).FirstOrDefault();
                    if (sizeRunPerArticleList.Count > 0)
                        sizeRunInOutsoleCode = sizeRunPerArticleList.Where(w => w.SizeNo == sizeRun.SizeNo).FirstOrDefault();

                    if (sizeRunInOutsoleCode == null)
                        continue;
                    outsoleSize = sizeRunInOutsoleCode.OutsoleSize;
                    midsoleSize = sizeRunInOutsoleCode.MidsoleSize;

                    if (String.IsNullOrEmpty(outsoleSize) == true && String.IsNullOrEmpty(midsoleSize) == true)
                        continue;

                    sizeRun.OutsoleSize = outsoleSize;
                    sizeRun.MidsoleSize = midsoleSize;
                    sizeRun.UpdateOutsoleSizeByArticle = sizeRunPerArticleList.Count > 0 ? true : false;

                    SizeRunController.UpdateSizeMap(sizeRun);
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    lblStatus.Text = "Updating OutsoleSize, MidsoleSize ...";
                    progressBar.Value = indexPO;
                }));
                indexPO++;
            }
        }

        private void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnImport.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            lblStatus.Text = "Finished !";
            progressBar.Value = 0;
            MessageBox.Show("Insert Completed!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
