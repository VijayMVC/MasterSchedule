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
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for DelayReportWindow.xaml
    /// </summary>
    public partial class UpperDeliveryReportWindow : Window
    {
        BackgroundWorker bwLoadData;
        BackgroundWorker bwReport;
        List<MaterialTypeModel> materialTypeList;
        List<OrdersModel> orderList;
        List<RawMaterialModel> rawMaterialList;
        public UpperDeliveryReportWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            bwReport = new BackgroundWorker();
            bwReport.DoWork +=new DoWorkEventHandler(bwReport_DoWork);
            bwReport.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwReport_RunWorkerCompleted);

            materialTypeList = new List<MaterialTypeModel>();
            orderList = new List<OrdersModel>();
            rawMaterialList = new List<RawMaterialModel>();
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpDate.SelectedDate = DateTime.Now;
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {            
            materialTypeList = MaterialTypeController.Select();
            orderList = OrdersController.Select();
            rawMaterialList = RawMaterialController.Select();
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnReport.IsEnabled = true;
            this.Cursor = null;
        }


        DateTime dateSearch = new DateTime(2000, 1, 1);
        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            if (bwReport.IsBusy == false)
            {
                dateSearch = dpDate.SelectedDate.Value;
                this.Cursor = Cursors.Wait;
                btnReport.IsEnabled = false;
                bwReport.RunWorkerAsync();
            }
        }

        private void bwReport_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dt = new DeliveryDataSet().Tables["DeliveryTable"];
            List<String> productNoList = rawMaterialList.Select(r => r.ProductNo).Distinct().ToList();
            foreach (string productNo in productNoList)
            {
                OrdersModel order = orderList.Where(o => o.ProductNo == productNo).FirstOrDefault();
                List<Int32> materialTypeIdList = rawMaterialList.Select(r => r.MaterialTypeId).Distinct().ToList();

                //Red!
                materialTypeIdList.Remove(11);
                materialTypeIdList.Remove(6);
                materialTypeIdList.Remove(12);
                materialTypeIdList.Remove(13);

                foreach (int materialTypeId in materialTypeIdList)
                {
                    RawMaterialModel rawMaterial = rawMaterialList.Where(r => r.ProductNo == productNo && r.MaterialTypeId == materialTypeId).FirstOrDefault();
                    if (rawMaterial != null && rawMaterial.ETD.Date != new DateTime(2000, 1, 1) && rawMaterial.ETD.Date == dateSearch.Date
                        && rawMaterial.ActualDate.Date == new DateTime(2000, 1, 1))
                    {
                        DataRow dr = dt.NewRow();
                        dr["ProductNo"] = productNo;
                        if (order != null)
                        {
                            dr["ArticleNo"] = order.ArticleNo;
                            dr["ShoeName"] = order.ShoeName;
                            dr["Quantity"] = order.Quantity;
                            dr["ETD"] = order.ETD;
                        }
                        dr["SupplierETD"] = rawMaterial.ETD;
                        dr["Remarks"] = rawMaterial.Remarks;
                        MaterialTypeModel materialType = materialTypeList.Where(m => m.MaterialTypeId == rawMaterial.MaterialTypeId).FirstOrDefault();
                        if (materialType != null)
                        {
                            dr["Supplier"] = materialType.Name;
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            e.Result = dt;
        }
        private void bwReport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            DataTable dt = e.Result as DataTable;
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "Delivery";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\documents\visual studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\DeliveryReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\DeliveryReport.rdlc";
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();

            this.Cursor = null;
            btnReport.IsEnabled = true;
        }
    }
}
