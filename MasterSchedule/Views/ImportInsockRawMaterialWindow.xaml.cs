using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using MasterSchedule.Models;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using MasterSchedule.Controllers;
using MasterSchedule.ViewModels;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ImportInsockRawMaterialWindow.xaml
    /// </summary>
    public partial class ImportInsockRawMaterialWindow : Window
    {
        string filePath;
        BackgroundWorker bwLoad;
        BackgroundWorker bwImport;
        List<InsockRawMaterialModel> insockRawMaterialList;
        List<InsockRawMaterialModel> insockRawMaterialImportList;
        public ImportInsockRawMaterialWindow()
        {
            filePath = "";
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwImport = new BackgroundWorker();
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);
            insockRawMaterialList = new List<InsockRawMaterialModel>();
            insockRawMaterialImportList = new List<InsockRawMaterialModel>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import Insock Raw Material";
            openFileDialog.Filter = "EXCEL Files (*.xls, *.xlsx)|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                if (bwLoad.IsBusy == false)
                {
                    this.Cursor = Cursors.Wait;
                    insockRawMaterialList.Clear();
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
            var insockSupplierList = InsockSuppliersController.Select();
            if (insockSupplierList.Count < 1)
            {
                return;
            }

            List<InsockTemp> insockTempList = new List<InsockTemp>();
            Excel.Application excelApplication = new Excel.Application();
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(filePath);
            //excelApplication.Visible = true;
            Excel.Worksheet excelWorksheet;
            Excel.Range excelRange;
            try
            {
                excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets[1];
                excelRange = excelWorksheet.UsedRange;
                int columnCheck = 8;
                while ((excelRange.Cells[2, columnCheck] as Excel.Range).Value2 != null)
                {
                    string insockSupplierName = (excelRange.Cells[2, columnCheck] as Excel.Range).Value2.ToString();
                    var insockSupplierPerName = insockSupplierList.Where(w => w.InsockSupplierName == insockSupplierName).FirstOrDefault();
                    if (insockSupplierPerName == null)
                    {
                        MessageBox.Show(string.Format("Insock Supplier: {0} doesn't exist!", insockSupplierName), "Infor", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        InsockTemp insockTemp = new InsockTemp();
                        insockTemp.InsockColumn = columnCheck;
                        insockTemp.InsockSupplierId = insockSupplierPerName.InsockSupplierId;
                        insockTempList.Add(insockTemp);
                    }
                    columnCheck++;
                }

                progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = excelRange.Rows.Count));
                for (int i = 3; i <= excelRange.Rows.Count; i++)
                {
                    var productNoValueCheck = (excelRange.Cells[i, 1] as Excel.Range).Value2;
                    if (productNoValueCheck != null)
                    {
                        string productNo = productNoValueCheck.ToString();
                        for (int j = 8; j <= 8 + insockTempList.Count() - 1; j++)
                        {
                            var etdInCell = (excelRange.Cells[i, j] as Excel.Range).Value2;
                            if (etdInCell != null)
                            {
                                double etd = 0;
                                double.TryParse(etdInCell.ToString(), out etd);
                                var insockPerColumn = insockTempList.Where(w => w.InsockColumn == j).FirstOrDefault();
                                if (insockPerColumn != null && etd != 0)
                                {
                                    insockRawMaterialList.Add(new InsockRawMaterialModel
                                    {
                                        ProductNo = productNo,
                                        InsockSupplierId = insockPerColumn.InsockSupplierId,
                                        InsockSupplierName = insockSupplierList.Where(w => w.InsockSupplierId == insockPerColumn.InsockSupplierId).FirstOrDefault().InsockSupplierName,
                                        ETD = DateTime.FromOADate(etd),
                                    });
                                }
                            }
                        }
                    }
                    progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = i));
                }
            }
            catch { }
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.Cursor = null;
                lblStatus.Text = "Read Completed !";
                btnImport.IsEnabled = true;
                dgInsockRawMaterial .ItemsSource = insockRawMaterialList;
            }
        }


        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (bwImport.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnImport.IsEnabled = false;
                insockRawMaterialImportList.Clear();
                progressBar.Value = 0;
                lblStatus.Text = "Importing ...";
                bwImport.RunWorkerAsync();
            }
        }
        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            insockRawMaterialImportList = dgInsockRawMaterial.Items.OfType<InsockRawMaterialModel>().ToList();
            progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = insockRawMaterialImportList.Count));
            int index = 0;
            foreach (var insockImport in insockRawMaterialImportList)
            {
                index++;
                InsockRawMaterialController.Insert(insockImport);
                dgInsockRawMaterial.Dispatcher.Invoke((Action)(() =>
                {
                    dgInsockRawMaterial.SelectedItem = insockImport;
                    dgInsockRawMaterial.ScrollIntoView(insockImport);
                }));
                progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = index));
            }
        }

        private void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.Cursor = null;
                lblStatus.Text = "Import Completed !";
                btnImport.IsEnabled = true;
            }
        }


        class InsockTemp
        {
            public int InsockColumn { get; set; }
            public int InsockSupplierId { get; set; }
        }
    }
}
