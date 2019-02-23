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
    /// Interaction logic for UpperComponentRawMaterialWindow.xaml
    /// </summary>
    public partial class UpperComponentRawMaterialWindow : Window
    {
        string productNo;
        BackgroundWorker bwLoadData;
        DateTime dtDefault;
        DateTime dtNothing;
        BackgroundWorker bwInsert;
        public RawMaterialModel rawMaterial;
        List<UpperComponentModel> upperComponentList;

        List<UpperComponentRawMaterialModel> upperComponentRawMaterialList;
        List<UpperComponentRawMaterialViewModel> upperComponentRawMaterialViewList;
        List<UpperComponentRawMaterialViewModel> upperComponentRawMaterialViewToInsertList;
        List<UpperComponentRawMaterialViewModel> upperComponentRawMaterialViewToDeleteList;

        public UpperComponentRawMaterialWindow(string productNo)
        {
            this.productNo = productNo;

            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);

            upperComponentList = new List<UpperComponentModel>();
            upperComponentRawMaterialList = new List<UpperComponentRawMaterialModel>();
            upperComponentRawMaterialViewList = new List<UpperComponentRawMaterialViewModel>();
            upperComponentRawMaterialViewToInsertList = new List<UpperComponentRawMaterialViewModel>();
            upperComponentRawMaterialViewToDeleteList = new List<UpperComponentRawMaterialViewModel>();

            dtNothing = new DateTime(1999, 12, 31);
            dtDefault = new DateTime(2000, 1, 1);

            rawMaterial = new RawMaterialModel
            {
                ProductNo = productNo,
                MaterialTypeId = 12,
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
            upperComponentList = UpperComponentController.Select();
            upperComponentRawMaterialList = UpperComponentRawMaterialController.Select(productNo);
        }
        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            colUpperComponents.ItemsSource = upperComponentList;
            foreach (var upperComponentRawMaterial in upperComponentRawMaterialList)
            {
                UpperComponentRawMaterialViewModel upperComponentRawMaterialView = new UpperComponentRawMaterialViewModel()
                {
                    UpperComponents = upperComponentList.Where(w => w.UpperComponentID == upperComponentRawMaterial.UpperComponentID).FirstOrDefault(),
                    ETD = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", upperComponentRawMaterial.ETD),
                    ETDReal = upperComponentRawMaterial.ETD,
                };
                upperComponentRawMaterialViewList.Add(upperComponentRawMaterialView);
            }

            dgUpperComponent.ItemsSource = null;
            dgUpperComponent.ItemsSource = upperComponentRawMaterialViewList;
            btnAddMore.IsEnabled = true;
            btnSave.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnAddMore_Click(object sender, RoutedEventArgs e)
        {
            UpperComponentRawMaterialViewModel upperComponentRawMaterialView = new UpperComponentRawMaterialViewModel
            {
                UpperComponents = upperComponentList.FirstOrDefault(),
                ETD = "",
            };
            upperComponentRawMaterialViewList.Add(upperComponentRawMaterialView);
            dgUpperComponent.ItemsSource = null;
            dgUpperComponent.ItemsSource = upperComponentRawMaterialViewList;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                upperComponentRawMaterialViewToInsertList = dgUpperComponent.Items.OfType<UpperComponentRawMaterialViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            rawMaterial.ETD = dtDefault;
            rawMaterial.IsETDUpdate = true;
            foreach (var upperRawMaterialView in upperComponentRawMaterialViewToDeleteList)
            {
                int upperComponentID = upperRawMaterialView.UpperComponents.UpperComponentID;
                UpperComponentRawMaterialController.Delete(productNo, upperComponentID);
            }
            if (upperComponentRawMaterialViewToInsertList.Count > 0)
            {
                rawMaterial.ETD = upperComponentRawMaterialViewToInsertList.Max(m => m.ETDReal);
            }

            foreach (var upperRawMaterialView in upperComponentRawMaterialViewToInsertList)
            {
                int upperComponentID = upperRawMaterialView.UpperComponents.UpperComponentID;
                DateTime etd = TimeHelper.Convert(upperRawMaterialView.ETD);
                if (etd != dtDefault && etd != dtNothing)
                {
                    UpperComponentRawMaterialModel model = new UpperComponentRawMaterialModel() 
                    {
                        ProductNo = productNo,
                        UpperComponentID = upperComponentID,
                        ETD = etd,
                    };
                    UpperComponentRawMaterialController.Insert(model);
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
            upperComponentRawMaterialViewToDeleteList.Clear();
            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
        }


        int[] colDatetimeList = { 1 };
        private void dgUpperComponent_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            UpperComponentRawMaterialViewModel upperComponentView = e.Row.Item as UpperComponentRawMaterialViewModel;
            if (upperComponentView == null)
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
                    upperComponentView.ETDReal = etd;
                }
            }
        }

        private void dgUpperComponent_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dgUpperComponent.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Confirm Delete?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    upperComponentRawMaterialViewToDeleteList.AddRange(dgUpperComponent.SelectedItems.OfType<UpperComponentRawMaterialViewModel>());
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

    }
}
