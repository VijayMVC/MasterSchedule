using System.Windows;
using System.Data;
using Microsoft.Reporting.WinForms;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleReleaseMaterialPrintWindow.xaml
    /// </summary>
    public partial class AssemblyReleaseReportWindow : Window
    {
        string reportId;
        DataTable dt;
        public AssemblyReleaseReportWindow(string reportId, DataTable dt)
        {
            this.reportId = reportId;
            this.dt = dt;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReportParameter rp = new ReportParameter("ReportId", reportId);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "AssemblyRelease";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\AssemblyReleaseReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\AssemblyReleaseReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });            
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
        }

    }
}
