using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Helpers;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHInventoryWindow.xaml
    /// </summary>
    public partial class UpperWHInventoryDetailWindow : Window
    {
        List<String> productNoList;
        List<SewingOutputModel> sewingOutputList;
        List<OutsoleOutputModel> outsoleOutputList;
        List<AssemblyReleaseModel> assemblyReleaseList;
        List<OrdersModel> orderList;
        public UpperWHInventoryDetailWindow(List<String> productNoList, List<SewingOutputModel> sewingOutputList, List<OutsoleOutputModel> outsoleOutputList, List<AssemblyReleaseModel> assemblyReleaseList, List<OrdersModel> orderList)
        {
            this.productNoList = productNoList;
            this.sewingOutputList = sewingOutputList;
            this.outsoleOutputList = outsoleOutputList;
            this.assemblyReleaseList = assemblyReleaseList;
            this.orderList = orderList;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<UpperWHInventoryDetailViewModel> upperWHInventoryDetailViewList = new List<UpperWHInventoryDetailViewModel>();
            foreach (string productNo in productNoList)
            {
                UpperWHInventoryDetailViewModel upperWHInventoryDetailView = new UpperWHInventoryDetailViewModel();
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
                if (upperWHInventoryDetailView.SewingOutput != 0 || upperWHInventoryDetailView.OutsoleOutput != 0)
                {
                    upperWHInventoryDetailViewList.Add(upperWHInventoryDetailView);
                }
            }

            dgInventory.ItemsSource = upperWHInventoryDetailViewList;
            lblQuantityTotal.Text = upperWHInventoryDetailViewList.Sum(u => u.Quantity).ToString();
            lblReleaseQuantityTotal.Text = upperWHInventoryDetailViewList.Sum(u => u.ReleaseQuantity).ToString();
            lblSewingOutput.Text = upperWHInventoryDetailViewList.Sum(u => u.SewingOutput).ToString();
            lblOutsoleOutput.Text = upperWHInventoryDetailViewList.Sum(u => u.OutsoleOutput).ToString();
            lblMatching.Text = upperWHInventoryDetailViewList.Sum(u => u.Matching).ToString();
        }

    }
}
