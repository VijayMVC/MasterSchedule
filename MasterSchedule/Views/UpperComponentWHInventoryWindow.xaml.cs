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
    /// Interaction logic for UpperComponentInventoryWindow.xaml
    /// </summary>
    public partial class UpperComponentWHInventoryWindow : Window
    {
        BackgroundWorker bwLoadData;
        List<UpperComponentMaterialModel> upperComponentMaterialList;
        List<UpperComponentModel> upperComponentList;
        List<OrdersModel> orderList;
        List<UpperComponentWHInventoryViewModel> upperComponentWHInventoryViewList;
        public UpperComponentWHInventoryWindow()
        {
            upperComponentMaterialList = new List<UpperComponentMaterialModel>();
            upperComponentList = new List<UpperComponentModel>();
            upperComponentWHInventoryViewList = new List<UpperComponentWHInventoryViewModel>();
            orderList = new List<OrdersModel>();

            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

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
            upperComponentList = UpperComponentController.Select();
            upperComponentMaterialList = UpperComponentMaterialController.Select();

            // lay ra nhung order da co uppercomponentrawmaterial
            orderList = OrdersController.SelectByUpperComponentRawMaterial();

            var upperComponentInventoryViewList = new List<UpperComponentWHInventoryViewModel>();
            List<String> outsoleCodeList = orderList.Select(o => o.OutsoleCode).Distinct().ToList();
            foreach (string outsoleCode in outsoleCodeList)
            {
                List<String> productNoList = orderList.Where(o => o.OutsoleCode == outsoleCode).Select(o => o.ProductNo).Distinct().ToList();
                List<UpperComponentMaterialModel> upperComponentMaterialList_D1 = upperComponentMaterialList.Where(o => productNoList.Contains(o.ProductNo)).ToList();
                List<Int32> upperComponentIDList = upperComponentMaterialList_D1.Select(o => o.UpperComponentID).Distinct().ToList();
                List<Int32> qtyTotalList = new List<Int32>();

                foreach (var upperComponentID in upperComponentIDList)
                {
                    List<UpperComponentMaterialModel> upperComponentMaterialList_D2 = upperComponentMaterialList_D1.Where(w => w.UpperComponentID == upperComponentID).ToList();
                    int qtyTotal = 0;
                    foreach (var productNo in productNoList)
                    {
                        List<UpperComponentMaterialModel> upperComponentMaterialList_D3 = upperComponentMaterialList_D2.Where(w => w.ProductNo == productNo).ToList();
                        List<String> sizeNoList = upperComponentMaterialList.Where(o => o.ProductNo == productNo).Select(o => o.SizeNo).Distinct().ToList();
                        foreach (var sizeNo in sizeNoList)
                        {
                            int qty = upperComponentMaterialList_D3.Where(w => w.SizeNo == sizeNo).Sum(s => s.Quantity - s.QuantityReject);
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
                foreach (var productNo in productNoList)
                {
                    List<UpperComponentMaterialModel> upperComponentMaterialList_D2 = upperComponentMaterialList_D1.Where(w => w.ProductNo == productNo).ToList();
                    List<String> sizeNoList = upperComponentMaterialList.Where(o => o.ProductNo == productNo).Select(o => o.SizeNo).Distinct().ToList();
                    foreach (string sizeNo in sizeNoList)
                    {
                        int qtyMatch = upperComponentMaterialList_D2.Where(w => w.SizeNo == sizeNo).Select(s => s.Quantity - s.QuantityReject).Min();
                        if (qtyMatch < 0)
                        {
                            qtyMatch = 0;
                        }
                        qtyMatchTotal += qtyMatch;
                    }
                }
                UpperComponentWHInventoryViewModel upperComponentWHInventoryView = new UpperComponentWHInventoryViewModel()
                {
                    OutsoleCode = outsoleCode,
                    ProductNoList = productNoList,
                    UpperComponentIDList = upperComponentIDList,
                    Quantity = qtyTotalList.Max(),
                    Matching = qtyMatchTotal,
                };
                if (upperComponentWHInventoryView.Quantity != 0)
                {
                    upperComponentWHInventoryViewList.Add(upperComponentWHInventoryView);
                }
            }
            e.Result = upperComponentWHInventoryViewList;
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<UpperComponentWHInventoryViewModel> upperComponentWHInventoryList = e.Result as List<UpperComponentWHInventoryViewModel>;
            dgInventory.ItemsSource = upperComponentWHInventoryList;
            lblTotalQTy.Text = upperComponentWHInventoryList.Sum(o => o.Quantity).ToString();
            lblMatching.Text = upperComponentWHInventoryList.Sum(o => o.Matching).ToString();
            //lblFinishedOutsole.Text = upperComponentWHInventoryList.Sum(o => o.FinishedOutsoleQuantity).ToString();
            this.Cursor = null;
        }

        private void dgInventory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpperComponentWHInventoryViewModel upperComponentWHInventoryView = (UpperComponentWHInventoryViewModel)dgInventory.CurrentItem;
            if (upperComponentWHInventoryView != null)
            {
                List<String> productNoList = upperComponentWHInventoryView.ProductNoList;
                List<Int32> upperComponentIDList = upperComponentMaterialList.Where(o => productNoList.Contains(o.ProductNo)).Select(o => o.UpperComponentID).Distinct().ToList();
                UpperComponentWHInventoryDetailWindow window = new UpperComponentWHInventoryDetailWindow(
                    productNoList,
                    orderList.Where(o => productNoList.Contains(o.ProductNo)).ToList(),
                    upperComponentMaterialList.Where(o => productNoList.Contains(o.ProductNo)).ToList(),
                    //outsoleReleaseMaterialList.Where(o => productNoList.Contains(o.ProductNo)).ToList(),
                    upperComponentList.Where(o => upperComponentIDList.Contains(o.UpperComponentID) == true).ToList()
                    );
                window.Title = String.Format("{0} for {1}", window.Title, upperComponentWHInventoryView.OutsoleCode);
                window.Show();
            }
        }
    }
}
