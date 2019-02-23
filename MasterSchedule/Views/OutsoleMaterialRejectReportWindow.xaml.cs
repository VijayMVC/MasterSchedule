using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
    public partial class OutsoleMaterialRejectReportWindow : Window
    {
        BackgroundWorker bwLoadData;
        List<OutsoleMaterialModel> outsoleMaterialRejectList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<OrdersModel> orderList;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        public OutsoleMaterialRejectReportWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            outsoleMaterialRejectList = new List<OutsoleMaterialModel>();
            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            orderList = new List<OrdersModel>();
            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            InitializeComponent();
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMaterialRejectList = OutsoleMaterialController.SelectReject();
            outsoleSupplierList = OutsoleSuppliersController.Select();
            orderList = OrdersController.SelectByOutsoleMaterialReject();
            outsoleRawMaterialList = OutsoleRawMaterialController.Select();
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = new OutsoleMaterialRejectDataSet().Tables["OutsoleMaterialRejectTable"];
            var regex = new Regex(@"[a-z]|[A-Z]");
            foreach (OutsoleMaterialModel outsoleMaterialReject in outsoleMaterialRejectList)
            {
                OutsoleSuppliersModel outsoleSupplier = outsoleSupplierList.Where(o => o.OutsoleSupplierId == outsoleMaterialReject.OutsoleSupplierId).FirstOrDefault();
                OrdersModel order = orderList.Where(o => o.ProductNo == outsoleMaterialReject.ProductNo).FirstOrDefault();
                OutsoleRawMaterialModel outsoleRawMaterial = outsoleRawMaterialList.Where(o => o.ProductNo == outsoleMaterialReject.ProductNo && o.OutsoleSupplierId == outsoleMaterialReject.OutsoleSupplierId).FirstOrDefault();
                DataRow dr = dt.NewRow();
                dr["ProductNo"] = outsoleMaterialReject.ProductNo;
                if (order != null)
                {
                    dr["OutsoleCode"] = order.OutsoleCode;
                    dr["ETD"] = order.ETD;
                }
                if (outsoleRawMaterial != null)
                {
                    dr["SupplierETD"] = outsoleRawMaterial.ETD;
                }
                if (outsoleSupplier != null)
                {
                    dr["OutsoleSupplier"] = outsoleSupplier.Name;
                }
                string sizeNoString = regex.IsMatch(outsoleMaterialReject.SizeNo) == true ? regex.Replace(outsoleMaterialReject.SizeNo, "") : outsoleMaterialReject.SizeNo;
                double sizeNoDouble = 0;
                Double.TryParse(sizeNoString, out sizeNoDouble);
                dr["SizeNoDouble"] = sizeNoDouble;

                dr["SizeNo"] = outsoleMaterialReject.SizeNo;

                dr["QuantityReject"] = outsoleMaterialReject.QuantityReject;
                dt.Rows.Add(dr);
            }

            // bo 3.5 thanh 35 roi orderby theo sizeno
            //Regex regex = new Regex(@"[^\d\.]");
            //dt = dt.AsEnumerable().OrderBy(r => double.Parse(regex.Replace(r.Field<String>("SizeNo"), ""))).CopyToDataTable();

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OutsoleMaterialReject";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleMaterialRejectReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\OutsoleMaterialRejectReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }
        }
    }
}
