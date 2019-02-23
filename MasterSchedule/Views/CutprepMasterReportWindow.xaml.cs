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
    public partial class CutprepMasterReportWindow : Window
    {
        List<CutprepMasterExportViewModel> cutprepMasterExportViewList;
        string line;
        public CutprepMasterReportWindow(List<CutprepMasterExportViewModel> cutprepMasterExportViewList, string line)
        {
            this.cutprepMasterExportViewList = cutprepMasterExportViewList;
            this.line = line;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = new CutprepMasterDataSet().Tables["CutprepMasterTable"];

            foreach (CutprepMasterExportViewModel cutprepMasterExportView in cutprepMasterExportViewList)
            {
                DataRow dr = dt.NewRow();
                dr["Sequence"] = cutprepMasterExportView.Sequence;
                dr["ProductNo"] = cutprepMasterExportView.ProductNo;
                dr["Country"] = cutprepMasterExportView.Country;
                dr["ShoeName"] = cutprepMasterExportView.ShoeName;
                dr["ArticleNo"] = cutprepMasterExportView.ArticleNo;
                dr["PatternNo"] = cutprepMasterExportView.PatternNo;
                dr["Quantity"] = cutprepMasterExportView.Quantity;
                dr["ETD"] = cutprepMasterExportView.ETD;
                dr["SewingLine"] = cutprepMasterExportView.SewingLine;
                dr["UpperMatsArrival"] = cutprepMasterExportView.UpperMatsArrival;
                dr["SewingStartDate"] = cutprepMasterExportView.SewingStartDate;
                dr["SewingQuota"] = cutprepMasterExportView.SewingQuota;
                dr["SewingBalance"] = cutprepMasterExportView.SewingBalance;
                dr["CutAStartDate"] = cutprepMasterExportView.CutAStartDate;
                dr["CutAFinishDate"] = cutprepMasterExportView.CutAFinishDate;
                dr["CutAQuota"] = cutprepMasterExportView.CutAQuota;
                dr["AutoCut"] = cutprepMasterExportView.AutoCut;
                dr["LaserCut"] = cutprepMasterExportView.LaserCut;
                dr["HuasenCut"] = cutprepMasterExportView.HuasenCut;
                dr["CutABalance"] = cutprepMasterExportView.CutABalance;
                dr["PrintingBalance"] = cutprepMasterExportView.PrintingBalance;
                dr["H_FBalance"] = cutprepMasterExportView.H_FBalance;
                dr["EmbroideryBalance"] = cutprepMasterExportView.EmbroideryBalance;
                dr["CutBBalance"] = cutprepMasterExportView.CutBBalance;
                dr["IsUpperMatsArrivalOk"] = cutprepMasterExportView.IsUpperMatsArrivalOk;
                dr["IsHaveMemo"] = !string.IsNullOrEmpty(cutprepMasterExportView.MemoId);

                dt.Rows.Add(dr);
            }

            ReportParameter rp = new ReportParameter("Line", line);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "CutprepMaster";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\CutprepMasterReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\CutprepMasterReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;
        }
    }
}
