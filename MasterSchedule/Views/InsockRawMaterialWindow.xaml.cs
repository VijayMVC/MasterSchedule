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
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;
using MasterSchedule.Helpers;
using System.Globalization;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for InsockRawMaterialWindow.xaml
    /// </summary>
    public partial class InsockRawMaterialWindow : Window
    {
        string productNo;
        BackgroundWorker bwLoadData;
        DateTime dtDefault;
        DateTime dtNothing;
        BackgroundWorker bwInsert;
        public RawMaterialModel rawMaterial;

        List<InsockSuppliersModel> insockSupplierList;
        List<InsockRawMaterialModel> insockRawMaterialList;
        List<InsockRawMaterialViewModel> insockRawMaterialViewList;
        List<InsockRawMaterialViewModel> insockRawMaterialViewToInsertList;
        List<InsockRawMaterialViewModel> insockRawMaterialViewToDeleteList;
        public InsockRawMaterialWindow(string productNo)
        {
            this.productNo = productNo;

            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);

            insockSupplierList = new List<InsockSuppliersModel>();
            insockRawMaterialList = new List<InsockRawMaterialModel>();
            insockRawMaterialViewList = new List<InsockRawMaterialViewModel>();
            insockRawMaterialViewToInsertList = new List<InsockRawMaterialViewModel>();
            insockRawMaterialViewToDeleteList = new List<InsockRawMaterialViewModel>();

            dtNothing = new DateTime(1999, 12, 31);
            dtDefault = new DateTime(2000, 1, 1);


            rawMaterial = new RawMaterialModel
            {
                ProductNo = productNo,
                MaterialTypeId = 13,
                ETD = dtDefault,
                ActualDate = dtDefault,
                Remarks = "",
                IsETDUpdate = false,
                IsActualDateUpdate = false,
                IsRemarksUpdate = false,
            };

            InitializeComponent();
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
            insockSupplierList = InsockSuppliersController.Select();
            insockRawMaterialList = InsockRawMaterialController.SelectPerPO(productNo);
        }
        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            colInsockSupplier.ItemsSource = insockSupplierList;
            foreach (var insockRawMaterial in insockRawMaterialList)
            {
                InsockRawMaterialViewModel insockRawMaterialView = new InsockRawMaterialViewModel()
                {
                    InsockSupplier = insockSupplierList.Where(w => w.InsockSupplierId == insockRawMaterial.InsockSupplierId).FirstOrDefault(),
                    ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", insockRawMaterial.ETD),
                    ETDReal = insockRawMaterial.ETD,
                };
                insockRawMaterialViewList.Add(insockRawMaterialView);
            }

            dgInsock.ItemsSource = null;
            dgInsock.ItemsSource = insockRawMaterialViewList;
            btnAddMore.IsEnabled = true;
            btnSave.IsEnabled = true;
            this.Cursor = null;
        }


        private void btnAddMore_Click(object sender, RoutedEventArgs e)
        {
            InsockRawMaterialViewModel insockMaterialViewAdd = new InsockRawMaterialViewModel()
            {
                InsockSupplier = insockSupplierList.FirstOrDefault(),
                ETD = "",
            };
            insockRawMaterialViewList.Add(insockMaterialViewAdd);
            dgInsock.ItemsSource = null;
            dgInsock.ItemsSource = insockRawMaterialViewList;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                insockRawMaterialViewToInsertList = dgInsock.Items.OfType<InsockRawMaterialViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            rawMaterial.ETD = dtDefault;
            rawMaterial.IsETDUpdate = true;
            foreach (var insockRawMaterial in insockRawMaterialViewToDeleteList)
            {
                int insockSupplierId = insockRawMaterial.InsockSupplier.InsockSupplierId;
                InsockRawMaterialController.Delete(productNo, insockSupplierId);
            }

            if (insockRawMaterialViewToInsertList.Count > 0)
                rawMaterial.ETD = insockRawMaterialViewToInsertList.Max(m => m.ETDReal);

            foreach (var insockRawMaterial in insockRawMaterialViewToInsertList)
            {
                int insockSupplierId = insockRawMaterial.InsockSupplier.InsockSupplierId;
                DateTime etd = TimeHelper.Convert(insockRawMaterial.ETD);

                if (etd != dtDefault && etd != dtNothing)
                {
                    var insertModel = new InsockRawMaterialModel() { 
                        ProductNo = productNo,
                        InsockSupplierId = insockSupplierId,
                        ETD = etd
                    };

                    InsockRawMaterialController.Insert(insertModel);
                    if (insertModel.ETD > rawMaterial.ETD)
                        rawMaterial.ETD = insertModel.ETD;
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
            insockRawMaterialViewToDeleteList.Clear();
            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
        }

        int[] colDatetimeList = { 1 };
        private void dgInsock_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var insockRawMaterialView = e.Row.Item as InsockRawMaterialViewModel;
            if (insockRawMaterialView == null)
                return;

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
                    insockRawMaterialView.ETDReal = etd;
                }
            }
        }

        private void dgInsock_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dgInsock.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Confirm Delete?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    insockRawMaterialViewToDeleteList.AddRange(dgInsock.SelectedItems.OfType<InsockRawMaterialViewModel>());
                }
                else
                {
                    e.Handled = true;
                }
            }
        }
    }
}
