using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using MasterSchedule.DataSets;
using System.Data;
using Microsoft.Reporting.WinForms;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleMaterialDetailVarianceReportWindow.xaml
    /// </summary>
    public partial class OutsoleMaterialDetailVarianceReportWindow : Window
    {
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OutsoleMaterialDetailModel> outsoleMaterialDetailList;
        List<OutsoleMaterialDetailModel> outsoleMaterialDetailPerSupplierList;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OrdersModel> orderList;

        BackgroundWorker bwLoadSupplier;
        BackgroundWorker bwLoadReport;
        DateTime dtDefault;

        public OutsoleMaterialDetailVarianceReportWindow()
        {
            bwLoadSupplier = new BackgroundWorker();
            bwLoadSupplier.DoWork +=new DoWorkEventHandler(bwLoadSupplier_DoWork);
            bwLoadSupplier.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoadSupplier_RunWorkerCompleted);

            bwLoadReport = new BackgroundWorker();
            bwLoadReport.DoWork +=new DoWorkEventHandler(bwLoadReport_DoWork);
            bwLoadReport.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoadReport_RunWorkerCompleted);

            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            outsoleMaterialDetailList = new List<OutsoleMaterialDetailModel>();
            orderList = new List<OrdersModel>();
            outsoleMaterialList = new List<OutsoleMaterialModel>();
            outsoleMaterialDetailPerSupplierList = new List<OutsoleMaterialDetailModel>();
            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();

            dtDefault = new DateTime(2000, 1, 1);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoadSupplier.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                cboSupplier.IsEnabled = false;
                bwLoadSupplier.RunWorkerAsync();
            }
        }

        private void bwLoadSupplier_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleSupplierList = OutsoleSuppliersController.Select();
            orderList = OrdersController.Select();
            outsoleRawMaterialList = OutsoleRawMaterialController.Select();
            outsoleMaterialList = OutsoleMaterialController.Select();
        }

        private void bwLoadSupplier_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cboSupplier.ItemsSource = outsoleSupplierList;
            cboSupplier.SelectedItem = outsoleSupplierList.FirstOrDefault();
            this.Cursor = null;
            cboSupplier.IsEnabled = true;
        }

        OutsoleSuppliersModel outsoleSupplierSelected = new OutsoleSuppliersModel();
        private void cboSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bwLoadReport.IsBusy == false)
            {
                outsoleSupplierSelected = cboSupplier.SelectedItem as OutsoleSuppliersModel;
                this.Cursor = Cursors.Wait;
                bwLoadReport.RunWorkerAsync();
            }
        }

        private void bwLoadReport_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMaterialDetailList = OutsoleMaterialDetailController.SelectAll();

            outsoleMaterialDetailPerSupplierList = outsoleMaterialDetailList.Where(w => w.OutsoleSupplierId == outsoleSupplierSelected.OutsoleSupplierId).OrderBy(o => o.CreatedTime.Date).ToList();
            DataTable dt = new OutsoleMaterialDetailVarianceDataSet().Tables["OutsoleMaterialDetailVarianceTable"];

            var productNoList = outsoleMaterialDetailPerSupplierList.Select(s => s.ProductNo).Distinct().ToList();
            foreach (var productNo in productNoList)
            {
                var order = orderList.Where(w => w.ProductNo.Contains(productNo)).FirstOrDefault();
                var outsoleMaterialDetailPerSupplierPerPO = outsoleMaterialDetailPerSupplierList.Where(w => w.ProductNo.Contains(productNo)).ToList();
                var outsoleRawMaterial = outsoleRawMaterialList.Where(w => w.ProductNo.Contains(productNo) && w.OutsoleSupplierId == outsoleSupplierSelected.OutsoleSupplierId).ToList().FirstOrDefault();

                if (order != null && outsoleMaterialDetailPerSupplierPerPO.Select(s => s.Quantity).Sum() > 0)
                {
                    DataRow dr = dt.NewRow();

                    dr["ProductNo"] = productNo;
                    dr["OutsoleCode"] = order != null ? order.OutsoleCode : "";

                    DateTime startDelivery = outsoleMaterialDetailPerSupplierPerPO.FirstOrDefault().CreatedTime.Date;
                    if (startDelivery != dtDefault)
                        dr["StartDelivery"] = String.Format("{0:dd/MM}", startDelivery);

                    int balance = order.Quantity - outsoleMaterialDetailPerSupplierPerPO.Select(s => s.Quantity).Sum()
                                                 + outsoleMaterialDetailPerSupplierPerPO.Select(s => s.Reject).Sum();

                    DateTime finishDelivery = dtDefault;
                    if (balance == 0)
                    {
                        finishDelivery = outsoleMaterialDetailPerSupplierPerPO.LastOrDefault().CreatedTime.Date;
                        dr["FinishDelivery"] = String.Format("{0:dd/MM}", finishDelivery);
                    }

                    DateTime deliveryEFD = dtDefault;
                    if (outsoleRawMaterial != null)
                    {
                        deliveryEFD = outsoleRawMaterial.ETD;
                    }

                    if (deliveryEFD != dtDefault)
                        dr["DeliveryEFD"] = String.Format("{0:dd/MM}", deliveryEFD);

                    double leadTime = 0;
                    if (finishDelivery != dtDefault)
                    {
                        leadTime = (finishDelivery - startDelivery).TotalDays;
                    }
                    if (leadTime > 0)
                        dr["LeadTime"] = leadTime.ToString();

                    string deliveryPerformance = "";

                    if (deliveryEFD != dtDefault && finishDelivery != dtDefault && finishDelivery <= deliveryEFD)
                    {
                        deliveryPerformance = "OnTime";
                    }
                    double delayVariance = 0;
                    if (deliveryEFD != dtDefault && finishDelivery != dtDefault && finishDelivery > deliveryEFD)
                    {
                        deliveryPerformance = "DeLay";
                        delayVariance = (finishDelivery - deliveryEFD).TotalDays;
                    }
                    dr["DeliveryPerformance"] = deliveryPerformance;
                    if (delayVariance > 0)
                        dr["DelayVariance"] = delayVariance;

                    dt.Rows.Add(dr);
                }
            }
            e.Result = dt;
        }

        private void bwLoadReport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            DataTable dt = e.Result as DataTable;

            ReportParameter rp = new ReportParameter("SupplierName", outsoleSupplierSelected.Name);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OutsoleMaterialDetailVariance";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"E:\SV PROJECT\MS\1.1.9.5 OutsoleRevise\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleMaterialDetailVarianceReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\OutsoleMaterialDetailVarianceReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();

            this.Cursor = null;
        }
    }
}
