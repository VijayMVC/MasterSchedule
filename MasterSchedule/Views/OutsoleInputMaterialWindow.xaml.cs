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
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for InputOutsoleMarterialWindow.xaml
    /// </summary>
    public partial class OutsoleInputMaterialWindow : Window
    {
        string productNo;
        BackgroundWorker threadLoadData;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<SizeRunModel> sizeRunList;
        DataTable dt;
        List<OutsoleMaterialModel> outsoleMaterialToInsertList;
        BackgroundWorker threadInsert;
        List<OutsoleMaterialModel> outsoleMaterialList;
        public RawMaterialModel rawMaterial;
        public int totalRejectAssemblyRespone;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleSuppliersModel> outsoleSupplierModifiedList;
        DateTime dtDefault;
        DateTime dtNothing;
        List<OutsoleRawMaterialModel> outsoleRawMaterialToInsertList;
        List<OutsoleMaterialRackPositionModel> outsoleMaterialRackPositionList;
        List<OutsoleMaterialRackPositionModel> outsoleMaterialRackPositionToInsertList;
        List<OutsoleMaterialRackPositionModel> outsoleMaterialRackPositionToDeleteList;
        BackgroundWorker threadUpdateRawMaterial;
        string _REJECT = "Reject", _REJECT_ASSEMBLY = "Reject Assembly / Stockfit";
        AccountModel account;
        public OutsoleInputMaterialWindow(string productNo, AccountModel account)
        {
            InitializeComponent();
            this.productNo = productNo;
            this.account = account;

            threadLoadData = new BackgroundWorker();
            threadLoadData.WorkerSupportsCancellation = true;
            threadLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            threadLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            sizeRunList = new List<SizeRunModel>();
            dt = new DataTable();
            outsoleMaterialToInsertList = new List<OutsoleMaterialModel>();

            threadInsert = new BackgroundWorker();
            threadInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            threadInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);

            outsoleMaterialList = new List<OutsoleMaterialModel>();

            dtDefault = new DateTime(2000, 1, 1);
            dtNothing = new DateTime(1999, 12, 31);
            rawMaterial = new RawMaterialModel
            {
                ProductNo = productNo,
                MaterialTypeId = 6,
                ETD = dtNothing,
                ActualDate = dtNothing,
                Remarks = "",
            };
            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleSupplierModifiedList = new List<OutsoleSuppliersModel>();
            outsoleRawMaterialToInsertList = new List<OutsoleRawMaterialModel>();

            outsoleMaterialRackPositionList = new List<OutsoleMaterialRackPositionModel>();
            outsoleMaterialRackPositionToInsertList = new List<OutsoleMaterialRackPositionModel>();
            outsoleMaterialRackPositionToDeleteList = new List<OutsoleMaterialRackPositionModel>();

            threadUpdateRawMaterial = new BackgroundWorker();
            threadUpdateRawMaterial.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadUpdateRawMaterial_RunWorkerCompleted);
            threadUpdateRawMaterial.DoWork += new DoWorkEventHandler(threadUpdateRawMaterial_DoWork);
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
            outsoleSupplierList = OutsoleSuppliersController.Select();
            outsoleMaterialList = OutsoleMaterialController.Select(productNo);
            sizeRunList = SizeRunController.Select(productNo);
            outsoleRawMaterialList = OutsoleRawMaterialController.Select(productNo);
            outsoleMaterialRackPositionList = OutsoleMaterialRackPositionController.Select(productNo);
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (OutsoleSuppliersModel outsoleSupplier in outsoleSupplierList)
            {
                outsoleSupplierModifiedList.Add(outsoleSupplier);
                outsoleSupplierModifiedList.Add(new OutsoleSuppliersModel
                {
                    Name = _REJECT,
                    OutsoleSupplierId = outsoleSupplier.OutsoleSupplierId,
                });
                outsoleSupplierModifiedList.Add(new OutsoleSuppliersModel
                {
                    Name = _REJECT_ASSEMBLY,
                    OutsoleSupplierId = outsoleSupplier.OutsoleSupplierId,
                });
            }
            colSuppliers.ItemsSource = outsoleSupplierModifiedList;
            //dt.Columns.Clear();
            dt.Columns.Add("Supplier", typeof(OutsoleSuppliersModel));
            dt.Columns.Add("ETD", typeof(String));
            dt.Columns.Add("ActualDate", typeof(String));

            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dt.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                DataGridTextColumn column = new DataGridTextColumn();
                column.SetValue(TagProperty, sizeRun.SizeNo);
                column.Header = String.Format("{0}\n{1}\n{2}\n({3})", sizeRun.SizeNo, sizeRun.OutsoleSize, sizeRun.MidsoleSize, sizeRun.Quantity);
                column.MinWidth = 40;
                if (account.OutsoleMaterialEdit == false)
                {
                    column.IsReadOnly = true;
                }
                column.Binding = new Binding(String.Format("Column{0}", i));
                dgOutsoleMaterial.Columns.Add(column);
            }
            colCompleted.DisplayIndex = dgOutsoleMaterial.Columns.Count - 1;
            colAddRack.DisplayIndex = dgOutsoleMaterial.Columns.Count - 1;

            dt.Columns.Add("RackPosition", typeof(String));
            DataGridTextColumn colRackPosition = new DataGridTextColumn();
            colRackPosition.Header = "RackNumber - CartonNumber";
            colRackPosition.MinWidth = 100;
            colRackPosition.IsReadOnly = true;
            colRackPosition.Binding = new Binding("RackPosition");
            dgOutsoleMaterial.Columns.Add(colRackPosition);
            colRackPosition.DisplayIndex = dgOutsoleMaterial.Columns.Count - 1;

            List<Int32> outsoleSupplierIdList = outsoleRawMaterialList.Select(o => o.OutsoleSupplierId).Distinct().ToList();
            for (int i = 0; i <= outsoleSupplierIdList.Count - 1; i++)
            {
                int outsoleSupplierId = outsoleSupplierIdList[i];
                DataRow dr = dt.NewRow();
                dr["Supplier"] = outsoleSupplierModifiedList.Where(w => w.OutsoleSupplierId == outsoleSupplierId && w.Name != _REJECT && w.Name != _REJECT_ASSEMBLY).FirstOrDefault();
                DateTime dtETD = outsoleRawMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplierId).FirstOrDefault().ETD;
                {
                    if (dtETD.Date != dtDefault)
                    {
                        dr["ETD"] = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", dtETD);
                    }
                }

                DateTime dtActualDate = outsoleRawMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplierId).FirstOrDefault().ActualDate;
                if (dtActualDate.Date != dtDefault)
                {
                    dr["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", dtActualDate);
                }
                DataRow drReject = dt.NewRow();
                DataRow drRejectAssembly = dt.NewRow();
                drReject["Supplier"] = outsoleSupplierModifiedList.Where(o => o.OutsoleSupplierId == outsoleSupplierId && o.Name == _REJECT).FirstOrDefault();
                drRejectAssembly["Supplier"] = outsoleSupplierModifiedList.Where(o => o.OutsoleSupplierId == outsoleSupplierId && o.Name == _REJECT_ASSEMBLY).FirstOrDefault();
                for (int j = 0; j <= sizeRunList.Count - 1; j++)
                {
                    SizeRunModel sizeRun = sizeRunList[j];

                    int qtyLoad = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplierId && o.SizeNo == sizeRun.SizeNo).Sum(o => o.Quantity);
                    if (qtyLoad > 0)
                    dr[String.Format("Column{0}", j)] = qtyLoad;

                    int rejectLoad = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplierId && o.SizeNo == sizeRun.SizeNo).Sum(o => o.QuantityReject);
                    if (rejectLoad > 0)
                    drReject[String.Format("Column{0}", j)] = rejectLoad;

                    int rejectAssemblyLoad = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplierId && o.SizeNo == sizeRun.SizeNo).Sum(o => o.RejectAssembly);
                    if (rejectAssemblyLoad > 0)
                    drRejectAssembly[String.Format("Column{0}", j)] = rejectAssemblyLoad;
                }


                var outsoleMaterialRackPositionPerSupplierList = outsoleMaterialRackPositionList.Where(w => w.OutsoleSupplierId == outsoleSupplierId).ToList();
                dr["RackPosition"] = RackPositionString(outsoleMaterialRackPositionPerSupplierList);

                dt.Rows.Add(dr);
                dt.Rows.Add(drReject);
                dt.Rows.Add(drRejectAssembly);
            }

            dgOutsoleMaterial.ItemsSource = dt.AsDataView();
            btnSave.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            dt = ((DataView)dgOutsoleMaterial.ItemsSource).ToTable();
            for (int r = 0; r <= dt.Rows.Count - 1; r = r + 3)
            {
                DataRow dr = dt.Rows[r];
                DataRow drReject = dt.Rows[r + 1];
                DataRow drRejectAssembly = dt.Rows[r + 2];
                var outsoleSupplier = (OutsoleSuppliersModel)dr["Supplier"];
                if (outsoleSupplier != null)
                {
                    int outsoleSupplierId = outsoleSupplier.OutsoleSupplierId;
                    string actualDate = dr["ActualDate"] as String;
                    DateTime dtActualDate = TimeHelper.Convert(actualDate);
                    if (String.IsNullOrEmpty(actualDate) == false && dtActualDate != dtNothing)
                    {
                        OutsoleRawMaterialModel outsoleRawMaterial = new OutsoleRawMaterialModel
                        {
                            ProductNo = productNo,
                            OutsoleSupplierId = outsoleSupplierId,
                            ActualDate = dtActualDate,
                        };

                        outsoleRawMaterialToInsertList.Add(outsoleRawMaterial);
                    }
                    for (int i = 0; i <= sizeRunList.Count - 1; i++)
                    {
                        string sizeNo = sizeRunList[i].SizeNo;
                        int quantity = 0;
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out quantity);

                        int quantityReject = 0;
                        Int32.TryParse(drReject[String.Format("Column{0}", i)].ToString(), out quantityReject);

                        int quantityRejectAssembly = 0;
                        Int32.TryParse(drRejectAssembly[String.Format("Column{0}", i)].ToString(), out quantityRejectAssembly);

                        if (quantity >= 0 && quantityReject >= 0)
                        {
                            var model = new OutsoleMaterialModel
                            {
                                ProductNo = productNo,
                                OutsoleSupplierId = outsoleSupplierId,
                                SizeNo = sizeNo,
                                Quantity = quantity,
                                QuantityReject = quantityReject,
                                RejectAssembly = quantityRejectAssembly
                            };
                            outsoleMaterialToInsertList.Add(model);
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
            // Delete RackPosition
            foreach (var rackModelDetele in outsoleMaterialRackPositionToDeleteList)
            {
                OutsoleMaterialRackPositionController.Delete(rackModelDetele);
            }

            // Update RackPosition
            foreach (var rackModelInsert in outsoleMaterialRackPositionToInsertList)
            {
                OutsoleMaterialRackPositionController.Insert(rackModelInsert);
            }

            foreach (OutsoleRawMaterialModel model in outsoleRawMaterialToInsertList)
            {
                OutsoleRawMaterialController.UpdateActualDate(model);
            }
            foreach (OutsoleMaterialModel model in outsoleMaterialToInsertList)
            {
                var osMaterialDB = OutsoleMaterialController.Select(model.ProductNo).Where(w => w.SizeNo == model.SizeNo && w.OutsoleSupplierId == model.OutsoleSupplierId).FirstOrDefault();
                // Insert
                if (osMaterialDB == null)
                {
                    OutsoleMaterialController.Insert(model);
                }

                // Update
                else
                {
                    // Update Quantity
                    if (osMaterialDB.Quantity != model.Quantity)
                        OutsoleMaterialController.Update(model, false, true, false);
                    
                    // Update Reject
                    if (osMaterialDB.QuantityReject != model.QuantityReject)
                        OutsoleMaterialController.Update(model, true, false, false);
                    
                    // Update Reject Assembly
                    if (osMaterialDB.RejectAssembly != model.RejectAssembly)
                        OutsoleMaterialController.Update(model, true, false, true);
                }
            }
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (OutsoleRawMaterialController.IsFull(sizeRunList, outsoleRawMaterialList, outsoleMaterialToInsertList) == true)
            {
                if (outsoleRawMaterialToInsertList.Count() > 0)
                {
                    rawMaterial.ActualDate = outsoleRawMaterialToInsertList.Select(o => o.ActualDate).Max();
                }
                else
                {
                    rawMaterial.ActualDate = outsoleRawMaterialList.Select(o => o.ActualDate).Max();

                }
                int balance = sizeRunList.Sum(s => s.Quantity) - sizeRunList.Select(s => outsoleMaterialToInsertList.Where(o => o.SizeNo == s.SizeNo).Min(o => (o.Quantity - o.QuantityReject))).Sum();
                if (balance > 0)
                {
                    rawMaterial.ActualDate = dtDefault;
                    rawMaterial.Remarks = balance.ToString();
                }
            }
            else
            {
                rawMaterial.ActualDate = dtDefault;
                rawMaterial.Remarks = (sizeRunList.Sum(s => s.Quantity) - sizeRunList.Select(s => outsoleMaterialToInsertList.Where(o => o.SizeNo == s.SizeNo).Min(o => (o.Quantity - o.QuantityReject))).Sum()).ToString();
            }

            // Get total reject assembly.
            totalRejectAssemblyRespone = outsoleMaterialToInsertList.Sum(s => s.RejectAssembly);

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
            DataRow dr = ((DataRowView)dgOutsoleMaterial.CurrentItem).Row;
            if (dr == null)
            {
                return;
            }
            var outsoleSupplier = dr["Supplier"] as OutsoleSuppliersModel;
            if (outsoleSupplier == null || outsoleSupplier.Name == _REJECT || outsoleSupplier.Name == _REJECT_ASSEMBLY)
            {
                return;
            }

            dr["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:M/dd}", DateTime.Now);
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dr[String.Format("Column{0}", i)] = sizeRun.Quantity;
            }
            dgOutsoleMaterial.ItemsSource = null;
            dgOutsoleMaterial.ItemsSource = dt.AsDataView();
        }

        private void dgOutsoleMaterial_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
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
            var outsoleSupplier = (OutsoleSuppliersModel)((DataRowView)e.Row.Item)["Supplier"];
            int qtyOld = 0;

            if (outsoleSupplier.Name != _REJECT && outsoleSupplier.Name != _REJECT_ASSEMBLY)
            {
                qtyOld = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId && o.SizeNo == sizeNo).Sum(o => o.Quantity);
            }
            else if (outsoleSupplier.Name == _REJECT)
            {
                qtyOld = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId && o.SizeNo == sizeNo).Sum(o => o.QuantityReject);
            }
            else if (outsoleSupplier.Name == _REJECT_ASSEMBLY)
            {
                qtyOld = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId && o.SizeNo == sizeNo).Sum(o => o.RejectAssembly);
            }
            int qtyOrder = sizeRunList.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity);

            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyNew = 0;
            if (Int32.TryParse(txtCurrent.Text, out qtyNew) == true)
            {
                txtCurrent.Text = (qtyOld + qtyNew).ToString();
                if (qtyOld + qtyNew < 0 || qtyOld + qtyNew > qtyOrder)
                {
                    txtCurrent.Text = qtyOld.ToString();
                }
                //if (qtyNew > 0)
                //    txtCurrent.Text = qtyNew.ToString();
                //else
                //    txtCurrent.Text = "";
            }

            int qtyCurrent = 0;
            Int32.TryParse(txtCurrent.Text.ToString(), out qtyCurrent);
            for (int r = 0; r <= dt.Rows.Count - 1; r++)
            {
                DataRow dr = dt.Rows[r];
                if ((OutsoleSuppliersModel)dr["Supplier"] != outsoleSupplier)
                    continue;
                int totalQtyRowCurrent = 0;
                for (int i = 0; i < sizeRunList.Count; i++)
                {
                    int qty = 0;
                    var sizeRunPerSize = sizeRunList[i];
                    if (sizeRunPerSize.SizeNo == sizeNo)
                    {
                        qty = qtyCurrent;
                        dr[String.Format("Column{0}", i)] = qty;
                    }
                    else
                    {
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out qty);
                    }
                    totalQtyRowCurrent += qty;
                }

                var drUpdate = dr;

                // If current row is Reject, get total quantiy on the [row - 1]. Compare Reject <= 0 && qty >= Quantity
                if (outsoleSupplier.Name == _REJECT && r > 0)
                {
                    int totalReject = totalQtyRowCurrent;
                    int totalQty = 0;
                    var drQty = dt.Rows[r - 1];
                    for (int i = 0; i < sizeRunList.Count; i++)
                    {
                        int qty = 0;
                        Int32.TryParse(drQty[String.Format("Column{0}", i)].ToString(), out qty);
                        totalQty += qty;
                    }

                    drUpdate = dt.Rows[r - 1];

                    if (totalReject <= 0 && totalQty >= sizeRunList.Sum(s => s.Quantity))
                        drUpdate["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:M/dd}", DateTime.Now);
                    else
                        drUpdate["ActualDate"] = "";
                }
                // If current row is Quantity, get total reject on the [row + 1], Compare Reject <=0 && qty >= Quantity
                else if (outsoleSupplier.Name != _REJECT_ASSEMBLY)
                {
                    int totalQty = totalQtyRowCurrent;
                    int totalReject = 0;
                    var drReject = dt.Rows[r + 1];
                    for (int i = 0; i < sizeRunList.Count; i++)
                    {
                        int reject = 0;
                        Int32.TryParse(drReject[String.Format("Column{0}", i)].ToString(), out reject);
                        totalReject += reject;
                    }

                    if (totalReject <= 0 && totalQty >= sizeRunList.Sum(s => s.Quantity))
                        drUpdate["ActualDate"] = String.Format(new CultureInfo("en-US"), "{0:M/dd}", DateTime.Now);
                    else
                        drUpdate["ActualDate"] = "";
                }
            }
        }

        private void btnAddRack_Click(object sender, RoutedEventArgs e)
        {
            DataRow drCurrent = ((DataRowView)dgOutsoleMaterial.CurrentItem).Row;
            if (drCurrent == null)
            {
                return;
            }
            var outsoleSupplier = drCurrent["Supplier"] as OutsoleSuppliersModel;
            var outsoleClicked = outsoleSupplierList.Where(w => w.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId).FirstOrDefault();

            if (outsoleSupplier == null || outsoleClicked == null)
            {
                return;
            }

            var outsoleMaterialRackPositionPerSupplier = outsoleMaterialRackPositionList.Where(w => w.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId).ToList();

            var window = new AddOutsoleMaterialRackPositionWindow(productNo, outsoleClicked);
            window.Title = String.Format("Add Rack for: {0}", productNo);
            window.ShowDialog();

            if (window.rackModel == null)
                return;

            if (window.IsRemove == true)
            {
                outsoleMaterialRackPositionList.RemoveAll(r => r.ProductNo == window.rackModel.ProductNo &&
                                                            r.OutsoleSupplierId == window.rackModel.OutsoleSupplierId &&
                                                            r.RackNumber == window.rackModel.RackNumber &&
                                                            r.CartonNumber == window.rackModel.CartonNumber);
                outsoleMaterialRackPositionToDeleteList.Add(window.rackModel);
            }
            else
            {
                var checkAlready = outsoleMaterialRackPositionList.Where(w => w.OutsoleSupplierId == window.rackModel.OutsoleSupplierId &&
                                                                     w.RackNumber == window.rackModel.RackNumber &&
                                                                     w.CartonNumber == window.rackModel.CartonNumber).FirstOrDefault();
                if (checkAlready != null)
                {
                    MessageBox.Show(String.Format("Rack: {0}-{1}\nAlready Input !", window.rackModel.RackNumber, window.rackModel.CartonNumber), "Add Rack", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                outsoleMaterialRackPositionList.Add(window.rackModel);
                outsoleMaterialRackPositionToInsertList.Add(window.rackModel);
            }

            // Update DataTable
            for (int r = 0; r <= dt.Rows.Count - 1; r++)
            {
                DataRow dr = dt.Rows[r];
                if ((OutsoleSuppliersModel)dr["Supplier"] != outsoleSupplier)
                    continue;

                var drUpdate = dr; 
                if (outsoleSupplier.Name == _REJECT && r > 0)
                {
                    drUpdate = dt.Rows[r - 1];
                }
                if (outsoleSupplier.Name == _REJECT_ASSEMBLY && r > 0)
                {
                    drUpdate = dt.Rows[r - 2];
                }
                var outsoleMaterialRackPositionPerSupplierList = outsoleMaterialRackPositionList.Where(w => w.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId).ToList();
                drUpdate["RackPosition"] = RackPositionString(outsoleMaterialRackPositionPerSupplierList);
            }
        }

        /// <summary>
        /// Show RackPosition
        /// </summary>
        /// <param name="sourceList">Outsole Material Rackposition List</param>
        /// <returns>Rack Position String</returns>
        private string RackPositionString(List<OutsoleMaterialRackPositionModel> sourceList)
        {
            if (sourceList.Count == 0)
                return "";
            var rackPositionList = new List<String>();
            foreach (var p in sourceList)
            {
                rackPositionList.Add(String.Format("{0}-{1}", p.RackNumber, p.CartonNumber));
            }
            return String.Join("  ;  ", rackPositionList);
        }
    }
}
