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
    /// Interaction logic for InsockInputMaterialWindow.xaml
    /// </summary>
    public partial class InsockInputMaterialWindow : Window
    {
        string productNo;
        BackgroundWorker threadLoadData;
        List<InsockSuppliersModel> insockSupplierList;
        List<SizeRunModel> sizeRunList;
        DataTable dt;
        List<InsockMaterialModel> insockMaterialToInsertList;
        BackgroundWorker threadInsert;
        List<InsockMaterialModel> insockMaterialList;
        public RawMaterialModel rawMaterial;
        List<InsockRawMaterialModel> insockRawMaterialList;
        List<InsockSuppliersModel> insockSupplierModifiedList;
        DateTime dtDefault;
        DateTime dtNothing;
        List<InsockRawMaterialModel> insockRawMaterialToInsertList;
        BackgroundWorker threadUpdateRawMaterial;

        public InsockInputMaterialWindow(string productNo)
        {
            this.productNo = productNo;

            threadLoadData = new BackgroundWorker();
            threadLoadData.WorkerSupportsCancellation = true;
            threadLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            threadLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            threadInsert = new BackgroundWorker();
            threadInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            threadInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);

            insockMaterialList = new List<InsockMaterialModel>();
            insockSupplierList = new List<InsockSuppliersModel>();
            sizeRunList = new List<SizeRunModel>();
            dt = new DataTable();
            insockMaterialToInsertList = new List<InsockMaterialModel>();

            dtDefault = new DateTime(2000, 1, 1);
            dtNothing = new DateTime(1999, 12, 31);
            rawMaterial = new RawMaterialModel
            {
                ProductNo = productNo,
                MaterialTypeId = 13,
                ETD = dtNothing,
                ActualDate = dtNothing,
                Remarks = "",
            };

            insockRawMaterialList = new List<InsockRawMaterialModel>();
            insockSupplierModifiedList = new List<InsockSuppliersModel>();
            insockRawMaterialToInsertList = new List<InsockRawMaterialModel>();

            threadUpdateRawMaterial = new BackgroundWorker();
            threadUpdateRawMaterial.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadUpdateRawMaterial_RunWorkerCompleted);
            threadUpdateRawMaterial.DoWork += new DoWorkEventHandler(threadUpdateRawMaterial_DoWork);

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
            insockSupplierList = InsockSuppliersController.Select();
            insockMaterialList = InsockMaterialController.Select(productNo);
            sizeRunList = SizeRunController.Select(productNo);
            insockRawMaterialList = InsockRawMaterialController.SelectPerPO(productNo);
        }
        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (InsockSuppliersModel insockSupplier in insockSupplierList)
            {
                insockSupplierModifiedList.Add(insockSupplier);
                insockSupplierModifiedList.Add(new InsockSuppliersModel
                {
                    InsockSupplierName = "Reject",
                    InsockSupplierId = insockSupplier.InsockSupplierId,
                });
            }
            colSuppliers.ItemsSource = insockSupplierModifiedList;
            //dt.Columns.Clear();
            dt.Columns.Add("Supplier", typeof(InsockSuppliersModel));
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
                dgInsockMaterial.Columns.Add(column);
            }
            colCompleted.DisplayIndex = dgInsockMaterial.Columns.Count - 1;

            List<Int32> insockSupplierIdList = insockRawMaterialList.Select(o => o.InsockSupplierId).Distinct().ToList();
            for (int i = 0; i <= insockSupplierIdList.Count - 1; i++)
            {
                int insockSupplierId = insockSupplierIdList[i];
                DataRow dr = dt.NewRow();
                dr["Supplier"] = insockSupplierModifiedList.Where(o => o.InsockSupplierId == insockSupplierId && o.InsockSupplierName != "Reject").FirstOrDefault();
                DateTime dtETD = insockRawMaterialList.Where(o => o.InsockSupplierId == insockSupplierId).FirstOrDefault().ETD;
                {
                    if (dtETD.Date != dtDefault)
                    {
                        dr["ETD"] = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", dtETD);
                    }
                }
                DateTime dtActualDate = insockRawMaterialList.Where(o => o.InsockSupplierId == insockSupplierId).FirstOrDefault().ActualDate;
                if (dtActualDate.Date != dtDefault)
                {
                    dr["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", dtActualDate);
                }
                DataRow dr1 = dt.NewRow();
                dr1["Supplier"] = insockSupplierModifiedList.Where(o => o.InsockSupplierId == insockSupplierId && o.InsockSupplierName == "Reject").FirstOrDefault();
                for (int j = 0; j <= sizeRunList.Count - 1; j++)
                {
                    SizeRunModel sizeRun = sizeRunList[j];
                    dr[String.Format("Column{0}", j)] = insockMaterialList.Where(o => o.InsockSupplierId == insockSupplierId && o.SizeNo == sizeRun.SizeNo).Sum(o => o.Quantity);
                    dr1[String.Format("Column{0}", j)] = insockMaterialList.Where(o => o.InsockSupplierId == insockSupplierId && o.SizeNo == sizeRun.SizeNo).Sum(o => o.QuantityReject);
                }
                dt.Rows.Add(dr);
                dt.Rows.Add(dr1);
            }

            dgInsockMaterial.ItemsSource = dt.AsDataView();
            btnSave.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            dt = ((DataView)dgInsockMaterial.ItemsSource).ToTable();
            for (int r = 0; r <= dt.Rows.Count - 1; r = r + 2)
            {
                DataRow dr = dt.Rows[r];
                DataRow dr1 = dt.Rows[r + 1];
                InsockSuppliersModel insockSupplier = (InsockSuppliersModel)dr["Supplier"];
                if (insockSupplier != null)
                {
                    int insockSupplierId = insockSupplier.InsockSupplierId;
                    string actualDate = dr["ActualDate"] as String;
                    DateTime dtActualDate = TimeHelper.Convert(actualDate);
                    if (String.IsNullOrEmpty(actualDate) == false && dtActualDate != dtNothing)
                    {
                        InsockRawMaterialModel insockRawMaterial = new InsockRawMaterialModel
                        {
                            ProductNo = productNo,
                            InsockSupplierId = insockSupplierId,
                            ActualDate = dtActualDate,
                        };

                        insockRawMaterialToInsertList.Add(insockRawMaterial);
                    }
                    for (int i = 0; i <= sizeRunList.Count - 1; i++)
                    {
                        string sizeNo = sizeRunList[i].SizeNo;
                        int quantity = (Int32)dr[String.Format("Column{0}", i)];
                        int quantityReject = (Int32)dr1[String.Format("Column{0}", i)];
                        if (quantity >= 0 && quantityReject >= 0)
                        {
                            InsockMaterialModel model = new InsockMaterialModel
                            {
                                ProductNo = productNo,
                                InsockSupplierId = insockSupplierId,
                                SizeNo = sizeNo,
                                Quantity = quantity,
                                QuantityReject = quantityReject,
                            };
                            insockMaterialToInsertList.Add(model);
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
        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (InsockRawMaterialModel model in insockRawMaterialToInsertList)
            {
                InsockRawMaterialController.UpdateActualDate(model);
            }
            foreach (InsockMaterialModel model in insockMaterialToInsertList)
            {
                InsockMaterialModel insockMaterialDB = InsockMaterialController.Select(model.ProductNo).Where(w => w.SizeNo == model.SizeNo && w.InsockSupplierId == model.InsockSupplierId).FirstOrDefault();
                // Insert
                if (insockMaterialDB == null)
                {
                    InsockMaterialController.Insert(model);
                }

                // Update
                else
                {
                    // Update quantity
                    if (insockMaterialDB.Quantity != model.Quantity)
                    {
                        InsockMaterialController.Update(model, false, true);
                    }
                    // Update Reject
                    if (insockMaterialDB.QuantityReject != model.QuantityReject)
                    {
                        InsockMaterialController.Update(model, true, false);
                    }
                }
            }
        }
        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            if (InsockRawMaterialController.IsFull(sizeRunList, insockRawMaterialList, insockMaterialToInsertList) == true)
            {
                if (insockRawMaterialToInsertList.Count() > 0)
                {
                    rawMaterial.ActualDate = insockRawMaterialToInsertList.Select(o => o.ActualDate).Max();
                }
                else
                {
                    rawMaterial.ActualDate = insockRawMaterialList.Select(o => o.ActualDate).Max();
                }
                int balance = sizeRunList.Sum(s => s.Quantity) - sizeRunList.Select(s => insockMaterialToInsertList.Where(o => o.SizeNo == s.SizeNo).Min(o => (o.Quantity - o.QuantityReject))).Sum();
                if (balance > 0)
                {
                    rawMaterial.ActualDate = dtDefault;
                    rawMaterial.Remarks = balance.ToString();
                }
            }
            else
            {
                rawMaterial.ActualDate = dtDefault;
                rawMaterial.Remarks = (sizeRunList.Sum(s => s.Quantity) - sizeRunList.Select(s => insockMaterialToInsertList.Where(o => o.SizeNo == s.SizeNo).Min(o => (o.Quantity - o.QuantityReject))).Sum()).ToString();
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

        private void btnCompleted_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = ((DataRowView)dgInsockMaterial.CurrentItem).Row;
            if (dr == null)
            {
                return;
            }
            InsockSuppliersModel insockSupplier = dr["Supplier"] as InsockSuppliersModel;
            if (insockSupplier == null)
            {
                return;
            }
            if (insockSupplier.InsockSupplierName != "Reject")
            {
                dr["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:M/dd}", DateTime.Now);
            }
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dr[String.Format("Column{0}", i)] = sizeRun.Quantity;
            }
            dgInsockMaterial.ItemsSource = null;
            dgInsockMaterial.ItemsSource = dt.AsDataView();
        }

        private void dgInsockMaterial_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
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
            InsockSuppliersModel insockSupplier = (InsockSuppliersModel)((DataRowView)e.Row.Item)["Supplier"];
            int qtyOld = 0;
            if (insockSupplier.InsockSupplierName != "Reject")
            {
                qtyOld = insockMaterialList.Where(o => o.InsockSupplierId == insockSupplier.InsockSupplierId && o.SizeNo == sizeNo).Sum(o => o.Quantity);
            }
            else if (insockSupplier.InsockSupplierName == "Reject")
            {
                qtyOld = insockMaterialList.Where(o => o.InsockSupplierId == insockSupplier.InsockSupplierId && o.SizeNo == sizeNo).Sum(o => o.QuantityReject);
            }
            int qtyOrder = sizeRunList.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity);
            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyNew = 0;
            if (int.TryParse(txtCurrent.Text, out qtyNew) == true)
            {
                txtCurrent.Text = (qtyOld + qtyNew).ToString();
                if (qtyOld + qtyNew < 0 || qtyOld + qtyNew > qtyOrder)
                {
                    txtCurrent.Text = qtyOld.ToString();
                }
            }
        }
    }
}
