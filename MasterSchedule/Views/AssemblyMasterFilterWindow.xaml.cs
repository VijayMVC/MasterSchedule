using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;

using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;
using System.Globalization;
using System.Collections.ObjectModel;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for SewingMasterReportWindow.xaml
    /// </summary>
    public partial class AssemblyMasterFilterWindow : Window
    {
        BackgroundWorker bwLoad;        
        List<OrdersModel> orderList;
        List<RawMaterialModel> rawMaterialList;
        List<SewingMasterModel> sewingMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<SockliningMasterModel> sockliningMasterList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<LineFilterViewModel> lineFilterViewList;
        List<ETDFilterViewModel> etdFilterViewList;
        List<AssemblyMasterExportViewModel> assemblyMasterExportViewList;
        ObservableCollection<AssemblyMasterExportViewModel> assemblyMasterExportViewFilteredList;
        DateTime dtDefault;
        DateTime dtNothing;
        List<ProductionMemoModel> productionMemoList;
        public AssemblyMasterFilterWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            orderList = new List<OrdersModel>();
            rawMaterialList = new List<RawMaterialModel>();
            sewingMasterList = new List<SewingMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            sockliningMasterList = new List<SockliningMasterModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            lineFilterViewList = new List<LineFilterViewModel>();
            etdFilterViewList = new List<ETDFilterViewModel>();
            assemblyMasterExportViewList = new List<AssemblyMasterExportViewModel>();
            assemblyMasterExportViewFilteredList = new ObservableCollection<AssemblyMasterExportViewModel>();
            dtDefault = new DateTime(2000, 1, 1);
            dtNothing = new DateTime(1999, 12, 31);
            productionMemoList = new List<ProductionMemoModel>();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnFilter.IsEnabled = false;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            orderList = OrdersController.Select();
            rawMaterialList = RawMaterialController.Select();
            sewingMasterList = SewingMasterController.Select();
            outsoleMasterList = OutsoleMasterController.Select();
            sockliningMasterList = SockliningMasterController.Select();
            assemblyMasterList = AssemblyMasterController.Select();
            productionMemoList = ProductionMemoController.Select();
            //sewingMasterList.RemoveAll(s => DateTimeHelper.Create(s.SewingBalance) != dtDefault && DateTimeHelper.Create(s.SewingBalance) != dtNothing);
            assemblyMasterList = assemblyMasterList.OrderBy(s => s.Sequence).ToList();

            int[] materialIdUpperArray = { 1, 2, 3, 4, 10 };
            int[] materialIdSewingArray = { 5, 7 };
            int[] materialIdOutsoleArray = { 6 };
            int[] materialIdAssemblyArray = { 8, 9 };
            int[] materialIdCartonArray = { 11 };

            foreach (AssemblyMasterModel assemblyMaster in assemblyMasterList)
            {
                AssemblyMasterExportViewModel assemblyMasterExportView = new AssemblyMasterExportViewModel();
                assemblyMasterExportView.Sequence = assemblyMaster.Sequence;
                assemblyMasterExportView.ProductNo = assemblyMaster.ProductNo;
                OrdersModel order = orderList.Where(o => o.ProductNo == assemblyMaster.ProductNo).FirstOrDefault();
                string memoId = "";
                if (order != null)
                {
                    assemblyMasterExportView.Country = order.Country;
                    assemblyMasterExportView.ShoeName = order.ShoeName;
                    assemblyMasterExportView.ArticleNo = order.ArticleNo;
                    assemblyMasterExportView.LastCode = order.LastCode;
                    assemblyMasterExportView.Quantity = order.Quantity;
                    assemblyMasterExportView.ETD = order.ETD;


                    List<ProductionMemoModel> productionMemoByProductionNumberList = productionMemoList.Where(p => p.ProductionNumbers.Contains(order.ProductNo) == true).ToList();
                    for (int p = 0; p <= productionMemoByProductionNumberList.Count - 1; p++)
                    {
                        ProductionMemoModel productionMemo = productionMemoByProductionNumberList[p];
                        memoId += productionMemo.MemoId;
                        if (p < productionMemoByProductionNumberList.Count - 1)
                        {
                            memoId += "\n";
                        }
                    }
                    assemblyMasterExportView.MemoId = memoId;
                }

                MaterialArrivalViewModel materialArrivalOutsole = MaterialArrival(order.ProductNo, materialIdOutsoleArray);
                assemblyMasterExportView.IsOutsoleMatsArrivalOk = false;
                if (materialArrivalOutsole != null)
                {                    
                    assemblyMasterExportView.OutsoleMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalOutsole.Date);
                    assemblyMasterExportView.IsOutsoleMatsArrivalOk = materialArrivalOutsole.IsMaterialArrivalOk;
                }

                MaterialArrivalViewModel materialArrivalAssembly = MaterialArrival(order.ProductNo, materialIdAssemblyArray);
                assemblyMasterExportView.IsAssemblyMatsArrivalOk = false;
                if (materialArrivalAssembly != null)
                {
                    assemblyMasterExportView.AssemblyMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalAssembly.Date);
                    assemblyMasterExportView.IsAssemblyMatsArrivalOk = materialArrivalAssembly.IsMaterialArrivalOk;
                }

                MaterialArrivalViewModel materialArrivalCarton = MaterialArrival(order.ProductNo, materialIdCartonArray);
                if (materialArrivalCarton != null)
                {
                    assemblyMasterExportView.CartonMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalCarton.Date);
                }

                assemblyMasterExportView.AssemblyLine = assemblyMaster.AssemblyLine;
                assemblyMasterExportView.AssemblyStartDate = assemblyMaster.AssemblyStartDate;
                assemblyMasterExportView.AssemblyFinishDate = assemblyMaster.AssemblyFinishDate;
                assemblyMasterExportView.AssemblyQuota = assemblyMaster.AssemblyQuota;
                assemblyMasterExportView.AssemblyBalance = assemblyMaster.AssemblyBalance;

                SewingMasterModel sewingMaster = sewingMasterList.Where(o => o.ProductNo == assemblyMaster.ProductNo).FirstOrDefault();
                if (sewingMaster != null)
                {

                    assemblyMasterExportView.SewingStartDate = sewingMaster.SewingStartDate;
                    assemblyMasterExportView.SewingFinishDate = sewingMaster.SewingFinishDate;
                    assemblyMasterExportView.SewingBalance = sewingMaster.SewingBalance;
                }
                else
                {
                    assemblyMasterExportView.SewingStartDate = dtDefault;
                    assemblyMasterExportView.SewingFinishDate = dtDefault;
                    assemblyMasterExportView.SewingBalance = "";
                }

                OutsoleMasterModel outsoleMaster = outsoleMasterList.Where(o => o.ProductNo == assemblyMaster.ProductNo).FirstOrDefault();
                if (outsoleMaster != null)
                {
                    assemblyMasterExportView.OutsoleBalance = outsoleMaster.OutsoleBalance;
                }
                else
                {
                    assemblyMasterExportView.OutsoleBalance = "";
                }

                SockliningMasterModel sockliningMaster = sockliningMasterList.Where(o => o.ProductNo == assemblyMaster.ProductNo).FirstOrDefault();
                if (sockliningMaster != null)
                {
                    assemblyMasterExportView.InsoleBalance = sockliningMaster.InsoleBalance;
                    assemblyMasterExportView.InsockBalance = sockliningMaster.InsockBalance;
                }
                else
                {
                    assemblyMasterExportView.InsoleBalance = "";
                    assemblyMasterExportView.InsockBalance = "";
                }
                assemblyMasterExportViewList.Add(assemblyMasterExportView);
            }
        }

        private MaterialArrivalViewModel MaterialArrival(string productNo, int[] materialIdArray)
        {
            List<RawMaterialModel> rawMaterialTypeList = rawMaterialList.Where(r => r.ProductNo == productNo && materialIdArray.Contains(r.MaterialTypeId)).ToList();
            rawMaterialTypeList.RemoveAll(r => r.ETD.Date == dtDefault.Date);
            MaterialArrivalViewModel materialArrivalView = new MaterialArrivalViewModel();
            materialArrivalView.IsMaterialArrivalOk = false;
            if (rawMaterialTypeList.Select(r => r.ActualDate).Count() > 0 && rawMaterialTypeList.Select(r => r.ActualDate.Date).Contains(dtDefault.Date) == false)
            {
                materialArrivalView.Date = rawMaterialTypeList.Select(r => r.ActualDate).Max();
                materialArrivalView.IsMaterialArrivalOk = true;
            }
            else
            {
                if (rawMaterialTypeList.Select(r => r.ETD).Count() > 0 && rawMaterialTypeList.Where(r => r.ETD.Date != dtDefault.Date).Count() > 0)
                {
                    materialArrivalView.Date = rawMaterialTypeList.Select(r => r.ETD).Max();
                }
                else
                {
                    materialArrivalView = null;
                }
            }
            return materialArrivalView;
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lineFilterViewList.Add(new
                LineFilterViewModel { IsSelected = false, Content = "All", IsRoot = true });
            foreach (string line in assemblyMasterExportViewList.Select(s => s.AssemblyLine).OrderBy(l => l).Distinct())
            {
                LineFilterViewModel lineFilter = new LineFilterViewModel
                {
                    IsSelected = false,
                    Content = line,
                    IsRoot = false,
                };
                lineFilterViewList.Add(lineFilter);
            }
            lvLine.ItemsSource = lineFilterViewList;

            etdFilterViewList.Add(new
                ETDFilterViewModel { IsSelected = false, Content = "All", IsRoot = true });
            foreach (DateTime etd in assemblyMasterExportViewList.Select(s => s.ETD).OrderBy(d => d).Distinct())
            {
                ETDFilterViewModel etdFilter = new ETDFilterViewModel
                {
                    IsSelected = false,
                    Date = etd,
                    Content = String.Format(new CultureInfo("en-US"), "{0:MM/dd/yyyy}", etd),
                    IsRoot = false,
                };
                etdFilterViewList.Add(etdFilter);
            }
            lvETD.ItemsSource = etdFilterViewList;

            assemblyMasterExportViewFilteredList = new ObservableCollection<AssemblyMasterExportViewModel>(assemblyMasterExportViewList);
            //dgMaster.ItemsSource = assemblyMasterExportViewFilteredList;
            btnFilter.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnLine_Click(object sender, RoutedEventArgs e)
        {
            if (popupLine.IsOpen == false)
            {
                popupLine.IsOpen = true;
                return;
            }
            else
            {
                popupLine.IsOpen = false;
                return;
            }
        }

        bool isAllChecked = false;
        private void chbIsSelected_Checked(object sender, RoutedEventArgs e)
        {
            if (isAllChecked == true)
            {
                return;
            }
            CheckBox chbIsSelected = (CheckBox)sender;
            if (chbIsSelected == null)
            {
                return;
            }            
            bool isRoot = chbIsSelected.IsThreeState;
            if (isRoot == true)
            {
                isAllChecked = true;
                bool isChecked = (chbIsSelected.IsChecked == true);
                foreach (LineFilterViewModel lineFilterView in lineFilterViewList)
                {
                    if (lineFilterView.IsRoot == false)
                    {
                        lineFilterView.IsSelected = isChecked;
                    }
                }
                isAllChecked = false;
                return;
            }
            else
            {                
                foreach (LineFilterViewModel lineFilterView in lineFilterViewList)
                {
                    if (lineFilterView.IsRoot == true)
                    {
                        lineFilterView.IsSelected = null;
                    }                    
                }
                if (lineFilterViewList.Where(l => l.IsRoot == false).Select(l => l.IsSelected.Value).Distinct().Count() == 1)
                {
                    foreach (LineFilterViewModel lineFilterView in lineFilterViewList)
                    {
                        if (lineFilterView.IsRoot == true)
                        {
                            lineFilterView.IsSelected = lineFilterViewList.Where(l => l.IsRoot == false).Select(l => l.IsSelected.Value).Distinct().FirstOrDefault();
                        }
                    }                    
                }                
                return;
            }
        }        

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            List<LineFilterViewModel> lineFilterViewSelectedList = lineFilterViewList.Where(l => l.IsSelected == true && l.IsRoot == false).ToList();
            if (lineFilterViewSelectedList.Count() == 0)
            {
                return;
            }
            string lineSelected = "";
            foreach (LineFilterViewModel lineFilterView in lineFilterViewSelectedList)
            {
                lineSelected += lineFilterView.Content + "; ";
            }
            lblLine.ToolTip = lineSelected;
            if (lineFilterViewSelectedList.Count() == 1)
            {
                lblLine.Text = lineSelected;
            }
            else
            {
                lblLine.Text = "(Multiple Items)";
            }
            popupLine.IsOpen = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            popupLine.IsOpen = false;
        }

        private void btnETD_Click(object sender, RoutedEventArgs e)
        {
            if (popupETD.IsOpen == false)
            {
                popupETD.IsOpen = true;
                return;
            }
            else
            {
                popupETD.IsOpen = false;
                return;
            }
        }
        bool isAllETDChecked = false;
        private void chbETDIsSelected_Checked(object sender, RoutedEventArgs e)
        {
            if (isAllETDChecked == true)
            {
                return;
            }
            CheckBox chbETDIsSelected = (CheckBox)sender;
            if (chbETDIsSelected == null)
            {
                return;
            }
            bool isRoot = chbETDIsSelected.IsThreeState;
            if (isRoot == true)
            {
                isAllETDChecked = true;
                bool isChecked = (chbETDIsSelected.IsChecked == true);
                foreach (ETDFilterViewModel etdFilterView in etdFilterViewList)
                {
                    if (etdFilterView.IsRoot == false)
                    {
                        etdFilterView.IsSelected = isChecked;
                    }
                }
                isAllETDChecked = false;
                return;
            }
            else
            {                
                foreach (ETDFilterViewModel etdFilterView in etdFilterViewList)
                {
                    if (etdFilterView.IsRoot == true)
                    {
                        etdFilterView.IsSelected = null;
                    }
                }
                if (etdFilterViewList.Where(l => l.IsRoot == false).Select(l => l.IsSelected.Value).Distinct().Count() == 1)
                {
                    foreach (ETDFilterViewModel etdFilterView in etdFilterViewList)
                    {
                        if (etdFilterView.IsRoot == true)
                        {
                            etdFilterView.IsSelected = etdFilterViewList.Where(l => l.IsRoot == false).Select(l => l.IsSelected.Value).Distinct().FirstOrDefault();
                        }
                    }
                }
                return;
            }
        }        

        private void btnETDOK_Click(object sender, RoutedEventArgs e)
        {
            List<ETDFilterViewModel> etdFilterViewSelectedList = etdFilterViewList.Where(d => d.IsSelected == true && d.IsRoot == false).ToList();
            if (etdFilterViewSelectedList.Count() == 0)
            {
                return;
            }
            string etdSelected = "";
            foreach (ETDFilterViewModel etdFilterView in etdFilterViewSelectedList)
            {
                etdSelected += etdFilterView.Content + "; ";
            }
            lblETD.ToolTip = etdSelected;
            if (etdFilterViewSelectedList.Count() == 1)
            {
                lblETD.Text = etdSelected;
            }
            else
            {
                lblETD.Text = "(Multiple Items)";
            }
            popupETD.IsOpen = false;
        }

        private void btnETDCancel_Click(object sender, RoutedEventArgs e)
        {
            popupETD.IsOpen = false;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            assemblyMasterExportViewFilteredList = new ObservableCollection<AssemblyMasterExportViewModel>(assemblyMasterExportViewList.Where(s => (lineFilterViewList.Where(l => l.IsRoot == false && l.IsSelected == true).Select(l => l.Content).Contains(s.AssemblyLine))
                && (etdFilterViewList.Where(d => d.IsRoot == false && d.IsSelected == true).Select(d => d.Date).Contains(s.ETD))));
            dgMaster.ItemsSource = assemblyMasterExportViewFilteredList;
        }

        private void miRemove_Click(object sender, RoutedEventArgs e)
        {
            List<AssemblyMasterExportViewModel> assemblyMasterExportViewRemoveList = dgMaster.SelectedItems.Cast<AssemblyMasterExportViewModel>().ToList();
            if (assemblyMasterExportViewRemoveList.Count <= 0)
            {
                return;
            }
            foreach (AssemblyMasterExportViewModel assemblyMasterExportView in assemblyMasterExportViewRemoveList)
            {
                assemblyMasterExportViewList.Remove(assemblyMasterExportView);
                assemblyMasterExportViewFilteredList.Remove(assemblyMasterExportView);
            }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            List<AssemblyMasterExportViewModel> assemblyMasterExportViewReportList = dgMaster.Items.Cast<AssemblyMasterExportViewModel>().ToList();
            if (assemblyMasterExportViewReportList.Count() <= 0)
            {
                return;
            }
            AssemblyMasterReportWindow window = new AssemblyMasterReportWindow(assemblyMasterExportViewReportList, lblLine.ToolTip.ToString());
            window.Show();
        }
    }
}
