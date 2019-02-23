using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;
using MasterSchedule.Helpers;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHInventoryWindow.xaml
    /// </summary>
    public partial class UpperWHInventoryWindow : Window
    {
        BackgroundWorker bwLoadData;
        List<SewingOutputModel> sewingOutputList;
        List<OutsoleOutputModel> outsoleOutputList;
        List<AssemblyReleaseModel> assemblyReleaseList;
        List<OrdersModel> orderList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<String> assemblyLineList;

        public UpperWHInventoryWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            sewingOutputList = new List<SewingOutputModel>();
            outsoleOutputList = new List<OutsoleOutputModel>();
            orderList = new List<OrdersModel>();
            assemblyReleaseList = new List<AssemblyReleaseModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            InitializeComponent();
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            assemblyMasterList = AssemblyMasterController.Select();
            sewingOutputList = SewingOutputController.SelectByAssemblyMaster();
            outsoleOutputList = OutsoleOutputController.SelectByAssemblyMaster();
            assemblyReleaseList = AssemblyReleaseController.SelectByAssemblyMaster();
            orderList = OrdersController.SelectByAssemblyMaster();

            List<UpperWHInventoryViewModel> upperWHInventoryViewList = new List<UpperWHInventoryViewModel>();
            assemblyLineList = assemblyMasterList.Where(a => String.IsNullOrEmpty(a.AssemblyLine) == false).Select(a => a.AssemblyLine).Distinct().OrderBy(l => l).ToList();
            foreach (string assemblyLine in assemblyLineList)
            {
                List<String> productNoList = assemblyMasterList.Where(a => a.AssemblyLine == assemblyLine).Select(o => o.ProductNo).Distinct().ToList();

                List<AssemblyReleaseModel> assemblyReleaseList_D1 = assemblyReleaseList.Where(a => productNoList.Contains(a.ProductNo) == true).ToList();
                List<SewingOutputModel> sewingOutputList_D1 = sewingOutputList.Where(s => productNoList.Contains(s.ProductNo) == true).ToList();
                List<OutsoleOutputModel> outsoleOutputList_D1 = outsoleOutputList.Where(o => productNoList.Contains(o.ProductNo) == true).ToList();

                int qtyUpperTotal = 0;
                int qtyOutsoleTotal = 0;
                int qtyMatchTotal = 0;
                foreach (string productNo in productNoList)
                {
                    List<AssemblyReleaseModel> assemblyReleaseList_D2 = assemblyReleaseList_D1.Where(a => a.ProductNo == productNo).ToList();
                    List<SewingOutputModel> sewingOutputList_D2 = sewingOutputList_D1.Where(s => s.ProductNo == productNo).ToList();
                    List<OutsoleOutputModel> outsoleOutputList_D2 = outsoleOutputList_D1.Where(o => o.ProductNo == productNo).ToList();

                    List<String> sizeNoList = sewingOutputList_D2.Select(s => s.SizeNo).Distinct().ToList();
                    if (sizeNoList.Count == 0)
                    {
                        sizeNoList = outsoleOutputList_D2.Select(o => o.SizeNo).Distinct().ToList();
                    }
                    foreach (string sizeNo in sizeNoList)
                    {
                        int qtyRelease = assemblyReleaseList_D2.Where(a => a.SizeNo == sizeNo).Sum(a => a.Quantity);
                        int qtyUpper = sewingOutputList_D2.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity)
                            - qtyRelease;
                        int qtyOutsole = outsoleOutputList_D2.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity)
                            - qtyRelease;
                        int qtyMatch =
                            MatchingHelper.Calculate(sewingOutputList_D2.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity),
                            outsoleOutputList_D2.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity), sizeNo)
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
                    }
                }
                UpperWHInventoryViewModel upperWHInventoryView = new UpperWHInventoryViewModel
                {
                    AssemblyLine = assemblyLine,
                    ProductNoList = productNoList,
                    SewingOutput = qtyUpperTotal,
                    OutsoleOutput = qtyOutsoleTotal,
                    Matching = qtyMatchTotal,
                };

                upperWHInventoryViewList.Add(upperWHInventoryView);

            }

            e.Result = upperWHInventoryViewList;
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<UpperWHInventoryViewModel> upperWHInventoryViewList = e.Result as List<UpperWHInventoryViewModel>;
            
            dgInventory.ItemsSource = upperWHInventoryViewList;
            lblSewingOutput.Text = upperWHInventoryViewList.Sum(u => u.SewingOutput).ToString();
            lblOutsoleOutput.Text = upperWHInventoryViewList.Sum(u => u.OutsoleOutput).ToString();
            lblMatching.Text = upperWHInventoryViewList.Sum(o => o.Matching).ToString();
            this.Cursor = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }
        }

        private void dgInventory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpperWHInventoryViewModel upperWHInventoryView = (UpperWHInventoryViewModel)dgInventory.CurrentItem;
            if (upperWHInventoryView != null)
            {
                List<String> productNoList = upperWHInventoryView.ProductNoList;
                UpperWHInventoryDetailWindow window = new UpperWHInventoryDetailWindow(
                    productNoList,
                    sewingOutputList.Where(s => productNoList.Contains(s.ProductNo) == true).ToList(),
                    outsoleOutputList.Where(o => productNoList.Contains(o.ProductNo) == true).ToList(),
                    assemblyReleaseList.Where(a => productNoList.Contains(a.ProductNo) == true).ToList(),
                    orderList.Where(o => productNoList.Contains(o.ProductNo) == true).ToList()
                    );
                window.Title = String.Format("{0} for {1}", window.Title, upperWHInventoryView.AssemblyLine);
                window.Show();
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            UpperWHInventoryDetailReportWindow upperWindow = new UpperWHInventoryDetailReportWindow();
            upperWindow.Show();
        }
    }
}
