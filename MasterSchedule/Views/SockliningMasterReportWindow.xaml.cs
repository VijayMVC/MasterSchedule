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
    public partial class SockliningMasterReportWindow : Window
    {
        List<SockliningMasterExportViewModel> sockliningMasterExportViewList;
        string line;
        public SockliningMasterReportWindow(List<SockliningMasterExportViewModel> sockliningMasterExportViewList, string line)
        {
            this.sockliningMasterExportViewList = sockliningMasterExportViewList;
            this.line = line;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = new SockliningMasterDataSet().Tables["SockliningMasterTable"];

            foreach (SockliningMasterExportViewModel sockliningMasterExportView in sockliningMasterExportViewList)
            {
                DataRow dr = dt.NewRow();
                dr["Sequence"] = sockliningMasterExportView.Sequence;
                dr["ProductNo"] = sockliningMasterExportView.ProductNo;
                dr["Country"] = sockliningMasterExportView.Country;
                dr["ShoeName"] = sockliningMasterExportView.ShoeName;
                dr["ArticleNo"] = sockliningMasterExportView.ArticleNo;
                dr["PatternNo"] = sockliningMasterExportView.PatternNo;
                dr["Quantity"] = sockliningMasterExportView.Quantity;
                dr["ETD"] = sockliningMasterExportView.ETD;
                dr["SockliningLine"] = sockliningMasterExportView.SockliningLine;
                dr["SockliningMatsArrival"] = sockliningMasterExportView.SockliningMatsArrival;
                dr["SewingStartDate"] = sockliningMasterExportView.SewingStartDate;
                dr["SewingBalance"] = sockliningMasterExportView.SewingBalance;
                dr["OutsoleStartDate"] = sockliningMasterExportView.OutsoleStartDate;
                dr["OutsoleBalance"] = sockliningMasterExportView.OutsoleBalance;
                dr["AssemblyStartDate"] = sockliningMasterExportView.AssemblyStartDate;
                dr["SockliningQuota"] = sockliningMasterExportView.SockliningQuota;
                dr["SockliningStartDate"] = sockliningMasterExportView.SockliningStartDate;
                dr["SockliningFinishDate"] = sockliningMasterExportView.SockliningFinishDate;
                dr["InsoleBalance"] = sockliningMasterExportView.InsoleBalance;
                dr["InsockBalance"] = sockliningMasterExportView.InsockBalance;
                dr["IsSockliningMatsArrivalOk"] = sockliningMasterExportView.IsSockliningMatsArrivalOk;
                dt.Rows.Add(dr);
            }

            ReportParameter rp = new ReportParameter("Line", line);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "SockliningMaster";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\SockliningMasterReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\SockliningMasterReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;
        }
    }
}
