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
    public partial class SockliningMasterFilterWindow : Window
    {
        BackgroundWorker bwLoad;
        List<OrdersModel> orderList;
        List<RawMaterialModel> rawMaterialList;
        List<SockliningMasterModel> sockliningMasterList;
        List<SewingMasterModel> sewingMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<LineFilterViewModel> lineFilterViewList;
        List<ETDFilterViewModel> etdFilterViewList;
        List<SockliningMasterExportViewModel> sockliningMasterExportViewList;
        ObservableCollection<SockliningMasterExportViewModel> sockliningMasterExportViewFilteredList;
        DateTime dtDefault;
        DateTime dtNothing;
        public SockliningMasterFilterWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            orderList = new List<OrdersModel>();
            rawMaterialList = new List<RawMaterialModel>();
            sockliningMasterList = new List<SockliningMasterModel>();
            sewingMasterList = new List<SewingMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            lineFilterViewList = new List<LineFilterViewModel>();
            etdFilterViewList = new List<ETDFilterViewModel>();
            sockliningMasterExportViewList = new List<SockliningMasterExportViewModel>();
            sockliningMasterExportViewFilteredList = new ObservableCollection<SockliningMasterExportViewModel>();
            dtDefault = new DateTime(2000, 1, 1);
            dtNothing = new DateTime(1999, 12, 31);
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
            sockliningMasterList = SockliningMasterController.Select();
            sewingMasterList = SewingMasterController.Select();
            outsoleMasterList = OutsoleMasterController.Select();
            assemblyMasterList = AssemblyMasterController.Select();

            //sewingMasterList.RemoveAll(s => DateTimeHelper.Create(s.SewingBalance) != dtDefault && DateTimeHelper.Create(s.SewingBalance) != dtNothing);
            sockliningMasterList = sockliningMasterList.OrderBy(s => s.Sequence).ToList();

            int[] materialIdUpperArray = { 1, 2, 3, 4, 10 };
            int[] materialIdSewingArray = { 5, 7 };
            int[] materialIdOutsoleArray = { 6 };
            int[] materialIdAssemblyArray = { 8 };
            int[] materialIdSockliningArray = { 9 };

            foreach (SockliningMasterModel sockliningMaster in sockliningMasterList)
            {
                SockliningMasterExportViewModel sockliningMasterExportView = new SockliningMasterExportViewModel();
                sockliningMasterExportView.Sequence = sockliningMaster.Sequence;
                sockliningMasterExportView.ProductNo = sockliningMaster.ProductNo;
                OrdersModel order = orderList.Where(o => o.ProductNo == sockliningMaster.ProductNo).FirstOrDefault();
                if (order != null)
                {
                    sockliningMasterExportView.Country = order.Country;
                    sockliningMasterExportView.ShoeName = order.ShoeName;
                    sockliningMasterExportView.ArticleNo = order.ArticleNo;
                    sockliningMasterExportView.PatternNo = order.PatternNo;
                    sockliningMasterExportView.Quantity = order.Quantity;
                    sockliningMasterExportView.ETD = order.ETD;
                }

                MaterialArrivalViewModel materialArrivalSocklining = MaterialArrival(order.ProductNo, materialIdSockliningArray);
                sockliningMasterExportView.IsSockliningMatsArrivalOk = false;
                if (materialArrivalSocklining != null)
                {
                    sockliningMasterExportView.SockliningMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalSocklining.Date);
                    sockliningMasterExportView.IsSockliningMatsArrivalOk = materialArrivalSocklining.IsMaterialArrivalOk;
                }

                sockliningMasterExportView.SockliningLine = sockliningMaster.SockliningLine;
                sockliningMasterExportView.SockliningQuota = sockliningMaster.SockliningQuota;
                sockliningMasterExportView.SockliningStartDate = sockliningMaster.SockliningStartDate;
                sockliningMasterExportView.SockliningFinishDate = sockliningMaster.SockliningFinishDate;
                sockliningMasterExportView.InsoleBalance = sockliningMaster.InsoleBalance;
                sockliningMasterExportView.InsockBalance = sockliningMaster.InsockBalance;

                SewingMasterModel sewingMaster = sewingMasterList.Where(o => o.ProductNo == sockliningMaster.ProductNo).FirstOrDefault();
                if (sewingMaster != null)
                {
                    sockliningMasterExportView.SewingStartDate = sewingMaster.SewingStartDate;
                    sockliningMasterExportView.SewingBalance = sewingMaster.SewingBalance;
                }
                else
                {
                    sockliningMasterExportView.SewingStartDate = dtDefault;
                    sockliningMasterExportView.SewingBalance = "";
                }

                OutsoleMasterModel outsoleMaster = outsoleMasterList.Where(o => o.ProductNo == sockliningMaster.ProductNo).FirstOrDefault();
                if (outsoleMaster != null)
                {
                    sockliningMasterExportView.OutsoleStartDate = outsoleMaster.OutsoleStartDate;
                    sockliningMasterExportView.OutsoleBalance = outsoleMaster.OutsoleBalance;
                }
                else
                {
                    sockliningMasterExportView.OutsoleStartDate = dtDefault;
                    sockliningMasterExportView.OutsoleBalance = "";
                }

                AssemblyMasterModel assemblyMaster = assemblyMasterList.Where(o => o.ProductNo == sockliningMaster.ProductNo).FirstOrDefault();
                if (assemblyMaster != null)
                {
                    sockliningMasterExportView.AssemblyStartDate = assemblyMaster.AssemblyStartDate;
                }
                else
                {
                    sockliningMasterExportView.AssemblyStartDate = dtDefault;
                }

                sockliningMasterExportViewList.Add(sockliningMasterExportView);
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
            foreach (string line in sockliningMasterExportViewList.Select(s => s.SockliningLine).OrderBy(l => l).Distinct())
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
            foreach (DateTime etd in sockliningMasterExportViewList.Select(s => s.ETD).OrderBy(d => d).Distinct())
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

            sockliningMasterExportViewFilteredList = new ObservableCollection<SockliningMasterExportViewModel>(sockliningMasterExportViewList);
            //dgMaster.ItemsSource = sockliningMasterExportViewFilteredList;
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
            sockliningMasterExportViewFilteredList = new ObservableCollection<SockliningMasterExportViewModel>(sockliningMasterExportViewList.Where(s => (lineFilterViewList.Where(l => l.IsRoot == false && l.IsSelected == true).Select(l => l.Content).Contains(s.SockliningLine))
                && (etdFilterViewList.Where(d => d.IsRoot == false && d.IsSelected == true).Select(d => d.Date).Contains(s.ETD))));
            dgMaster.ItemsSource = sockliningMasterExportViewFilteredList;
        }

        private void miRemove_Click(object sender, RoutedEventArgs e)
        {
            List<SockliningMasterExportViewModel> sockliningMasterExportViewRemoveList = dgMaster.SelectedItems.Cast<SockliningMasterExportViewModel>().ToList();
            if (sockliningMasterExportViewRemoveList.Count <= 0)
            {
                return;
            }
            foreach (SockliningMasterExportViewModel sockliningMasterExportView in sockliningMasterExportViewRemoveList)
            {
                sockliningMasterExportViewList.Remove(sockliningMasterExportView);
                sockliningMasterExportViewFilteredList.Remove(sockliningMasterExportView);
            }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            List<SockliningMasterExportViewModel> sockliningMasterExportViewReportList = dgMaster.Items.Cast<SockliningMasterExportViewModel>().ToList();
            if (sockliningMasterExportViewReportList.Count() <= 0)
            {
                return;
            }
            SockliningMasterReportWindow window = new SockliningMasterReportWindow(sockliningMasterExportViewReportList, lblLine.ToolTip.ToString());
            window.Show();
        }
    }
}
