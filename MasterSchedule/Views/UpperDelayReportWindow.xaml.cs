using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    public partial class UpperDelayReportWindow : Window
    {
        BackgroundWorker bwLoadData;
        List<MaterialTypeModel> materialTypeList;
        List<OrdersModel> orderList;
        List<RawMaterialModel> rawMaterialList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<SewingMasterModel> sewingMasterList;
        public UpperDelayReportWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            materialTypeList = new List<MaterialTypeModel>();
            orderList = new List<OrdersModel>();
            rawMaterialList = new List<RawMaterialModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            InitializeComponent();
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            materialTypeList = MaterialTypeController.Select();
            orderList = OrdersController.Select();
            rawMaterialList = RawMaterialController.Select();
            sewingMasterList = SewingMasterController.SelectByProductNo();

            DataTable dt = new DelayDataSet().Tables["DelayTable"];

            List<String> productNoList = rawMaterialList.Select(r => r.ProductNo).Distinct().ToList();
            foreach (string productNo in productNoList)
            {
                OrdersModel order = orderList.Where(o => o.ProductNo == productNo).FirstOrDefault();
                List<Int32> materialTypeIdList = rawMaterialList.Select(r => r.MaterialTypeId).Distinct().ToList();
                // get assemblyline from productNo
                //AssemblyMasterModel assemblyMaster = assemblyMasterList.Where(o => o.ProductNo == productNo).FirstOrDefault();

                // get sewingLing from productNo
                SewingMasterModel sewingMaster = sewingMasterList.Where(o => o.ProductNo == productNo).FirstOrDefault();
                materialTypeIdList.Remove(6);
                materialTypeIdList.Remove(11);
                materialTypeIdList.Remove(12);
                materialTypeIdList.Remove(13);

                foreach (int materialTypeId in materialTypeIdList)
                {
                    RawMaterialModel rawMaterial = rawMaterialList.Where(r => r.ProductNo == productNo && r.MaterialTypeId == materialTypeId).FirstOrDefault();
                    if (rawMaterial != null && rawMaterial.ETD.Date != new DateTime(2000, 1, 1) && rawMaterial.ETD.Date < DateTime.Now.Date
                        && rawMaterial.ActualDate.Date == new DateTime(2000, 1, 1))
                    {
                        DataRow dr = dt.NewRow();
                        // binding data to reportviewer
                        dr["SewingLine"] = sewingMaster.SewingLine;
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

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            DataTable dt = e.Result as DataTable;

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DelayDataSetDetail";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\DelayReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\DelayReport.rdlc";
            reportViewer.LocalReport.DataSources.Clear();
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
