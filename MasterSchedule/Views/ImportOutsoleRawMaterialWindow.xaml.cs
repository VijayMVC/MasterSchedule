using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

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
    /// Interaction logic for ImportSizeRunWindow.xaml
    /// </summary>
    public partial class ImportOutsoleRawMaterialWindow : Window
    {
        string filePath;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        BackgroundWorker bwLoad;
        BackgroundWorker bwImport;
        List<OutsoleRawMaterialViewModel> outsoleRawMaterialViewList;
        List<OutsoleRawMaterialViewModel> outsoleRawMaterialViewToImportList;
        public ImportOutsoleRawMaterialWindow()
        {
            filePath = "";
            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwImport = new BackgroundWorker();
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);
            outsoleRawMaterialViewList = new List<OutsoleRawMaterialViewModel>();
            outsoleRawMaterialViewToImportList = new List<OutsoleRawMaterialViewModel>();
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
                    outsoleRawMaterialViewList.Clear();
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
            outsoleSupplierList = OutsoleSuppliersController.Select();
            if(outsoleSupplierList.Count < 1)
            {
                return;
            }

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
                List<OutsoleSupplierColumnModel> outsoleSupplierColumnList = new List<OutsoleSupplierColumnModel>();
                for (int i = 3; i <= excelRange.Rows.Count; i++)
                {
                    if ((excelRange.Cells[i, 1] as Excel.Range).Value2 == null ||
                        String.IsNullOrEmpty((excelRange.Cells[i, 1] as Excel.Range).Value2.ToString()) == true)
                    {
                        outsoleSupplierColumnList.Clear();
                        for (int j = 12; j <= 22; j++)
                        {
                            if ((excelRange.Cells[i, j] as Excel.Range).Value2 != null &&
                                String.IsNullOrEmpty((excelRange.Cells[i, j] as Excel.Range).Value2.ToString()) == false)
                            {
                                string supplierName = (excelRange.Cells[i, j] as Excel.Range).Value2.ToString();
                                OutsoleSuppliersModel outsoleSupplier = outsoleSupplierList.Where(o => o.Name == supplierName).FirstOrDefault();
                                if (outsoleSupplier == null)
                                {
                                    MessageBox.Show(String.Format("Supplier Name at R{0}C{1} Error!", i, j), "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                else
                                {
                                    outsoleSupplierColumnList.Add(new OutsoleSupplierColumnModel 
                                    {
                                        Supplier = outsoleSupplier,
                                        ColumnIndex = j,
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        string productNo = (excelRange.Cells[i, 1] as Excel.Range).Value2.ToString();
                        for (int j = 12; j <= 22; j++)
                        {
                            if ((excelRange.Cells[i, j] as Excel.Range).Value2 != null &&
                                String.IsNullOrEmpty((excelRange.Cells[i, j] as Excel.Range).Value2.ToString()) == false)
                            {
                                double etd = 0;
                                double.TryParse((excelRange.Cells[i, j] as Excel.Range).Value2.ToString(), out etd);
                                OutsoleSupplierColumnModel outsoleSupplierColumn = outsoleSupplierColumnList.Where(o => o.ColumnIndex == j).FirstOrDefault();
                                if (outsoleSupplierColumn != null && etd > 0)
                                {
                                    DateTime etdReal = DateTime.FromOADate(etd);
                                    outsoleRawMaterialViewList.Add(new OutsoleRawMaterialViewModel 
                                    {
                                        ProductNo = productNo,
                                        Supplier = outsoleSupplierColumn.Supplier,
                                        ETD = String.Format("{0:dd-MMM}", etdReal),
                                        ETDReal = etdReal,
                                    });
                                }
                            }  
                        }
                    }
                    progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = i));
                }
            }
            catch
            {
                outsoleRawMaterialViewList.Clear();
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
            lblStatus.Text = "Completed!";
            colSuppliers.ItemsSource = outsoleSupplierList;
            if (outsoleRawMaterialViewList.Count() > 0)
            {
                dgSizeRun.ItemsSource = outsoleRawMaterialViewList;
                btnImport.IsEnabled = true;
                MessageBox.Show(string.Format("Read Completed. {0} Outsole Raw Material!", outsoleRawMaterialViewList.Count()), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
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
                outsoleRawMaterialViewToImportList = dgSizeRun.Items.OfType<OutsoleRawMaterialViewModel>().ToList();
                bwImport.RunWorkerAsync();
            }
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (OutsoleRawMaterialViewModel outsoleRawMaterialView in outsoleRawMaterialViewToImportList)
            {
                OutsoleRawMaterialModel model = new OutsoleRawMaterialModel 
                {
                    ProductNo = outsoleRawMaterialView.ProductNo,
                    OutsoleSupplierId = outsoleRawMaterialView.Supplier.OutsoleSupplierId,
                    ETD = outsoleRawMaterialView.ETDReal,
                };
                OutsoleRawMaterialController.Insert(model);
                dgSizeRun.Dispatcher.Invoke((Action)(() =>
                {
                    dgSizeRun.SelectedItem = outsoleRawMaterialView;
                    dgSizeRun.ScrollIntoView(outsoleRawMaterialView);
                }));
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
            MessageBox.Show("Insert Completed!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }   
    }
}
