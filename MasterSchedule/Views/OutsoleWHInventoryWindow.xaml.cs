using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHInventoryWindow.xaml
    /// </summary>
    public partial class OutsoleWHInventoryWindow : Window
    {
        BackgroundWorker bwLoadData;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList;
        List<OrdersModel> orderList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OutsoleOutputModel> outsoleOutputList;
        List<AssemblyReleaseModel> assemblyReleaseList;

        public OutsoleWHInventoryWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            outsoleMaterialList = new List<OutsoleMaterialModel>();
            outsoleReleaseMaterialList = new List<OutsoleReleaseMaterialModel>();
            orderList = new List<OrdersModel>();
            outsoleSupplierList = new List<OutsoleSuppliersModel>();

            outsoleOutputList = new List<OutsoleOutputModel>();
            assemblyReleaseList = new List<AssemblyReleaseModel>();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMaterialList = OutsoleMaterialController.Select();
            outsoleReleaseMaterialList = OutsoleReleaseMaterialController.SelectByOutsoleMaterial();
            outsoleOutputList = OutsoleOutputController.SelectByAssemblyMaster();
            // lay ra nhung order co outsolematerial roi.
            orderList = OrdersController.SelectByOutsoleMaterial();

            outsoleSupplierList = OutsoleSuppliersController.Select();
            assemblyReleaseList = AssemblyReleaseController.SelectByAssemblyMaster();

            var outsoleWHInventoryViewList = new List<OutsoleWHInventoryViewModel>();
            // lay ra outsolecode tu orderlist(la nhung order co outsolematerial roi) 
            var outsoleCodeList = orderList.Select(o => o.OutsoleCode).Distinct().ToList();
            foreach (string outsoleCode in outsoleCodeList)
            {
                var productNoList = orderList.Where(o => o.OutsoleCode == outsoleCode).Select(o => o.ProductNo).Distinct().ToList();
                var outsoleMaterialList_D1 = outsoleMaterialList.Where(o => productNoList.Contains(o.ProductNo)).ToList();
                var outsoleReleaseMaterialList_D1 = outsoleReleaseMaterialList.Where(o => productNoList.Contains(o.ProductNo)).ToList();
                var supplierIdList = outsoleMaterialList_D1.Select(o => o.OutsoleSupplierId).Distinct().ToList();
                var qtyTotalList = new List<Int32>();

                var outsoleOutputList_D1 = outsoleOutputList.Where(o => productNoList.Contains(o.ProductNo) == true).ToList();
                var assemblyReleaseList_D1 = assemblyReleaseList.Where(a => productNoList.Contains(a.ProductNo) == true).ToList();

                // add Finished Outsole Column
                int qtyOutsoleTotal = 0;
                foreach (string productNo in productNoList)
                {
                    var sizeNoList = outsoleMaterialList.Where(o => o.ProductNo == productNo).Select(o => o.SizeNo).Distinct().ToList();
                    var outsoleOutputList_D2 = outsoleOutputList_D1.Where(o => o.ProductNo == productNo).ToList();
                    var assemblyReleaseList_D2 = assemblyReleaseList_D1.Where(a => a.ProductNo == productNo).ToList();
                    foreach (string sizeNo in sizeNoList)
                    {
                        int qtyRelease = assemblyReleaseList_D2.Where(a => a.SizeNo == sizeNo).Sum(a => a.Quantity);
                        int qtyOutsole = outsoleOutputList_D2.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity) - qtyRelease;
                        qtyOutsoleTotal += qtyOutsole;
                    }
                }
                foreach (Int32 supplierId in supplierIdList)
                {
                    var outsoleMaterialList_D2 = outsoleMaterialList_D1.Where(o => o.OutsoleSupplierId == supplierId).ToList();
                    int qtyTotal = 0;
                    foreach (string productNo in productNoList)
                    {
                        var outsoleMaterialList_D3 = outsoleMaterialList_D2.Where(o => o.ProductNo == productNo).ToList();
                        var outsoleReleaseMaterialList_D3 = outsoleReleaseMaterialList_D1.Where(o => o.ProductNo == productNo).ToList();
                        var sizeNoList = outsoleMaterialList.Where(o => o.ProductNo == productNo).Select(o => o.SizeNo).Distinct().ToList();
                        foreach (string sizeNo in sizeNoList)
                        {
                            int qtyMax = outsoleMaterialList_D3.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity - o.QuantityReject);
                            int qtyRelease = outsoleReleaseMaterialList_D3.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity);
                            int qty = qtyMax - qtyRelease;
                            if (qty < 0)
                            {
                                qty = 0;
                            }
                            qtyTotal += qty;
                        }
                    }
                    qtyTotalList.Add(qtyTotal);
                }

                int qtyMatchTotal = 0;
                foreach (string productNo in productNoList)
                {
                    var outsoleMaterialList_D2 = outsoleMaterialList_D1.Where(o => o.ProductNo == productNo).ToList();
                    var outsoleReleaseMaterialList_D2 = outsoleReleaseMaterialList_D1.Where(o => o.ProductNo == productNo).ToList();
                    var sizeNoList = outsoleMaterialList.Where(o => o.ProductNo == productNo).Select(o => o.SizeNo).Distinct().ToList();
                    foreach (string sizeNo in sizeNoList)
                    {
                        int qtyMin = outsoleMaterialList_D2.Where(o => o.SizeNo == sizeNo).Select(o => o.Quantity - o.QuantityReject).Min();
                        int qtyRelease = outsoleReleaseMaterialList_D2.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity);
                        int qtyMatch = qtyMin - qtyRelease;
                        if (qtyMatch < 0)
                        {
                            qtyMatch = 0;
                        }
                        qtyMatchTotal += qtyMatch;
                    }
                }
                var outsoleWHInventoryView = new OutsoleWHInventoryViewModel
                {
                    OutsoleCode = outsoleCode,
                    ProductNoList = productNoList,
                    SupplierIdList = supplierIdList,
                    Quantity = qtyTotalList.Max(),
                    Matching = qtyMatchTotal,
                    FinishedOutsoleQuantity = qtyOutsoleTotal,
                };
                if (outsoleWHInventoryView.Quantity != 0)
                {
                    outsoleWHInventoryViewList.Add(outsoleWHInventoryView);
                }
            }

            e.Result = outsoleWHInventoryViewList;
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var outsoleWHInventoryViewList = e.Result as List<OutsoleWHInventoryViewModel>;
            dgInventory.ItemsSource = outsoleWHInventoryViewList;
            lblTotalQTy.Text = outsoleWHInventoryViewList.Sum(o => o.Quantity).ToString();
            lblMatching.Text = outsoleWHInventoryViewList.Sum(o => o.Matching).ToString();
            lblFinishedOutsole.Text = outsoleWHInventoryViewList.Sum(o => o.FinishedOutsoleQuantity).ToString();
            this.Cursor = null;
        }

        private void dgInventory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var outsoleWHInventoryView = (OutsoleWHInventoryViewModel)dgInventory.CurrentItem;
            if (outsoleWHInventoryView != null)
            {
                var productNoList = outsoleWHInventoryView.ProductNoList;
                var supplierList = outsoleMaterialList.Where(o => productNoList.Contains(o.ProductNo)).Select(o => o.OutsoleSupplierId).Distinct().ToList();
                var window = new OutsoleWHInventoryDetailWindow(
                    productNoList,
                    orderList.Where(o => productNoList.Contains(o.ProductNo)).ToList(),
                    outsoleReleaseMaterialList.Where(o => productNoList.Contains(o.ProductNo)).ToList(),
                    outsoleSupplierList.Where(o => supplierList.Contains(o.OutsoleSupplierId) == true).ToList()
                    );
                window.Title = String.Format("{0} for {1}", window.Title, outsoleWHInventoryView.OutsoleCode);
                window.Show();
            }
        }
    }
}
