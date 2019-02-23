using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using MasterSchedule.ViewModels;
using System.Collections.ObjectModel;
using wf = System.Windows.Forms;
using MasterSchedule.Helpers;
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for SewingMasterWindow.xaml
    /// </summary>
    public partial class SockliningMasterWindow : Window
    {
        AccountModel account;
        BackgroundWorker bwLoad;
        List<OffDayModel> offDayList;
        List<OrdersModel> orderList;
        List<SockliningMasterViewModel> sockliningMasterViewList;
        public ObservableCollection<SockliningMasterViewModel> sockliningMasterViewFindList;
        List<SockliningMasterModel> sockliningMasterList;
        List<SewingMasterModel> sewingMasterList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<SockliningMasterViewModel> sockliningMasterViewToInsertList;
        List<RawMaterialModel> rawMaterialList;
        DateTime dtNothing;
        DateTime dtDefault;
        BackgroundWorker bwInsert;
        bool isEditing;
        BackgroundWorker bwReload;

        bool isSequenceEditing;
        List<String> lineSockliningEditingList;

        List<String> sockliningLineUpdateList;

        List<String> sockliningQuotaUpdateList;
        List<String> sockliningActualStartDateUpdateList;
        List<String> sockliningActualFinishDateUpdateList;
        List<String> insoleBalanceUpdateList;
        List<String> insockBalanceUpdateList;

        RawMaterialSearchBoxWindow searchBox;

        String _SECTIONID = "SOCKLINING";
        public SockliningMasterWindow(AccountModel account)
        {
            InitializeComponent();
            this.account = account;
            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            offDayList = new List<OffDayModel>();
            orderList = new List<OrdersModel>();
            sockliningMasterViewList = new List<SockliningMasterViewModel>();
            sockliningMasterViewFindList = new ObservableCollection<SockliningMasterViewModel>();
            sewingMasterList = new List<SewingMasterModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            sockliningMasterViewToInsertList = new List<SockliningMasterViewModel>();
            rawMaterialList = new List<RawMaterialModel>();
            dtNothing = new DateTime(1999, 12, 31);
            dtDefault = new DateTime(2000, 1, 1);
            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);
            isEditing = false;
            bwReload = new BackgroundWorker();
            bwReload.DoWork += new DoWorkEventHandler(bwReload_DoWork);
            bwReload.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwReload_RunWorkerCompleted);
            isSequenceEditing = false;
            lineSockliningEditingList = new List<String>();

            sockliningLineUpdateList = new List<String>();

            sockliningQuotaUpdateList = new List<String>();
            sockliningActualStartDateUpdateList = new List<String>();
            sockliningActualFinishDateUpdateList = new List<String>();
            insoleBalanceUpdateList = new List<String>();
            insockBalanceUpdateList = new List<String>();

            searchBox = new RawMaterialSearchBoxWindow();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (account.SockliningMaster == true)
            {
                colSockliningLine.IsReadOnly = false;
                colSockliningQuota.IsReadOnly = false;
                colSockliningActualStartDate.IsReadOnly = false;
                colSockliningActualFinishDate.IsReadOnly = false;
                colInsoleBalance.IsReadOnly = false;
                colInsockBalance.IsReadOnly = false;
            }

            if (account.Sortable == true)
            {
                colCountry.CanUserSort = true;
                colStyle.CanUserSort = true;
                colETD.CanUserSort = true;
            }

            //if (account.Simulation == true)
            //{
            //    btnEnableSimulation.Visibility = Visibility.Visible;
            //}

            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            offDayList = OffDayController.Select();
            orderList = OrdersController.Select();
            sockliningMasterList = SockliningMasterController.Select();
            sewingMasterList = SewingMasterController.Select();
            assemblyMasterList = AssemblyMasterController.Select();
            outsoleMasterList = OutsoleMasterController.Select();
            rawMaterialList = RawMaterialController.Select();

            int[] materialIdUpperArray = { 1, 2, 3, 4, 10 };
            int[] materialIdSewingArray = { 5, 7 };
            int[] materialIdOutsoleArray = { 6 };
            int[] materialIdAssemblyArray = { 8 };
            int[] materialIdSockliningArray = { 9 };
            for (int i = 0; i <= orderList.Count - 1; i++)
            {
                OrdersModel order = orderList[i];
                SockliningMasterViewModel sockliningMasterView = new SockliningMasterViewModel
                {
                    ProductNo = order.ProductNo,
                    ProductNoBackground = Brushes.Transparent,
                    Country = order.Country,
                    ShoeName = order.ShoeName,
                    ArticleNo = order.ArticleNo,
                    PatternNo = order.PatternNo,
                    Quantity = order.Quantity,
                    ETD = order.ETD,
                };

                MaterialArrivalViewModel materialArrivalSocklining = MaterialArrival(order.ProductNo, materialIdSockliningArray);
                sockliningMasterView.SockliningMatsArrivalOrginal = dtDefault;
                if (materialArrivalSocklining != null)
                {
                    sockliningMasterView.SockliningMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalSocklining.Date);
                    sockliningMasterView.SockliningMatsArrivalOrginal = materialArrivalSocklining.Date;
                    sockliningMasterView.SockliningMatsArrivalForeground = materialArrivalSocklining.Foreground;
                    sockliningMasterView.SockliningMatsArrivalBackground = materialArrivalSocklining.Background;
                }

                SockliningMasterModel sockliningMaster = sockliningMasterList.Where(s => s.ProductNo == order.ProductNo).FirstOrDefault();
                if (sockliningMaster != null)
                {
                    sockliningMasterView.Sequence = sockliningMaster.Sequence;
                    sockliningMasterView.SockliningLine = sockliningMaster.SockliningLine;
                    sockliningMasterView.SockliningStartDate = sockliningMaster.SockliningStartDate;
                    sockliningMasterView.SockliningFinishDate = sockliningMaster.SockliningFinishDate;
                    sockliningMasterView.SockliningQuota = sockliningMaster.SockliningQuota;
                    sockliningMasterView.SockliningActualStartDate = TimeHelper.ConvertDateToView(sockliningMaster.SockliningActualStartDate);
                    sockliningMasterView.SockliningActualFinishDate = TimeHelper.ConvertDateToView(sockliningMaster.SockliningActualFinishDate);
                    sockliningMasterView.InsoleBalance = sockliningMaster.InsoleBalance;
                    sockliningMasterView.InsockBalance = sockliningMaster.InsockBalance;
                }
                else
                {
                    sockliningMasterView.SockliningLine = "";
                    sockliningMasterView.SockliningStartDate = dtDefault;
                    sockliningMasterView.SockliningFinishDate = dtDefault;
                    sockliningMasterView.SockliningQuota = 0;
                    sockliningMasterView.SockliningActualStartDate = "";
                    sockliningMasterView.SockliningActualFinishDate = "";
                    sockliningMasterView.InsoleBalance = "";
                    sockliningMasterView.InsockBalance = "";
                }

                OutsoleMasterModel outsoleMaster = outsoleMasterList.Where(o => o.ProductNo == order.ProductNo).FirstOrDefault();
                if (outsoleMaster != null)
                {
                    sockliningMasterView.OutsoleStartDate = outsoleMaster.OutsoleStartDate;
                    sockliningMasterView.OutsoleBalance = outsoleMaster.OutsoleBalance;
                }
                else
                {
                    sockliningMasterView.OutsoleStartDate = dtDefault;
                    sockliningMasterView.OutsoleBalance = "";
                }

                SewingMasterModel sewingMaster = sewingMasterList.Where(s => s.ProductNo == order.ProductNo).FirstOrDefault();
                if (sewingMaster != null)
                {
                    sockliningMasterView.SewingStartDate = sewingMaster.SewingStartDate;
                    sockliningMasterView.SewingBalance = sewingMaster.SewingBalance;
                    sockliningMasterView.SewingLine = sewingMaster.SewingLine;
                }
                else
                {
                    sockliningMasterView.SewingStartDate = dtDefault;
                    sockliningMasterView.SewingBalance = "";
                    sockliningMasterView.SewingLine = "";
                }

                AssemblyMasterModel assemblyMaster = assemblyMasterList.Where(a => a.ProductNo == order.ProductNo).FirstOrDefault();
                if (assemblyMaster != null)
                {
                    sockliningMasterView.AssemblyStartDate = assemblyMaster.AssemblyStartDate;
                    sockliningMasterView.AssemblyBalance = assemblyMaster.AssemblyBalance;
                }
                else
                {
                    sockliningMasterView.AssemblyStartDate = dtDefault;
                    sockliningMasterView.AssemblyBalance = "";
                }

                sockliningMasterView.SockliningStartDateForeground = Brushes.Black;
                sockliningMasterView.SockliningFinishDateForeground = Brushes.Black;
                if (sockliningMasterView.SockliningStartDate < sockliningMasterView.SockliningMatsArrivalOrginal)
                {
                    sockliningMasterView.SockliningStartDateForeground = Brushes.Red;
                }
                if (sockliningMasterView.SockliningFinishDate > sockliningMasterView.ETD)
                {
                    sockliningMasterView.SockliningFinishDateForeground = Brushes.Red;
                }

                sockliningMasterViewList.Add(sockliningMasterView);
            }
            sockliningMasterViewList = sockliningMasterViewList.OrderBy(s => s.SockliningLine).ThenBy(s => s.Sequence).ToList();
        }

        private MaterialArrivalViewModel MaterialArrival(string productNo, int[] materialIdArray)
        {
            List<RawMaterialModel> rawMaterialTypeList = rawMaterialList.Where(r => r.ProductNo == productNo && materialIdArray.Contains(r.MaterialTypeId)).ToList();
            rawMaterialTypeList.RemoveAll(r => r.ETD.Date == dtDefault.Date);
            MaterialArrivalViewModel materialArrivalView = new MaterialArrivalViewModel();
            if (rawMaterialTypeList.Select(r => r.ActualDate).Count() > 0 && rawMaterialTypeList.Select(r => r.ActualDate.Date).Contains(dtDefault.Date) == false)
            {
                materialArrivalView.Date = rawMaterialTypeList.Select(r => r.ActualDate).Max();
                materialArrivalView.Foreground = Brushes.Blue;
                materialArrivalView.Background = Brushes.Transparent;
            }
            else
            {
                if (rawMaterialTypeList.Select(r => r.ETD).Count() > 0 && rawMaterialTypeList.Where(r => r.ETD.Date != dtDefault.Date).Count() > 0)
                {
                    materialArrivalView.Date = rawMaterialTypeList.Where(r => r.ActualDate.Date == dtDefault.Date).Select(r => r.ETD).Max();
                    materialArrivalView.Foreground = Brushes.Black;
                    materialArrivalView.Background = Brushes.Transparent;
                    if (materialArrivalView.Date < DateTime.Now.Date)
                    {
                        materialArrivalView.Background = Brushes.Red;
                    }
                    else
                    {
                        if (rawMaterialTypeList.Where(r => String.IsNullOrEmpty(r.Remarks) == false).Count() > 0)
                        {
                            materialArrivalView.Background = Brushes.Yellow;
                        }
                    }
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
            sockliningMasterViewFindList = new ObservableCollection<SockliningMasterViewModel>(sockliningMasterViewList);
            dgSewingMaster.ItemsSource = sockliningMasterViewFindList;
            btnCaculate.IsEnabled = true;
            btnSave.IsEnabled = true;

            //btnEnableSimulation.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnCaculate_Click(object sender, RoutedEventArgs e)
        {
            if (bwReload.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnCaculate.IsEnabled = false;
                bwReload.RunWorkerAsync();
            }

        }

        private void bwReload_DoWork(object sender, DoWorkEventArgs e)
        {
            sockliningMasterList = SockliningMasterController.Select();
        }

        private void bwReload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Load Newest Data
            foreach (SockliningMasterViewModel sockliningMasterView in sockliningMasterViewFindList)
            {
                SockliningMasterModel sockliningMaster = sockliningMasterList.Where(s => s.ProductNo == sockliningMasterView.ProductNo).FirstOrDefault();
                if (sockliningMaster != null)
                {
                    string productNo = sockliningMaster.ProductNo;
                    if (isSequenceEditing == false)
                    {
                        sockliningMasterView.Sequence = sockliningMaster.Sequence;
                    }
                    if (sockliningLineUpdateList.Contains(productNo) == false)
                    {
                        sockliningMasterView.SockliningLine = sockliningMaster.SockliningLine;
                    }
                    if (sockliningQuotaUpdateList.Contains(productNo) == false)
                    {
                        sockliningMasterView.SockliningQuota = sockliningMaster.SockliningQuota;
                    }
                    if (sockliningActualStartDateUpdateList.Contains(productNo) == false)
                    {
                        sockliningMasterView.SockliningActualStartDate = sockliningMaster.SockliningActualStartDate;
                    }
                    if (sockliningActualFinishDateUpdateList.Contains(productNo) == false)
                    {
                        sockliningMasterView.SockliningActualFinishDate = sockliningMaster.SockliningActualFinishDate;
                    }
                    if (insoleBalanceUpdateList.Contains(productNo) == false)
                    {
                        sockliningMasterView.InsoleBalance = sockliningMaster.InsoleBalance;
                    }
                    if (insockBalanceUpdateList.Contains(productNo) == false)
                    {
                        sockliningMasterView.InsockBalance = sockliningMaster.InsockBalance;
                    }
                }
            }

            //Sort By LineId, Sequence
            sockliningMasterViewFindList = new ObservableCollection<SockliningMasterViewModel>(sockliningMasterViewList.OrderBy(s => s.SockliningLine).ThenBy(s => s.Sequence));
            dgSewingMaster.ItemsSource = sockliningMasterViewFindList;
            for (int i = 0; i <= sockliningMasterViewFindList.Count - 1; i++)
            {
                sockliningMasterViewFindList[i].Sequence = i;
            }

            //Caculate
            List<String> sockliningLineList = sockliningMasterViewFindList.Select(s => s.SockliningLine).Distinct().ToList();
            foreach (string sockliningLine in sockliningLineList)
            {
                if (String.IsNullOrEmpty(sockliningLine) == false)
                {
                    List<SockliningMasterViewModel> sockliningMasterViewLineList = sockliningMasterViewFindList.Where(s => s.SockliningLine == sockliningLine).ToList();
                    if (sockliningMasterViewLineList.Count > 0)
                    {
                        //DateTime dtSockliningFinishDate = new DateTime();
                        //DateTime dtSockliningStartDate = new DateTime();
                        DateTime dtSockliningFinishDate = dtDefault;
                        DateTime dtSockliningStartDate = dtDefault;
                        int daySockliningAddition = 0;
                        for (int i = 0; i <= sockliningMasterViewLineList.Count - 1; i++)
                        {
                            #region Caculate for Socklining
                            SockliningMasterViewModel sockliningMasterView = sockliningMasterViewLineList[i];
                            int qtySockliningQuota = sockliningMasterView.SockliningQuota;
                            int optSocklining = 0;
                            if (qtySockliningQuota > 0)
                            {
                                DateTime dtSockliningStartDateTemp = TimeHelper.Convert(sockliningMasterView.SockliningActualStartDate);
                                if ((String.IsNullOrEmpty(sockliningMasterView.SockliningActualStartDate) == false && dtSockliningStartDateTemp != dtNothing)
                                     || (sockliningMasterView == sockliningMasterViewLineList.First()))
                                {
                                    dtSockliningStartDate = dtSockliningStartDateTemp;
                                }
                                else
                                {
                                    dtSockliningStartDate = dtSockliningFinishDate.AddDays(daySockliningAddition);
                                }
                                //sewingMasterView.SewingStartDate = dtSewingStartDate;
                                daySockliningAddition = 0;
                                DateTime dtSockliningFinishDateTemp = TimeHelper.Convert(sockliningMasterView.SockliningActualFinishDate);
                                if (String.IsNullOrEmpty(sockliningMasterView.SockliningActualFinishDate) == false && dtSockliningFinishDateTemp != dtNothing)
                                {
                                    dtSockliningFinishDate = dtSockliningFinishDateTemp;
                                }
                                else
                                {
                                    int qtyInsoleBalance = 0;
                                    int qtyInsockBalance = 0;
                                    sockliningMasterView.InsoleBalance = sockliningMasterView.InsoleBalance.Trim();
                                    int.TryParse(sockliningMasterView.InsoleBalance, out qtyInsoleBalance);
                                    sockliningMasterView.InsockBalance = sockliningMasterView.InsockBalance.Trim();
                                    int.TryParse(sockliningMasterView.InsockBalance, out qtyInsockBalance);
                                    int qtySockliningBalance = Math.Max(qtyInsoleBalance, qtyInsockBalance);
                                    if (qtySockliningBalance > 0)
                                    {
                                        dtSockliningFinishDate = DateTime.Now.Date.AddDays((double)(qtySockliningBalance) / (double)qtySockliningQuota);
                                        optSocklining = 1;
                                    }
                                    else
                                    {
                                        DateTime dtInsoleBalance = TimeHelper.Convert(sockliningMasterView.InsoleBalance);
                                        DateTime dtInsockBalance = TimeHelper.Convert(sockliningMasterView.InsockBalance);
                                        DateTime dtSockliningBalance = new DateTime(Math.Max(dtInsoleBalance.Ticks, dtInsockBalance.Ticks));
                                        if (String.IsNullOrEmpty(sockliningMasterView.InsoleBalance) == true || String.IsNullOrEmpty(sockliningMasterView.InsockBalance) == true)
                                        {
                                            dtSockliningFinishDate = dtSockliningStartDate.AddDays((double)sockliningMasterView.Quantity / (double)qtySockliningQuota);
                                            optSocklining = 2;
                                        }
                                        else if (String.IsNullOrEmpty(sockliningMasterView.InsoleBalance) == false && String.IsNullOrEmpty(sockliningMasterView.InsockBalance) == false && dtSockliningBalance != dtNothing)
                                        {
                                            dtSockliningFinishDate = dtSockliningBalance.AddDays(0);
                                            optSocklining = 0;
                                            daySockliningAddition = 1;
                                        }
                                    }
                                }
                                //sewingMasterView.SewingFinishDate = dtSewingFinishDate;
                                if (optSocklining == 0)
                                {
                                    sockliningMasterView.SockliningStartDate = dtSockliningStartDate;
                                    sockliningMasterView.SockliningFinishDate = dtSockliningFinishDate;
                                }
                                else if (optSocklining == 1)
                                {
                                    List<DateTime> dtCheckOffDateList1 = CheckOffDay(DateTime.Now.Date.AddDays(0), dtSockliningFinishDate);
                                    sockliningMasterView.SockliningStartDate = dtSockliningStartDate;
                                    sockliningMasterView.SockliningFinishDate = new DateTime(dtCheckOffDateList1.Last().Year, dtCheckOffDateList1.Last().Month, dtCheckOffDateList1.Last().Day,
                                        dtSockliningFinishDate.Hour, dtSockliningFinishDate.Minute, dtSockliningFinishDate.Second);
                                }
                                else if (optSocklining == 2)
                                {
                                    List<DateTime> dtCheckOffDateList2 = CheckOffDay(dtSockliningStartDate, dtSockliningFinishDate);
                                    sockliningMasterView.SockliningStartDate = new DateTime(dtCheckOffDateList2.First().Year, dtCheckOffDateList2.First().Month, dtCheckOffDateList2.First().Day,
                                        dtSockliningStartDate.Hour, dtSockliningStartDate.Minute, dtSockliningStartDate.Second);
                                    sockliningMasterView.SockliningFinishDate = new DateTime(dtCheckOffDateList2.Last().Year, dtCheckOffDateList2.Last().Month, dtCheckOffDateList2.Last().Day,
                                        dtSockliningFinishDate.Hour, dtSockliningFinishDate.Minute, dtSockliningFinishDate.Second);
                                }

                                sockliningMasterView.SockliningStartDateForeground = Brushes.Black;
                                if (sockliningMasterView.SockliningStartDate < sockliningMasterView.SockliningMatsArrivalOrginal)
                                {
                                    sockliningMasterView.SockliningStartDateForeground = Brushes.Red;
                                }
                                sockliningMasterView.SockliningFinishDateForeground = Brushes.Black;
                                if (sockliningMasterView.SockliningFinishDate > sockliningMasterView.ETD)
                                {
                                    sockliningMasterView.SockliningFinishDateForeground = Brushes.Red;
                                }

                                dtSockliningFinishDate = sockliningMasterView.SockliningFinishDate;
                            }
                            #endregion
                        }
                    }
                }
            }

            btnCaculate.IsEnabled = true;
            this.Cursor = null;
        }

        private List<DateTime> CheckOffDay(DateTime dtStartDate, DateTime dtFinishDate)
        {
            List<DateTime> dtResultList = new List<DateTime>();
            for (DateTime dt = dtStartDate.Date; dt <= dtFinishDate.Date; dt = dt.AddDays(1))
            {
                dtResultList.Add(dt);
            }
            do
            {
                dtStartDate = dtResultList.First();
                dtFinishDate = dtResultList.Last();
                for (DateTime dt = dtStartDate.Date; dt <= dtFinishDate.Date; dt = dt.AddDays(1))
                {
                    if (offDayList.Select(o => o.Date).Contains(dt) == true && dtResultList.Contains(dt) == true)
                    {
                        dtResultList.Add(dtResultList.Last().AddDays(1));
                        dtResultList.Remove(dt);
                    }
                }
            }
            while (offDayList.Where(o => dtResultList.Contains(o.Date) == true).Count() > 0);
            return dtResultList;
        }

        private void dgSewingMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.F)
            {
                bool isVisible = searchBox.IsVisible;
                if (isVisible == false)
                {
                    searchBox = new RawMaterialSearchBoxWindow();
                    searchBox.GetFindWhat = new RawMaterialSearchBoxWindow.GetString(SearchSockliningMaster);
                    searchBox.Show();
                }
            }
        }

        private void SearchSockliningMaster(string findWhat, bool isMatch, bool isShow)
        {
            sockliningMasterViewList = sockliningMasterViewList.OrderBy(s => s.SockliningLine).ThenBy(s => s.Sequence).ToList();
            sockliningMasterViewFindList = new ObservableCollection<SockliningMasterViewModel>(sockliningMasterViewList);
            if (String.IsNullOrEmpty(findWhat) == false)
            {
                if (isMatch == true)
                {
                    SockliningMasterViewModel sockliningMasterViewFind = sockliningMasterViewFindList.Where(r =>
                        r.ProductNo.ToLower() == findWhat.ToLower() || r.Country.ToLower() == findWhat.ToLower() || r.ShoeName.ToLower() == findWhat.ToLower() || r.ArticleNo.ToLower() == findWhat.ToLower() ||
                        r.PatternNo.ToLower() == findWhat.ToLower() || r.ETD.ToString("dd/MM/yyyy") == findWhat.ToLower() || r.SockliningLine.ToLower() == findWhat.ToLower()).FirstOrDefault();
                    if (sockliningMasterViewFind != null)
                    {
                        dgSewingMaster.ItemsSource = sockliningMasterViewFindList;
                        dgSewingMaster.SelectedItem = sockliningMasterViewFind;
                        dgSewingMaster.ScrollIntoView(sockliningMasterViewFind);
                        colSockliningLine.CanUserSort = true;
                    }
                    else
                    {
                        dgSewingMaster.ItemsSource = sockliningMasterViewFindList;
                        MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    if (isMatch == false)
                    {
                        if (isShow == true)
                        {
                            sockliningMasterViewFindList = new ObservableCollection<SockliningMasterViewModel>(sockliningMasterViewFindList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == true || r.Country.ToLower().Contains(findWhat.ToLower()) == true || r.ShoeName.ToLower().Contains(findWhat.ToLower()) == true || r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == true ||
                            r.PatternNo.ToLower().Contains(findWhat.ToLower()) == true || r.ETD.ToString("dd/MM/yyyy").Contains(findWhat.ToLower()) == true || r.SockliningLine.ToLower().Contains(findWhat.ToLower()) == true));
                        }
                        else
                        {
                            sockliningMasterViewFindList = new ObservableCollection<SockliningMasterViewModel>(sockliningMasterViewFindList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == false && r.Country.ToLower().Contains(findWhat.ToLower()) == false && r.ShoeName.ToLower().Contains(findWhat.ToLower()) == false && r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == false &&
                            r.PatternNo.ToLower().Contains(findWhat.ToLower()) == false && r.ETD.ToString("dd/MM/yyyy").Contains(findWhat.ToLower()) == false && r.SockliningLine.ToLower().Contains(findWhat.ToLower()) == false));
                        }

                        if (sockliningMasterViewFindList.Count > 0)
                        {
                            dgSewingMaster.ItemsSource = sockliningMasterViewFindList;
                            colSockliningLine.CanUserSort = false;
                        }
                        else
                        {
                            dgSewingMaster.ItemsSource = sockliningMasterViewFindList;
                            MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                            sockliningMasterViewFindList = new ObservableCollection<SockliningMasterViewModel>(sockliningMasterViewList);
                            dgSewingMaster.ItemsSource = sockliningMasterViewFindList;
                        }
                    }
                }
            }
            else
            {
                colSockliningLine.CanUserSort = true;
                dgSewingMaster.ItemsSource = sockliningMasterViewFindList;
            }
        }

        private void dgSewingMaster_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            SockliningMasterViewModel sockliningMasterView = (SockliningMasterViewModel)e.Row.Item;
            if (sockliningMasterView == null)
            {
                return;
            }

            string productNo = sockliningMasterView.ProductNo;
            if (e.Column == colSockliningLine || e.Column == colSockliningQuota || e.Column == colSockliningActualStartDate ||
                e.Column == colSockliningActualFinishDate || e.Column == colInsoleBalance || e.Column == colInsockBalance)
            {
                lineSockliningEditingList.Add(sockliningMasterView.SockliningLine);
                if (e.Column == colSockliningLine)
                {
                    sockliningLineUpdateList.Add(productNo);
                }
                if (e.Column == colSockliningQuota)
                {
                    sockliningQuotaUpdateList.Add(productNo);
                }
                if (e.Column == colSockliningActualStartDate)
                {
                    sockliningActualStartDateUpdateList.Add(productNo);
                }
                if (e.Column == colSockliningActualFinishDate)
                {
                    sockliningActualFinishDateUpdateList.Add(productNo);
                }
                if (e.Column == colInsoleBalance)
                {
                    insoleBalanceUpdateList.Add(productNo);
                }
                if (e.Column == colInsockBalance)
                {
                    insockBalanceUpdateList.Add(productNo);
                }
            }

            if (e.Column == colSockliningLine)
            {
                //SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)e.Row.Item;
                string sockliningLine = sockliningMasterView.SockliningLine;
                if (String.IsNullOrEmpty(sockliningLine) == true)
                {
                    return;
                }
                int sockliningSequence = 0;
                if (sockliningMasterViewList.Where(s => s.SockliningLine == sockliningLine).Count() > 0)
                {
                    sockliningSequence = sockliningMasterViewList.Where(s => s.SockliningLine == sockliningLine).Select(s => s.Sequence).Max() + 1;
                }
                sockliningMasterView.Sequence = sockliningSequence;
                isSequenceEditing = true;
            }
            if (e.Column == colSockliningActualStartDate || e.Column == colSockliningActualFinishDate)
            {
                TextBox txtElement = (TextBox)e.EditingElement as TextBox;
                if (String.IsNullOrEmpty(txtElement.Text) == false && TimeHelper.Convert(txtElement.Text) == dtNothing)
                {
                    txtElement.Foreground = Brushes.Red;
                    txtElement.Text = "!";
                    txtElement.SelectAll();
                }
            }
            isEditing = false;
        }

        private void dgSewingMaster_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column == colSockliningLine)
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection.Value == ListSortDirection.Descending)
                {
                    sockliningMasterViewList = sockliningMasterViewList.OrderBy(s => s.SockliningLine).ThenBy(s => s.Sequence).ToList();
                    sockliningMasterViewFindList = new ObservableCollection<SockliningMasterViewModel>(sockliningMasterViewList.OrderBy(s => s.SockliningLine).ThenBy(s => s.Sequence));
                }
                else
                {
                    sockliningMasterViewList = sockliningMasterViewList.OrderByDescending(s => s.SockliningLine).ThenBy(s => s.Sequence).ToList();
                    sockliningMasterViewFindList = new ObservableCollection<SockliningMasterViewModel>(sockliningMasterViewList.OrderByDescending(s => s.SockliningLine).ThenBy(s => s.Sequence));
                }
                dgSewingMaster.ItemsSource = sockliningMasterViewFindList;
                for (int i = 0; i <= sockliningMasterViewFindList.Count - 1; i++)
                {
                    sockliningMasterViewFindList[i].Sequence = i;
                }
                dgSewingMaster.ScrollIntoView(sockliningMasterViewFindList.Where(o => String.IsNullOrEmpty(o.SockliningLine) == false).FirstOrDefault());
                e.Handled = true;
            }
        }

        private void dgSewingMaster_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false && simulationMode == false)
            {
                this.Cursor = Cursors.Wait;
                sockliningMasterViewToInsertList = dgSewingMaster.Items.OfType<SockliningMasterViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false && simulationMode == false)
            {
                this.Cursor = Cursors.Wait;
                sockliningMasterViewToInsertList = dgSewingMaster.Items.OfType<SockliningMasterViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (SockliningMasterViewModel sockliningMaster in sockliningMasterViewToInsertList)
            {
                string productNo = sockliningMaster.ProductNo;
                string sockliningLine = sockliningMaster.SockliningLine;
                SockliningMasterModel model = new SockliningMasterModel
                {
                    ProductNo = sockliningMaster.ProductNo,
                    Sequence = sockliningMaster.Sequence,
                    SockliningLine = sockliningMaster.SockliningLine,
                    SockliningStartDate = sockliningMaster.SockliningStartDate,
                    SockliningFinishDate = sockliningMaster.SockliningFinishDate,
                    SockliningQuota = sockliningMaster.SockliningQuota,
                    SockliningActualStartDate = sockliningMaster.SockliningActualStartDate,
                    SockliningActualFinishDate = sockliningMaster.SockliningActualFinishDate,
                    InsoleBalance = sockliningMaster.InsoleBalance,
                    InsockBalance = sockliningMaster.InsockBalance,

                    IsSequenceUpdate = false,
                    IsSockliningLineUpdate = false,
                    IsSockliningStartDateUpdate = false,
                    IsSockliningFinishDateUpdate = false,
                    IsSockliningQuotaUpdate = false,
                    IsSockliningActualStartDateUpdate = false,
                    IsSockliningActualFinishDateUpdate = false,
                    IsInsoleBalanceUpdate = false,
                    IsInsockBalanceUpdate = false,
                };

                model.IsSequenceUpdate = isSequenceEditing;

                model.IsSockliningLineUpdate = sockliningLineUpdateList.Contains(productNo);
                model.IsSockliningStartDateUpdate = lineSockliningEditingList.Contains(sockliningLine);
                model.IsSockliningFinishDateUpdate = lineSockliningEditingList.Contains(sockliningLine);
                model.IsSockliningQuotaUpdate = sockliningQuotaUpdateList.Contains(productNo);
                model.IsSockliningActualStartDateUpdate = sockliningActualStartDateUpdateList.Contains(productNo);
                model.IsSockliningActualFinishDateUpdate = sockliningActualFinishDateUpdateList.Contains(productNo);
                model.IsInsoleBalanceUpdate = insoleBalanceUpdateList.Contains(productNo);
                model.IsInsockBalanceUpdate = insockBalanceUpdateList.Contains(productNo);

                if (model.IsSequenceUpdate == true ||
                    model.IsSockliningLineUpdate == true ||
                    model.IsSockliningStartDateUpdate == true ||
                    model.IsSockliningFinishDateUpdate == true ||
                    model.IsSockliningQuotaUpdate == true ||
                    model.IsSockliningActualStartDateUpdate == true ||
                    model.IsSockliningActualFinishDateUpdate == true ||
                    model.IsInsoleBalanceUpdate == true ||
                    model.IsInsockBalanceUpdate)
                {
                    SockliningMasterController.Insert_2(model);

                    // Insert ProductNoRevise
                    //var productNoReviseInsertModel = new ProductNoReviseModel()
                    //{
                    //    ProductNo = model.ProductNo,
                    //    ReviseDate = DateTime.Now.Date,
                    //    SectionId = _SECTIONID
                    //};
                    //ProductNoReviseController.Insert(productNoReviseInsertModel);
                }
            }
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSave.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            isSequenceEditing = false;
            lineSockliningEditingList.Clear();

            sockliningLineUpdateList.Clear();

            sockliningQuotaUpdateList.Clear();
            sockliningActualStartDateUpdateList.Clear();
            sockliningActualFinishDateUpdateList.Clear();
            insoleBalanceUpdateList.Clear();
            insockBalanceUpdateList.Clear();

            
            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void dgSewingMaster_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isEditing = true;
        }

        private List<SockliningMasterViewModel> sockliningMasterViewSelectList = new List<SockliningMasterViewModel>();
        private void dgSewingMaster_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popup.IsOpen = false;
            sockliningMasterViewSelectList.Clear();
            var dataGrid = (DataGrid)sender;
            if (dataGrid != null)
            {
                foreach (DataGridCellInfo cellInfo in dataGrid.SelectedCells)
                {
                    sockliningMasterViewSelectList.Add((SockliningMasterViewModel)cellInfo.Item);
                }
                sockliningMasterViewSelectList = sockliningMasterViewSelectList.Distinct().ToList();
            }
        }

        private void dgSewingMaster_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && account.SockliningMaster == true && isEditing == false)
            //if (e.LeftButton == MouseButtonState.Pressed && isEditing == false)
            {
                var dataGrid = (DataGrid)sender;
                if (dataGrid != null)
                {
                    if (e.OriginalSource.GetType() == typeof(Thumb))
                    {
                        return;
                    }
                    if ((SockliningMasterViewModel)dataGrid.CurrentItem != null && sockliningMasterViewSelectList.Contains((SockliningMasterViewModel)dataGrid.CurrentItem) == false)
                    {
                        sockliningMasterViewSelectList.Add((SockliningMasterViewModel)dataGrid.CurrentItem);
                    }
                    if (sockliningMasterViewSelectList.Count > 0)
                    {
                        listView.ItemsSource = sockliningMasterViewSelectList;
                        popup.PlacementTarget = lblPopup;
                        DragDrop.DoDragDrop(dataGrid, sockliningMasterViewSelectList, DragDropEffects.Move);
                    }
                }
            }
        }

        private void dgSewingMaster_DragLeave(object sender, DragEventArgs e)
        {
            FrameworkElement frameworkElement = (FrameworkElement)e.OriginalSource;

            if (frameworkElement != null && frameworkElement.DataContext != null
                && frameworkElement.DataContext.GetType() == typeof(SockliningMasterViewModel))
            {
                SockliningMasterViewModel sockliningMasterView = (SockliningMasterViewModel)frameworkElement.DataContext;
                dgSewingMaster.SelectedItem = sockliningMasterView;
                dgSewingMaster.ScrollIntoView(sockliningMasterView);
            }
            else
            {
                return;
            }
            Point point = new Point(wf.Control.MousePosition.X, wf.Control.MousePosition.Y);
            Point point1 = dgSewingMaster.PointFromScreen(point);
            popup.HorizontalOffset = point1.X + 5;
            popup.VerticalOffset = point1.Y + 5;
            popup.IsOpen = true;
        }

        private void dgSewingMaster_Drop(object sender, DragEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                popup.IsOpen = false;
                SockliningMasterViewModel sockliningMasterView = (SockliningMasterViewModel)dataGrid.SelectedItem;
                int index = dataGrid.Items.IndexOf(sockliningMasterView);
                int indexFirst = dataGrid.Items.IndexOf(sockliningMasterViewSelectList.First());
                int indexLast = dataGrid.Items.IndexOf(sockliningMasterViewSelectList.Last());
                if (index < indexFirst && index < indexLast)
                {
                    for (int i = sockliningMasterViewSelectList.Count - 1; i >= 0; i = i - 1)
                    {
                        sockliningMasterViewFindList.Remove(sockliningMasterViewSelectList[i]);
                        sockliningMasterViewFindList.Insert(index, sockliningMasterViewSelectList[i]);
                        sockliningMasterViewSelectList[i].Sequence = sockliningMasterView.Sequence + i;
                    }
                    for (int i = index + sockliningMasterViewSelectList.Count; i <= sockliningMasterViewFindList.Count - 1; i++)
                    {
                        sockliningMasterViewFindList[i].Sequence = sockliningMasterViewFindList[i].Sequence + sockliningMasterViewSelectList.Count;
                    }
                    isSequenceEditing = true;
                }
                else if (index > indexFirst && index > indexLast)
                {
                    for (int i = 0; i <= sockliningMasterViewSelectList.Count - 1; i = i + 1)
                    {
                        sockliningMasterViewFindList.Remove(sockliningMasterViewSelectList[i]);
                        sockliningMasterViewFindList.Insert(index - 1, sockliningMasterViewSelectList[i]);
                        sockliningMasterViewSelectList[i].Sequence = sockliningMasterView.Sequence + i;
                    }
                    for (int i = index; i <= sockliningMasterViewFindList.Count - 1; i++)
                    {
                        sockliningMasterViewFindList[i].Sequence = sockliningMasterViewFindList[i].Sequence + sockliningMasterViewSelectList.Count;
                    }
                    isSequenceEditing = true;
                }
                dgSewingMaster.SelectedItems.Clear();
            }
        }

        private void dgSewingMaster_DragOver(object sender, DragEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            DependencyObject dependencyObject = dataGrid;
            while (dependencyObject.GetType() != typeof(ScrollViewer))
            {
                dependencyObject = VisualTreeHelper.GetChild(dependencyObject, 0);
            }
            ScrollViewer scrollViewer = dependencyObject as ScrollViewer;
            if (scrollViewer == null)
            {
                return;
            }

            double toleranceHeight = 60;
            double verticalPosition = e.GetPosition(dataGrid).Y;
            //double offset = 5;

            if (verticalPosition < toleranceHeight) // Top of visible list? 
            {
                //Scroll up.
                scrollViewer.LineUp();
                //scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset);
            }
            else if (verticalPosition > dgSewingMaster.ActualHeight - toleranceHeight) //Bottom of visible list? 
            {
                //Scroll down.
                scrollViewer.LineDown();
                //scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if ((isSequenceEditing == true || lineSockliningEditingList.Count > 0 ||
                sockliningLineUpdateList.Count > 0 || sockliningQuotaUpdateList.Count > 0 || sockliningActualStartDateUpdateList.Count > 0 ||
                sockliningActualFinishDateUpdateList.Count > 0 || insoleBalanceUpdateList.Count > 0 || insockBalanceUpdateList.Count > 0) && simulationMode == false)
            {
                MessageBoxResult result = MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (bwInsert.IsBusy == false)
                    {
                        e.Cancel = true;
                        this.Cursor = Cursors.Wait;
                        sockliningMasterViewToInsertList = dgSewingMaster.Items.OfType<SockliningMasterViewModel>().ToList();
                        btnSave.IsEnabled = false;
                        bwInsert.RunWorkerAsync();
                    }
                }
                else if (result == MessageBoxResult.No)
                { }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void dgSewingMaster_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            foreach (DataGridCellInfo dataGridCellInfo in e.RemovedCells)
            {
                if (dataGridCellInfo.Item != DependencyProperty.UnsetValue)
                {
                    SockliningMasterViewModel sockliningMasterView = (SockliningMasterViewModel)dataGridCellInfo.Item;
                    if (sockliningMasterView != null)
                    {
                        sockliningMasterView.ProductNoBackground = Brushes.Transparent;
                    }
                }
            }
            foreach (DataGridCellInfo dataGridCellInfo in e.AddedCells)
            {
                SockliningMasterViewModel sockliningMasterView = (SockliningMasterViewModel)dataGridCellInfo.Item;
                if (sockliningMasterView != null)
                {
                    sockliningMasterView.ProductNoBackground = Brushes.RoyalBlue;
                }
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            searchBox.Topmost = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            searchBox.Topmost = false;
        }

        bool simulationMode = false;
        string title = "";
        private void btnEnableSimulation_Click(object sender, RoutedEventArgs e)
        {
            dgSewingMaster.AlternatingRowBackground = Brushes.White;
            dgSewingMaster.RowBackground = Brushes.White;

            title = "Master Schedule - Socklining Simulation File";
            this.Title = title;

            simulationMode = true;

            //btnEnableSimulation.IsEnabled = false;
            //btnDisableSimulation.IsEnabled = true;
            //btnDisableSimulation.Visibility = Visibility.Visible;

            ctmTranfer.Visibility = Visibility.Visible;
            btnSave.IsEnabled = false;
        }

        private void btnDisableSimulation_Click(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                dgSewingMaster.ItemsSource = null;
                simulationMode = true;
                dgSewingMaster.AlternatingRowBackground = Brushes.LightCyan;
                dgSewingMaster.RowBackground = Brushes.White;

                title = "Master Schedule - Socklining Master File";
                this.Title = title;

                ctmTranfer.Visibility = Visibility.Collapsed;
                simulationMode = false;

                //btnDisableSimulation.IsEnabled = false;

                btnSave.IsEnabled = false;
                btnCaculate.IsEnabled = false;

                sockliningMasterViewList = new List<SockliningMasterViewModel>();
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void miTranfer_Click(object sender, RoutedEventArgs e)
        {
            sockliningMasterViewToInsertList = dgSewingMaster.SelectedItems.OfType<SockliningMasterViewModel>().ToList();
            if (sockliningMasterViewToInsertList.Count <= 0 ||
                MessageBox.Show("Confirm Tranfer?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            if (bwInsert.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwInsert.RunWorkerAsync();
            }
        }   

    }
}
