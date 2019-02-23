using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;

using MasterSchedule.ViewModels;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Globalization;
using MasterSchedule.Helpers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for SelectOutsoleSuppliersWindow.xaml
    /// </summary>
    public partial class OutsoleRawMaterialWindow : Window
    {
        string productNo;
        BackgroundWorker bwLoadData;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleRawMaterialViewModel> outsoleRawMaterialViewList;
        DateTime dtDefault;
        DateTime dtNothing;
        BackgroundWorker bwInsert;
        List<OutsoleRawMaterialViewModel> outsoleRawMaterialViewToInsertList;
        List<OutsoleRawMaterialViewModel> outsoleRawMaterialViewToDeleteList;
        public RawMaterialModel rawMaterial;
        public OutsoleRawMaterialWindow(string productNo)
        {
            InitializeComponent();
            this.productNo = productNo;

            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleRawMaterialViewList = new List<OutsoleRawMaterialViewModel>();
            dtNothing = new DateTime(1999, 12, 31);
            dtDefault = new DateTime(2000, 1, 1);

            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);

            outsoleRawMaterialViewToInsertList = new List<OutsoleRawMaterialViewModel>();
            outsoleRawMaterialViewToDeleteList = new List<OutsoleRawMaterialViewModel>();
            rawMaterial = new RawMaterialModel
            {
                ProductNo = productNo,
                MaterialTypeId = 6,
                ETD = dtDefault,  
                ActualDate = dtDefault,
                Remarks = "",
                IsETDUpdate = false,
                IsActualDateUpdate = false,
                IsRemarksUpdate = false,
            };            
        }        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = String.Format("{0} for {1}", this.Title, productNo);
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleSupplierList = OutsoleSuppliersController.Select();
            outsoleRawMaterialList = OutsoleRawMaterialController.Select(productNo);
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            colSuppliers.ItemsSource = outsoleSupplierList;
            foreach (OutsoleRawMaterialModel outsoleRawMaterial in outsoleRawMaterialList)
            {
                OutsoleRawMaterialViewModel outsoleRawMaterialView = new OutsoleRawMaterialViewModel
                {
                    Supplier = outsoleSupplierList.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId).FirstOrDefault(),
                    ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", outsoleRawMaterial.ETD),
                    ETDReal = outsoleRawMaterial.ETD,
                };

                outsoleRawMaterialViewList.Add(outsoleRawMaterialView);
            }
            dgOutsoleMaterial.ItemsSource = null;
            dgOutsoleMaterial.ItemsSource = outsoleRawMaterialViewList;
            btnAddMore.IsEnabled = true;
            btnSave.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnAddMore_Click(object sender, RoutedEventArgs e)
        {
            OutsoleRawMaterialViewModel outsoleRawMaterialView = new OutsoleRawMaterialViewModel
            {
                Supplier = outsoleSupplierList.FirstOrDefault(),
                ETD = "",
            };
            outsoleRawMaterialViewList.Add(outsoleRawMaterialView);
            dgOutsoleMaterial.ItemsSource = null;
            dgOutsoleMaterial.ItemsSource = outsoleRawMaterialViewList;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                outsoleRawMaterialViewToInsertList = dgOutsoleMaterial.Items.OfType<OutsoleRawMaterialViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            rawMaterial.ETD = dtDefault;
            rawMaterial.IsETDUpdate = true;
            foreach (OutsoleRawMaterialViewModel outsoleRawMaterialView in outsoleRawMaterialViewToDeleteList)
            {
                int outsoleSupplierId = outsoleRawMaterialView.Supplier.OutsoleSupplierId;
                OutsoleRawMaterialController.Delete(productNo, outsoleSupplierId);
            }

            if (outsoleRawMaterialViewToInsertList.Count > 0)
            {
                rawMaterial.ETD = outsoleRawMaterialViewToInsertList.Max(o => o.ETDReal);
            }

            foreach (OutsoleRawMaterialViewModel outsoleRawMaterialView in outsoleRawMaterialViewToInsertList)
            {
                int outsoleSupplierId = outsoleRawMaterialView.Supplier.OutsoleSupplierId;
                DateTime etd = TimeHelper.Convert(outsoleRawMaterialView.ETD);
                if (etd != dtDefault && etd != dtNothing)
                {
                    OutsoleRawMaterialModel model = new OutsoleRawMaterialModel
                        {
                            ProductNo = productNo,
                            OutsoleSupplierId = outsoleSupplierId,
                            ETD = etd,
                        };

                    OutsoleRawMaterialController.Insert(model);
                    if (model.ETD > rawMaterial.ETD)
                    {
                        rawMaterial.ETD = model.ETD.Date;
                    }
                }
            }
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSave.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            outsoleRawMaterialViewToDeleteList.Clear();
            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
        }
     
        int[] colDatetimeList = { 1 };

        private void dgOutsoleMaterial_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            OutsoleRawMaterialViewModel outsoleRawMaterialView = e.Row.Item as OutsoleRawMaterialViewModel;
            if (outsoleRawMaterialView == null)
            {
                return;
            }
            int columnIndex = e.Column.DisplayIndex;
            if (colDatetimeList.Contains(columnIndex))
            {
                TextBox txtElement = (TextBox)e.EditingElement as TextBox;
                DateTime etd = TimeHelper.Convert(txtElement.Text);
                if (String.IsNullOrEmpty(txtElement.Text) == false && etd == dtNothing)
                {
                    txtElement.Foreground = Brushes.Red;
                    txtElement.Text = "!";
                    txtElement.SelectAll();
                }
                else
                {
                    outsoleRawMaterialView.ETDReal = etd;
                }
            }
        }

        private void dgOutsoleMaterial_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dgOutsoleMaterial.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Confirm Delete?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    outsoleRawMaterialViewToDeleteList.AddRange(dgOutsoleMaterial.SelectedItems.OfType<OutsoleRawMaterialViewModel>());
                }
                else
                {
                    e.Handled = true;
                }
            }
        }
    }
}
