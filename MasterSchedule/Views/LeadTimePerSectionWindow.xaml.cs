using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WinForms;

using MasterSchedule.Models;
using MasterSchedule.Controllers;
using MasterSchedule.Helpers;
using MasterSchedule.DataSets;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for LeadTimePerSectionWindow.xaml
    /// </summary>
    public partial class LeadTimePerSectionWindow : Window
    {
        List<OffDayModel> offDayList;
        BackgroundWorker threadLoad;
        List<SewingMasterModel> sewingMasterList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<SockliningMasterModel> sockliningMasterList;
        List<OrdersModel> ordersList;
        DateTime dtDefault;
        string chartTitle = "";
        string sewing = "S", assembly = "A", outsole = "O", socklining = "SL";
        string currentSection = "";
        DateTime dtNothing;
        string modeViewStatistics;
        string cutAB;
        string cuta, cutb;
        DataTable dt;
        public LeadTimePerSectionWindow(string _modeViewStatistics,string _cutAB)
        {
            InitializeComponent();

            threadLoad = new BackgroundWorker();
            threadLoad.DoWork += new DoWorkEventHandler(threadLoad_DoWork);
            threadLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadLoad_RunWorkerCompleted);

            sewingMasterList = new List<SewingMasterModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            sockliningMasterList = new List<SockliningMasterModel>();
            ordersList = new List<OrdersModel>();
            dtDefault = new DateTime(2000, 1, 1);
            dtNothing = new DateTime(1999, 12, 31);
            offDayList = new List<OffDayModel>();
            dt = new DataTable();
            this.modeViewStatistics = _modeViewStatistics;
            this.cutAB = _cutAB;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (modeViewStatistics == "1")
            {
                this.Title = "Master Schedule - Lead Time Per Section Statistics";
                cboSection.SelectedIndex = 0;
            }
            if (modeViewStatistics == "2" && cutAB == "Cut A")
            {
                cboSection.Visibility = Visibility.Collapsed;
                this.Title = "Master Schedule - Cut A Lead Time Statistics";
                cuta = cutAB;
            }
            if (modeViewStatistics == "2" && cutAB == "Cut B")
            {
                cboSection.Visibility = Visibility.Collapsed;
                this.Title = "Master Schedule - Cut B Lead Time Statistics";
                cutb = cutAB;
            }
            dpDateFrom.SelectedDate = DateTime.Now;
            dpDateTo.SelectedDate = DateTime.Now;
            offDayList = OffDayController.Select();
        }

        DateTime dateFrom, dateTo;
        private void btnCreateChart_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (threadLoad.IsBusy == true)
            {
                return;
            }

            dateFrom = dpDateFrom.SelectedDate.Value.Date;
            dateTo = dpDateTo.SelectedDate.Value.Date;

            string cbId = "";
            ComboBoxItem sectionItem = cboSection.SelectedItem as ComboBoxItem;
            if (modeViewStatistics == "1")
            {
                cbId = sectionItem.Name;
                chartTitle = sectionItem.Content as string;
            }
            if (modeViewStatistics == "2")
            {
                cbId = cutAB;
                chartTitle = cutAB;
            }
            currentSection = cbId;

            sewingMasterList.Clear();
            assemblyMasterList.Clear();

            btnCreateChart.IsEnabled = false;
            object[] args = { dateFrom, dateTo, cbId };
            threadLoad.RunWorkerAsync(args);
        }

        private void threadLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = e.Argument as object[];
            string sectionId = args[2] as string;
            if ((sectionId == sewing || sectionId == cuta || sectionId == cutb) && sewingMasterList.Count <= 0)
            {
                sewingMasterList = SewingMasterController.Select();
            }
            if (sectionId == assembly || sectionId == cuta || sectionId == cutb && assemblyMasterList.Count <= 0)
            {
                assemblyMasterList = AssemblyMasterController.Select();
            }
            if (sectionId == outsole && outsoleMasterList.Count <= 0)
            {
                outsoleMasterList = OutsoleMasterController.Select();
            }
            if (sectionId == socklining && sockliningMasterList.Count <= 0)
            {
                sockliningMasterList = SockliningMasterController.Select();
            }

            List<string> lineList = new List<string>();
            Regex regex = new Regex(@"\D");
            dt = new LeadTimeReportDataSet().Tables["LeadTimeTable"];

            #region SEWING
            if (currentSection == sewing)
            {
                var sewingLineList = sewingMasterList
                    .Where(w => w.SewingLine.Contains("sewing"))
                    .Select(s => new { line = s.SewingLine, sort = Int32.Parse(regex.Replace(s.SewingLine, "")) })
                    .OrderBy(o => o.sort)
                    .ThenBy(t => t.line)
                    .Distinct()
                    .ToList();
                lineList = sewingLineList.Select(s => s.line).ToList();
                foreach (string line in lineList)
                {
                    int qtyXDay = 0;
                    int qtyQuota = 0;
                    var productNoList = sewingMasterList.Where(w => w.SewingLine == line).Select(s => s.ProductNo).ToList();
                    foreach (string productNo in productNoList)
                    {
                        var sewingModel = sewingMasterList.Where(w => w.ProductNo == productNo
                            && TimeHelper.Convert(w.SewingActualFinishDateAuto) >= dateFrom
                            && TimeHelper.Convert(w.SewingActualFinishDateAuto) < dateTo)
                            .FirstOrDefault();
                        if (sewingModel != null)
                        {
                            DateTime sewingActualStartDate = TimeHelper.Convert(sewingModel.SewingActualStartDateAuto);
                            DateTime sewingActualFinishDate = TimeHelper.Convert(sewingModel.SewingActualFinishDateAuto);
                            if (sewingActualStartDate != dtDefault && sewingActualStartDate != dtNothing
                                && sewingActualFinishDate != dtDefault && sewingActualFinishDate != dtNothing)
                            {
                                qtyXDay += CalculateDateRange(sewingActualStartDate, sewingActualFinishDate) * sewingModel.SewingQuota;
                                qtyQuota += sewingModel.SewingQuota;
                            }
                        }
                    }
                    CreateData(qtyXDay, qtyQuota, line);
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    ExportReport();
                }));
            }
            #endregion

            #region ASSEMBLY
            if (currentSection == assembly)
            {
                var assyLineList = assemblyMasterList
                .Where(w => w.AssemblyLine.Contains("assy"))
                .Select(s => new { line = s.AssemblyLine, sort = Int32.Parse(regex.Replace(s.AssemblyLine, "")) })
                .OrderBy(o => o.sort)
                .ThenBy(t => t.line)
                .Distinct()
                .ToList();
                lineList = assyLineList.Select(s => s.line).ToList();

                foreach (string line in lineList)
                {
                    int qtyXDay = 0;
                    int qtyQuota = 0;
                    var assemblyMasterListPerLine = assemblyMasterList.Where(w => w.AssemblyLine == line).ToList();
                    var productNoList = assemblyMasterList.Where(w => w.AssemblyLine == line).Select(s => s.ProductNo).ToList();
                    foreach (string productNo in productNoList)
                    {
                        var assemblyModel = assemblyMasterList.Where(w => w.ProductNo == productNo
                            && TimeHelper.Convert(w.AssemblyActualFinishDate) >= dateFrom
                            && TimeHelper.Convert(w.AssemblyActualFinishDate) < dateTo)
                            .FirstOrDefault();
                        if (assemblyModel != null)
                        {
                            DateTime assemblyActualStartDate = TimeHelper.Convert(assemblyModel.AssemblyActualStartDate);
                            DateTime assemblyActualFinishDate = TimeHelper.Convert(assemblyModel.AssemblyActualFinishDate);
                            if (assemblyActualStartDate != dtDefault && assemblyActualStartDate != dtNothing
                                && assemblyActualFinishDate != dtDefault && assemblyActualFinishDate != dtNothing)
                            {
                                qtyXDay += CalculateDateRange(assemblyActualStartDate, assemblyActualFinishDate) * assemblyModel.AssemblyQuota;
                                qtyQuota += assemblyModel.AssemblyQuota;
                            }
                        }
                    }
                    CreateData(qtyXDay, qtyQuota, line);
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    ExportReport();
                }));
            }
            #endregion

            #region OUTSOLE
            if (currentSection == outsole)
            {
                var outSoleLineList = outsoleMasterList
                .Where(w => w.OutsoleLine.Contains("stockfit"))
                .Select(s => new { line = s.OutsoleLine, sort = Int32.Parse(regex.Replace(s.OutsoleLine, "")) })
                .OrderBy(o => o.sort)
                .ThenBy(t => t.line)
                .Distinct()
                .ToList();
                lineList = outSoleLineList.Select(s => s.line).ToList();
                foreach (string line in lineList)
                {
                    int qtyXDay = 0;
                    int qtyQuota = 0;
                    var productNoList = outsoleMasterList.Where(w => w.OutsoleLine == line).Select(s => s.ProductNo).ToList();
                    foreach (string productNo in productNoList)
                    {
                        var outsoleModel = outsoleMasterList
                            .Where(w => w.ProductNo == productNo
                            && TimeHelper.Convert(w.OutsoleActualFinishDateAuto) >= dateFrom
                            && TimeHelper.Convert(w.OutsoleActualFinishDateAuto) < dateTo)
                            .FirstOrDefault();
                        if (outsoleModel != null)
                        {
                            DateTime outsoleModelActualStartDate = TimeHelper.Convert(outsoleModel.OutsoleActualStartDateAuto);
                            DateTime outsoleModelActualFinishDate = TimeHelper.Convert(outsoleModel.OutsoleActualFinishDateAuto);
                            if (outsoleModelActualStartDate != dtDefault && outsoleModelActualStartDate != dtNothing
                                && outsoleModelActualFinishDate != dtDefault && outsoleModelActualFinishDate != dtNothing)
                            {
                                qtyXDay += CalculateDateRange(outsoleModelActualStartDate, outsoleModelActualFinishDate) * outsoleModel.OutsoleQuota;
                                qtyQuota += outsoleModel.OutsoleQuota;
                            }
                        }
                    }
                    CreateData(qtyXDay, qtyQuota, line);
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    ExportReport();
                }));
            }
            #endregion

            #region SOCKLINING
            if (currentSection == socklining)
            {
                var sockliningLineList = sockliningMasterList
                .Where(w => w.SockliningLine.Contains("insock cell"))
                .Select(s => new { line = s.SockliningLine, sort = Int32.Parse(regex.Replace(s.SockliningLine, "")) })
                .OrderBy(o => o.sort)
                .ThenBy(t => t.line)
                .Distinct()
                .ToList();
                lineList = sockliningLineList.Select(s => s.line).ToList();
                foreach (string line in lineList)
                {
                    int qtyXDay = 0;
                    int qtyQuota = 0;
                    var productNoList = sockliningMasterList.Where(w => w.SockliningLine == line).Select(s => s.ProductNo).ToList();
                    foreach (string productNo in productNoList)
                    {
                        var sockliningModel = sockliningMasterList
                               .Where(w => w.ProductNo == productNo
                               && TimeHelper.Convert(w.SockliningActualFinishDate) >= dateFrom
                               && TimeHelper.Convert(w.SockliningActualFinishDate) < dateTo)
                               .FirstOrDefault();
                        if (sockliningModel != null)
                        {
                            DateTime sockliningModelActualStartDate = TimeHelper.Convert(sockliningModel.SockliningActualStartDate);
                            DateTime sockliningModelActualFinishDate = TimeHelper.Convert(sockliningModel.SockliningActualFinishDate);
                            if (sockliningModelActualStartDate != dtDefault && sockliningModelActualStartDate != dtNothing
                                && sockliningModelActualFinishDate != dtDefault && sockliningModelActualFinishDate != dtNothing)
                            {
                                qtyXDay += CalculateDateRange(sockliningModelActualStartDate, sockliningModelActualFinishDate) * sockliningModel.SockliningQuota;
                                qtyQuota += sockliningModel.SockliningQuota;
                            }
                        }
                    }
                    CreateData(qtyXDay, qtyQuota, line);
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    ExportReport();
                }));
            }
            #endregion

            #region CUTA
            if (currentSection == cuta)
            {
                var cutALineList = sewingMasterList
                    .Where(w => w.SewingLine.Contains("sewing"))
                    .Select(s => new { line = s.SewingLine, sort = Int32.Parse(regex.Replace(s.SewingLine, "")) })
                    .OrderBy(o => o.sort)
                    .ThenBy(t => t.line)
                    .Distinct()
                    .ToList();
                lineList = cutALineList.Select(s => s.line).ToList();
                foreach (string line in lineList)
                {
                    var productNoList = sewingMasterList.Where(w => w.SewingLine == line).Select(s => s.ProductNo).ToList();

                    int qtyXDay = 0;
                    int qtyQuota = 0;
                    foreach (var productNo in productNoList)
                    {
                        var sewingModel = sewingMasterList.Where(w => w.ProductNo == productNo).FirstOrDefault();
                        var assyModel = assemblyMasterList
                            .Where(w => w.ProductNo == productNo
                                && TimeHelper.Convert(w.AssemblyActualFinishDate) >= dateFrom
                                && TimeHelper.Convert(w.AssemblyActualFinishDate) < dateTo)
                            .FirstOrDefault();
                        if (sewingModel != null && assyModel != null)
                        {
                            DateTime cutAActualStartDate = TimeHelper.Convert(sewingModel.CutAActualStartDate);
                            DateTime sewingActualFinishDate = TimeHelper.Convert(sewingModel.SewingActualFinishDate);
                            DateTime assyActualStartDate = TimeHelper.Convert(assyModel.AssemblyActualStartDate);
                            DateTime assyActualFinishDate = TimeHelper.Convert(assyModel.AssemblyActualFinishDate);

                            int timeRange = 0;
                            if (cutAActualStartDate != dtNothing && cutAActualStartDate != dtDefault && assyActualFinishDate != dtNothing && assyActualFinishDate != dtDefault)
                            {
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
                    }
                    CreateData(qtyXDay, qtyQuota, line);
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    ExportReport();
                }));
            }
            #endregion

            #region CUTB
            if (currentSection == cutb)
            {
                var cutBLineList = sewingMasterList
                    .Where(w => w.SewingLine.Contains("sewing"))
                    .Select(s => new { line = s.SewingLine, sort = Int32.Parse(regex.Replace(s.SewingLine, "")) })
                    .OrderBy(o => o.sort)
                    .ThenBy(t => t.line)
                    .Distinct()
                    .ToList();
                lineList = cutBLineList.Select(s => s.line).ToList();
                foreach (string line in lineList)
                {
                    var productNoList = sewingMasterList.Where(w => w.SewingLine == line).Select(s => s.ProductNo).ToList();

                    int qtyXDay = 0;
                    int qtyQuota = 0;
                    foreach (var productNo in productNoList)
                    {
                        var sewingModel = sewingMasterList.Where(w => w.ProductNo == productNo).FirstOrDefault();
                        var assyModel = assemblyMasterList
                            .Where(w => w.ProductNo == productNo
                                && TimeHelper.Convert(w.AssemblyActualFinishDate) >= dateFrom
                                && TimeHelper.Convert(w.AssemblyActualFinishDate) < dateTo)
                            .FirstOrDefault();
                        if (sewingModel != null && assyModel != null)
                        {
                            DateTime cutBActualStartDate = TimeHelper.Convert(sewingModel.CutBActualStartDate);
                            DateTime sewingActualFinishDate = TimeHelper.Convert(sewingModel.SewingActualFinishDateAuto);
                            DateTime assyActualStartDate = TimeHelper.Convert(assyModel.AssemblyActualStartDate);
                            DateTime assyActualFinishDate = TimeHelper.Convert(assyModel.AssemblyActualFinishDate);

                            int timeRange = 0;
                            if (cutBActualStartDate != dtNothing && cutBActualStartDate != dtDefault && assyActualFinishDate != dtNothing && assyActualFinishDate != dtDefault)
                            {
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
                    }
                    CreateData(qtyXDay, qtyQuota, line);
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    ExportReport();
                }));
            }
            #endregion
            //#region ARTICLE
            //if (currentSection == article)
            //{
            //    var articleNoList = ordersList.Select(s => new { article = s.ArticleNo.Contains("-") ? s.ArticleNo.Split('-')[0] : s.ArticleNo }).OrderBy(o => o.article).Distinct().Select(s => s.article).ToList();
            //    foreach (var articleNo in articleNoList)
            //    {
            //        int qtyXDay = 0;
            //        int qtyQuota = 0;
            //        var productNoList = ordersList.Where(w => w.ArticleNo.Contains(articleNo)).Select(s => s.ProductNo).ToList();
            //        foreach (var productNo in productNoList)
            //        {
            //            var sewingModel = sewingMasterList.Where(w => w.ProductNo == productNo).FirstOrDefault();
            //            var assyModel = assemblyMasterList
            //                .Where(w => w.ProductNo == productNo 
            //                    && TimeHelper.ConvertOldData(w.AssemblyActualFinishDate) >= dateFrom 
            //                    && TimeHelper.ConvertOldData(w.AssemblyActualFinishDate) < dateTo)
            //                .FirstOrDefault();
            //            if (sewingModel != null && assyModel != null)
            //            {
            //                DateTime cutAActualStartDate = TimeHelper.ConvertOldData(sewingModel.CutAActualStartDate);
            //                DateTime sewingActualFinishDate = TimeHelper.ConvertOldData(sewingModel.SewingActualFinishDate);
            //                DateTime assyActualStartDate = TimeHelper.ConvertOldData(assyModel.AssemblyActualStartDate);
            //                DateTime assyActualFinishDate = TimeHelper.ConvertOldData(assyModel.AssemblyActualFinishDate);

            //                int timeRange = 0;
            //                if (cutAActualStartDate != dtNothing && cutAActualStartDate != dtDefault && assyActualFinishDate != dtNothing && assyActualFinishDate != dtDefault)
            //                {
            //                    if (assyActualStartDate != dtDefault && assyActualStartDate != dtNothing && sewingActualFinishDate != dtDefault && sewingActualFinishDate != dtNothing && assyActualStartDate > sewingActualFinishDate)
            //                    {
            //                        timeRange = TimeHelper.CalculateDate(sewingActualFinishDate, assyActualStartDate);
            //                        qtyXDay += (CalculateDateRange(cutAActualStartDate, assyActualFinishDate) - timeRange) * sewingModel.CutAQuota;
            //                        qtyQuota += sewingModel.CutAQuota;
            //                    }
            //                    else
            //                    {
            //                        qtyXDay += CalculateDateRange(cutAActualStartDate, assyActualFinishDate) * sewingModel.CutAQuota;
            //                        qtyQuota += sewingModel.CutAQuota;
            //                    }
            //                }
            //            }
            //        }

            //        if (qtyXDay > 0)
            //        {
            //            CreateData(qtyXDay, qtyQuota, "", articleNo);
            //        }
            //    }
            //    ExportReport();
            //}
            //#endregion

            //#region PM
            //if (currentSection == pm)
            //{
            //    var pmNoList = ordersList.Select(s => s.PatternNo).Distinct().ToList();
            //    foreach (var pmNo in pmNoList)
            //    {
            //        int qtyXDay = 0;
            //        int qtyQuota = 0;
            //        var productNoList = ordersList.Where(w => w.PatternNo == pmNo).Select(s => s.ProductNo).ToList();
            //        foreach (var productNo in productNoList)
            //        {
            //            var sewingModel = sewingMasterList.Where(w => w.ProductNo == productNo).FirstOrDefault();
            //            var assyModel = assemblyMasterList
            //                .Where(w => w.ProductNo == productNo
            //                    && TimeHelper.ConvertOldData(w.AssemblyActualFinishDate) >= dateFrom
            //                    && TimeHelper.ConvertOldData(w.AssemblyActualFinishDate) < dateTo)
            //                .FirstOrDefault();
            //            if (sewingModel != null && assyModel != null)
            //            {
            //                DateTime cutAActualStartDate = TimeHelper.ConvertOldData(sewingModel.CutAActualStartDate);
            //                DateTime sewingActualFinishDate = TimeHelper.ConvertOldData(sewingModel.SewingActualFinishDate);
            //                DateTime assyActualStartDate = TimeHelper.ConvertOldData(assyModel.AssemblyActualStartDate);
            //                DateTime assyActualFinishDate = TimeHelper.ConvertOldData(assyModel.AssemblyActualFinishDate);

            //                int timeRange = 0;
            //                if (cutAActualStartDate != dtNothing && cutAActualStartDate != dtDefault && assyActualFinishDate != dtNothing && assyActualFinishDate != dtDefault)
            //                {
            //                    if (assyActualStartDate != dtDefault && assyActualStartDate != dtNothing && sewingActualFinishDate != dtDefault && sewingActualFinishDate != dtNothing && assyActualStartDate > sewingActualFinishDate)
            //                    {
            //                        timeRange = TimeHelper.CalculateDate(sewingActualFinishDate, assyActualStartDate);
            //                        qtyXDay += (CalculateDateRange(cutAActualStartDate, assyActualFinishDate) - timeRange) * sewingModel.CutAQuota;
            //                        qtyQuota += sewingModel.CutAQuota;
            //                    }
            //                    else
            //                    {
            //                        qtyXDay += CalculateDateRange(cutAActualStartDate, assyActualFinishDate) * sewingModel.CutAQuota;
            //                        qtyQuota += sewingModel.CutAQuota;
            //                    }
            //                }
            //            }
            //        }

            //        if (qtyXDay > 0)
            //        {
            //            CreateData(qtyXDay, qtyQuota, "", pmNo);
            //        }
            //    }
            //    ExportReport();
            //}
            //#endregion
        }

        private void threadLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnCreateChart.IsEnabled = true;
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
        private void CreateData(int qtyXDay, int qtySum, string line)
        {
            string timeAvegareString = "";
            double timeAvegare = 0;
            timeAvegare = (double)qtyXDay / (double)qtySum;
            if (timeAvegare > 0)
            {
                timeAvegareString = timeAvegare.ToString();
            }
            DataRow dr = dt.NewRow();

            if (currentSection == sewing || currentSection == cuta || currentSection == cutb)
            {
                dr["Line"] = line.Replace("sewing ", "");
            }
            if (currentSection == assembly)
            {
                dr["Line"] = line.Replace("assy ", "");
            }
            if (currentSection == outsole)
            {
                dr["Line"] = line.Replace("stockfit ", "");
            }
            if (currentSection == socklining)
            {
                dr["Line"] = line.Replace("insock cell ", "");
            }
            if (timeAvegare > 0)
            {
                timeAvegareString = string.Format("{0:0.00}", timeAvegare);
            }
            dr["TimeAvegare"] = timeAvegareString;
            dr["Section"] = "Lead Time Chart for " + chartTitle;
            dt.Rows.Add(dr);
        }
        private void ExportReport()
        {
            ReportDataSource rds = new ReportDataSource("LeadTime", dt);
            //reportViewer.LocalReport.ReportPath = @"E:\SV PROJECT\MS\1.1.7.1\Saoviet Master Schedule Solution\MasterSchedule\Reports\LeadTimeSectionReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\LeadTimeSectionReport.rdlc";
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
        }
    }
}
