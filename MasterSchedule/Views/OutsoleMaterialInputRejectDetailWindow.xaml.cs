using System.Collections.Generic;
using System.Windows;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Data;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using System;
using System.Windows.Input;
using MasterSchedule.Helpers;
using System.Windows.Threading;
using System.Linq;
using Microsoft.Reporting.WinForms;
using System.Data;
using MasterSchedule.DataSets;
using System.Text.RegularExpressions;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleMaterialInputRejectIssuesDetailWindow.xaml
    /// </summary>
    public partial class OutsoleMaterialInputRejectDetailWindow : Window
    {
        List<SizeRunModel> sizeRunList = new List<SizeRunModel>();
        string productNo;
        OutsoleSuppliersModel supplierClicked;
        DataTable dt;

        List<OutsoleMaterialRejectIssuesModel> rejectIssuesList;
        public List<OutsoleMaterialRejectDetailModel> rejectDetailFromTableList;
        List<OutsoleMaterialRejectDetailModel> rejectDetailList;

        BackgroundWorker bwLoad;
        BackgroundWorker bwInsert;
        AccountModel account;
        List<AccountModel> accountList;
        string INDEXNO;
        int ROUND;
        bool showReport;
        public OutsoleMaterialInputRejectDetailWindow(string productNo, OutsoleSuppliersModel supplierClicked, List<SizeRunModel> sizeRunList, AccountModel account, List<AccountModel> accountList, string INDEXNO, int ROUND, bool showReport)
        {
            this.productNo = productNo;
            this.supplierClicked = supplierClicked;
            this.sizeRunList = sizeRunList;
            this.INDEXNO = INDEXNO;
            this.ROUND = ROUND;
            this.account = account;
            this.accountList = accountList;
            this.showReport = showReport;

            rejectIssuesList = new List<OutsoleMaterialRejectIssuesModel>();
            dt = new DataTable();

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnSave.IsEnabled = false;
                txtProductNo.Text = productNo;
                txtSupplierName.Text = supplierClicked.Name;

                txtIndexNo.Text = INDEXNO;
                txtRound.Text = ROUND.ToString();

                if (showReport == true)
                {
                    gridShowReport.Visibility = Visibility.Visible;
                    gridInputData.Visibility = Visibility.Collapsed;
                }
                else
                {
                    gridShowReport.Visibility = Visibility.Collapsed;
                    gridInputData.Visibility = Visibility.Visible;
                }

                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            // Excute DB
            rejectIssuesList = OutsoleMaterialRejectIssuesController.Select();
            rejectDetailList = OutsoleMaterialRejectDetailController.SelectPerPOPerSupplierPerIndexNoPerRound(productNo, supplierClicked.OutsoleSupplierId, INDEXNO, ROUND);

            // Create Table
            Dispatcher.Invoke(new Action(() => {
                // Reject Issues Id
                dt.Columns.Add("OutsoleMaterialRejectIssuesId", typeof(Int32));
                DataGridTextColumn columnRejectIssuesId = new DataGridTextColumn();
                columnRejectIssuesId.Binding = new Binding("OutsoleMaterialRejectIssuesId");
                columnRejectIssuesId.Visibility = Visibility.Hidden;
                dgOutsoleMaterialRejectDetail.Columns.Add(columnRejectIssuesId);

                // Reject Issues Name
                dt.Columns.Add("OutsoleMaterialRejectIssuesVietNamese", typeof(String));
                DataGridTextColumn columnRejectIssuesVietNamese = new DataGridTextColumn();
                columnRejectIssuesVietNamese.Binding = new Binding("OutsoleMaterialRejectIssuesVietNamese");
                columnRejectIssuesVietNamese.Header = "Vấn Đề";
                columnRejectIssuesVietNamese.MinWidth = 100;
                columnRejectIssuesVietNamese.IsReadOnly = true;
                dgOutsoleMaterialRejectDetail.Columns.Add(columnRejectIssuesVietNamese);

                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    SizeRunModel sizeRun = sizeRunList[i];

                    dt.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                    DataGridTextColumn column = new DataGridTextColumn();
                    column.SetValue(TagProperty, sizeRun.SizeNo);
                    column.Header = String.Format("{0}\n{1}\n{2}", sizeRun.SizeNo, sizeRun.OutsoleSize, sizeRun.MidsoleSize);

                    Style style = new Style(typeof(DataGridCell));
                    style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                    column.CellStyle = style;

                    column.MinWidth = 40;
                    column.Binding = new Binding(String.Format("Column{0}", i)) { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.LostFocus };
                    dgOutsoleMaterialRejectDetail.Columns.Add(column); 
                }

                // Fill Data
                foreach (var rejectIssues in rejectIssuesList)
                {
                    var rejectDetailPerRejectIssuesId = rejectDetailList.Where(w=> w.OutsoleMaterialRejectIssuesId == rejectIssues.OutsoleMaterialRejectIssuesId).ToList();

                    DataRow dr = dt.NewRow();
                    dr["OutsoleMaterialRejectIssuesId"] = rejectIssues.OutsoleMaterialRejectIssuesId;
                    dr["OutsoleMaterialRejectIssuesVietNamese"] = rejectIssues.OutsoleMaterialRejectIssuesVietNamese;

                    for (int i = 0; i <= sizeRunList.Count - 1; i++)
                    {
                        var sizeRun = sizeRunList[i];
                        var rejectDetailPerRejectIssuesIdPerSize = rejectDetailPerRejectIssuesId.Where(w => w.SizeNo == sizeRun.SizeNo).FirstOrDefault();
                        if (rejectDetailPerRejectIssuesIdPerSize != null)
                            dr[String.Format("Column{0}", i)] = rejectDetailPerRejectIssuesIdPerSize.QuantityReject;
                    }
                    dt.Rows.Add(dr);
                }
                dgOutsoleMaterialRejectDetail.ItemsSource = dt.AsDataView();
                e.Result = dt;
            }));
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnSave.IsEnabled = true;

            if (showReport == true)
            {
                DataTable dtResult = e.Result as DataTable;
                DataTable dtReport = new OutsoleMaterialRejectDetailDataSet().Tables["OutsoleMaterialRejectDetailTable"];

                var regex = new Regex("[a-z]|[A-Z]");
                foreach (DataRow dr in dtResult.Rows)
                {
                    for (int i = 0; i <= sizeRunList.Count - 1; i++)
                    {                        
                        DataRow drReport = dtReport.NewRow();
                        drReport["OutsoleMaterialRejectIssuesVietNamese"] = dr["OutsoleMaterialRejectIssuesVietNamese"];

                        var sizeRun = sizeRunList[i];
                        string sizeNoString = regex.IsMatch(sizeRun.SizeNo) == true ? regex.Replace(sizeRun.SizeNo, "") : sizeRun.SizeNo;
                        double sizeNoDouble = 0;
                        Double.TryParse(sizeNoString, out sizeNoDouble);

                        drReport["SizeNoDouble"] = sizeNoDouble;
                        drReport["SizeNo"] = String.Format("{0}\n{1}\n{2}", sizeRun.SizeNo, sizeRun.OutsoleSize, sizeRun.MidsoleSize);
                        drReport["Quantity"] = dr[String.Format("Column{0}", i)];

                        dtReport.Rows.Add(drReport);
                    }
                }

                ReportDataSource rds = new ReportDataSource();
                rds.Name = "OutsoleMaterialReject_Detail";
                rds.Value = dtReport;

                ReportParameter rp1 = new ReportParameter("ProductNo", productNo);
                ReportParameter rp2 = new ReportParameter("Supplier", supplierClicked.Name);
                ReportParameter rp3 = new ReportParameter("IndexNo", INDEXNO);
                ReportParameter rp4 = new ReportParameter("Round", ROUND.ToString());

                //reportViewer.LocalReport.ReportPath = @"E:\SV PROJECT\MS\1.2.0.6\Saoviet Master Schedule Solution\MasterSchedule\Reports\OutsoleMaterialRejectDetailReport.rdlc";
                reportViewer.LocalReport.ReportPath = @"Reports\OutsoleMaterialRejectDetailReport.rdlc";
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4 });
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(rds);
                reportViewer.RefreshReport();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwInsert.IsBusy == true || dt == null)
            {
                return;
            }
            rejectDetailFromTableList = new List<OutsoleMaterialRejectDetailModel>();
            dt = ((DataView)dgOutsoleMaterialRejectDetail.ItemsSource).ToTable();
            if (dt.Rows.Count == 0)
                return;
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            var rejectIssuesIdList = rejectIssuesList.Select(s => s.OutsoleMaterialRejectIssuesId.ToString()).ToList();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int rejectIssuesId = 0;
                DataRow dr = dt.Rows[i];
                string rejectIssuesIdString = "";
                rejectIssuesIdString = dr["OutsoleMaterialRejectIssuesId"].ToString();
                if (rejectIssuesIdList.Contains(rejectIssuesIdString) == false)
                {
                    continue;
                }
                Int32.TryParse(rejectIssuesIdString, out rejectIssuesId);
                for (int j = 0; j <= sizeRunList.Count - 1; j++)
                {
                    int rejectDetailPerSize = 0;
                    Int32.TryParse(dr[String.Format("Column{0}", j)].ToString(), out rejectDetailPerSize);
                    var rejectDetailModel = new OutsoleMaterialRejectDetailModel()
                    {
                        ProductNo = productNo,
                        SizeNo = sizeRunList[j].SizeNo,
                        OutsoleSupplierId = supplierClicked.OutsoleSupplierId,
                        CreatedBy = account.UserName,
                        QuantityReject = rejectDetailPerSize,
                        OutsoleMaterialRejectIssuesId = rejectIssuesId,
                        IndexNo = INDEXNO,
                        Round = ROUND,
                    };

                    rejectDetailFromTableList.Add(rejectDetailModel);
                }
            }
            this.Cursor = Cursors.Wait;
            btnSave.IsEnabled = false;
            bwInsert.RunWorkerAsync();
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var insertModel in rejectDetailFromTableList)
            {
                OutsoleMaterialRejectDetailController.Insert(insertModel);
            }

            // create OutsoleMaterialDetail and Update
            var updateRejectToOutsoleMaterialDetailList = new List<OutsoleMaterialDetailModel>();

            var ousoleMaterialRejectDetailModel = rejectDetailFromTableList.FirstOrDefault();
            if (ousoleMaterialRejectDetailModel != null)
            {
                var sizeNoList = rejectDetailFromTableList.Select(s => s.SizeNo).Distinct().ToList();
                foreach (var sizeNo in sizeNoList)
                {
                    var osMaterialDetail = new OutsoleMaterialDetailModel()
                    {
                        ProductNo = ousoleMaterialRejectDetailModel.ProductNo,
                        OutsoleSupplierId = ousoleMaterialRejectDetailModel.OutsoleSupplierId,
                        SizeNo = sizeNo,
                        CreatedBy = ousoleMaterialRejectDetailModel.CreatedBy,
                        IndexNo = ousoleMaterialRejectDetailModel.IndexNo,
                        Round = ousoleMaterialRejectDetailModel.Round,
                        Reject = rejectDetailFromTableList.Where(w => w.SizeNo == sizeNo).Select(s => s.QuantityReject).Sum()
                    };
                    updateRejectToOutsoleMaterialDetailList.Add(osMaterialDetail);
                }
            }

            foreach (var updateModel in updateRejectToOutsoleMaterialDetailList)
            {
                OutsoleMaterialRejectDetailController.UpdateRejectToOutsoleMaterialDetail(updateModel);
            }
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;

            if (e.Error == null)
                MessageBox.Show("Saved !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("An Error Occurred !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);

            btnSave.IsEnabled = true;
            this.Close();
        }
    }
}
