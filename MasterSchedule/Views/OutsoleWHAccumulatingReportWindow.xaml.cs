using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

using System.ComponentModel;
using System.Data;
using Microsoft.Reporting.WinForms;

using MasterSchedule.Models;
using MasterSchedule.Controllers;
using MasterSchedule.DataSets;
using System.Text.RegularExpressions;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleAccumulatingReportWindow.xaml
    /// </summary>
    public partial class OutsoleWHAccumulatingReportWindow : Window
    {
        List<OutsoleMaterialDetailModel> outsoleMaterialDetailList;
        List<OutsoleMaterialDetailModel> outsoleMaterialDetailFromToList;
        List<OrdersModel> orderList;
        List<OutsoleSuppliersModel> supplierList;

        BackgroundWorker bwLoad;
        BackgroundWorker bwPreview;

        DateTime dateFrom, dateTo;

        public OutsoleWHAccumulatingReportWindow()
        {
            outsoleMaterialDetailList = new List<OutsoleMaterialDetailModel>();
            outsoleMaterialDetailFromToList = new List<OutsoleMaterialDetailModel>();
            orderList = new List<OrdersModel>();
            supplierList = new List<OutsoleSuppliersModel>();

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwPreview = new BackgroundWorker();
            bwPreview.DoWork +=new DoWorkEventHandler(bwPreview_DoWork);
            bwPreview.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwPreview_RunWorkerCompleted);

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }
        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            orderList = OrdersController.Select();
            supplierList = OutsoleSuppliersController.Select();
            //outsoleMaterialDetailList = OutsoleMaterialDetailController.SelectAll();
        }
        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            dpFrom.SelectedDate = DateTime.Now.Date;
            dpTo.SelectedDate = DateTime.Now.Date;
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (bwPreview.IsBusy == false)
            {
                btnPreview.IsEnabled = false;
                this.Cursor = Cursors.Wait;

                dateFrom = dpFrom.SelectedDate.Value;
                dateTo = dpTo.SelectedDate.Value;
                bwPreview.RunWorkerAsync();
            }
        }

        private void bwPreview_DoWork(object sender, DoWorkEventArgs e)
        {
            var productNoAvailableList = orderList.Select(s => s.ProductNo).ToList();
            outsoleMaterialDetailList = OutsoleMaterialDetailController.SelectAll().Where(w => productNoAvailableList.Contains(w.ProductNo)).ToList();

            outsoleMaterialDetailFromToList = outsoleMaterialDetailList.Where(w => dateFrom.Date <= w.UpdatedTime.Date && w.UpdatedTime.Date <= dateTo.Date).ToList();

            DataTable dt = new OutsoleWHAccumulatingDataSet().Tables["OutsoleWHAccumulatingTable"];

            List<OutsoleWHAccumulatingModel> outsoleWHAccumulatingList = new List<OutsoleWHAccumulatingModel>();
            var productNoPerSupplierList = outsoleMaterialDetailFromToList.Select(s => new { ProductNo = s.ProductNo, OutsoleSupplierId = s.OutsoleSupplierId }).Distinct().ToList();
            foreach (var poPerSupp in productNoPerSupplierList)
            {
                var order = orderList.Where(w => w.ProductNo == poPerSupp.ProductNo).FirstOrDefault();
                if (order == null)
                {
                    continue;
                }
                string supplierName = supplierList.Where(w => w.OutsoleSupplierId == poPerSupp.OutsoleSupplierId).FirstOrDefault().Name;
                if (outsoleMaterialDetailFromToList.Where(w => w.OutsoleSupplierId == poPerSupp.OutsoleSupplierId && w.ProductNo == poPerSupp.ProductNo).Select(s => s.Reject).Sum() > 0)
                {
                    OutsoleWHAccumulatingModel outsoleWHAccumulating = new OutsoleWHAccumulatingModel()
                    {
                        OutsoleCode = order.OutsoleCode,
                        SupplierName = supplierName,
                        SupplierId = poPerSupp.OutsoleSupplierId,
                        ProductNo = poPerSupp.ProductNo,
                        ArticleNo = order.ArticleNo
                    };
                    outsoleWHAccumulatingList.Add(outsoleWHAccumulating);
                }
            }

            outsoleWHAccumulatingList = outsoleWHAccumulatingList.OrderBy(o => o.OutsoleCode)
                                                                 .ThenBy(t => t.SupplierName)
                                                                 .ThenBy(t => t.ProductNo)
                                                                 .ThenBy(t => t.ArticleNo)
                                                                 .ToList();

            foreach (var outsoleWHAccumulating in outsoleWHAccumulatingList)
            {
                var sizeRunList = SizeRunController.Select(outsoleWHAccumulating.ProductNo).ToList();
                if (outsoleMaterialDetailFromToList.Where(w => w.ProductNo == outsoleWHAccumulating.ProductNo).Select(s => s.Quantity).Sum() < sizeRunList.Select(s => s.Quantity).Sum() ||
                    outsoleMaterialDetailFromToList.Where(w => w.ProductNo == outsoleWHAccumulating.ProductNo).Select(s => s.Reject).Sum() <= 0)
                {
                    continue;
                }

                int indexColumn = 0;
                var regex = new Regex(@"[A-Z]|[a-z]");
                foreach (var sizeRun in sizeRunList)
                {
                    var outsoleMaterialDetail = outsoleMaterialDetailFromToList.Where(w => w.ProductNo == outsoleWHAccumulating.ProductNo
                                                                                        && w.OutsoleSupplierId == outsoleWHAccumulating.SupplierId
                                                                                        && w.SizeNo == sizeRun.SizeNo).FirstOrDefault();
                    if (outsoleMaterialDetail == null)
                        continue;

                    DataRow dr = dt.NewRow();
                    dr["OutsoleCode"] = outsoleWHAccumulating.OutsoleCode;
                    dr["SupplierName"] = outsoleWHAccumulating.SupplierName;
                    dr["ProductNo"] = outsoleWHAccumulating.ProductNo;
                    dr["ArticleNo"] = outsoleWHAccumulating.ArticleNo;
                    dr["SizeNo"] = regex.IsMatch(sizeRun.SizeNo) ? regex.Replace(sizeRun.SizeNo, "") : sizeRun.SizeNo;
                    if (outsoleMaterialDetail.Reject > 0)
                    {
                        dr["QtyReject"] = outsoleMaterialDetail.Reject;
                    }

                    dt.Rows.Add(dr);
                    indexColumn++;
                }
            }
            e.Result = dt;
        }
        private void bwPreview_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            DataTable dt = e.Result as DataTable;
            string fromTo = string.Format("From: {0:MM/dd/yyyy} to {1:MM/dd/yyyy}", dateFrom, dateTo);
            if (dateFrom.Date == dateTo.Date)
            {
                fromTo = string.Format("Date: {0:MM/dd/yyyy}", dateFrom);
            }

            ReportParameter rp = new ReportParameter("FromTo", fromTo);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OutsoleWHAccumulatingDetail";
            rds.Value = dt;

            //reportViewer.LocalReport.ReportPath = @"E:\SV PROJECT\MS\1.1.9.6 OutsoleAccumulating\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleWHAccumulatingReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\OutsoleWHAccumulatingReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();

            this.Cursor = null;
            btnPreview.IsEnabled = true;
        }
    }

    class OutsoleWHAccumulatingModel 
    {
        public string OutsoleCode { get; set; }
        public string SupplierName { get; set; }
        public int SupplierId { get; set; }
        public string ProductNo { get; set; }
        public string ArticleNo { get; set; }
    }
}
