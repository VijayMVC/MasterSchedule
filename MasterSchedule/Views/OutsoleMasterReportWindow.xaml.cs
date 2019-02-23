using System.Collections.Generic;
using System.Windows;

using Microsoft.Reporting.WinForms;
using System.Data;
using MasterSchedule.DataSets;
using MasterSchedule.ViewModels;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for DelayReportWindow.xaml
    /// </summary>
    public partial class OutsoleMasterReportWindow : Window
    {
        List<OutsoleMasterExportViewModel> outsoleMasterExportViewList;
        string line;
        public OutsoleMasterReportWindow(List<OutsoleMasterExportViewModel> outsoleMasterExportViewList, string line)
        {
            this.outsoleMasterExportViewList = outsoleMasterExportViewList;
            this.line = line;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = new OutsoleMasterDataSet().Tables["OutsoleMasterTable"];

            foreach (OutsoleMasterExportViewModel outsoleMasterExportView in outsoleMasterExportViewList)
            {
                DataRow dr = dt.NewRow();
                dr["Sequence"] = outsoleMasterExportView.Sequence;
                dr["ProductNo"] = outsoleMasterExportView.ProductNo;
                dr["Country"] = outsoleMasterExportView.Country;
                dr["ShoeName"] = outsoleMasterExportView.ShoeName;
                dr["ArticleNo"] = outsoleMasterExportView.ArticleNo;
                dr["OutsoleCode"] = outsoleMasterExportView.OutsoleCode;
                dr["Quantity"] = outsoleMasterExportView.Quantity;
                dr["ETD"] = outsoleMasterExportView.ETD;
                dr["OutsoleLine"] = outsoleMasterExportView.OutsoleLine;
                dr["SewingStartDate"] = outsoleMasterExportView.SewingStartDate;
                dr["SewingFinishDate"] = outsoleMasterExportView.SewingFinishDate;
                dr["OutsoleMatsArrival"] = outsoleMasterExportView.OutsoleMatsArrival;
                dr["OutsoleWHBalance"] = outsoleMasterExportView.OutsoleWHBalance;
                dr["OutsoleStartDate"] = outsoleMasterExportView.OutsoleStartDate;
                dr["OutsoleFinishDate"] = outsoleMasterExportView.OutsoleFinishDate;
                dr["SewingQuota"] = outsoleMasterExportView.SewingQuota;
                dr["OutsoleQuota"] = outsoleMasterExportView.OutsoleQuota;
                dr["SewingBalance"] = outsoleMasterExportView.SewingBalance;
                dr["OutsoleBalance"] = outsoleMasterExportView.OutsoleBalance;
                dr["ReleasedQuantity"] = outsoleMasterExportView.ReleasedQuantity;
                dr["IsOutsoleMatsArrivalOk"] = outsoleMasterExportView.IsOutsoleMatsArrivalOk;
                dr["IsHaveMemo"] = !string.IsNullOrEmpty(outsoleMasterExportView.MemoId);

                dt.Rows.Add(dr);
            }

            ReportParameter rp = new ReportParameter("Line", line);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "OutsoleMaster";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleMasterReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\OutsoleMasterReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;
        }
    }
}
