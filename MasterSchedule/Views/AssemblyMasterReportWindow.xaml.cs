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
    public partial class AssemblyMasterReportWindow : Window
    {
        List<AssemblyMasterExportViewModel> assemblyMasterExportViewList;
        string line;
        public AssemblyMasterReportWindow(List<AssemblyMasterExportViewModel> assemblyMasterExportViewList, string line)
        {
            this.assemblyMasterExportViewList = assemblyMasterExportViewList;
            this.line = line;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = new AssemblyMasterDataSet().Tables["AssemblyMasterTable"];

            foreach (AssemblyMasterExportViewModel assemblyMasterExportView in assemblyMasterExportViewList)
            {
                DataRow dr = dt.NewRow();
                dr["Sequence"] = assemblyMasterExportView.Sequence;
                dr["ProductNo"] = assemblyMasterExportView.ProductNo;
                dr["Country"] = assemblyMasterExportView.Country;
                dr["ShoeName"] = assemblyMasterExportView.ShoeName;
                dr["ArticleNo"] = assemblyMasterExportView.ArticleNo;
                dr["LastCode"] = assemblyMasterExportView.LastCode;
                dr["Quantity"] = assemblyMasterExportView.Quantity;
                dr["ETD"] = assemblyMasterExportView.ETD;
                dr["AssemblyLine"] = assemblyMasterExportView.AssemblyLine;
                dr["SewingStartDate"] = assemblyMasterExportView.SewingStartDate;
                dr["SewingFinishDate"] = assemblyMasterExportView.SewingFinishDate;
                dr["OutsoleMatsArrival"] = assemblyMasterExportView.OutsoleMatsArrival;
                dr["AssemblyMatsArrival"] = assemblyMasterExportView.AssemblyMatsArrival;
                dr["CartonMatsArrival"] = assemblyMasterExportView.CartonMatsArrival;
                dr["AssemblyStartDate"] = assemblyMasterExportView.AssemblyStartDate;
                dr["AssemblyFinishDate"] = assemblyMasterExportView.AssemblyFinishDate;
                dr["AssemblyQuota"] = assemblyMasterExportView.AssemblyQuota;
                dr["SewingBalance"] = assemblyMasterExportView.SewingBalance;
                dr["OutsoleBalance"] = assemblyMasterExportView.OutsoleBalance;
                dr["InsoleBalance"] = assemblyMasterExportView.InsoleBalance;
                dr["InsockBalance"] = assemblyMasterExportView.InsockBalance;
                dr["AssemblyBalance"] = assemblyMasterExportView.AssemblyBalance;
                dr["IsAssemblyMatsArrivalOk"] = assemblyMasterExportView.IsAssemblyMatsArrivalOk;
                dr["IsOutsoleMatsArrivalOk"] = assemblyMasterExportView.IsOutsoleMatsArrivalOk;
                dr["IsHaveMemo"] = !string.IsNullOrEmpty(assemblyMasterExportView.MemoId);

                dt.Rows.Add(dr);
            }

            ReportParameter rp = new ReportParameter("Line", line);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "AssemblyMaster";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\AssemblyMasterReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\AssemblyMasterReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;
        }
    }
}
