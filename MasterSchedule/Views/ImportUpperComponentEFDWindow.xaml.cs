using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Win32;

using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;

using MasterSchedule.Controllers;
using MasterSchedule.Models;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ImportUpperComponent.xaml
    /// </summary>
    public partial class ImportUpperComponentEFDWindow : Window
    {
        string filePath;
        List<UpperComponentRawMaterialModel> upperRawMaterialList;
        List<UpperComponentRawMaterialModel> upperRawMaterialImportList;
        BackgroundWorker bwLoad;
        BackgroundWorker bwImport;
        DateTime dtDefault = new DateTime(2000, 01, 01);

        public ImportUpperComponentEFDWindow()
        {
            filePath = "";
            upperRawMaterialList = new List<UpperComponentRawMaterialModel>();
            upperRawMaterialImportList = new List<UpperComponentRawMaterialModel>();

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwImport = new BackgroundWorker();
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import UpperComponent ETD";
            openFileDialog.Filter = "EXCEL Files (*.xls, *.xlsx)|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                if (bwLoad.IsBusy == false)
                {
                    this.Cursor = Cursors.Wait;
                    //ordersList.Clear();
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
            var upperComponentList = UpperComponentController.Select();
            if (upperComponentList.Count < 1)
            {
                return;
            }

            List<UpperComponentTemp> upperTempList = new List<UpperComponentTemp>();

            Excel.Application excelApplication = new Excel.Application();
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(filePath);
            //excelApplication.Visible = true;
            Excel.Worksheet excelWorksheet;
            Excel.Range excelRange;
            try
            {
                excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets[1];
                excelRange = excelWorksheet.UsedRange;

                int columnCheck = 9;
                while ((excelRange.Cells[2, columnCheck] as Excel.Range).Value2 != null)
                {
                    string upperName = (excelRange.Cells[2, columnCheck] as Excel.Range).Value2.ToString();
                    var upperPerName = upperComponentList.Where(w => w.UpperComponentName == upperName).FirstOrDefault();
                    if (upperPerName == null)
                    {
                        MessageBox.Show(string.Format("Upper Component: {0} doesn't exist!", upperName), "Infor", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        UpperComponentTemp upperTemp = new UpperComponentTemp();
                        upperTemp.UpperComponentColumn = columnCheck;
                        upperTemp.UpperComponentID = upperPerName.UpperComponentID;
                        upperTempList.Add(upperTemp);
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
                        for (int j = 9; j <= 9 + upperTempList.Count() - 1; j++)
                        {
                            var etdInCell = (excelRange.Cells[i, j] as Excel.Range).Value2;
                            if (etdInCell != null)
                            {
                                double etd = 0;
                                double.TryParse(etdInCell.ToString(), out etd);
                                var upperPerColumn = upperTempList.Where(w => w.UpperComponentColumn == j).FirstOrDefault();
                                if (upperPerColumn != null && etd != 0)
                                {
                                    upperRawMaterialList.Add(new UpperComponentRawMaterialModel
                                    {
                                        ProductNo = productNo,
                                        UpperComponentID = upperPerColumn.UpperComponentID,
                                        UpperComponentName = upperComponentList.Where(w => w.UpperComponentID == upperPerColumn.UpperComponentID).FirstOrDefault().UpperComponentName,
                                        ETD = DateTime.FromOADate(etd),
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

            }
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.Cursor = null;
                lblStatus.Text = "Read Completed !";
                btnImport.IsEnabled = true;
                dgUpperComponents.ItemsSource = upperRawMaterialList;
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (bwImport.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnImport.IsEnabled = false;
                upperRawMaterialImportList.Clear();
                progressBar.Value = 0;
                lblStatus.Text = "Importing ...";
                bwImport.RunWorkerAsync();
            }
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            upperRawMaterialImportList = dgUpperComponents.Items.OfType<UpperComponentRawMaterialModel>().ToList();
            progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = upperRawMaterialImportList.Count));
            int index = 0;
            foreach (var upperImport in upperRawMaterialImportList)
            {
                index++;
                UpperComponentRawMaterialController.Insert(upperImport);
                dgUpperComponents.Dispatcher.Invoke((Action)(() =>
                {
                    dgUpperComponents.SelectedItem = upperImport;
                    dgUpperComponents.ScrollIntoView(upperImport);
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

        public class UpperComponentTemp
        {
            public int UpperComponentColumn { get; set; }
            public int UpperComponentID { get; set; }
        }

    }
}
