using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using Microsoft.Reporting.WinForms;
using System.Data;
using MasterSchedule.DataSets;
using MasterSchedule.Helpers;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpperWHInventoryReportWindow.xaml
    /// </summary>
    public partial class UpperWHInventoryReportWindow : Window
    {
        List<String> productNoList;
        List<SewingOutputModel> sewingOutputList;
        List<OutsoleOutputModel> outsoleOutputList;
        List<AssemblyReleaseModel> assemblyReleaseList;
        List<OrdersModel> orderList;
        BackgroundWorker bwLoad;
        public UpperWHInventoryReportWindow(List<String> productNoList, List<SewingOutputModel> sewingOutputList, List<OutsoleOutputModel> outsoleOutputList, List<AssemblyReleaseModel> assemblyReleaseList, List<OrdersModel> orderList)
        {
            this.productNoList = productNoList;
            this.sewingOutputList = sewingOutputList;
            this.outsoleOutputList = outsoleOutputList;
            this.assemblyReleaseList = assemblyReleaseList;
            this.orderList = orderList;
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
            List<UpperWHInventoryDetailViewModel> upperWHInventoryDetailViewList = new List<UpperWHInventoryDetailViewModel>();
            DataTable dt = new UpperWHInventoryDataSet().Tables["UpperWHInventoryTable"];
            foreach (string productNo in productNoList)
            {
                DataRow dr = dt.NewRow();
                UpperWHInventoryDetailViewModel upperWHInventoryDetailView = new UpperWHInventoryDetailViewModel();
                dr["ProductNo"] = productNo;
                upperWHInventoryDetailView.ProductNo = productNo;
                OrdersModel order = orderList.Where(o => o.ProductNo == productNo).FirstOrDefault();
                if (order != null)
                {
                    upperWHInventoryDetailView.ShoeName = order.ShoeName;
                    upperWHInventoryDetailView.ArticleNo = order.ArticleNo;
                    upperWHInventoryDetailView.ETD = order.ETD;
                }
                int qtyUpperTotal = 0;
                int qtyOutsoleTotal = 0;
                int qtyMatchTotal = 0;
                int qtyQuantity = order.Quantity;
                int qtyReleaseTotal = 0;
                List<AssemblyReleaseModel> assemblyReleaseList_D1 = assemblyReleaseList.Where(a => a.ProductNo == productNo).ToList();
                List<SewingOutputModel> sewingOutputList_D1 = sewingOutputList.Where(s => s.ProductNo == productNo).ToList();
                List<OutsoleOutputModel> outsoleOutputList_D1 = outsoleOutputList.Where(o => o.ProductNo == productNo).ToList();

                List<String> sizeNoList = sewingOutputList_D1.Select(s => s.SizeNo).Distinct().ToList();
                if (sizeNoList.Count == 0)
                {
                    sizeNoList = outsoleOutputList_D1.Select(o => o.SizeNo).Distinct().ToList();
                }
                foreach (string sizeNo in sizeNoList)
                {
                    int qtyRelease = assemblyReleaseList_D1.Where(a => a.SizeNo == sizeNo).Sum(a => a.Quantity);
                    int qtyUpper = sewingOutputList_D1.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity)
                        - qtyRelease;
                    int qtyOutsole = outsoleOutputList_D1.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity)
                        - qtyRelease;
                    int qtyMatch =
                        MatchingHelper.Calculate(sewingOutputList_D1.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity),
                        outsoleOutputList_D1.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity), sizeNo)
                        - qtyRelease;
                    if (qtyUpper < 0)
                    {
                        qtyUpper = 0;
                    }
                    qtyUpperTotal += qtyUpper;
                    if (qtyOutsole < 0)
                    {
                        qtyOutsole = 0;
                    }
                    qtyOutsoleTotal += qtyOutsole;
                    if (qtyMatch < 0)
                    {
                        qtyMatch = 0;
                    }
                    qtyMatchTotal += qtyMatch;
                    if (qtyRelease < 0)
                    {
                        qtyRelease = 0;
                    }
                    qtyReleaseTotal += qtyRelease;
                }
                upperWHInventoryDetailView.Quantity = qtyQuantity;
                upperWHInventoryDetailView.ReleaseQuantity = qtyReleaseTotal;
                upperWHInventoryDetailView.SewingOutput = qtyUpperTotal;
                upperWHInventoryDetailView.OutsoleOutput = qtyOutsoleTotal;
                upperWHInventoryDetailView.Matching = qtyMatchTotal;
                dr["Quantity"] = qtyQuantity;
                dr["ReleaseQuantity"] = qtyReleaseTotal;
                dr["SewingOutput"] = qtyUpperTotal;
                dr["OutsoleOutput"] = qtyOutsoleTotal;
                dr["Matching"] = qtyMatchTotal;
                if (upperWHInventoryDetailView.SewingOutput != 0 || upperWHInventoryDetailView.OutsoleOutput != 0)
                {
                    upperWHInventoryDetailViewList.Add(upperWHInventoryDetailView);
                }
                dt.Rows.Add(dr);
            }
            e.Result = dt;
            
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = e.Result as DataTable;

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "UpperWHInventory";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\SewingMasterReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\UpperWHInventory.rdlc";
            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;
        }
    }
}
