using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Data;
using System.Globalization;
using MasterSchedule.Helpers;


namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpperComponentInputMaterialWindow.xaml
    /// </summary>
    public partial class UpperComponentInputMaterialWindow : Window
    {
        string productNo;
        BackgroundWorker threadLoadData;
        BackgroundWorker threadInsert;
        BackgroundWorker threadUpdateRawMaterial;
        List<SizeRunModel> sizeRunList;
        public RawMaterialModel rawMaterial;
        DateTime dtDefault;
        DateTime dtNothing;
        List<UpperComponentModel> upperComponentList;
        List<UpperComponentModel> upperComponentModifiedList;

        List<UpperComponentMaterialModel> upperComponentMaterialList;
        List<UpperComponentRawMaterialModel> upperComponentRawMaterialList;

        List<UpperComponentMaterialModel> upperComponentMaterialToInsertList;
        List<UpperComponentRawMaterialModel> upperComponentRawMaterialToInsertList;

        DataTable dt;

        public UpperComponentInputMaterialWindow(string productNo)
        {
            this.productNo = productNo;
            sizeRunList = new List<SizeRunModel>();
            upperComponentList = new List<UpperComponentModel>();
            upperComponentModifiedList = new List<UpperComponentModel>();
            upperComponentMaterialList = new List<UpperComponentMaterialModel>();
            upperComponentRawMaterialList = new List<UpperComponentRawMaterialModel>();

            upperComponentRawMaterialToInsertList = new List<UpperComponentRawMaterialModel>();
            upperComponentMaterialToInsertList = new List<UpperComponentMaterialModel>();

            threadLoadData = new BackgroundWorker();
            threadLoadData.WorkerSupportsCancellation = true;
            threadLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            threadLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            threadInsert = new BackgroundWorker();
            threadInsert.DoWork +=new DoWorkEventHandler(threadInsert_DoWork);
            threadInsert.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(threadInsert_RunWorkerCompleted);

            threadUpdateRawMaterial = new BackgroundWorker();
            threadUpdateRawMaterial.DoWork +=new DoWorkEventHandler(threadUpdateRawMaterial_DoWork);
            threadUpdateRawMaterial.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(threadUpdateRawMaterial_RunWorkerCompleted);


            dt = new DataTable();
            dtDefault = new DateTime(2000, 1, 1);
            dtNothing = new DateTime(1999, 12, 31);
            rawMaterial = new RawMaterialModel
            {
                ProductNo = productNo,
                MaterialTypeId = 12,
                ETD = dtNothing,
                ActualDate = dtNothing,
                Remarks = "",
            };
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = String.Format("{0} for {1}", this.Title, productNo);
            if (threadLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                threadLoadData.RunWorkerAsync();
            }
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            upperComponentList = UpperComponentController.Select();
            upperComponentMaterialList = UpperComponentMaterialController.Select(productNo);
            sizeRunList = SizeRunController.Select(productNo);
            upperComponentRawMaterialList = UpperComponentRawMaterialController.Select(productNo);
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (var upperComponent in upperComponentList)
            {
                upperComponentModifiedList.Add(upperComponent);
                upperComponentModifiedList.Add(new UpperComponentModel
                {
                    UpperComponentName = "Reject",
                    UpperComponentID = upperComponent.UpperComponentID,
                });
            }
            colUpperComponents.ItemsSource = upperComponentModifiedList;
            //dt.Columns.Clear();
            dt.Columns.Add("UpperComponent", typeof(UpperComponentModel));
            dt.Columns.Add("ETD", typeof(String));
            dt.Columns.Add("ActualDate", typeof(String));
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dt.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                DataGridTextColumn column = new DataGridTextColumn();
                column.SetValue(TagProperty, sizeRun.SizeNo);
                column.Header = String.Format("{0}\n({1})", sizeRun.SizeNo, sizeRun.Quantity);
                column.MinWidth = 40;
                column.Binding = new Binding(String.Format("Column{0}", i));
                dgUpperComponent.Columns.Add(column);
            }
            colCompleted.DisplayIndex = dgUpperComponent.Columns.Count - 1;

            List<Int32> upperComponentIDIdList = upperComponentMaterialList.Select(o => o.UpperComponentID).Distinct().ToList();
            for (int i = 0; i <= upperComponentIDIdList.Count - 1; i++)
            {
                int upperComponentID = upperComponentIDIdList[i];
                DataRow dr = dt.NewRow();
                dr["UpperComponent"] = upperComponentModifiedList.Where(o => o.UpperComponentID == upperComponentID && o.UpperComponentName != "Reject").FirstOrDefault();
                DateTime dtETD = upperComponentRawMaterialList.Where(o => o.UpperComponentID == upperComponentID).FirstOrDefault().ETD;
                {
                    if (dtETD.Date != dtDefault)
                    {
                        dr["ETD"] = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", dtETD);
                    }
                }
                DateTime dtActualDate = upperComponentRawMaterialList.Where(o => o.UpperComponentID == upperComponentID).FirstOrDefault().ActualDate;
                if (dtActualDate.Date != dtDefault)
                {
                    dr["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", dtActualDate);
                }
                DataRow dr1 = dt.NewRow();
                dr1["UpperComponent"] = upperComponentModifiedList.Where(o => o.UpperComponentID == upperComponentID && o.UpperComponentName == "Reject").FirstOrDefault();
                for (int j = 0; j <= sizeRunList.Count - 1; j++)
                {
                    SizeRunModel sizeRun = sizeRunList[j];
                    dr[String.Format("Column{0}", j)] = upperComponentMaterialList.Where(o => o.UpperComponentID == upperComponentID && o.SizeNo == sizeRun.SizeNo).Sum(o => o.Quantity);
                    dr1[String.Format("Column{0}", j)] = upperComponentMaterialList.Where(o => o.UpperComponentID == upperComponentID && o.SizeNo == sizeRun.SizeNo).Sum(o => o.QuantityReject);
                }
                dt.Rows.Add(dr);
                dt.Rows.Add(dr1);
            }

            dgUpperComponent.ItemsSource = dt.AsDataView();
            btnSave.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnCompleted_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = ((DataRowView)dgUpperComponent.CurrentItem).Row;
            if (dr == null)
            {
                return;
            }
            UpperComponentModel upperComponent = dr["UpperComponent"] as UpperComponentModel;
            if (upperComponent == null)
            {
                return;
            }
            if (upperComponent.UpperComponentName != "Reject")
            {
                dr["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:M/dd}", DateTime.Now);
            }
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dr[String.Format("Column{0}", i)] = sizeRun.Quantity;
            }
            dgUpperComponent.ItemsSource = null;
            dgUpperComponent.ItemsSource = dt.AsDataView();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            dt = ((DataView)dgUpperComponent.ItemsSource).ToTable();
            for (int r = 0; r <= dt.Rows.Count - 1; r = r + 2)
            {
                DataRow dr = dt.Rows[r];
                DataRow dr1 = dt.Rows[r + 1];
                UpperComponentModel upperComponent = (UpperComponentModel)dr["UpperComponent"];
                if (upperComponent != null)
                {
                    int upperComponentID = upperComponent.UpperComponentID;
                    string actualDate = dr["ActualDate"] as String;
                    DateTime dtActualDate = TimeHelper.Convert(actualDate);
                    if (String.IsNullOrEmpty(actualDate) == false && dtActualDate != dtNothing)
                    {
                        UpperComponentRawMaterialModel upperComponentRawMaterialModel = new UpperComponentRawMaterialModel
                        {
                            ProductNo = productNo,
                            UpperComponentID = upperComponentID,
                            ActualDate = dtActualDate,
                        };

                        upperComponentRawMaterialToInsertList.Add(upperComponentRawMaterialModel);
                    }
                    for (int i = 0; i <= sizeRunList.Count - 1; i++)
                    {
                        string sizeNo = sizeRunList[i].SizeNo;
                        int quantity = (Int32)dr[String.Format("Column{0}", i)];
                        int quantityReject = (Int32)dr1[String.Format("Column{0}", i)];
                        if (quantity >= 0 && quantityReject >= 0)
                        {
                            UpperComponentMaterialModel model = new UpperComponentMaterialModel
                            {
                                ProductNo = productNo,
                                UpperComponentID = upperComponentID,
                                SizeNo = sizeNo,
                                Quantity = quantity,
                                QuantityReject = quantityReject,
                            };
                            upperComponentMaterialToInsertList.Add(model);
                        }
                    }
                }
            }
            if (threadInsert.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnSave.IsEnabled = false;
                threadInsert.RunWorkerAsync();
            }
        }

        private void threadInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (UpperComponentRawMaterialModel model in upperComponentRawMaterialToInsertList)
            {
                UpperComponentRawMaterialController.UpdateActualDate(model);
            }
            foreach (UpperComponentMaterialModel model in upperComponentMaterialToInsertList)
            {
                UpperComponentMaterialModel upperComponentMaterialDB = UpperComponentMaterialController.Select(model.ProductNo).Where(w => w.SizeNo == model.SizeNo && w.UpperComponentID == model.UpperComponentID).FirstOrDefault();
                // Insert
                if (upperComponentMaterialDB == null)
                {
                    UpperComponentMaterialController.Insert(model);
                }

                // Update
                else
                {
                    // Update quantity
                    if (upperComponentMaterialDB.Quantity != model.Quantity)
                    {
                        UpperComponentMaterialController.Update(model, false, true);
                    }
                    // Update Reject
                    if (upperComponentMaterialDB.QuantityReject != model.QuantityReject)
                    {
                        UpperComponentMaterialController.Update(model, true, false);
                    }
                }
            }
        }

        private void threadInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (UpperComponentRawMaterialController.IsFull(sizeRunList, upperComponentRawMaterialList, upperComponentMaterialList) == true)
            {
                if (upperComponentRawMaterialToInsertList.Count() > 0)
                {
                    rawMaterial.ActualDate = upperComponentRawMaterialToInsertList.Select(o => o.ActualDate).Max();
                }
                else
                {
                    rawMaterial.ActualDate = upperComponentRawMaterialList.Select(o => o.ActualDate).Max();

                }
                int balance = sizeRunList.Sum(s => s.Quantity) - sizeRunList.Select(s => upperComponentMaterialToInsertList.Where(o => o.SizeNo == s.SizeNo).Min(o => (o.Quantity - o.QuantityReject))).Sum();
                if (balance > 0)
                {
                    rawMaterial.ActualDate = dtDefault;
                    rawMaterial.Remarks = balance.ToString();
                }
            }
            else
            {
                rawMaterial.ActualDate = dtDefault;
                rawMaterial.Remarks = (sizeRunList.Sum(s => s.Quantity) - sizeRunList.Select(s => upperComponentMaterialToInsertList.Where(o => o.SizeNo == s.SizeNo).Min(o => (o.Quantity - o.QuantityReject))).Sum()).ToString();
            }
            if (threadUpdateRawMaterial.IsBusy == false)
            {
                threadUpdateRawMaterial.RunWorkerAsync();
            }
        }

        private void threadUpdateRawMaterial_DoWork(object sender, DoWorkEventArgs e)
        {
            RawMaterialController.Insert(rawMaterial);
        }
        private void threadUpdateRawMaterial_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSave.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
        }
        private void dgUpperComponent_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.GetValue(TagProperty) == null)
            {
                return;
            }
            string sizeNo = e.Column.GetValue(TagProperty).ToString();
            if (sizeRunList.Select(s => s.SizeNo).Contains(sizeNo) == false)
            {
                return;
            }
            UpperComponentModel upperComponent = (UpperComponentModel)((DataRowView)e.Row.Item)["UpperComponent"];
            int qtyOld = 0;
            if (upperComponent.UpperComponentName != "Reject")
            {
                qtyOld = upperComponentMaterialList.Where(o => o.UpperComponentID == upperComponent.UpperComponentID && o.SizeNo == sizeNo).Sum(o => o.Quantity);
            }
            else if (upperComponent.UpperComponentName == "Reject")
            {
                qtyOld = upperComponentMaterialList.Where(o => o.UpperComponentID == upperComponent.UpperComponentID && o.SizeNo == sizeNo).Sum(o => o.QuantityReject);
            }
            int qtyOrder = sizeRunList.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity);
            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyNew = 0;
            if (int.TryParse(txtCurrent.Text, out qtyNew) == true)
            {
                txtCurrent.Text = (qtyOld + qtyNew).ToString();
                if (qtyOld + qtyNew < 0)
                {
                    txtCurrent.Text = qtyOld.ToString();
                }
            }
        }
    }
}
