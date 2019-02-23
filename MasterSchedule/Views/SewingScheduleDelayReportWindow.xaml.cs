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
    public partial class SewingScheduleDelayReportWindow : Window
    {
        BackgroundWorker bwLoad;
        List<OrdersModel> orderList;
        List<SewingMasterModel> sewingMasterList;
        public SewingScheduleDelayReportWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            orderList = new List<OrdersModel>();
            sewingMasterList = new List<SewingMasterModel>();
            InitializeComponent();
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            orderList = OrdersController.Select();
            sewingMasterList = SewingMasterController.Select();
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = new SewingScheduleDelayDataSet().Tables["SewingScheduleDelayTable"];

            //sewingMasterList.RemoveAll(s => DateTimeHelper.Create(s.SewingBalance) != dtDefault && DateTimeHelper.Create(s.SewingBalance) != dtNothing);
            sewingMasterList = sewingMasterList.OrderBy(s => s.Sequence).ToList();

            foreach (SewingMasterModel sewingMaster in sewingMasterList)
            {
                OrdersModel order = orderList.Where(o => o.ProductNo == sewingMaster.ProductNo).FirstOrDefault();
                int qtyBalance = 0;
                if (order != null && sewingMaster.SewingFinishDate.AddDays(3) > order.ETD
                    && (String.IsNullOrEmpty(sewingMaster.SewingBalance) == true || int.TryParse(sewingMaster.SewingBalance, out qtyBalance) == true))
                {
                    DataRow dr = dt.NewRow();
                    dr["ProductNo"] = order.ProductNo;
                    dr["Country"] = order.Country;
                    dr["ShoeName"] = order.ShoeName;
                    dr["ArticleNo"] = order.ArticleNo;
                    dr["ETD"] = order.ETD;
                    dr["Quantity"] = order.Quantity;
                    dr["SewingLine"] = sewingMaster.SewingLine;
                    dr["SewingFinishDate"] = sewingMaster.SewingFinishDate;
                    dr["SewingBalance"] = sewingMaster.SewingBalance;

                    dt.Rows.Add(dr);
                }
            }
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "SewingScheduleDelay";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\SewingScheduleDelayReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\SewingScheduleDelayReport.rdlc";
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
