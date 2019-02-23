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
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WinForms;
using MasterSchedule.DataSets;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleBalanceReportWindow.xaml
    /// </summary>
    public partial class OutsoleOutputBalanceReportWindow : Window
    {
        BackgroundWorker bwLoad;
        DataTable dt; 
        List<String> sizeNoList;
        public OutsoleOutputBalanceReportWindow(DataTable dt, List<String> sizeNoList)
        {
            this.dt = dt;
            this.sizeNoList = sizeNoList;
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            var regex = new Regex("[a-z]|[A-Z]");

            DataTable dtReport = new OutsoleOutputBalanceDataSet().Tables["OutsoleOutputBalanceTable"];
            Dispatcher.Invoke(new Action(() =>
            {
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i <= sizeNoList.Count - 1; i++)
                    {
                        DataRow drReport = dtReport.NewRow();
                        drReport["ProductNo"] = dr["ProductNo"];
                        drReport["Country"] = dr["Country"];
                        drReport["Style"] = dr["Style"];
                        drReport["ArticleNo"] = dr["ArticleNo"];
                        drReport["ETD"] = dr["ETD"];
                        drReport["OutsoleLine"] = dr["OutsoleLine"];

                        string sizeNoString = regex.IsMatch(sizeNoList[i]) == true ? regex.Replace(sizeNoList[i], "100") : sizeNoList[i];
                        double sizeNoDouble = 0;
                        Double.TryParse(sizeNoString, out sizeNoDouble);

                        drReport["SizeNo"] = sizeNoList[i];
                        drReport["SizeNoDouble"] = sizeNoDouble;
                        drReport["Quantity"] = dr[String.Format("Column{0}", i)];

                        dtReport.Rows.Add(drReport);
                    }
                }

                ReportDataSource rds = new ReportDataSource();
                rds.Name = "OutsoleOutputBalance_Detail";
                rds.Value = dtReport;

                reportViewer.LocalReport.ReportPath = @"E:\SV PROJECT\MS\1.2.0.6\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleOutputBalanceReport.rdlc";
                //reportViewer.LocalReport.ReportPath = @"Reports\OutsoleOutputBalanceReport.rdlc";
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(rds);
                reportViewer.RefreshReport();

            }));
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
        }

    }
}
