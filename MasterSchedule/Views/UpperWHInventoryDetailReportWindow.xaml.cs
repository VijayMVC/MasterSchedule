using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;


using Microsoft.Reporting.WinForms;
using System.Data;
using MasterSchedule.DataSets;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Helpers;
using MasterSchedule.Controllers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpperWHInventoryDetailReportWindow.xaml
    /// </summary>
    public partial class UpperWHInventoryDetailReportWindow : Window
    {
        List<String> assemblyLineList;
        List<OrdersModel> orderList;
        List<AssemblyReleaseModel> assemblyReleaseList;
        List<SewingOutputModel> sewingOutputList;
        List<OutsoleOutputModel> outsoleOutputList;
        List<AssemblyMasterModel> assemblyMasterList;

        public UpperWHInventoryDetailReportWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = new UpperWHInventoryDetailDataSet().Tables["UpperWHInventoryDetailTable"];
            
            assemblyMasterList = AssemblyMasterController.Select();
            sewingOutputList = SewingOutputController.SelectByAssemblyMaster();
            outsoleOutputList = OutsoleOutputController.SelectByAssemblyMaster();
            assemblyReleaseList = AssemblyReleaseController.SelectByAssemblyMaster();
            orderList = OrdersController.SelectByAssemblyMaster();

            List<UpperWHInventoryViewModel> upperWHInventoryViewList = new List<UpperWHInventoryViewModel>();
            assemblyLineList = assemblyMasterList.Where(a => String.IsNullOrEmpty(a.AssemblyLine) == false).Select(a => a.AssemblyLine).Distinct().OrderBy(l => l).ToList();

            // Total value
            //int upperTotal = 0;
            //int outsoleTotal = 0;
            //int matchingTotal = 0;
            // Sum value
            int upperSum, outsoleSum, matchingSum;

            foreach (string assemblyLine in assemblyLineList)
            {
                List<String> productNoList = assemblyMasterList.Where(a => a.AssemblyLine == assemblyLine).Select(o => o.ProductNo).OrderBy(l => l).Distinct().ToList();
                List<UpperWHInventoryDetailViewModel> upperWHInventoryDetailViewList = new List<UpperWHInventoryDetailViewModel>();
                foreach (string productNo in productNoList)
                {
                    UpperWHInventoryDetailViewModel upperWHInventoryDetailView = new UpperWHInventoryDetailViewModel();
                    OrdersModel order = orderList.Where(o => o.ProductNo == productNo).FirstOrDefault();
                    if (order != null)
                    {
                        upperWHInventoryDetailView.ProductNo = productNo;
                        upperWHInventoryDetailView.ShoeName = order.ShoeName;
                        upperWHInventoryDetailView.ArticleNo = order.ArticleNo;
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

                    //UpperWHInventoryViewModel upperWHInventoryView = new UpperWHInventoryViewModel
                    //{
                    //    AssemblyLine = assemblyLine,
                    //    ProductNoList = productNoList,
                    //    SewingOutput = qtyUpperTotal,
                    //    OutsoleOutput = qtyOutsoleTotal,
                    //    Matching = qtyMatchTotal,
                    //};
                    //upperWHInventoryViewList.Add(upperWHInventoryView);


                    if (upperWHInventoryDetailView.SewingOutput != 0 || upperWHInventoryDetailView.OutsoleOutput != 0)
                    {
                        upperWHInventoryDetailViewList.Add(upperWHInventoryDetailView);
                    }
                }

                Int32.TryParse(upperWHInventoryDetailViewList.Sum(o => o.SewingOutput).ToString(), out upperSum);
                Int32.TryParse(upperWHInventoryDetailViewList.Sum(o => o.OutsoleOutput).ToString(), out outsoleSum);
                Int32.TryParse(upperWHInventoryDetailViewList.Sum(o => o.Matching).ToString(), out matchingSum);
               
                //List<int> upperSumList = upperWHInventoryDetailViewList.Where(o => o.SewingOutput != 0).Select(p => p.SewingOutput).ToList();
                //List<int> ousoleSumList = upperWHInventoryDetailViewList.Where(o => o.OutsoleOutput != 0).Select(p => p.OutsoleOutput).ToList();
                //List<int> matchingSumList = upperWHInventoryDetailViewList.Where(o => o.Matching != 0).Select(p => p.Matching).ToList();
                
                foreach (UpperWHInventoryDetailViewModel print in upperWHInventoryDetailViewList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ProductNo"] = print.ProductNo;
                    dr["ShoeName"] = print.ShoeName;
                    dr["ArticleNo"] = print.ArticleNo;
                    dr["AssemblyLine"] = assemblyLine;
                    //dr["AssemblyLineSum"] = assemblyLine;
                    dr["Quantity"] = print.Quantity;
                    dr["Release"] = print.ReleaseQuantity;
                    dr["Upper"] = print.SewingOutput;
                    dr["OutSole"] = print.OutsoleOutput;
                    dr["Matching"] = print.Matching;

                    //dr["UpperSum"] = upperWHInventoryDetailViewList.Sum(o => o.SewingOutput).ToString();
                    //dr["OutSoleSum"] = upperWHInventoryDetailViewList.Sum(o => o.OutsoleOutput).ToString();
                    //dr["MatchingSum"] = upperWHInventoryDetailViewList.Sum(o => o.Matching).ToString();
                    dr["UpperSum"] = upperSum;
                    dr["OutSoleSum"] = outsoleSum;
                    dr["MatchingSum"] = matchingSum;
                    
                    dt.Rows.Add(dr);
                }

            }


            ReportDataSource rds = new ReportDataSource();
            rds.Name = "UpperWHInventoryDetail_DataSetV9";
            //rds.Name = "UpperWHInventoryDetailFinal";
            rds.Value = dt;
            //reportViewer.LocalReport.ReportPath = @"C:\Users\IT02\Documents\Visual Studio 2010\Projects\Saoviet Master Schedule Solution\MasterSchedule\Reports\DelayReport.rdlc";
            reportViewer.LocalReport.ReportPath = @"Reports\UpperWHInventoryDetailReport.rdlc";
            //reportViewer.LocalReport.ReportPath = @"Reports\UpperWHInventoryDetailReportV11.rdlc";
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.RefreshReport();
            this.Cursor = null;

        }
    }
}
