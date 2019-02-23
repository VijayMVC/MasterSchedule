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
    public partial class AssemblyMasterWindow : Window
    {
        AccountModel account;
        BackgroundWorker bwLoad;
        List<OffDayModel> offDayList;
        List<OrdersModel> orderList;
        List<AssemblyMasterViewModel> assemblyMasterViewList;
        public ObservableCollection<AssemblyMasterViewModel> assemblyMasterViewFindList;
        List<SewingMasterModel> sewingMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<SockliningMasterModel> sockliningMasterList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<AssemblyMasterViewModel> assemblyMasterViewToInsertList;
        List<RawMaterialModel> rawMaterialList;
        DateTime dtNothing;
        DateTime dtDefault;
        BackgroundWorker bwInsert;
        bool isEditing;
        BackgroundWorker bwReload;
        bool isSequenceEditing;

        List<String> lineAssemblyEditingList;

        List<String> assemblyLineUpdateList;

        List<String> assemblyQuotaUpdateList;
        List<String> assemblyActualStartDateUpdateList;
        List<String> assemblyActualFinishDateUpdateList;
        List<String> assemblyBalanceUpdateList;

        RawMaterialSearchBoxWindow searchBox;
        List<AssemblyReleaseModel> assemblyReleaseList;
        List<ProductionMemoModel> productionMemoList;

        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList;

        String _SECTIONID = "ASSY";
        public AssemblyMasterWindow(AccountModel account)
        {
            InitializeComponent();
            this.account = account;
            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            offDayList = new List<OffDayModel>();
            orderList = new List<OrdersModel>();
            assemblyMasterViewList = new List<AssemblyMasterViewModel>();
            assemblyMasterViewFindList = new ObservableCollection<AssemblyMasterViewModel>();
            sewingMasterList = new List<SewingMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            sockliningMasterList = new List<SockliningMasterModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            assemblyMasterViewToInsertList = new List<AssemblyMasterViewModel>();
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
            lineAssemblyEditingList = new List<String>();

            assemblyLineUpdateList = new List<String>();

            assemblyQuotaUpdateList = new List<String>();
            assemblyActualStartDateUpdateList = new List<String>();
            assemblyActualFinishDateUpdateList = new List<String>();
            assemblyBalanceUpdateList = new List<String>();

            searchBox = new RawMaterialSearchBoxWindow();
            assemblyReleaseList = new List<AssemblyReleaseModel>();
            productionMemoList = new List<ProductionMemoModel>();

            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleMaterialList = new List<OutsoleMaterialModel>();
            outsoleReleaseMaterialList = new List<OutsoleReleaseMaterialModel>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (account.AssemblyMaster == true)
            {
                colAssemblyLine.IsReadOnly = false;
                colAssemblyQuota.IsReadOnly = false;
                colAssemblyActualStartDate.IsReadOnly = false;
                colAssemblyActualFinishDate.IsReadOnly = false;
                colAssemblyBalance.IsReadOnly = false;
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
            sewingMasterList = SewingMasterController.Select();
            outsoleMasterList = OutsoleMasterController.Select();
            sockliningMasterList = SockliningMasterController.Select();
            assemblyMasterList = AssemblyMasterController.Select();
            rawMaterialList = RawMaterialController.Select();
            assemblyReleaseList = AssemblyReleaseController.SelectByAssemblyMaster();
            productionMemoList = ProductionMemoController.Select();

            outsoleRawMaterialList = OutsoleRawMaterialController.Select();
            outsoleMaterialList = OutsoleMaterialController.Select();

            outsoleReleaseMaterialList = OutsoleReleaseMaterialController.SelectByOutsoleMaster();


            int[] materialIdUpperArray = { 1, 2, 3, 4, 10 };
            int[] materialIdSewingArray = { 5, 7 };
            int[] materialIdOutsoleArray = { 6 };
            int[] materialIdAssemblyArray = { 8 };
            int[] materialIdSockliningArray = { 9 };
            int[] materialIdCartonArray = { 11 };
            for (int i = 0; i <= orderList.Count - 1; i++)
            {
                OrdersModel order = orderList[i];
                AssemblyMasterViewModel assemblyMasterView = new AssemblyMasterViewModel
                {
                    ProductNo = order.ProductNo,
                    ProductNoBackground = Brushes.Transparent,
                    Country = order.Country,
                    ShoeName = order.ShoeName,
                    ArticleNo = order.ArticleNo,
                    LastCode = order.LastCode,
                    Quantity = order.Quantity,
                    ETD = order.ETD,
                };

                string memoId = "";
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
                assemblyMasterView.MemoId = memoId;

                //// Fix code
                List<OutsoleMaterialModel> outsoleMaterialQuantityZero = new List<OutsoleMaterialModel>();
                outsoleMaterialQuantityZero = outsoleMaterialList.Where(w => w.Quantity == 0 && w.ProductNo == order.ProductNo).ToList();
                List<int> outsoleSupplierIdList = new List<int>();
                outsoleSupplierIdList = outsoleMaterialQuantityZero.Select(s => s.OutsoleSupplierId).Distinct().ToList();
                List<OutsoleRawMaterialModel> outsoleRawMaterialNotSupplied = new List<OutsoleRawMaterialModel>();
                foreach (int supplierId in outsoleSupplierIdList)
                {
                    OutsoleRawMaterialModel outsoleRawMaterialBySupplierId = new OutsoleRawMaterialModel();
                    outsoleRawMaterialBySupplierId = outsoleRawMaterialList.FirstOrDefault(f => f.ProductNo == order.ProductNo && f.OutsoleSupplierId == supplierId);
                    outsoleRawMaterialNotSupplied.Add(outsoleRawMaterialBySupplierId);
                }
           
                if (outsoleRawMaterialNotSupplied.Count > 0)
                {
                    List<RawMaterialModel> rawMaterialTypeList = rawMaterialList.Where(r => r.ProductNo == order.ProductNo && r.MaterialTypeId == 6).ToList();
                    OutsoleRawMaterialModel outsoleRaw = outsoleRawMaterialNotSupplied.OrderBy(o => o.ETD).LastOrDefault();
                    assemblyMasterView.OSMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", outsoleRaw.ETD);
                    assemblyMasterView.OSMatsArrivalForeground = Brushes.Black;
                    assemblyMasterView.OSMatsArrivalBackground = Brushes.Transparent;
                    if (outsoleRaw.ETD < DateTime.Now.Date)
                    {
                        assemblyMasterView.OSMatsArrivalBackground = Brushes.Red;
                    }
                    else
                    {
                        if (rawMaterialTypeList.Where(r => String.IsNullOrEmpty(r.Remarks) == false).Count() > 0)
                        {
                            assemblyMasterView.OSMatsArrivalBackground = Brushes.Yellow;
                        }
                    }
                }
                else
                {
                    // Normal code
                    MaterialArrivalViewModel materialArrivalOutsole = MaterialArrival(order.ProductNo, materialIdOutsoleArray);
                    if (materialArrivalOutsole != null)
                    {
                        assemblyMasterView.OSMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalOutsole.Date);
                        assemblyMasterView.OSMatsArrivalForeground = materialArrivalOutsole.Foreground;
                        assemblyMasterView.OSMatsArrivalBackground = materialArrivalOutsole.Background;
                    }
                }

                MaterialArrivalViewModel materialArrivalAssembly = MaterialArrival(order.ProductNo, materialIdAssemblyArray);
                if (materialArrivalAssembly != null)
                {
                    assemblyMasterView.AssemblyMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalAssembly.Date);
                    assemblyMasterView.AssemblyMatsArrivalForeground = materialArrivalAssembly.Foreground;
                    assemblyMasterView.AssemblyMatsArrivalBackground = materialArrivalAssembly.Background;
                }

                MaterialArrivalViewModel materialArrivalSocklining = MaterialArrival(order.ProductNo, materialIdSockliningArray);
                if (materialArrivalSocklining != null)
                {
                    assemblyMasterView.SockliningMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalSocklining.Date);
                    assemblyMasterView.SockliningMatsArrivalForeground = materialArrivalSocklining.Foreground;
                    assemblyMasterView.SockliningMatsArrivalBackground = materialArrivalSocklining.Background;
                }

                MaterialArrivalViewModel materialArrivalCarton = MaterialArrival(order.ProductNo, materialIdCartonArray);
                if (materialArrivalCarton != null)
                {
                    assemblyMasterView.CartonMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalCarton.Date);
                    assemblyMasterView.CartonMatsArrivalForeground = materialArrivalCarton.Foreground;
                    assemblyMasterView.CartonMatsArrivalBackground = materialArrivalCarton.Background;
                }

                AssemblyMasterModel assemblyMaster = assemblyMasterList.FirstOrDefault(f => f.ProductNo == order.ProductNo);
                if (assemblyMaster != null)
                {
                    assemblyMasterView.Sequence = assemblyMaster.Sequence;
                    assemblyMasterView.AssemblyLine = assemblyMaster.AssemblyLine;
                    assemblyMasterView.AssemblyStartDate = assemblyMaster.AssemblyStartDate;
                    assemblyMasterView.AssemblyFinishDate = assemblyMaster.AssemblyFinishDate;
                    assemblyMasterView.AssemblyQuota = assemblyMaster.AssemblyQuota;
                    assemblyMasterView.AssemblyActualStartDate = TimeHelper.ConvertDateToView(assemblyMaster.AssemblyActualStartDate);
                    assemblyMasterView.AssemblyActualFinishDate = TimeHelper.ConvertDateToView(assemblyMaster.AssemblyActualFinishDate);
                    assemblyMasterView.AssemblyBalance = assemblyMaster.AssemblyBalance;
                }
                else
                {
                    assemblyMasterView.AssemblyLine = "";
                    assemblyMasterView.AssemblyStartDate = dtDefault;
                    assemblyMasterView.AssemblyFinishDate = dtDefault;
                    assemblyMasterView.AssemblyQuota = 0;
                    assemblyMasterView.AssemblyActualStartDate = "";
                    assemblyMasterView.AssemblyActualFinishDate = "";
                    assemblyMasterView.AssemblyBalance = "";
                }

                SewingMasterModel sewingMaster = sewingMasterList.FirstOrDefault(f => f.ProductNo == order.ProductNo);
                if (sewingMaster != null)
                {
                    assemblyMasterView.SewingLine = sewingMaster.SewingLine;
                    assemblyMasterView.SewingStartDate = sewingMaster.SewingStartDate;
                    assemblyMasterView.SewingFinishDate = sewingMaster.SewingFinishDate;
                    assemblyMasterView.SewingQuota = sewingMaster.SewingQuota;
                    assemblyMasterView.SewingBalance = sewingMaster.SewingBalance;
                }
                else
                {
                    assemblyMasterView.SewingLine = "";
                    assemblyMasterView.SewingStartDate = dtDefault;
                    assemblyMasterView.SewingFinishDate = dtDefault;
                    assemblyMasterView.SewingQuota = 0;
                    assemblyMasterView.SewingBalance = "";
                }

                OutsoleMasterModel outsoleMaster = outsoleMasterList.FirstOrDefault(f => f.ProductNo == order.ProductNo);
                if (outsoleMaster != null)
                {
                    assemblyMasterView.OutsoleFinishDate = outsoleMaster.OutsoleFinishDate;
                    assemblyMasterView.OutsoleBalance = outsoleMaster.OutsoleBalance;

                    assemblyMasterView.OutsoleLine = outsoleMaster.OutsoleLine;
                }
                else
                {
                    assemblyMasterView.OutsoleFinishDate = dtDefault;
                    assemblyMasterView.OutsoleBalance = "";
                }

                SockliningMasterModel sockliningMaster = sockliningMasterList.FirstOrDefault(f => f.ProductNo == order.ProductNo);
                if (sockliningMaster != null)
                {
                    //assemblyMasterView.SockliningBalance = outsoleMaster.OutsoleFinishDate;  

                    int qtyInsoleBalance = 0;
                    int qtyInsockBalance = 0;
                    int.TryParse(sockliningMaster.InsoleBalance, out qtyInsoleBalance);
                    int.TryParse(sockliningMaster.InsockBalance, out qtyInsockBalance);
                    int qtySockliningBalance = Math.Max(qtyInsoleBalance, qtyInsockBalance);
                    if (qtySockliningBalance > 0)
                    {
                        assemblyMasterView.SockliningBalance = qtySockliningBalance.ToString();
                    }
                    else
                    {
                        DateTime dtInsoleBalance = TimeHelper.Convert(sockliningMaster.InsoleBalance);
                        DateTime dtInsockBalance = TimeHelper.Convert(sockliningMaster.InsockBalance);
                        DateTime dtSockliningBalance = new DateTime(Math.Max(dtInsoleBalance.Ticks, dtInsockBalance.Ticks));
                        if (String.IsNullOrEmpty(sockliningMaster.InsoleBalance) == false && String.IsNullOrEmpty(sockliningMaster.InsockBalance) == false && dtSockliningBalance != dtNothing)
                        {
                            assemblyMasterView.SockliningBalance = assemblyMasterView.AssemblyMatsArrival = String.Format(new CultureInfo("en-US"), "{0:M/d}", dtSockliningBalance);
                        }
                    }
                }
                else
                {
                    assemblyMasterView.SockliningBalance = "";
                }

                assemblyMasterView.AssemblyStartDateForeground = Brushes.Black;
                assemblyMasterView.AssemblyFinishDateForeground = Brushes.Black;
                if (assemblyMasterView.AssemblyStartDate < new DateTime(Math.Max(assemblyMasterView.OutsoleFinishDate.Ticks, assemblyMasterView.SewingFinishDate.Ticks)))
                {
                    assemblyMasterView.AssemblyStartDateForeground = Brushes.Red;
                }
                if (assemblyMasterView.AssemblyFinishDate > assemblyMasterView.ETD)
                {
                    assemblyMasterView.AssemblyFinishDateForeground = Brushes.Red;
                }

                List<AssemblyReleaseModel> assemblyReleaseList_D1 = assemblyReleaseList.Where(a => a.ProductNo == order.ProductNo).ToList();
                int qtyReleased = assemblyReleaseList_D1.Sum(o => o.Quantity);
                assemblyMasterView.ReleasedQuantity = qtyReleased.ToString();
                if (qtyReleased <= 0)
                {
                    assemblyMasterView.ReleasedQuantity = "";
                }
                if (qtyReleased >= order.Quantity)
                {
                    DateTime releasedDate = assemblyReleaseList_D1.OrderBy(a => a.ModifiedTime).LastOrDefault().ModifiedTime;
                    assemblyMasterView.ReleasedQuantity = string.Format("{0:M/d}", releasedDate);
                }

                List<OutsoleReleaseMaterialModel> outsoleReleaseList_D1 = outsoleReleaseMaterialList.Where(w=>w.ProductNo == order.ProductNo).ToList();
                int qtyOutsoleReleased = outsoleReleaseList_D1.Sum(s => s.Quantity);
                assemblyMasterView.OutsoleReleasedQuantity = qtyOutsoleReleased.ToString();
                if (qtyOutsoleReleased <= 0)
                {
                    assemblyMasterView.OutsoleReleasedQuantity = "";
                }
                if (qtyOutsoleReleased >= order.Quantity)
                {
                    DateTime outsoleReleaseDate = outsoleReleaseList_D1.OrderBy(o => o.ModifiedTime).LastOrDefault().ModifiedTime;
                    assemblyMasterView.OutsoleReleasedQuantity = String.Format("{0:M/d}", outsoleReleaseDate);
                }

                assemblyMasterViewList.Add(assemblyMasterView);
            }
            assemblyMasterViewList = assemblyMasterViewList.OrderBy(a => a.AssemblyLine).ThenBy(a => a.Sequence).ToList();
        }

        // OUTSOLE
        private MaterialArrivalViewModel OutsoleMaterialArrival(string productNo, int[] materialIdArray)
        {
            // OUTSOLE
            MaterialArrivalViewModel materialArrivalView = new MaterialArrivalViewModel();
            List<RawMaterialModel> rawMaterialTypeList = rawMaterialList.Where(r => r.ProductNo == productNo && materialIdArray.Contains(r.MaterialTypeId)).ToList();
            return materialArrivalView;
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
            assemblyMasterViewFindList = new ObservableCollection<AssemblyMasterViewModel>(assemblyMasterViewList);
            dgSewingMaster.ItemsSource = assemblyMasterViewFindList;
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
            assemblyMasterList = AssemblyMasterController.Select();
        }

        private void bwReload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Load Newest Data
            foreach (AssemblyMasterViewModel assemblyMasterView in assemblyMasterViewFindList)
            {
                AssemblyMasterModel assemblyMaster = assemblyMasterList.FirstOrDefault(f => f.ProductNo == assemblyMasterView.ProductNo);
                if (assemblyMaster != null)
                {
                    string productNo = assemblyMaster.ProductNo;
                    if (isSequenceEditing == false)
                    {
                        assemblyMasterView.Sequence = assemblyMaster.Sequence;
                    }
                    if (assemblyLineUpdateList.Contains(productNo) == false)
                    {
                        assemblyMasterView.AssemblyLine = assemblyMaster.AssemblyLine;
                    }
                    if (assemblyQuotaUpdateList.Contains(productNo) == false)
                    {
                        assemblyMasterView.AssemblyQuota = assemblyMaster.AssemblyQuota;
                    }
                    if (assemblyActualStartDateUpdateList.Contains(productNo) == false)
                    {
                        assemblyMasterView.AssemblyActualStartDate = assemblyMaster.AssemblyActualStartDate;
                    }
                    if (assemblyActualFinishDateUpdateList.Contains(productNo) == false)
                    {
                        assemblyMasterView.AssemblyActualFinishDate = assemblyMaster.AssemblyActualFinishDate;
                    }
                    if (assemblyBalanceUpdateList.Contains(productNo) == false)
                    {
                        assemblyMasterView.AssemblyBalance = assemblyMaster.AssemblyBalance;
                    }
                }
            }

            //Sort By LineId, Sequence
            assemblyMasterViewFindList = new ObservableCollection<AssemblyMasterViewModel>(assemblyMasterViewList.OrderBy(a => a.AssemblyLine).ThenBy(s => s.Sequence));
            dgSewingMaster.ItemsSource = assemblyMasterViewFindList;
            for (int i = 0; i <= assemblyMasterViewFindList.Count - 1; i++)
            {
                assemblyMasterViewFindList[i].Sequence = i;
            }

            //Caculate
            List<String> assemblyLineList = assemblyMasterViewFindList.Select(a => a.AssemblyLine).Distinct().ToList();
            foreach (string assemblyLine in assemblyLineList)
            {
                if (String.IsNullOrEmpty(assemblyLine) == false)
                {
                    List<AssemblyMasterViewModel> assemblyMasterViewLineList = assemblyMasterViewFindList.Where(a => a.AssemblyLine == assemblyLine).ToList();
                    if (assemblyMasterViewLineList.Count > 0)
                    {
                        //DateTime dtAssemblyFinishDate = new DateTime();
                        //DateTime dtAssemblyStartDate = new DateTime();
                        DateTime dtAssemblyFinishDate = dtDefault;
                        DateTime dtAssemblyStartDate = dtDefault;
                        int dayAssemblyAddition = 0;
                        for (int i = 0; i <= assemblyMasterViewLineList.Count - 1; i++)
                        {
                            #region Caculate for Assembly
                            AssemblyMasterViewModel assemblyMasterView = assemblyMasterViewLineList[i];
                            int qtyAssemblyQuota = assemblyMasterView.AssemblyQuota;
                            int optAssembly = 0;
                            if (qtyAssemblyQuota > 0)
                            {
                                DateTime dtAssemblyStartDateTemp = TimeHelper.Convert(assemblyMasterView.AssemblyActualStartDate);
                                if ((String.IsNullOrEmpty(assemblyMasterView.AssemblyActualStartDate) == false && dtAssemblyStartDateTemp != dtNothing)
                                    || (assemblyMasterView == assemblyMasterViewLineList.First()))
                                {
                                    dtAssemblyStartDate = dtAssemblyStartDateTemp;
                                }
                                else
                                {
                                    dtAssemblyStartDate = dtAssemblyFinishDate.AddDays(dayAssemblyAddition);
                                }
                                //sewingMasterView.SewingStartDate = dtSewingStartDate;
                                dayAssemblyAddition = 0;
                                DateTime dtAssemblyFinishDateTemp = TimeHelper.Convert(assemblyMasterView.AssemblyActualFinishDate);
                                if (String.IsNullOrEmpty(assemblyMasterView.AssemblyActualFinishDate) == false && dtAssemblyFinishDateTemp != dtNothing)
                                {
                                    dtAssemblyFinishDate = dtAssemblyFinishDateTemp;
                                }
                                else
                                {
                                    int qtyAssemblyBalance = 0;
                                    assemblyMasterView.AssemblyBalance = assemblyMasterView.AssemblyBalance.Trim();
                                    int.TryParse(assemblyMasterView.AssemblyBalance, out qtyAssemblyBalance);
                                    if (qtyAssemblyBalance > 0)
                                    {
                                        dtAssemblyFinishDate = DateTime.Now.Date.AddDays((double)(qtyAssemblyBalance) / (double)qtyAssemblyQuota);
                                        optAssembly = 1;
                                    }
                                    else
                                    {
                                        DateTime dtAssemblyBalance = TimeHelper.Convert(assemblyMasterView.AssemblyBalance);
                                        if (String.IsNullOrEmpty(assemblyMasterView.AssemblyBalance) == true)
                                        {
                                            dtAssemblyFinishDate = dtAssemblyStartDate.AddDays((double)assemblyMasterView.Quantity / (double)qtyAssemblyQuota);
                                            optAssembly = 2;
                                        }
                                        else if (String.IsNullOrEmpty(assemblyMasterView.AssemblyBalance) == false && dtAssemblyBalance != dtNothing)
                                        {
                                            dtAssemblyFinishDate = dtAssemblyBalance.AddDays(0);
                                            optAssembly = 0;
                                            dayAssemblyAddition = 1;
                                        }
                                    }
                                }
                                //sewingMasterView.SewingFinishDate = dtSewingFinishDate;
                                if (optAssembly == 0)
                                {
                                    assemblyMasterView.AssemblyStartDate = dtAssemblyStartDate;
                                    assemblyMasterView.AssemblyFinishDate = dtAssemblyFinishDate;
                                }
                                else if (optAssembly == 1)
                                {
                                    List<DateTime> dtCheckOffDateList1 = CheckOffDay(DateTime.Now.Date.AddDays(0), dtAssemblyFinishDate);
                                    assemblyMasterView.AssemblyStartDate = dtAssemblyStartDate;
                                    assemblyMasterView.AssemblyFinishDate = new DateTime(dtCheckOffDateList1.Last().Year, dtCheckOffDateList1.Last().Month, dtCheckOffDateList1.Last().Day,
                                        dtAssemblyFinishDate.Hour, dtAssemblyFinishDate.Minute, dtAssemblyFinishDate.Second);
                                }
                                else if (optAssembly == 2)
                                {
                                    List<DateTime> dtCheckOffDateList2 = CheckOffDay(dtAssemblyStartDate, dtAssemblyFinishDate);
                                    assemblyMasterView.AssemblyStartDate = new DateTime(dtCheckOffDateList2.First().Year, dtCheckOffDateList2.First().Month, dtCheckOffDateList2.First().Day,
                                        dtAssemblyStartDate.Hour, dtAssemblyStartDate.Minute, dtAssemblyStartDate.Second);
                                    assemblyMasterView.AssemblyFinishDate = new DateTime(dtCheckOffDateList2.Last().Year, dtCheckOffDateList2.Last().Month, dtCheckOffDateList2.Last().Day,
                                        dtAssemblyFinishDate.Hour, dtAssemblyFinishDate.Minute, dtAssemblyFinishDate.Second);
                                }

                                assemblyMasterView.AssemblyStartDateForeground = Brushes.Black;
                                if (assemblyMasterView.AssemblyStartDate < new DateTime(Math.Max(assemblyMasterView.OutsoleFinishDate.Ticks, assemblyMasterView.SewingFinishDate.Ticks)))
                                {
                                    assemblyMasterView.AssemblyStartDateForeground = Brushes.Red;
                                }
                                assemblyMasterView.AssemblyFinishDateForeground = Brushes.Black;
                                if (assemblyMasterView.AssemblyFinishDate > assemblyMasterView.ETD)
                                {
                                    assemblyMasterView.AssemblyFinishDateForeground = Brushes.Red;
                                }

                                dtAssemblyFinishDate = assemblyMasterView.AssemblyFinishDate;
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
                    if (offDayList.Select(a => a.Date).Contains(dt) == true && dtResultList.Contains(dt) == true)
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
                    searchBox.GetFindWhat = new RawMaterialSearchBoxWindow.GetString(SearchAssemblyMaster);
                    searchBox.Show();
                }
            }
        }

        private void SearchAssemblyMaster(string findWhat, bool isMatch, bool isShow)
        {
            assemblyMasterViewList = assemblyMasterViewList.OrderBy(a => a.AssemblyLine).ThenBy(a => a.Sequence).ToList();
            assemblyMasterViewFindList = new ObservableCollection<AssemblyMasterViewModel>(assemblyMasterViewList);
            if (String.IsNullOrEmpty(findWhat) == false)
            {
                if (isMatch == true)
                {
                    AssemblyMasterViewModel assemblyMasterViewFind = assemblyMasterViewFindList.Where(r =>
                        r.ProductNo.ToLower() == findWhat.ToLower() || r.Country.ToLower() == findWhat.ToLower() || r.ShoeName.ToLower() == findWhat.ToLower() || r.ArticleNo.ToLower() == findWhat.ToLower() ||
                        r.LastCode.ToLower() == findWhat.ToLower() || r.ETD.ToString("dd/MM/yyyy") == findWhat.ToLower() || r.AssemblyLine.ToLower() == findWhat.ToLower()).FirstOrDefault();
                    if (assemblyMasterViewFind != null)
                    {
                        dgSewingMaster.ItemsSource = assemblyMasterViewFindList;
                        dgSewingMaster.SelectedItem = assemblyMasterViewFind;
                        dgSewingMaster.ScrollIntoView(assemblyMasterViewFind);
                        colAssemblyLine.CanUserSort = true;
                    }
                    else
                    {
                        dgSewingMaster.ItemsSource = assemblyMasterViewFindList;
                        MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    if (isMatch == false)
                    {
                        if (isShow == true)
                        {
                            assemblyMasterViewFindList = new ObservableCollection<AssemblyMasterViewModel>(assemblyMasterViewFindList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == true || r.Country.ToLower().Contains(findWhat.ToLower()) == true || r.ShoeName.ToLower().Contains(findWhat.ToLower()) == true || r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == true ||
                            r.LastCode.ToLower().Contains(findWhat.ToLower()) == true || r.ETD.ToString("dd/MM/yyyy").Contains(findWhat.ToLower()) == true || r.AssemblyLine.ToLower().Contains(findWhat.ToLower()) == true));
                        }
                        else
                        {
                            assemblyMasterViewFindList = new ObservableCollection<AssemblyMasterViewModel>(assemblyMasterViewFindList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == false && r.Country.ToLower().Contains(findWhat.ToLower()) == false && r.ShoeName.ToLower().Contains(findWhat.ToLower()) == false && r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == false &&
                            r.LastCode.ToLower().Contains(findWhat.ToLower()) == false && r.ETD.ToString("dd/MM/yyyy").Contains(findWhat.ToLower()) == false && r.AssemblyLine.ToLower().Contains(findWhat.ToLower()) == false));
                        }

                        if (assemblyMasterViewFindList.Count > 0)
                        {
                            dgSewingMaster.ItemsSource = assemblyMasterViewFindList;
                            colAssemblyLine.CanUserSort = false;
                        }
                        else
                        {
                            dgSewingMaster.ItemsSource = assemblyMasterViewFindList;
                            MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                            assemblyMasterViewFindList = new ObservableCollection<AssemblyMasterViewModel>(assemblyMasterViewList);
                            dgSewingMaster.ItemsSource = assemblyMasterViewFindList;
                        }
                    }
                }
            }
            else
            {
                colAssemblyLine.CanUserSort = true;
                dgSewingMaster.ItemsSource = assemblyMasterViewFindList;
            }
        }

        private void dgSewingMaster_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            AssemblyMasterViewModel assemblyMasterView = (AssemblyMasterViewModel)e.Row.Item;
            if (assemblyMasterView == null)
            {
                return;
            }

            string productNo = assemblyMasterView.ProductNo;

            if (e.Column == colAssemblyLine || e.Column == colAssemblyQuota || e.Column == colAssemblyActualStartDate ||
                e.Column == colAssemblyActualFinishDate || e.Column == colAssemblyBalance)
            {
                lineAssemblyEditingList.Add(assemblyMasterView.AssemblyLine);
                if (e.Column == colAssemblyLine)
                {
                    assemblyLineUpdateList.Add(productNo);
                }
                if (e.Column == colAssemblyQuota)
                {
                    assemblyQuotaUpdateList.Add(productNo);
                }
                if (e.Column == colAssemblyActualStartDate)
                {
                    assemblyActualStartDateUpdateList.Add(productNo);
                }
                if (e.Column == colAssemblyActualFinishDate)
                {
                    assemblyActualFinishDateUpdateList.Add(productNo);
                }
                if (e.Column == colAssemblyBalance)
                {
                    assemblyBalanceUpdateList.Add(productNo);
                }
            }

            if (e.Column == colAssemblyLine)
            {
                //SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)e.Row.Item;
                string assemblyLine = assemblyMasterView.AssemblyLine;
                if (String.IsNullOrEmpty(assemblyLine) == true)
                {
                    return;
                }
                int assemblySequence = 0;
                if (assemblyMasterViewList.Where(a => a.AssemblyLine == assemblyLine).Count() > 0)
                {
                    assemblySequence = assemblyMasterViewList.Where(a => a.AssemblyLine == assemblyLine).Select(a => a.Sequence).Max() + 1;
                }
                assemblyMasterView.Sequence = assemblySequence;
                isSequenceEditing = true;
            }
            if (e.Column == colAssemblyActualStartDate || e.Column == colAssemblyActualFinishDate)
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
            if (e.Column == colAssemblyLine)
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection.Value == ListSortDirection.Descending)
                {
                    assemblyMasterViewList = assemblyMasterViewList.OrderBy(a => a.AssemblyLine).ThenBy(a => a.Sequence).ToList();
                    assemblyMasterViewFindList = new ObservableCollection<AssemblyMasterViewModel>(assemblyMasterViewList.OrderBy(a => a.AssemblyLine).ThenBy(a => a.Sequence));
                }
                else
                {
                    assemblyMasterViewList = assemblyMasterViewList.OrderByDescending(a => a.AssemblyLine).ThenBy(a => a.Sequence).ToList();
                    assemblyMasterViewFindList = new ObservableCollection<AssemblyMasterViewModel>(assemblyMasterViewList.OrderByDescending(a => a.AssemblyLine).ThenBy(a => a.Sequence));
                }
                dgSewingMaster.ItemsSource = assemblyMasterViewFindList;
                for (int i = 0; i <= assemblyMasterViewFindList.Count - 1; i++)
                {
                    assemblyMasterViewFindList[i].Sequence = i;
                }
                dgSewingMaster.ScrollIntoView(assemblyMasterViewFindList.Where(a => String.IsNullOrEmpty(a.AssemblyLine) == false).FirstOrDefault());
                e.Handled = true;
            }
        }

        private void dgSewingMaster_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //AssemblyMasterViewModel assemblyMasterView = (AssemblyMasterViewModel)dgSewingMaster.CurrentItem;
            //if (dgSewingMaster.CurrentCell.Column == colAssemblyBalance && assemblyMasterView != null)
            //{
            //    OutsoleInputOutputWindow window = new OutsoleInputOutputWindow(assemblyMasterView.ProductNo);
            //    if (window.ShowDialog() == true)
            //    {
            //        assemblyMasterView.AssemblyBalance = window.resultString;
            //        lineAssemblyEditingList.Add(assemblyMasterView.AssemblyLine);
            //    }
            //}
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false && simulationMode == false)
            {
                this.Cursor = Cursors.Wait;
                assemblyMasterViewToInsertList = dgSewingMaster.Items.OfType<AssemblyMasterViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false && simulationMode == false)
            {
                this.Cursor = Cursors.Wait;
                assemblyMasterViewToInsertList = dgSewingMaster.Items.OfType<AssemblyMasterViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (AssemblyMasterViewModel assemblyMaster in assemblyMasterViewToInsertList)
            {
                string productNo = assemblyMaster.ProductNo;
                string assemblyLine = assemblyMaster.AssemblyLine;
                AssemblyMasterModel model = new AssemblyMasterModel
                {
                    ProductNo = assemblyMaster.ProductNo,
                    Sequence = assemblyMaster.Sequence,
                    AssemblyLine = assemblyMaster.AssemblyLine,
                    AssemblyStartDate = assemblyMaster.AssemblyStartDate,
                    AssemblyFinishDate = assemblyMaster.AssemblyFinishDate,
                    AssemblyQuota = assemblyMaster.AssemblyQuota,
                    AssemblyActualStartDate = assemblyMaster.AssemblyActualStartDate,
                    AssemblyActualFinishDate = assemblyMaster.AssemblyActualFinishDate,
                    AssemblyBalance = assemblyMaster.AssemblyBalance,

                    IsSequenceUpdate = false,
                    IsAssemblyLineUpdate = false,
                    IsAssemblyStartDateUpdate = false,
                    IsAssemblyFinishDateUpdate = false,
                    IsAssemblyQuotaUpdate = false,
                    IsAssemblyActualStartDateUpdate = false,
                    IsAssemblyActualFinishDateUpdate = false,
                    IsAssemblyBalanceUpdate = false,
                };

                model.IsSequenceUpdate = isSequenceEditing;

                model.IsAssemblyLineUpdate = assemblyLineUpdateList.Contains(productNo);
                model.IsAssemblyStartDateUpdate = lineAssemblyEditingList.Contains(assemblyLine);
                model.IsAssemblyFinishDateUpdate = lineAssemblyEditingList.Contains(assemblyLine);
                model.IsAssemblyQuotaUpdate = assemblyQuotaUpdateList.Contains(productNo);
                model.IsAssemblyActualStartDateUpdate = assemblyActualStartDateUpdateList.Contains(productNo);
                model.IsAssemblyActualFinishDateUpdate = assemblyActualFinishDateUpdateList.Contains(productNo);
                model.IsAssemblyBalanceUpdate = assemblyBalanceUpdateList.Contains(productNo);

                if (model.IsSequenceUpdate == true ||
                    model.IsAssemblyLineUpdate == true ||
                    model.IsAssemblyStartDateUpdate == true ||
                    model.IsAssemblyFinishDateUpdate == true ||
                    model.IsAssemblyQuotaUpdate == true ||
                    model.IsAssemblyActualStartDateUpdate == true ||
                    model.IsAssemblyActualFinishDateUpdate == true ||
                    model.IsAssemblyBalanceUpdate == true)
                {
                    AssemblyMasterController.Insert_2(model);

                    // Insert ProductNoRevise
                    //var productNoReviseInsertModel = new ProductNoReviseModel() {
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
            lineAssemblyEditingList.Clear();

            assemblyLineUpdateList.Clear();

            assemblyQuotaUpdateList.Clear();
            assemblyActualStartDateUpdateList.Clear();
            assemblyActualFinishDateUpdateList.Clear();
            assemblyBalanceUpdateList.Clear();
            
            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void dgSewingMaster_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isEditing = true;
        }

        private List<AssemblyMasterViewModel> assemblyMasterViewSelectList = new List<AssemblyMasterViewModel>();
        private void dgSewingMaster_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popup.IsOpen = false;
            assemblyMasterViewSelectList.Clear();
            var dataGrid = (DataGrid)sender;
            if (dataGrid != null)
            {
                foreach (DataGridCellInfo cellInfo in dataGrid.SelectedCells)
                {
                    assemblyMasterViewSelectList.Add((AssemblyMasterViewModel)cellInfo.Item);
                }
                assemblyMasterViewSelectList = assemblyMasterViewSelectList.Distinct().ToList();
            }
        }

        private void dgSewingMaster_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && account.AssemblyMaster == true && isEditing == false)
            //if (e.LeftButton == MouseButtonState.Pressed && isEditing == false)
            {
                var dataGrid = (DataGrid)sender;
                if (dataGrid != null)
                {
                    if (e.OriginalSource.GetType() == typeof(Thumb))
                    {
                        return;
                    }
                    if ((AssemblyMasterViewModel)dataGrid.CurrentItem != null && assemblyMasterViewSelectList.Contains((AssemblyMasterViewModel)dataGrid.CurrentItem) == false)
                    {
                        assemblyMasterViewSelectList.Add((AssemblyMasterViewModel)dataGrid.CurrentItem);
                    }
                    if (assemblyMasterViewSelectList.Count > 0)
                    {
                        listView.ItemsSource = assemblyMasterViewSelectList;
                        popup.PlacementTarget = lblPopup;
                        DragDrop.DoDragDrop(dataGrid, assemblyMasterViewSelectList, DragDropEffects.Move);
                    }
                }
            }
        }

        private void dgSewingMaster_DragLeave(object sender, DragEventArgs e)
        {
            FrameworkElement frameworkElement = (FrameworkElement)e.OriginalSource;

            if (frameworkElement != null && frameworkElement.DataContext != null
                && frameworkElement.DataContext.GetType() == typeof(AssemblyMasterViewModel))
            {
                AssemblyMasterViewModel assemblyMasterView = (AssemblyMasterViewModel)frameworkElement.DataContext;
                dgSewingMaster.SelectedItem = assemblyMasterView;
                dgSewingMaster.ScrollIntoView(assemblyMasterView);
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
                AssemblyMasterViewModel assemblyMasterView = (AssemblyMasterViewModel)dataGrid.SelectedItem;
                int index = dataGrid.Items.IndexOf(assemblyMasterView);
                int indexFirst = dataGrid.Items.IndexOf(assemblyMasterViewSelectList.First());
                int indexLast = dataGrid.Items.IndexOf(assemblyMasterViewSelectList.Last());
                if (index < indexFirst && index < indexLast)
                {
                    for (int i = assemblyMasterViewSelectList.Count - 1; i >= 0; i = i - 1)
                    {
                        assemblyMasterViewFindList.Remove(assemblyMasterViewSelectList[i]);
                        assemblyMasterViewFindList.Insert(index, assemblyMasterViewSelectList[i]);
                        assemblyMasterViewSelectList[i].Sequence = assemblyMasterView.Sequence + i;
                    }
                    for (int i = index + assemblyMasterViewSelectList.Count; i <= assemblyMasterViewFindList.Count - 1; i++)
                    {
                        assemblyMasterViewFindList[i].Sequence = assemblyMasterViewFindList[i].Sequence + assemblyMasterViewSelectList.Count;
                    }
                    isSequenceEditing = true;
                }
                else if (index > indexFirst && index > indexLast)
                {
                    for (int i = 0; i <= assemblyMasterViewSelectList.Count - 1; i = i + 1)
                    {
                        assemblyMasterViewFindList.Remove(assemblyMasterViewSelectList[i]);
                        assemblyMasterViewFindList.Insert(index - 1, assemblyMasterViewSelectList[i]);
                        assemblyMasterViewSelectList[i].Sequence = assemblyMasterView.Sequence + i;
                    }
                    for (int i = index; i <= assemblyMasterViewFindList.Count - 1; i++)
                    {
                        assemblyMasterViewFindList[i].Sequence = assemblyMasterViewFindList[i].Sequence + assemblyMasterViewSelectList.Count;
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
            if ((isSequenceEditing == true || lineAssemblyEditingList.Count > 0 ||
                assemblyLineUpdateList.Count > 0 || assemblyQuotaUpdateList.Count > 0 || assemblyActualStartDateUpdateList.Count > 0 ||
                assemblyActualFinishDateUpdateList.Count > 0 || assemblyBalanceUpdateList.Count > 0) && simulationMode == false)
            {
                MessageBoxResult result = MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (bwInsert.IsBusy == false)
                    {
                        e.Cancel = true;
                        this.Cursor = Cursors.Wait;
                        assemblyMasterViewToInsertList = dgSewingMaster.Items.OfType<AssemblyMasterViewModel>().ToList();
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
                    AssemblyMasterViewModel assemblyMasterView = (AssemblyMasterViewModel)dataGridCellInfo.Item;
                    if (assemblyMasterView != null)
                    {
                        assemblyMasterView.ProductNoBackground = Brushes.Transparent;
                    }
                }
            }
            foreach (DataGridCellInfo dataGridCellInfo in e.AddedCells)
            {
                AssemblyMasterViewModel assemblyMasterView = (AssemblyMasterViewModel)dataGridCellInfo.Item;
                if (assemblyMasterView != null)
                {
                    assemblyMasterView.ProductNoBackground = Brushes.RoyalBlue;
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

            title = "Master Schedule - Assembly Simulation File";
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

                title = "Master Schedule - Assembly Master File";
                this.Title = title;

                ctmTranfer.Visibility = Visibility.Collapsed;
                simulationMode = false;

                //btnDisableSimulation.IsEnabled = false;

                btnSave.IsEnabled = false;
                btnCaculate.IsEnabled = false;

                assemblyMasterViewList = new List<AssemblyMasterViewModel>();
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void miTranfer_Click(object sender, RoutedEventArgs e)
        {
            assemblyMasterViewToInsertList = dgSewingMaster.SelectedItems.OfType<AssemblyMasterViewModel>().ToList();
            if (assemblyMasterViewToInsertList.Count <= 0 ||
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
