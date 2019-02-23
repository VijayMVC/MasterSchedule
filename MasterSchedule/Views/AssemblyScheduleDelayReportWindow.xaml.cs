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
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for DelayReportWindow.xaml
    /// </summary>
    public partial class AssemblyScheduleDelayReportWindow : Window
    {
        BackgroundWorker bwLoad;
        List<OrdersModel> orderList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<SewingMasterModel> sewingMasterList;
        public AssemblyScheduleDelayReportWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            orderList = new List<OrdersModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            sewingMasterList = new List<SewingMasterModel>();
            InitializeComponent();
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            orderList = OrdersController.Select();            
            assemblyMasterList = AssemblyMasterController.Select();
            sewingMasterList = SewingMasterController.Select();
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = new AssemblyScheduleDelayDataSet().Tables["AssemblyScheduleDelayTable"];

            //sewingMasterList.RemoveAll(s => DateTimeHelper.Create(s.SewingBalance) != dtDefault && DateTimeHelper.Create(s.SewingBalance) != dtNothing);
            assemblyMasterList = assemblyMasterList.OrderBy(s => s.Sequence).ToList();

            foreach (AssemblyMasterModel assemblyMaster in assemblyMasterList)
            {
                OrdersModel order = orderList.Where(o => o.ProductNo == assemblyMaster.ProductNo).FirstOrDefault();
                int qtyBalance = 0;
                if (order != null && assemblyMaster.AssemblyFinishDate > order.ETD
                    && (String.IsNullOrEmpty(assemblyMaster.AssemblyBalance) == true || int.TryParse(assemblyMaster.AssemblyBalance, out qtyBalance) == true))
                {
                    DataRow dr = dt.NewRow();
                    dr["ProductNo"] = order.ProductNo;
                    dr["Country"] = order.Country;
                    dr["ShoeName"] = order.ShoeName;
                    dr["ArticleNo"] = order.ArticleNo;
                    dr["ETD"] = order.ETD;
                    dr["Quantity"] = order.Quantity;
                    dr["AssemblyLine"] = assemblyMaster.AssemblyLine;
                    dr["AssemblyBalance"] = assemblyMaster.AssemblyBalance;

                    SewingMasterModel sewingMaster = sewingMasterList.Where(s => s.ProductNo == order.ProductNo).FirstOrDefault();
                    if (sewingMaster != null)
                    {
                        dr["SewingFinishDate"] = sewingMaster.SewingFinishDate;
                    }

                    dr["AssemblyFinishDate"] = assemblyMaster.AssemblyFinishDate;

                    dt.Rows.Add(dr);
                }
            }            

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "AssemblyScheduleDelay";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\AssemblyScheduleDelayReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\AssemblyScheduleDelayReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;                
                bwLoad.RunWorkerAsync();
            }
        }
    }
}
