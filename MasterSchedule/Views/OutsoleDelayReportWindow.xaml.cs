using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

using Microsoft.Reporting.WinForms;
using System.Data;
using MasterSchedule.DataSets;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Text.RegularExpressions;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for DelayReportWindow.xaml
    /// </summary>
    public partial class OutsoleDelayReportWindow : Window
    {
        BackgroundWorker bwLoadData;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<SizeRunModel> sizeRunList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OrdersModel> ordersList;
        public OutsoleDelayReportWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleMaterialList = new List<OutsoleMaterialModel>();
            sizeRunList = new List<SizeRunModel>();
            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            ordersList = new List<OrdersModel>();
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
            var regex = new Regex(@"[a-z]|[A-Z]");
            outsoleRawMaterialList = OutsoleRawMaterialController.Select();
            outsoleMaterialList = OutsoleMaterialController.SelectByOutsoleRawMaterial();
            sizeRunList = SizeRunController.SelectByOutsoleRawMaterial();
            outsoleSupplierList = OutsoleSuppliersController.Select();
            ordersList = OrdersController.SelectByOutsoleRawMaterial();

            DataTable dt = new OutsoleDelayDataSet().Tables["OutsoleDelayTable"];

            List<String> productNoList = outsoleRawMaterialList.Select(r => r.ProductNo).Distinct().ToList();
            foreach (string productNo in productNoList)
            {
                OrdersModel order = ordersList.Where(o => o.ProductNo == productNo).FirstOrDefault();
                List<OutsoleRawMaterialModel> outsoleRawMaterialList_D1 = outsoleRawMaterialList.Where(o => o.ProductNo == productNo).ToList();
                List<SizeRunModel> sizeRunList_D1 = sizeRunList.Where(s => s.ProductNo == productNo).OrderBy(o => Double.Parse(regex.IsMatch(o.SizeNo) ? regex.Replace(o.SizeNo, "") : o.SizeNo)).ToList();
                List<OutsoleMaterialModel> outsoleMaterialList_D1 = outsoleMaterialList.Where(o => o.ProductNo == productNo).ToList();
                foreach (OutsoleRawMaterialModel outsoleRawMaterial in outsoleRawMaterialList_D1)
                {
                    List<OutsoleMaterialModel> outsoleMaterialList_D2 = outsoleMaterialList_D1.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId).ToList();
                    OutsoleSuppliersModel outsoleSupplier = outsoleSupplierList.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId).FirstOrDefault();
                    DateTime etd = outsoleRawMaterial.ETD.Date;
                    //bool isFull = OutsoleRawMaterialController.IsFull(sizeRunList_D1, new List<OutsoleRawMaterialModel>() { outsoleRawMaterial }, outsoleMaterialList_D1);
                    //if (etd != new DateTime(2000, 1, 1) && etd < DateTime.Now.Date && isFull == false)
                    if (etd != new DateTime(2000, 1, 1) && etd < DateTime.Now.Date)
                    {
                        foreach (SizeRunModel sizeRun in sizeRunList_D1)
                        {
                            int qtyDelay = sizeRun.Quantity -
                                outsoleMaterialList_D2.Where(o => o.SizeNo == sizeRun.SizeNo).Sum(o => o.Quantity);
                            if (qtyDelay > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr["ProductNo"] = productNo;

                                string sizeNoString = regex.IsMatch(sizeRun.SizeNo) == true ? regex.Replace(sizeRun.SizeNo, "") : sizeRun.SizeNo;
                                double sizeNoDouble = 0;
                                Double.TryParse(sizeNoString, out sizeNoDouble);
                                dr["SizeNoDouble"] = sizeNoDouble;

                                dr["SizeNo"] = sizeRun.SizeNo;

                                dr["SupplierETD"] = outsoleRawMaterial.ETD;
                                if (order != null)
                                {
                                    dr["OutsoleCode"] = order.OutsoleCode;
                                    dr["ArticleNo"] = order.ArticleNo;
                                    dr["ETD"] = order.ETD;
                                }
                                if (outsoleSupplier != null)
                                {
                                    dr["OutsoleSupplier"] = outsoleSupplier.Name;
                                }
                                dr["QuantityDelay"] = qtyDelay;
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            e.Result = dt;
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            DataTable dt = e.Result as DataTable;
            Regex regex = new Regex(@"[^\d\.]");
            dt = dt.AsEnumerable().OrderBy(r => double.Parse(regex.Replace(r.Field<String>("SizeNo"), ""))).CopyToDataTable();
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OutsoleDelay";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleDelayReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\OutsoleDelayReport.rdlc";
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;
        }
    }
}
