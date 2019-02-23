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
    /// Interaction logic for ImportOrdersWindow.xaml
    /// </summary>
    public partial class ImportOrdersWindow : Window
    {
        string filePath;
        List<OrdersModel> ordersList;
        BackgroundWorker bwLoad;
        BackgroundWorker bwImport;
        List<OrdersModel> ordersToImportList;
        public ImportOrdersWindow()
        {
            filePath = "";
            ordersList = new List<OrdersModel>();
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwImport = new BackgroundWorker();
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);
            ordersToImportList = new List<OrdersModel>();
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
                    ordersList.Clear();
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
                    var orders = new OrdersModel();
                    var productNoValue = (excelRange.Cells[i, 4] as Excel.Range).Value2;
                    if (productNoValue != null)
                    {
                        string UCustomerCode = "";
                        var uCustomerCodeValue = (excelRange.Cells[i, 2] as Excel.Range).Value2;
                        if (uCustomerCodeValue != null)
                        {
                            UCustomerCode = uCustomerCodeValue.ToString();
                        }
                        orders.UCustomerCode = UCustomerCode;

                        string GTNPONo = "";
                        var GTNPONoValue = (excelRange.Cells[i, 3] as Excel.Range).Value2;
                        if (GTNPONoValue != null)
                        {
                            GTNPONo = GTNPONoValue.ToString();
                        }
                        orders.GTNPONo = GTNPONo;

                        string productNo = productNoValue.ToString();
                        orders.ProductNo = productNo;

                        //DateTime csd = new DateTime(2000, 1, 1, 0, 0, 0);
                        //DateTime.TryParse((excelRange.Cells[i, 5] as Excel.Range).Value2.ToString(), out csd);
                        double csdOADate = 0;
                        Double.TryParse((excelRange.Cells[i, 6] as Excel.Range).Value2.ToString(), out csdOADate);
                        DateTime csd = DateTime.FromOADate(csdOADate);
                        orders.ETD = csd.AddDays(-10);

                        string articleNo = (excelRange.Cells[i, 7] as Excel.Range).Value2.ToString();
                        orders.ArticleNo = articleNo;

                        string shoeName = (excelRange.Cells[i, 8] as Excel.Range).Value2.ToString();
                        orders.ShoeName = shoeName;

                        int quantity = 0;
                        int.TryParse((excelRange.Cells[i, 9] as Excel.Range).Value2.ToString(), out quantity);
                        orders.Quantity = quantity;

                        string patternNo = (excelRange.Cells[i, 11] as Excel.Range).Value2.ToString();
                        orders.PatternNo = patternNo;

                        var midsoleCodeValue = (excelRange.Cells[i, 12] as Excel.Range).Value2;
                        string midsoleCode = "";
                        if (midsoleCodeValue != null)
                        {
                            midsoleCode = midsoleCodeValue.ToString();
                        }
                        orders.MidsoleCode = midsoleCode;

                        var outsoleCodeValue = (excelRange.Cells[i, 13] as Excel.Range).Value2;
                        string outsoleCode = "";
                        if (outsoleCodeValue != null)
                        {
                            outsoleCode = outsoleCodeValue.ToString();
                        }
                        orders.OutsoleCode = outsoleCode;

                        var lastCodeValue = (excelRange.Cells[i, 14] as Excel.Range).Value2;
                        string lastCode = "";
                        if (lastCodeValue != null)
                        {
                            lastCode = lastCodeValue.ToString();
                        }
                        orders.LastCode = lastCode;

                        var countryValue = (excelRange.Cells[i, 15] as Excel.Range).Value2;
                        string country = "";
                        if (countryValue != null)
                        {
                            country = countryValue.ToString();
                        }
                        orders.Country = country;

                        ordersList.Add(orders);
                    }
                    progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = i));
                }
            }
            catch
            {
                ordersList.Clear();
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
            if (ordersList.Count() > 0)
            {
                dgOrders.ItemsSource = ordersList;
                btnImport.IsEnabled = true;
                MessageBox.Show(string.Format("Read Completed. {0} Prod. No.!", ordersList.Count()), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
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
                ordersToImportList = dgOrders.Items.OfType<OrdersModel>().ToList();
                bwImport.RunWorkerAsync();
            }
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var orders in ordersToImportList)
            {
                OrdersController.Insert(orders);
                dgOrders.Dispatcher.Invoke((Action)(() =>
                    {
                        dgOrders.SelectedItem = orders;
                        dgOrders.ScrollIntoView(orders);
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
