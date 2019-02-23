using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;
using Microsoft.Reporting.WinForms;
using System.Data;

using MasterSchedule.Models;
using MasterSchedule.Controllers;
using MasterSchedule.Helpers;
using MasterSchedule.DataSets;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for LeadTimerPerStyleWindow.xaml
    /// </summary>
    public partial class LeadTimerPerStyleWindow : Window
    {
        string modePerStyle;
        List<string> pmList;
        List<string> articleList;
        BackgroundWorker bwLoad;
        BackgroundWorker bwSearch;
        List<AssemblyMasterModel> assemblyMasterList;
        List<SewingMasterModel> sewingMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<SockliningMasterModel> sockliningMasterList;
        List<OrdersModel> orderList;
        List<OffDayModel> offDayList;
        DateTime dtNothing;
        DateTime dtDefault;

        string[] phaseList = { "Sewing", "Socklining", "Outsole", "Assembly", "Cut A To Box", "Cut B To Box" };
        public LeadTimerPerStyleWindow(string _modePerStyle)
        {
            this.modePerStyle = _modePerStyle;
            pmList = new List<string>();
            articleList = new List<string>();
            sewingMasterList = new List<SewingMasterModel>();
            sockliningMasterList = new List<SockliningMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            orderList = new List<OrdersModel>();
            offDayList = new List<OffDayModel>();

            dtDefault = new DateTime(2000, 1, 1);
            dtNothing = new DateTime(1999, 12, 31);

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwSearch = new BackgroundWorker();
            bwSearch.DoWork += new DoWorkEventHandler(bwSearch_DoWork);
            bwSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSearch_RunWorkerCompleted);

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            bwLoad.RunWorkerAsync();
        }
        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            orderList = OrdersController.Select();
            offDayList = OffDayController.Select();
        }
        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pmList = orderList.OrderBy(o => o.PatternNo).Select(s => s.PatternNo).Distinct().ToList();
            articleList = orderList.OrderBy(o => o.ArticleNo).Select(s => s.ArticleNo.Contains("-") ? s.ArticleNo.Split('-')[0] : s.ArticleNo).Distinct().ToList();

            if (modePerStyle == "PM" && pmList.Count > 0)
            {
                cboPatternNo.Visibility = Visibility.Visible;
                cboPatternNo.ItemsSource = pmList;
                cboPatternNo.SelectedItem = pmList[0];
            }

            if (modePerStyle == "Article" && articleList.Count > 0)
            {
                cboArticleNo.Visibility = Visibility.Visible;
                tblChoose.Text = "Choose an Article No:";
                cboArticleNo.ItemsSource = articleList;
                cboArticleNo.SelectedItem = articleList[0];
            }
            this.Cursor = null;
        }

        string pm = "", articleNo = "";
        string reportTitle = "";
        private void btnViewChart_Click(object sender, RoutedEventArgs e)
        {
            if (bwSearch.IsBusy == true)
            { 
                return;
            }
            btnViewChart.IsEnabled = false;
            this.Cursor = Cursors.Wait;

            pm = cboPatternNo.Text;
            articleNo = cboArticleNo.Text;

            bwSearch.RunWorkerAsync();
        }
        private void bwSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            if (modePerStyle == "PM")
            {
                reportTitle = pm;
                orderList = OrdersController.Select().Where(w => w.PatternNo == pm).ToList();
            }
            if (modePerStyle == "Article")
            {
                reportTitle = "Article: " + articleNo;
                orderList = OrdersController.Select().Where(w => w.ArticleNo.Contains(articleNo)).ToList();
            }
            sewingMasterList = SewingMasterController.Select();
            sockliningMasterList = SockliningMasterController.Select();
            outsoleMasterList = OutsoleMasterController.Select();
            assemblyMasterList = AssemblyMasterController.Select();

            DataTable dt = new LeadTimePerStyleDataSet().Tables["LeadTimePerStyleTable"];
            List<string> productNoList = orderList.Select(s => s.ProductNo).Distinct().ToList();

            double timeAverage = 0;
            foreach (string phase in phaseList)
            {
                int qtyXDay = 0, qtyQuota = 0;
                timeAverage = 0;
                // Sewing
                if (phase == phaseList[0])
                {
                    foreach (string productNo in productNoList)
                    {
                        var sewingModel = sewingMasterList.Where(w => w.ProductNo == productNo &&
                                                                        TimeHelper.Convert(w.SewingActualStartDateAuto) != dtNothing &&
                                                                        TimeHelper.Convert(w.SewingActualStartDateAuto) != dtDefault &&
                                                                        TimeHelper.Convert(w.SewingActualFinishDateAuto) != dtNothing &&
                                                                        TimeHelper.Convert(w.SewingActualFinishDateAuto) != dtDefault).FirstOrDefault();
                        if (sewingModel != null)
                        {
                            DateTime sewingActualStartDate = TimeHelper.Convert(sewingModel.SewingActualStartDateAuto);
                            DateTime sewingActualFinishDate = TimeHelper.Convert(sewingModel.SewingActualFinishDateAuto);
                            qtyXDay += CalculateDateRange(sewingActualStartDate, sewingActualFinishDate) * sewingModel.SewingQuota;
                            qtyQuota += sewingModel.SewingQuota;
                        }
                    }
                    timeAverage = (double)qtyXDay / (double)qtyQuota;
                }

                // Socklining
                if (phase == phaseList[1])
                {
                    timeAverage = 0;
                    foreach (string productNo in productNoList)
                    {
                        var sockliningModel = sockliningMasterList.Where(w => w.ProductNo == productNo &&
                                                                        TimeHelper.Convert(w.SockliningActualStartDate) != dtNothing &&
                                                                        TimeHelper.Convert(w.SockliningActualStartDate) != dtDefault &&
                                                                        TimeHelper.Convert(w.SockliningActualFinishDate) != dtNothing &&
                                                                        TimeHelper.Convert(w.SockliningActualFinishDate) != dtDefault).FirstOrDefault();
                        if (sockliningModel != null)
                        {
                            DateTime sockliningActualStartDate = TimeHelper.Convert(sockliningModel.SockliningActualStartDate);
                            DateTime sockliningActualFinishDate = TimeHelper.Convert(sockliningModel.SockliningActualFinishDate);
                            qtyXDay += CalculateDateRange(sockliningActualStartDate, sockliningActualFinishDate) * sockliningModel.SockliningQuota;
                            qtyQuota += sockliningModel.SockliningQuota;
                        }
                    }
                    timeAverage = (double)qtyXDay / (double)qtyQuota;
                }


                // Outsole
                if (phase == phaseList[2])
                {
                    timeAverage = 0;
                    foreach (string productNo in productNoList)
                    {
                        var outsoleModel = outsoleMasterList.Where(w => w.ProductNo == productNo &&
                                                                        TimeHelper.Convert(w.OutsoleActualStartDateAuto) != dtNothing &&
                                                                        TimeHelper.Convert(w.OutsoleActualStartDateAuto) != dtDefault &&
                                                                        TimeHelper.Convert(w.OutsoleActualFinishDateAuto) != dtNothing &&
                                                                        TimeHelper.Convert(w.OutsoleActualFinishDateAuto) != dtDefault).FirstOrDefault();
                        if (outsoleModel != null)
                        {
                            DateTime outsoleActualStartDate = TimeHelper.Convert(outsoleModel.OutsoleActualStartDateAuto);
                            DateTime outsoleActualFinishDate = TimeHelper.Convert(outsoleModel.OutsoleActualFinishDateAuto);
                            qtyXDay += CalculateDateRange(outsoleActualStartDate, outsoleActualFinishDate) * outsoleModel.OutsoleQuota;
                            qtyQuota += outsoleModel.OutsoleQuota;
                        }
                    }
                    timeAverage = (double)qtyXDay / (double)qtyQuota;
                }

                // Assembly
                if (phase == phaseList[3])
                {
                    timeAverage = 0;
                    foreach (string productNo in productNoList)
                    {
                        var assemblyModel = assemblyMasterList.Where(w => w.ProductNo == productNo &&
                                                                        TimeHelper.Convert(w.AssemblyActualStartDate) != dtNothing &&
                                                                        TimeHelper.Convert(w.AssemblyActualStartDate) != dtDefault &&
                                                                        TimeHelper.Convert(w.AssemblyActualFinishDate) != dtNothing &&
                                                                        TimeHelper.Convert(w.AssemblyActualFinishDate) != dtDefault).FirstOrDefault();
                        if (assemblyModel != null)
                        {
                            DateTime assemblyActualStartDate = TimeHelper.Convert(assemblyModel.AssemblyActualStartDate);
                            DateTime assemblyActualFinishDate = TimeHelper.Convert(assemblyModel.AssemblyActualFinishDate);
                            qtyXDay += CalculateDateRange(assemblyActualStartDate, assemblyActualFinishDate) * assemblyModel.AssemblyQuota;
                            qtyQuota += assemblyModel.AssemblyQuota;
                        }
                    }
                    timeAverage = (double)qtyXDay / (double)qtyQuota;
                }

                // Cut A To Box
                if (phase == phaseList[4])
                {
                    timeAverage = 0;
                    foreach (string productNo in productNoList)
                    {
                        var sewingModel = sewingMasterList.Where(w => w.ProductNo == productNo &&
                                                                        TimeHelper.Convert(w.CutAActualStartDate) != dtNothing &&
                                                                        TimeHelper.Convert(w.CutAActualStartDate) != dtDefault).FirstOrDefault();

                        var assemblyModel = assemblyMasterList.Where(w => w.ProductNo == productNo &&
                                                                        TimeHelper.Convert(w.AssemblyActualFinishDate) != dtNothing &&
                                                                        TimeHelper.Convert(w.AssemblyActualFinishDate) != dtDefault).FirstOrDefault();

                        if (sewingModel != null && assemblyModel != null)
                        {
                            DateTime cutAActualStartDate = TimeHelper.Convert(sewingModel.CutAActualStartDate);
                            DateTime sewingActualFinishDate = TimeHelper.Convert(sewingModel.SewingActualFinishDateAuto);
                            DateTime assyActualStartDate = TimeHelper.Convert(assemblyModel.AssemblyActualStartDate);
                            DateTime assyActualFinishDate = TimeHelper.Convert(assemblyModel.AssemblyActualFinishDate);

                            int timeRange = 0;
                            if (assyActualStartDate != dtDefault && assyActualStartDate != dtNothing && sewingActualFinishDate != dtDefault && sewingActualFinishDate != dtNothing && assyActualStartDate > sewingActualFinishDate)
                            {
                                //timeRange = TimeHelper.CalculateDate(sewingActualFinishDate, assyActualStartDate);
                                timeRange = (Int32)((assyActualStartDate - sewingActualFinishDate).TotalDays);
                                qtyXDay += (CalculateDateRange(cutAActualStartDate, assyActualFinishDate) - timeRange) * sewingModel.CutAQuota;
                                qtyQuota += sewingModel.CutAQuota;
                            }
                            else
                            {
                                qtyXDay += CalculateDateRange(cutAActualStartDate, assyActualFinishDate) * sewingModel.CutAQuota;
                                qtyQuota += sewingModel.CutAQuota;
                            }
                        }
                    }
                    timeAverage = (double)qtyXDay / (double)qtyQuota;
                }

                // Cut B To Box
                if (phase == phaseList[4])
                {
                    timeAverage = 0;
                    foreach (string productNo in productNoList)
                    {
                        var sewingModel = sewingMasterList.Where(w => w.ProductNo == productNo &&
                                                                        TimeHelper.Convert(w.CutBActualStartDate) != dtNothing &&
                                                                        TimeHelper.Convert(w.CutBActualStartDate) != dtDefault).FirstOrDefault();

                        var assemblyModel = assemblyMasterList.Where(w => w.ProductNo == productNo &&
                                                                        TimeHelper.Convert(w.AssemblyActualFinishDate) != dtNothing &&
                                                                        TimeHelper.Convert(w.AssemblyActualFinishDate) != dtDefault).FirstOrDefault();

                        if (sewingModel != null && assemblyModel != null)
                        {
                            DateTime cutBActualStartDate = TimeHelper.Convert(sewingModel.CutBActualStartDate);
                            DateTime sewingActualFinishDate = TimeHelper.Convert(sewingModel.SewingActualFinishDateAuto);
                            DateTime assyActualStartDate = TimeHelper.Convert(assemblyModel.AssemblyActualStartDate);
                            DateTime assyActualFinishDate = TimeHelper.Convert(assemblyModel.AssemblyActualFinishDate);

                            int timeRange = 0;
                            if (assyActualStartDate != dtDefault && assyActualStartDate != dtNothing && sewingActualFinishDate != dtDefault && sewingActualFinishDate != dtNothing && assyActualStartDate > sewingActualFinishDate)
                            {
                                //timeRange = TimeHelper.CalculateDate(sewingActualFinishDate, assyActualStartDate);
                                timeRange = (Int32)((assyActualStartDate - sewingActualFinishDate).TotalDays);
                                qtyXDay += (CalculateDateRange(cutBActualStartDate, assyActualFinishDate) - timeRange) * sewingModel.CutAQuota;
                                qtyQuota += sewingModel.CutAQuota;
                            }
                            else
                            {
                                qtyXDay += CalculateDateRange(cutBActualStartDate, assyActualFinishDate) * sewingModel.CutAQuota;
                                qtyQuota += sewingModel.CutAQuota;
                            }
                        }
                    }
                    timeAverage = (double)qtyXDay / (double)qtyQuota;
                }

                string timeAverageString = "";
                if (timeAverage > 0)
                {
                    timeAverageString = string.Format("{0:0.00}", timeAverage);
                }

                DataRow dr = dt.NewRow();
                dr["Phase"] = phase;
                dr["TimeAverage"] = timeAverageString;
                dt.Rows.Add(dr);
            }

            e.Result = dt;
        }
        private void bwSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            DataTable dt = e.Result as DataTable;

            ReportParameter rp = new ReportParameter("Style", reportTitle);
            ReportDataSource rds = new ReportDataSource("LeadTimePerStyle", dt);
            //reportViewer.LocalReport.ReportPath = @"E:\SV PROJECT\MS\1.1.7.1\Saoviet Master Schedule Solution\MasterSchedule\Reports\LeadTimePerStyleReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\LeadTimePerStyleReport.rdlc";
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();

            btnViewChart.IsEnabled = true;
            this.Cursor = null;
        }
        private int CalculateDateRange(DateTime actualStartDate, DateTime actualFinishDate)
        {
            //int dateRange = TimeHelper.CalculateDate(actualStartDate, actualFinishDate);
            int dateRange = (Int32)((actualFinishDate - actualStartDate).TotalDays);
            List<DateTime> offList = offDayList.Select(s => s.Date).Distinct().ToList();
            for (DateTime date = actualStartDate.Date; date <= actualFinishDate.Date; date = date.AddDays(1))
            {
                if (offList.Contains(date) == true)
                {
                    dateRange -= 1;
                }
            }
            return dateRange;
        }
    }
}
