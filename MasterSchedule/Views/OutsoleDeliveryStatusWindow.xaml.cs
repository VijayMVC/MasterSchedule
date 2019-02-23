using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using MasterSchedule.ViewModels;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleDeliveryStatusWindow.xaml
    /// </summary>
    public partial class OutsoleDeliveryStatusWindow : Window
    {
        BackgroundWorker threadLoad;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<SizeRunModel> sizeRunList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OrdersModel> orderList;
        DateTime dtDefault;
        List<OutsoleDeliveryStatusViewModel> outsoleDeliveryStatusViewList;
        List<SewingMasterModel> sewingMasterList;
        public OutsoleDeliveryStatusWindow()
        {
            InitializeComponent();
            threadLoad = new BackgroundWorker();
            threadLoad.WorkerSupportsCancellation = true;
            threadLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadLoad_RunWorkerCompleted);
            threadLoad.DoWork += new DoWorkEventHandler(threadLoad_DoWork);

            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleMaterialList = new List<OutsoleMaterialModel>();
            sizeRunList = new List<SizeRunModel>();
            sewingMasterList = new List<SewingMasterModel>();
            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            orderList = new List<OrdersModel>();
            dtDefault = new DateTime(2000, 1, 1);
            outsoleDeliveryStatusViewList = new List<OutsoleDeliveryStatusViewModel>();
        }

        void threadLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            //outsoleRawMaterialList = OutsoleRawMaterialController.Select();
            outsoleRawMaterialList = OutsoleRawMaterialController.Select();
            outsoleMaterialList = OutsoleMaterialController.SelectByOutsoleRawMaterial();
            sizeRunList = SizeRunController.SelectByOutsoleRawMaterial();
            outsoleSupplierList = OutsoleSuppliersController.Select();
            orderList = OrdersController.SelectByOutsoleRawMaterial();
            sewingMasterList = SewingMasterController.Select();

            List<String> productNoList = outsoleRawMaterialList.Select(r => r.ProductNo).Distinct().ToList();
            foreach (string productNo in productNoList)
            {
                OrdersModel order = orderList.Where(o => o.ProductNo == productNo).FirstOrDefault();
                List<OutsoleRawMaterialModel> outsoleRawMaterialList_D1 = outsoleRawMaterialList.Where(o => o.ProductNo == productNo).ToList();
                List<SizeRunModel> sizeRunList_D1 = sizeRunList.Where(s => s.ProductNo == productNo).ToList();
                List<OutsoleMaterialModel> outsoleMaterialList_D1 = outsoleMaterialList.Where(o => o.ProductNo == productNo).ToList();
                var sewingMasterList_D1 = sewingMasterList.Where(w => w.ProductNo == productNo).ToList();
                foreach (OutsoleRawMaterialModel outsoleRawMaterial in outsoleRawMaterialList_D1)
                {
                    //bool isFull = OutsoleRawMaterialController.IsFull(sizeRunList_D1, new List<OutsoleRawMaterialModel>() { outsoleRawMaterial, }, outsoleMaterialList_D1);
                    if (
                        //isFull == false && 
                        outsoleRawMaterial.ETD.Date != dtDefault
                        //&& outsoleRawMaterial.ActualDate.Date == dtDefault
                        )
                    {
                        OutsoleDeliveryStatusViewModel outsoleDeliveryStatusView = new OutsoleDeliveryStatusViewModel();
                        outsoleDeliveryStatusView.ProductNo = productNo;
                        if (order != null)
                        {
                            outsoleDeliveryStatusView.Country = order.Country;
                            outsoleDeliveryStatusView.ShoeName = order.ShoeName;
                            outsoleDeliveryStatusView.ArticleNo = order.ArticleNo;
                            outsoleDeliveryStatusView.OutsoleCode = order.OutsoleCode;
                            outsoleDeliveryStatusView.Quantity = order.Quantity;
                            outsoleDeliveryStatusView.ETD = order.ETD;
                        }
                        
                        OutsoleSuppliersModel outsoleSupplier = outsoleSupplierList.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId).FirstOrDefault();
                        if (outsoleSupplier != null)
                        {
                            outsoleDeliveryStatusView.Supplier = outsoleSupplier.Name;
                        }

                        outsoleDeliveryStatusView.SupplierETD = outsoleRawMaterial.ETD;
                        //outsoleDeliveryStatusView.Actual = sizeRunList_D1.Sum(s => (s.Quantity - outsoleMaterialList_D1.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId && o.SizeNo == s.SizeNo).Sum(o => (o.Quantity - o.QuantityReject)))).ToString();

                        int actualQty = sizeRunList_D1.Sum(s => (outsoleMaterialList_D1.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId && o.SizeNo == s.SizeNo).Sum(o => (o.Quantity - o.QuantityReject))));
                        outsoleDeliveryStatusView.ActualQuantity = actualQty.ToString();

                        if (outsoleRawMaterial.ActualDate != dtDefault)
                        {
                            outsoleDeliveryStatusView.Actual = string.Format("{0:M/d}", outsoleRawMaterial.ActualDate);
                        }

                        outsoleDeliveryStatusView.IsFinished = false;
                        if (actualQty >= order.Quantity) // && outsoleRawMaterial.ActualDate != dtDefault
                        {
                            outsoleDeliveryStatusView.IsFinished = true;
                        }

                        outsoleDeliveryStatusView.SewingStartDate = dtDefault;
                        var sewingMasterModel = sewingMasterList_D1.FirstOrDefault();
                        if (sewingMasterModel != null)
                        {
                            outsoleDeliveryStatusView.SewingStartDate = sewingMasterModel.SewingStartDate;
                        }

                        outsoleDeliveryStatusViewList.Add(outsoleDeliveryStatusView);
                    }
                }
            }
        }

        void threadLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnView.IsEnabled = true;
            this.Cursor = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpETDStart.SelectedDate = DateTime.Now;
            dpETDEnd.SelectedDate = DateTime.Now;

            dpSupplierETDStart.SelectedDate = DateTime.Now;
            dpSupplierETDEnd.SelectedDate = DateTime.Now;
            if (threadLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                threadLoad.RunWorkerAsync();
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            List<OutsoleDeliveryStatusViewModel> outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewList;
            if (chboETD.IsChecked == true)
            {
                DateTime etdStart = dpETDStart.SelectedDate.Value;
                DateTime etdEnd = dpETDEnd.SelectedDate.Value;
                outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(o => etdStart.Date <= o.ETD.Date && o.ETD.Date <= etdEnd.Date).ToList();
            }

            string shoeName = txtArticleStyle.Text;
            if (string.IsNullOrEmpty(shoeName) == false)
            {
                outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(o => o.OutsoleCode.ToLower().Contains(shoeName.ToLower()) == true).ToList();
            }

            string supplier = txtSupplier.Text;
            if (string.IsNullOrEmpty(supplier) == false)
            {
                outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(o => o.Supplier.ToLower().Contains(supplier.ToLower()) == true).ToList();
            }

            if (chboSupplierETD.IsChecked == true)
            {
                DateTime etdStartSupplier = dpSupplierETDStart.SelectedDate.Value;
                DateTime etdEndSupplier = dpSupplierETDEnd.SelectedDate.Value;
                outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(o => etdStartSupplier.Date <= o.SupplierETD.Date && o.SupplierETD.Date <= etdEndSupplier.Date).ToList();
            }

            if (chboFinished.IsChecked.Value == true || chboUnfinished.IsChecked.Value == true)
            {
                outsoleDeliveryStatusViewFilterList = outsoleDeliveryStatusViewFilterList.Where(o => o.IsFinished == chboFinished.IsChecked.Value || !o.IsFinished == chboUnfinished.IsChecked.Value).ToList();
            }
            else
            {
                outsoleDeliveryStatusViewFilterList = null;
            }
            dgMain.ItemsSource = null;
            dgMain.ItemsSource = outsoleDeliveryStatusViewFilterList;
        }
    }
}
