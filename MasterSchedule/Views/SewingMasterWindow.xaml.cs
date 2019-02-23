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
    public partial class SewingMasterWindow : Window
    {
        AccountModel account;
        BackgroundWorker bwLoad;
        List<OffDayModel> offDayList;
        List<OrdersModel> orderList;
        List<SewingMasterViewModel> sewingMasterViewList;
        public ObservableCollection<SewingMasterViewModel> sewingMasterViewFindList;
        List<SewingMasterModel> sewingMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<SewingMasterViewModel> sewingMasterViewToInsertList;
        List<RawMaterialModel> rawMaterialList;
        DateTime dtNothing;
        DateTime dtDefault;
        BackgroundWorker bwInsert;
        bool isEditing;
        BackgroundWorker bwReload;
        bool isSequenceEditing;

        List<String> lineSewingEditingList;
        List<String> lineCutPrepEditingList;

        List<String> sewingLineUpdateList;

        List<String> sewingQuotaUpdateList;

        List<String> sewingPrepUpdateList;

        List<String> sewingActualStartDateUpdateList;
        List<String> sewingActualFinishDateUpdateList;

        List<String> sewingActualStartDateUpdateAutoList;
        List<String> sewingActualFinishDateUpdateAutoList;

        List<String> sewingBalanceUpdateList;

        List<String> cutAQuotaUpdateList;
        List<String> cutAActualStartDateUpdateList;
        List<String> cutAActualFinishDateUpdateList;
        List<String> cutABalanceUpdateList;

        List<String> printingBalanceUpdateList;
        List<String> h_fBalanceUpdateList;
        List<String> embroideryBalanceUpdateList;

        List<String> cutBActualStartDateUpdateList;
        List<String> cutBBalanceUpdateList;

        List<String> autoCutUpdateList;
        List<String> laserCutUpdateList;
        List<String> huasenCutUpdateList;


        RawMaterialSearchBoxWindow searchBox;
        List<ProductionMemoModel> productionMemoList;

        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleMaterialModel> outsoleMaterialList;

        String _SECTIONID = "SEW";

        public SewingMasterWindow(AccountModel account)
        {
            InitializeComponent();
            this.account = account;
            bwLoad = new BackgroundWorker();
            bwLoad.WorkerSupportsCancellation = true;
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            offDayList = new List<OffDayModel>();
            orderList = new List<OrdersModel>();
            sewingMasterViewList = new List<SewingMasterViewModel>();
            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>();
            sewingMasterList = new List<SewingMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            sewingMasterViewToInsertList = new List<SewingMasterViewModel>();
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
            lineSewingEditingList = new List<String>();
            lineCutPrepEditingList = new List<String>();

            sewingLineUpdateList = new List<String>();

            sewingQuotaUpdateList = new List<String>();

            sewingPrepUpdateList = new List<String>();

            sewingActualStartDateUpdateList = new List<String>();
            sewingActualFinishDateUpdateList = new List<String>();

            sewingActualStartDateUpdateAutoList = new List<String>();
            sewingActualFinishDateUpdateAutoList = new List<String>();

            sewingBalanceUpdateList = new List<String>();

            cutAQuotaUpdateList = new List<String>();
            cutAActualStartDateUpdateList = new List<String>();
            cutAActualFinishDateUpdateList = new List<String>();
            cutABalanceUpdateList = new List<String>();

            printingBalanceUpdateList = new List<String>();
            h_fBalanceUpdateList = new List<String>();
            embroideryBalanceUpdateList = new List<String>();

            cutBActualStartDateUpdateList = new List<String>();
            cutBBalanceUpdateList = new List<String>();

            autoCutUpdateList = new List<String>();
            laserCutUpdateList = new List<String>();
            huasenCutUpdateList = new List<String>();

            searchBox = new RawMaterialSearchBoxWindow();

            productionMemoList = new List<ProductionMemoModel>();

            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();
            outsoleMaterialList = new List<OutsoleMaterialModel>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (account.SewingMaster == true)
            {
                colSewingLine.IsReadOnly = false;
                colSewingQuota.IsReadOnly = false;
                colSewingActualStartDate.IsReadOnly = false;
                colSewingActualFinishDate.IsReadOnly = false;
                colSewingPrep.IsReadOnly = false;
            }
            if (account.CutPrepMaster == true)
            {
                colCutAQuota.IsReadOnly = false;
                colCutAActualStartDate.IsReadOnly = false;
                colCutAActualFinishDate.IsReadOnly = false;
                colCutABalance.IsReadOnly = false;
                colPrintingBalance.IsReadOnly = false;
                colH_FBalance.IsReadOnly = false;
                colEmbroideryBalance.IsReadOnly = false;
                colCutBActualStartDate.IsReadOnly = false;
                colCutBBalance.IsReadOnly = false;
                colAutoCut.IsReadOnly = false;
                colLaserCut.IsReadOnly = false;
                colHuasenCut.IsReadOnly = false;
            }

            if (account.Sortable == true)
            {
                colCountry.CanUserSort = true;
                colStyle.CanUserSort = true;
                colETD.CanUserSort = true;
            }

            //if (account.Simulation == true)
            //{
            //    //btnEnableSimulation.Visibility = Visibility.Visible;
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
            rawMaterialList = RawMaterialController.Select();
            outsoleMasterList = OutsoleMasterController.Select();

            productionMemoList = ProductionMemoController.Select();

            outsoleRawMaterialList = OutsoleRawMaterialController.Select();
            outsoleMaterialList = OutsoleMaterialController.Select();

            int[] materialIdUpperArray = { 1, 2, 3, 4, 10 };
            int[] materialIdSewingArray = { 5, 7 };
            int[] materialIdOutsoleArray = { 6 };
            int[] materialIdAssemblyArray = { 8, 9 };
            for (int i = 0; i <= orderList.Count - 1; i++)
            {
                OrdersModel order = orderList[i];
                SewingMasterViewModel sewingMasterView = new SewingMasterViewModel
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
                sewingMasterView.MemoId = memoId;

                MaterialArrivalViewModel materialArrivalUpper = MaterialArrival(order.ProductNo, materialIdUpperArray);
                sewingMasterView.UpperMatsArrivalOrginal = dtDefault;
                if (materialArrivalUpper != null)
                {
                    sewingMasterView.UpperMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalUpper.Date);
                    sewingMasterView.UpperMatsArrivalOrginal = materialArrivalUpper.Date;
                    sewingMasterView.UpperMatsArrivalForeground = materialArrivalUpper.Foreground;
                    sewingMasterView.UpperMatsArrivalBackground = materialArrivalUpper.Background;
                }

                MaterialArrivalViewModel materialArrivalSewing = MaterialArrival(order.ProductNo, materialIdSewingArray);
                sewingMasterView.SewingMatsArrivalOrginal = dtDefault;
                if (materialArrivalSewing != null)
                {
                    sewingMasterView.SewingMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalSewing.Date);
                    sewingMasterView.SewingMatsArrivalOrginal = materialArrivalSewing.Date;
                    sewingMasterView.SewingMatsArrivalForeground = materialArrivalSewing.Foreground;
                    sewingMasterView.SewingMatsArrivalBackground = materialArrivalSewing.Background;
                }

                // Fix Code
                List<OutsoleMaterialModel> outsoleMaterialQuantityZero = new List<OutsoleMaterialModel>();
                outsoleMaterialQuantityZero = outsoleMaterialList.Where(w => w.Quantity == 0 && w.ProductNo == order.ProductNo).ToList();
                List<int> outsoleSupplierIdList = new List<int>();
                outsoleSupplierIdList = outsoleMaterialQuantityZero.Select(s => s.OutsoleSupplierId).Distinct().ToList();
                List<OutsoleRawMaterialModel> outsoleRawMaterialNotSupplied = new List<OutsoleRawMaterialModel>();
                foreach (int supplierId in outsoleSupplierIdList)
                {
                    OutsoleRawMaterialModel outsoleRawMaterialBySupplierId = new OutsoleRawMaterialModel();
                    outsoleRawMaterialBySupplierId = outsoleRawMaterialList.Where(w => w.ProductNo == order.ProductNo && w.OutsoleSupplierId == supplierId).FirstOrDefault();
                    outsoleRawMaterialNotSupplied.Add(outsoleRawMaterialBySupplierId);
                }

                if (outsoleRawMaterialNotSupplied.Count > 0)
                {
                    List<RawMaterialModel> rawMaterialTypeList = rawMaterialList.Where(r => r.ProductNo == order.ProductNo && r.MaterialTypeId == 6).ToList();
                    OutsoleRawMaterialModel outsoleRaw = outsoleRawMaterialNotSupplied.OrderBy(o => o.ETD).LastOrDefault();
                    sewingMasterView.OSMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", outsoleRaw.ETD);
                    sewingMasterView.OSMatsArrivalForeground = Brushes.Black;
                    sewingMasterView.OSMatsArrivalBackground = Brushes.Transparent;
                    if (outsoleRaw.ETD < DateTime.Now.Date)
                    {
                        sewingMasterView.OSMatsArrivalBackground = Brushes.Red;
                    }
                    else
                    {
                        if (rawMaterialTypeList.Where(r => String.IsNullOrEmpty(r.Remarks) == false).Count() > 0)
                        {
                            sewingMasterView.OSMatsArrivalBackground = Brushes.Yellow;
                        }
                    }
                }
                else
                {
                    // Normal code
                    MaterialArrivalViewModel materialArrivalOutsole = MaterialArrival(order.ProductNo, materialIdOutsoleArray);
                    if (materialArrivalOutsole != null)
                    {
                        sewingMasterView.OSMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalOutsole.Date);
                        sewingMasterView.OSMatsArrivalForeground = materialArrivalOutsole.Foreground;
                        sewingMasterView.OSMatsArrivalBackground = materialArrivalOutsole.Background;
                    }
                }

                MaterialArrivalViewModel materialArrivalAssembly = MaterialArrival(order.ProductNo, materialIdAssemblyArray);
                if (materialArrivalAssembly != null)
                {
                    sewingMasterView.AssemblyMatsArrival = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", materialArrivalAssembly.Date);
                    sewingMasterView.AssemblyMatsArrivalForeground = materialArrivalAssembly.Foreground;
                    sewingMasterView.AssemblyMatsArrivalBackground = materialArrivalAssembly.Background;
                }

                SewingMasterModel sewingMaster = sewingMasterList.Where(s => s.ProductNo == order.ProductNo).FirstOrDefault();
                if (sewingMaster != null)
                {
                    sewingMasterView.Sequence = sewingMaster.Sequence;
                    sewingMasterView.SewingLine = sewingMaster.SewingLine;
                    sewingMasterView.SewingStartDate = sewingMaster.SewingStartDate;
                    sewingMasterView.SewingFinishDate = sewingMaster.SewingFinishDate;
                    sewingMasterView.SewingQuota = sewingMaster.SewingQuota;

                    sewingMasterView.SewingPrep = sewingMaster.SewingPrep;

                    sewingMasterView.SewingActualStartDate = sewingMaster.SewingActualStartDate;
                    sewingMasterView.SewingActualFinishDate = sewingMaster.SewingActualFinishDate;

                    sewingMasterView.SewingActualStartDateAuto = TimeHelper.ConvertDateToView(sewingMaster.SewingActualStartDateAuto);
                    sewingMasterView.SewingActualFinishDateAuto = TimeHelper.ConvertDateToView(sewingMaster.SewingActualFinishDateAuto);
                    

                    sewingMasterView.SewingBalance = sewingMaster.SewingBalance;
                    sewingMasterView.CutAStartDate = sewingMaster.CutAStartDate;
                    sewingMasterView.CutAFinishDate = sewingMaster.CutAFinishDate;
                    sewingMasterView.CutAQuota = sewingMaster.CutAQuota;
                    sewingMasterView.CutAActualStartDate = TimeHelper.ConvertDateToView(sewingMaster.CutAActualStartDate);
                    sewingMasterView.CutAActualFinishDate = TimeHelper.ConvertDateToView(sewingMaster.CutAActualFinishDate);

                    sewingMasterView.CutBActualStartDate = TimeHelper.ConvertDateToView(sewingMaster.CutBActualStartDate);
                    sewingMasterView.CutABalance = sewingMaster.CutABalance;

                    sewingMasterView.PrintingBalance = sewingMaster.PrintingBalance;
                    sewingMasterView.H_FBalance = sewingMaster.H_FBalance;
                    sewingMasterView.EmbroideryBalance = sewingMaster.EmbroideryBalance;
                    sewingMasterView.CutBBalance = sewingMaster.CutBBalance;
                    sewingMasterView.AutoCut = sewingMaster.AutoCut;
                    sewingMasterView.LaserCut = sewingMaster.LaserCut;
                    sewingMasterView.HuasenCut = sewingMaster.HuasenCut;
                }
                else
                {
                    sewingMasterView.Sequence = 0;
                    sewingMasterView.SewingLine = "";
                    sewingMasterView.SewingStartDate = dtDefault;
                    sewingMasterView.SewingFinishDate = dtDefault;
                    sewingMasterView.SewingQuota = 0;

                    sewingMasterView.SewingPrep = "";

                    sewingMasterView.SewingActualStartDate = "";
                    sewingMasterView.SewingActualFinishDate = "";

                    sewingMasterView.SewingActualStartDateAuto = "";
                    sewingMasterView.SewingActualFinishDateAuto = "";

                    sewingMasterView.SewingBalance = "";
                    sewingMasterView.CutAStartDate = dtDefault;
                    sewingMasterView.CutAFinishDate = dtDefault;
                    sewingMasterView.CutAQuota = 0;
                    sewingMasterView.CutAActualStartDate = "";
                    sewingMasterView.CutAActualFinishDate = "";
                    sewingMasterView.CutABalance = "";
                    sewingMasterView.PrintingBalance = "";
                    sewingMasterView.H_FBalance = "";
                    sewingMasterView.EmbroideryBalance = "";
                    sewingMasterView.CutBActualStartDate = "";
                    sewingMasterView.CutBBalance = "";
                    sewingMasterView.AutoCut = "";
                    sewingMasterView.LaserCut = "";
                    sewingMasterView.HuasenCut = "";
                }

                OutsoleMasterModel outsoleMaster = outsoleMasterList.Where(o => o.ProductNo == order.ProductNo).FirstOrDefault();
                if (outsoleMaster != null)
                {
                    sewingMasterView.OSFinishDate = outsoleMaster.OutsoleFinishDate;
                    sewingMasterView.OSBalance = outsoleMaster.OutsoleBalance;
                }
                else
                {
                    sewingMasterView.OSFinishDate = dtDefault;
                    sewingMasterView.OSBalance = "";
                }

                sewingMasterView.SewingStartDateForeground = Brushes.Black;
                sewingMasterView.SewingFinishDateForeground = Brushes.Black;
                sewingMasterView.CutAStartDateForeground = Brushes.Black;

                // addition code: Orange is: sewing startdate start after uppermaterial come in 5 days
                //int rangeSewing = TimeHelper.CalculateDate(sewingMasterView.UpperMatsArrivalOrginal, sewingMasterView.SewingStartDate);
                int rangeSewing = (Int32)((sewingMasterView.SewingStartDate - sewingMasterView.UpperMatsArrivalOrginal).TotalDays);
                //if (0 <= rangeSewing && rangeSewing <= 5)
                if (sewingMasterView.UpperMatsArrivalOrginal <= sewingMasterView.SewingStartDate && sewingMasterView.SewingStartDate <= sewingMasterView.UpperMatsArrivalOrginal.AddDays(5))
                {
                    sewingMasterView.SewingStartDateForeground = Brushes.Orange;
                }  

                if (sewingMasterView.SewingStartDate < new DateTime(Math.Max(sewingMasterView.UpperMatsArrivalOrginal.Ticks, sewingMasterView.SewingMatsArrivalOrginal.Ticks)))
                {
                    sewingMasterView.SewingStartDateForeground = Brushes.Red;
                }                
                if (sewingMasterView.SewingFinishDate > sewingMasterView.ETD)
                {
                    sewingMasterView.SewingFinishDateForeground = Brushes.Red;
                }

                if (sewingMasterView.CutAStartDate < sewingMasterView.UpperMatsArrivalOrginal)
                {
                    sewingMasterView.CutAStartDateForeground = Brushes.Red;
                }
                else
                {
                    if (materialArrivalUpper != null)
                    {
                        if (sewingMasterView.CutAStartDateForeground == Brushes.Orange)
                        {
                            sewingMasterView.CutAStartDateForeground = Brushes.Orange;
                        }
                        else
                        {
                            sewingMasterView.CutAStartDateForeground = materialArrivalUpper.Foreground;
                        }
                    }
                    // addtion code : Orange is: cut a startdate start after uppermaterial come in 3 days
                    //if (0 <= rangeCutA && rangeCutA <= 3)
                    //int rangeCutA = TimeHelper.CalculateDate(sewingMasterView.UpperMatsArrivalOrginal, sewingMasterView.CutAStartDate);
                    int rangeCutA = (Int32)((sewingMasterView.CutAStartDate - sewingMasterView.UpperMatsArrivalOrginal).TotalDays);
                    //if (sewingMasterView.UpperMatsArrivalOrginal <= sewingMasterView.CutAStartDate && sewingMasterView.CutAStartDate <= sewingMasterView.UpperMatsArrivalOrginal.AddDays(3))
                    if (0 <= rangeCutA && rangeCutA <= 3)
                    {
                        sewingMasterView.CutAStartDateForeground = Brushes.Orange;
                    }
                }
                sewingMasterViewList.Add(sewingMasterView);
            }
            sewingMasterViewList = sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence).ToList();
        }

        private MaterialArrivalViewModel MaterialArrival(string productNo, int[] materialIdArray)
        {
            var rawMaterialTypeList = rawMaterialList.Where(r => r.ProductNo == productNo && materialIdArray.Contains(r.MaterialTypeId)).ToList();
            //foreach (var materialId in materialIdArray)
            //{
            //    var rawMaterialNA = rawMaterialTypeList.Where(w => w.Remarks.Contains("N/A") || w.Remarks.Contains("n/a")).FirstOrDefault();
            //    if (rawMaterialNA != null)
            //        rawMaterialTypeList.Remove(rawMaterialNA);
            //}

            var materialArrivalView = new MaterialArrivalViewModel();

            // Blue is: actual date
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

            //// Yellow is:
            //if (materialArrivalView != null && (rawMaterialTypeList.Where(w => w.Remarks != "" && w.Remarks != " ").Count() > 0))
            if (materialArrivalView != null && (rawMaterialTypeList.Where(w => String.IsNullOrEmpty(w.Remarks) == false 
                                                                        && (w.Remarks != "TL" || w.Remarks != "DL") 
                                                                        && w.ETD == dtDefault 
                                                                        && w.ActualDate == dtDefault).Count() > 0))
            {
                materialArrivalView.Foreground = Brushes.Black;
                materialArrivalView.Background = Brushes.Yellow;
            }

            return materialArrivalView;
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList);
            dgSewingMaster.ItemsSource = sewingMasterViewFindList;
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
            sewingMasterList = SewingMasterController.Select();
        }

        private void bwReload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Load Newest Data
            foreach (SewingMasterViewModel sewingMasterView in sewingMasterViewFindList)
            {
                SewingMasterModel sewingMaster = sewingMasterList.Where(s => s.ProductNo == sewingMasterView.ProductNo).FirstOrDefault();
                if (sewingMaster != null)
                {
                    string productNo = sewingMaster.ProductNo;
                    if (isSequenceEditing == false)
                    {
                        sewingMasterView.Sequence = sewingMaster.Sequence;
                    }
                    if (sewingLineUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingLine = sewingMaster.SewingLine;
                    }
                    if (sewingQuotaUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingQuota = sewingMaster.SewingQuota;
                    }

                    if (sewingPrepUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingPrep = sewingMaster.SewingPrep;
                    }

                    if (sewingActualStartDateUpdateAutoList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingActualStartDateAuto = TimeHelper.ConvertDateToView(sewingMaster.SewingActualStartDateAuto);
                    }
                    if (sewingActualFinishDateUpdateAutoList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingActualFinishDateAuto = TimeHelper.ConvertDateToView(sewingMaster.SewingActualFinishDateAuto);
                    }

                    if (sewingActualStartDateUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingActualStartDate = sewingMaster.SewingActualStartDate;
                        
                    }
                    if (sewingActualFinishDateUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingActualFinishDate = sewingMaster.SewingActualFinishDate;
                    }

                    if (sewingBalanceUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.SewingBalance = sewingMaster.SewingBalance;
                    }

                    if (cutAQuotaUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.CutAQuota = sewingMaster.CutAQuota;
                    }
                    if (cutAActualStartDateUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.CutAActualStartDate = TimeHelper.ConvertDateToView(sewingMaster.CutAActualStartDate);
                    }
                    if (cutAActualFinishDateUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.CutAActualFinishDate = TimeHelper.ConvertDateToView(sewingMaster.CutAActualFinishDate);
                    }
                    if (cutABalanceUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.CutABalance = sewingMaster.CutABalance;
                    }

                    if (printingBalanceUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.PrintingBalance = sewingMaster.PrintingBalance;
                    }
                    if (h_fBalanceUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.H_FBalance = sewingMaster.H_FBalance;
                    }
                    if (embroideryBalanceUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.EmbroideryBalance = sewingMaster.EmbroideryBalance;
                    }

                    if (cutBActualStartDateUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.CutBActualStartDate = TimeHelper.ConvertDateToView(sewingMaster.CutBActualStartDate);
                    }

                    if (cutBBalanceUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.CutBBalance = sewingMaster.CutBBalance;
                    }
                    if (autoCutUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.AutoCut = sewingMaster.AutoCut;
                    }
                    if (laserCutUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.LaserCut = sewingMaster.LaserCut;
                    }
                    if (huasenCutUpdateList.Contains(productNo) == false)
                    {
                        sewingMasterView.HuasenCut = sewingMaster.HuasenCut;
                    }
                }
            }

            //Sort By LineId, Sequence
            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence));
            dgSewingMaster.ItemsSource = sewingMasterViewFindList;
            for (int i = 0; i <= sewingMasterViewFindList.Count - 1; i++)
            {
                sewingMasterViewFindList[i].Sequence = i;
            }

            //Caculate
            List<String> sewingLineList = sewingMasterViewFindList.Select(l => l.SewingLine).Distinct().ToList();
            foreach (string sewingLine in sewingLineList)
            {
                if (String.IsNullOrEmpty(sewingLine) == false)
                {
                    List<SewingMasterViewModel> sewingMasterViewLineList = sewingMasterViewFindList.Where(s => s.SewingLine == sewingLine).ToList();
                    if (sewingMasterViewLineList.Count > 0)
                    {
                        //DateTime dtSewingFinishDate = new DateTime();
                        //DateTime dtSewingStartDate = new DateTime();
                        //DateTime dtCutAFinishDate = new DateTime();
                        //DateTime dtCutAStartDate = new DateTime();
                        DateTime dtSewingFinishDate = dtDefault;
                        DateTime dtSewingStartDate = dtDefault;
                        DateTime dtCutAFinishDate = dtDefault;
                        DateTime dtCutAStartDate = dtDefault;
                        int daySewingAddition = 0;
                        int dayCutAAddition = 0;
                        for (int i = 0; i <= sewingMasterViewLineList.Count - 1; i++)
                        {

                            SewingMasterViewModel sewingMasterView = sewingMasterViewLineList[i];

                            #region Calculate for Sewing
                            int qtySewingQuota = sewingMasterView.SewingQuota;
                            int optSewing = 0;
                            if (qtySewingQuota > 0)
                            {
                                DateTime dtSewingStartDateTemp = TimeHelper.Convert(sewingMasterView.SewingActualStartDate);
                                if ((String.IsNullOrEmpty(sewingMasterView.SewingActualStartDate) == false && dtSewingStartDateTemp != dtNothing)
                                    || sewingMasterView == sewingMasterViewLineList.First())
                                {
                                    dtSewingStartDate = dtSewingStartDateTemp;
                                }
                                else
                                {
                                    dtSewingStartDate = dtSewingFinishDate.AddDays(daySewingAddition);
                                }
                                //sewingMasterView.SewingStartDate = dtSewingStartDate;
                                daySewingAddition = 0;
                                DateTime dtSewingFinishDateTemp = TimeHelper.Convert(sewingMasterView.SewingActualFinishDate);
                                if (String.IsNullOrEmpty(sewingMasterView.SewingActualFinishDate) == false && dtSewingFinishDateTemp != dtNothing)
                                {
                                    dtSewingFinishDate = dtSewingFinishDateTemp;
                                }
                                else
                                {
                                    int qtySewingBalance = 0;
                                    sewingMasterView.SewingBalance = sewingMasterView.SewingBalance.Trim();
                                    int.TryParse(sewingMasterView.SewingBalance, out qtySewingBalance);
                                    if (qtySewingBalance > 0)
                                    {
                                        dtSewingFinishDate = DateTime.Now.Date.AddDays((double)(qtySewingBalance) / (double)qtySewingQuota);
                                        optSewing = 1;
                                    }
                                    else
                                    {
                                        DateTime dtSewingBalance = TimeHelper.Convert(sewingMasterView.SewingBalance);
                                        if (String.IsNullOrEmpty(sewingMasterView.SewingBalance) == true)
                                        {
                                            dtSewingFinishDate = dtSewingStartDate.AddDays((double)sewingMasterView.Quantity / (double)qtySewingQuota);
                                            optSewing = 2;
                                        }
                                        else if (String.IsNullOrEmpty(sewingMasterView.SewingBalance) == false && dtSewingBalance != dtNothing)
                                        {
                                            dtSewingFinishDate = dtSewingBalance.AddDays(0);
                                            optSewing = 0;
                                            daySewingAddition = 1;
                                        }
                                    }
                                }
                                //sewingMasterView.SewingFinishDate = dtSewingFinishDate;
                                if (optSewing == 0)
                                {
                                    sewingMasterView.SewingStartDate = dtSewingStartDate;
                                    sewingMasterView.SewingFinishDate = dtSewingFinishDate;
                                }
                                else if (optSewing == 1)
                                {
                                    //List<DateTime> dtCheckOffDateList1_1 = CheckOffDay(dtSewingStartDate, DateTime.Now.Date);
                                    List<DateTime> dtCheckOffDateList1 = CheckOffDay(DateTime.Now.Date.AddDays(0), dtSewingFinishDate);
                                    //sewingMasterView.SewingStartDate = new DateTime(dtCheckOffDateList1.First().Year, dtCheckOffDateList1.First().Month, dtCheckOffDateList1.First().Day,
                                    //dtSewingStartDate.Hour, dtSewingStartDate.Minute, dtSewingStartDate.Second);
                                    sewingMasterView.SewingStartDate = dtSewingStartDate;
                                    sewingMasterView.SewingFinishDate = new DateTime(dtCheckOffDateList1.Last().Year, dtCheckOffDateList1.Last().Month, dtCheckOffDateList1.Last().Day,
                                        dtSewingFinishDate.Hour, dtSewingFinishDate.Minute, dtSewingFinishDate.Second);
                                }
                                else if (optSewing == 2)
                                {
                                    List<DateTime> dtCheckOffDateList2 = CheckOffDay(dtSewingStartDate, dtSewingFinishDate);
                                    sewingMasterView.SewingStartDate = new DateTime(dtCheckOffDateList2.First().Year, dtCheckOffDateList2.First().Month, dtCheckOffDateList2.First().Day,
                                        dtSewingStartDate.Hour, dtSewingStartDate.Minute, dtSewingStartDate.Second);
                                    sewingMasterView.SewingFinishDate = new DateTime(dtCheckOffDateList2.Last().Year, dtCheckOffDateList2.Last().Month, dtCheckOffDateList2.Last().Day,
                                        dtSewingFinishDate.Hour, dtSewingFinishDate.Minute, dtSewingFinishDate.Second);
                                }

                                sewingMasterView.SewingStartDateForeground = Brushes.Black;
                                if (sewingMasterView.SewingStartDate < new DateTime(Math.Max(sewingMasterView.UpperMatsArrivalOrginal.Ticks, sewingMasterView.SewingMatsArrivalOrginal.Ticks)))
                                {
                                    sewingMasterView.SewingStartDateForeground = Brushes.Red;
                                }
                                sewingMasterView.SewingFinishDateForeground = Brushes.Black;
                                if (sewingMasterView.SewingFinishDate > sewingMasterView.ETD)
                                {
                                    sewingMasterView.SewingFinishDateForeground = Brushes.Red;
                                }

                                dtSewingFinishDate = sewingMasterView.SewingFinishDate;
                            }
                            #endregion

                            // CutAStartDate should be start before sewingStartDate more than 10 days.
                            #region Caculate for CutA
                            int qtyCutAQuota = sewingMasterView.CutAQuota;
                            int optCutA = 0;
                            if (qtyCutAQuota > 0)
                            {
                                DateTime dtCutAStartDateTemp = TimeHelper.Convert(sewingMasterView.CutAActualStartDate);
                                if ((String.IsNullOrEmpty(sewingMasterView.CutAActualStartDate) == false && dtCutAStartDateTemp != dtNothing)
                                    || sewingMasterView == sewingMasterViewLineList.First())
                                {
                                    dtCutAStartDate = dtCutAStartDateTemp;

                                    // CutAStartDate should be start before SewingStartDate more than 10 days
                                    if ((sewingMasterView.SewingStartDate - dtCutAStartDate).TotalDays < 10)
                                    {
                                        var beforeDate = sewingMasterView.SewingStartDate.AddDays(-10);
                                        var dtCheckOffDateSewingStartDateList = CheckOffDay(beforeDate, sewingMasterView.SewingStartDate);

                                        dtCutAStartDate = new DateTime(dtCheckOffDateSewingStartDateList.First().Year, dtCheckOffDateSewingStartDateList.First().Month, dtCheckOffDateSewingStartDateList.First().Day,
                                        dtCutAStartDate.Hour, dtCutAStartDate.Minute, dtCutAStartDate.Second);
                                    }
                                }
                                else
                                {
                                    dtCutAStartDate = dtCutAFinishDate.AddDays(dayCutAAddition);

                                    // CutAStartDate should be start before SewingStartDate more than 10 days
                                    if ((sewingMasterView.SewingStartDate - dtCutAStartDate).TotalDays < 10)
                                    {
                                        var beforeDate = sewingMasterView.SewingStartDate.AddDays(-10);
                                        var dtCheckOffDateSewingStartDateList = CheckOffDay(beforeDate, sewingMasterView.SewingStartDate);

                                        dtCutAStartDate = new DateTime(dtCheckOffDateSewingStartDateList.First().Year, dtCheckOffDateSewingStartDateList.First().Month, dtCheckOffDateSewingStartDateList.First().Day,
                                        dtCutAStartDate.Hour, dtCutAStartDate.Minute, dtCutAStartDate.Second);
                                    }
                                }
                                dayCutAAddition = 0;
                                DateTime dtCutAFinishDateTemp = TimeHelper.Convert(sewingMasterView.CutAActualFinishDate);
                                if (String.IsNullOrEmpty(sewingMasterView.CutAActualFinishDate) == false && dtCutAFinishDateTemp != dtNothing)
                                {
                                    dtCutAFinishDate = dtCutAFinishDateTemp;
                                }
                                else
                                {
                                    int qtyCutABalance = 0;
                                    sewingMasterView.CutABalance = sewingMasterView.CutABalance.Trim();
                                    int.TryParse(sewingMasterView.CutABalance, out qtyCutABalance);
                                    if (qtyCutABalance > 0)
                                    {
                                        dtCutAFinishDate = DateTime.Now.Date.AddDays((double)(qtyCutABalance) / (double)qtyCutAQuota);
                                        optCutA = 1;
                                    }
                                    else
                                    {
                                        DateTime dtCutABalance = TimeHelper.Convert(sewingMasterView.CutABalance);
                                        if (String.IsNullOrEmpty(sewingMasterView.CutABalance) == true)
                                        {
                                            dtCutAFinishDate = dtCutAStartDate.AddDays((double)sewingMasterView.Quantity / (double)qtyCutAQuota);
                                            optCutA = 2;
                                        }
                                        else if (String.IsNullOrEmpty(sewingMasterView.CutABalance) == false && dtCutABalance != dtNothing)
                                        {
                                            dtCutAFinishDate = dtCutABalance.AddDays(0);
                                            optCutA = 0;
                                            dayCutAAddition = 1;
                                        }
                                    }
                                }

                                if (optCutA == 0)
                                {
                                    sewingMasterView.CutAStartDate = dtCutAStartDate;
                                    sewingMasterView.CutAFinishDate = dtCutAFinishDate;
                                }
                                else if (optCutA == 1)
                                {
                                    List<DateTime> dtCutACheckOffDateList1 = CheckOffDay(DateTime.Now.Date.AddDays(0), dtCutAFinishDate);
                                    sewingMasterView.CutAStartDate = dtCutAStartDate;
                                    sewingMasterView.CutAFinishDate = new DateTime(dtCutACheckOffDateList1.Last().Year, dtCutACheckOffDateList1.Last().Month, dtCutACheckOffDateList1.Last().Day,
                                        dtCutAFinishDate.Hour, dtCutAFinishDate.Minute, dtCutAFinishDate.Second);
                                }
                                else if (optCutA == 2)
                                {
                                    List<DateTime> dtCutACheckOffDateList2 = CheckOffDay(dtCutAStartDate, dtCutAFinishDate);
                                    sewingMasterView.CutAStartDate = new DateTime(dtCutACheckOffDateList2.First().Year, dtCutACheckOffDateList2.First().Month, dtCutACheckOffDateList2.First().Day,
                                        dtCutAStartDate.Hour, dtCutAStartDate.Minute, dtCutAStartDate.Second);
                                    sewingMasterView.CutAFinishDate = new DateTime(dtCutACheckOffDateList2.Last().Year, dtCutACheckOffDateList2.Last().Month, dtCutACheckOffDateList2.Last().Day,
                                        dtCutAFinishDate.Hour, dtCutAFinishDate.Minute, dtCutAFinishDate.Second);
                                }
                                dtCutAFinishDate = sewingMasterView.CutAFinishDate;
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
                    searchBox.GetFindWhat = new RawMaterialSearchBoxWindow.GetString(SearchSewingMaster);
                    searchBox.Show();
                }
            }
        }

        private void SearchSewingMaster(string findWhat, bool isMatch, bool isShow)
        {
            //var a = dgSewingMaster.SelectedCells;
            //DataGridColumn b = a[0].Column;
            //Binding c = (Binding)b.ClipboardContentBinding;
            //PropertyPath path = c.Path;
            //DependencyProperty d = DependencyProperty.Register(path.Path, typeof(string), typeof(SewingMasterViewModel));
            //var e = sewingMasterViewList.Select(s => GetValue(d)).First();
            sewingMasterViewList = sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence).ToList();
            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList);
            if (String.IsNullOrEmpty(findWhat) == false)
            {
                if (isMatch == true)
                {
                    SewingMasterViewModel sewingMasterViewFind = sewingMasterViewFindList.Where(r =>
                        r.ProductNo.ToLower() == findWhat.ToLower() || r.Country.ToLower() == findWhat.ToLower() || r.ShoeName.ToLower() == findWhat.ToLower() || r.ArticleNo.ToLower() == findWhat.ToLower() ||
                        r.PatternNo.ToLower() == findWhat.ToLower() || r.ETD.ToString("dd/MM/yyyy") == findWhat.ToLower() || r.SewingLine.ToLower() == findWhat.ToLower()).FirstOrDefault();
                    if (sewingMasterViewFind != null)
                    {
                        dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                        dgSewingMaster.SelectedItem = sewingMasterViewFind;
                        dgSewingMaster.ScrollIntoView(sewingMasterViewFind);
                        colSewingLine.CanUserSort = true;
                    }
                    else
                    {
                        dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                        MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    if (isMatch == false)
                    {
                        if (isShow == true)
                        {
                            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewFindList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == true || r.Country.ToLower().Contains(findWhat.ToLower()) == true || r.ShoeName.ToLower().Contains(findWhat.ToLower()) == true || r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == true ||
                            r.PatternNo.ToLower().Contains(findWhat.ToLower()) == true || r.ETD.ToString("dd/MM/yyyy").Contains(findWhat.ToLower()) == true || r.SewingLine.ToLower().Contains(findWhat.ToLower()) == true));
                        }
                        else
                        {
                            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewFindList.Where(r =>
                            r.ProductNo.ToLower().Contains(findWhat.ToLower()) == false && r.Country.ToLower().Contains(findWhat.ToLower()) == false && r.ShoeName.ToLower().Contains(findWhat.ToLower()) == false && r.ArticleNo.ToLower().Contains(findWhat.ToLower()) == false &&
                            r.PatternNo.ToLower().Contains(findWhat.ToLower()) == false && r.ETD.ToString("dd/MM/yyyy").Contains(findWhat.ToLower()) == false && r.SewingLine.ToLower().Contains(findWhat.ToLower()) == false));
                        }

                        if (sewingMasterViewFindList.Count > 0)
                        {
                            dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                            colSewingLine.CanUserSort = false;
                        }
                        else
                        {
                            dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                            MessageBox.Show("Not Found!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                            sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList);
                            dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                        }
                    }
                }
            }
            else
            {
                colSewingLine.CanUserSort = true;
                dgSewingMaster.ItemsSource = sewingMasterViewFindList;
            }
        }

        private void dgSewingMaster_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)e.Row.Item;
            if (sewingMasterView == null)
            {
                return;
            }

            string productNo = sewingMasterView.ProductNo;

            if (e.Column == colSewingLine || e.Column == colSewingQuota || e.Column == colSewingPrep || e.Column == colSewingActualStartDate ||
                e.Column == colSewingActualFinishDate || e.Column == colSewingBalance)
            {
                lineSewingEditingList.Add(sewingMasterView.SewingLine);
                if (e.Column == colSewingLine)
                {
                    sewingLineUpdateList.Add(productNo);
                }
                if (e.Column == colSewingQuota)
                {
                    sewingQuotaUpdateList.Add(productNo);
                }

                if (e.Column == colSewingPrep)
                {
                    sewingPrepUpdateList.Add(productNo);
                }

                if (e.Column == colSewingActualStartDate)
                {
                    sewingActualStartDateUpdateList.Add(productNo);
                }
                if (e.Column == colSewingActualFinishDate)
                {
                    sewingActualFinishDateUpdateList.Add(productNo);
                }
                if (e.Column == colSewingBalance)
                {
                    sewingBalanceUpdateList.Add(productNo);
                }

            }
            else if (e.Column == colCutAQuota || e.Column == colCutAActualStartDate || e.Column == colCutAActualFinishDate ||
                e.Column == colCutABalance)
            {
                lineCutPrepEditingList.Add(sewingMasterView.SewingLine);
                if (e.Column == colCutAQuota)
                {
                    cutAQuotaUpdateList.Add(productNo);
                }
                if (e.Column == colCutAActualStartDate)
                {
                    cutAActualStartDateUpdateList.Add(productNo);
                }
                if (e.Column == colCutAActualFinishDate)
                {
                    cutAActualFinishDateUpdateList.Add(productNo);
                }
                if (e.Column == colCutABalance)
                {
                    cutABalanceUpdateList.Add(productNo);
                }
            }
            else if (e.Column == colCutBActualStartDate || e.Column == colPrintingBalance || e.Column == colH_FBalance || e.Column == colEmbroideryBalance ||
                e.Column == colCutBBalance || e.Column == colAutoCut || e.Column == colLaserCut || e.Column == colHuasenCut)
            {
                if (e.Column == colPrintingBalance)
                {
                    printingBalanceUpdateList.Add(productNo);
                }
                if (e.Column == colH_FBalance)
                {
                    h_fBalanceUpdateList.Add(productNo);
                }
                if (e.Column == colEmbroideryBalance)
                {
                    embroideryBalanceUpdateList.Add(productNo);
                }

                if (e.Column == colCutBActualStartDate)
                {
                    cutBActualStartDateUpdateList.Add(productNo);
                }

                if (e.Column == colCutBBalance)
                {
                    cutBBalanceUpdateList.Add(productNo);
                }
                if (e.Column == colAutoCut)
                {
                    autoCutUpdateList.Add(productNo);
                }
                if (e.Column == colLaserCut)
                {
                    laserCutUpdateList.Add(productNo);
                }
                if (e.Column == colHuasenCut)
                {
                    huasenCutUpdateList.Add(productNo);
                }
            }

            if (e.Column == colSewingLine)
            {
                //SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)e.Row.Item;
                string sewingLine = sewingMasterView.SewingLine;
                if (String.IsNullOrEmpty(sewingLine) == true)
                {
                    return;
                }
                int sewingSequence = 0;
                if (sewingMasterViewList.Where(s => s.SewingLine == sewingLine).Count() > 0)
                {
                    sewingSequence = sewingMasterViewList.Where(s => s.SewingLine == sewingLine).Select(s => s.Sequence).Max() + 1;
                }
                sewingMasterView.Sequence = sewingSequence;
                isSequenceEditing = true;
            }
            if (e.Column == colSewingActualStartDate || e.Column == colSewingActualFinishDate)
            {
                TextBox txtElement = (TextBox)e.EditingElement as TextBox;
                if (String.IsNullOrEmpty(txtElement.Text) == false && TimeHelper.Convert(txtElement.Text) == dtNothing)
                {
                    txtElement.Foreground = Brushes.Red;
                    txtElement.Text = "!";
                    txtElement.SelectAll();
                }
            }
            if (e.Column == colCutAActualStartDate || e.Column == colSewingActualFinishDate || e.Column == colCutBActualStartDate)
            {
                TextBox txtElement = (TextBox)e.EditingElement as TextBox;
                if (String.IsNullOrEmpty(txtElement.Text) == false && TimeHelper.Convert(txtElement.Text) == dtNothing)
                {
                    txtElement.Foreground = Brushes.Red;
                    txtElement.Text = "!";
                    txtElement.SelectAll();
                }
            }

            if (e.Column == colSewingPrep)
            {
                TextBox txtElement = (TextBox)e.EditingElement as TextBox;
                int sewingPrep = 0;
                Int32.TryParse(txtElement.Text,out sewingPrep);
                if (sewingPrep == sewingMasterView.SewingQuota && sewingPrep != 0)
                {
                    txtElement.Text = String.Format("{0:MM/dd}", DateTime.Now);
                }
            }

            isEditing = false;
        }

        private void dgSewingMaster_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column == colSewingLine)
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection.Value == ListSortDirection.Descending)
                {
                    sewingMasterViewList = sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence).ToList();
                    sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList.OrderBy(s => s.SewingLine).ThenBy(s => s.Sequence));
                }
                else
                {
                    sewingMasterViewList = sewingMasterViewList.OrderByDescending(s => s.SewingLine).ThenBy(s => s.Sequence).ToList();
                    sewingMasterViewFindList = new ObservableCollection<SewingMasterViewModel>(sewingMasterViewList.OrderByDescending(s => s.SewingLine).ThenBy(s => s.Sequence));
                }
                dgSewingMaster.ItemsSource = sewingMasterViewFindList;
                for (int i = 0; i <= sewingMasterViewFindList.Count - 1; i++)
                {
                    sewingMasterViewFindList[i].Sequence = i;
                }
                dgSewingMaster.ScrollIntoView(sewingMasterViewFindList.Where(s => String.IsNullOrEmpty(s.SewingLine) == false).FirstOrDefault());
                e.Handled = true;
            }
        }

        private void dgSewingMaster_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)dgSewingMaster.CurrentItem;
            if (account.AssemblyMaster == true && dgSewingMaster.CurrentCell.Column == colSewingBalance && sewingMasterView != null)
            {
                SewingInputOutputWindow window = new SewingInputOutputWindow(sewingMasterView.ProductNo, sewingMasterView.SewingActualStartDateAuto, sewingMasterView.SewingActualFinishDateAuto);
                if (window.ShowDialog() == true)
                {
                    string productNo = sewingMasterView.ProductNo;
                    string sewingLine = sewingMasterView.SewingLine;
                    sewingMasterView.SewingBalance = window.resultString;
                    sewingMasterView.SewingActualStartDateAuto = window.sewingActualStartDateAuto;
                    sewingMasterView.SewingActualFinishDateAuto = window.sewingActualFinishDateAuto;
                    if (String.IsNullOrEmpty(window.resultString) == true)
                    {
                        sewingMasterView.SewingActualStartDateAuto = "";
                        sewingMasterView.SewingActualFinishDateAuto = "";
                    }
                    lineSewingEditingList.Add(sewingLine);
                    sewingActualStartDateUpdateAutoList.Add(productNo);
                    sewingActualFinishDateUpdateAutoList.Add(productNo);
                    sewingBalanceUpdateList.Add(productNo);
                }
            }
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false && simulationMode == false)
            {
                this.Cursor = Cursors.Wait;
                sewingMasterViewToInsertList = dgSewingMaster.Items.OfType<SewingMasterViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwInsert.IsBusy == false && simulationMode == false)
            {
                this.Cursor = Cursors.Wait;
                sewingMasterViewToInsertList = dgSewingMaster.Items.OfType<SewingMasterViewModel>().ToList();
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (SewingMasterViewModel sewingMaster in sewingMasterViewToInsertList)
            {
                var productNo = sewingMaster.ProductNo;
                var sewingLine = sewingMaster.SewingLine;
                var model = new SewingMasterModel
                {
                    ProductNo = productNo,
                    Sequence = sewingMaster.Sequence,
                    SewingLine = sewingLine,
                    SewingStartDate = sewingMaster.SewingStartDate,
                    SewingFinishDate = sewingMaster.SewingFinishDate,
                    SewingPrep = sewingMaster.SewingPrep,
                    SewingQuota = sewingMaster.SewingQuota,
                    SewingActualStartDate = sewingMaster.SewingActualStartDate,
                    SewingActualFinishDate = sewingMaster.SewingActualFinishDate,

                    SewingActualStartDateAuto = sewingMaster.SewingActualStartDateAuto,
                    SewingActualFinishDateAuto = sewingMaster.SewingActualFinishDateAuto,

                    SewingBalance = sewingMaster.SewingBalance,
                    CutAStartDate = sewingMaster.CutAStartDate,
                    CutAFinishDate = sewingMaster.CutAFinishDate,
                    CutAQuota = sewingMaster.CutAQuota,
                    CutAActualStartDate = sewingMaster.CutAActualStartDate,
                    CutAActualFinishDate = sewingMaster.CutAActualFinishDate,
                    CutABalance = sewingMaster.CutABalance,
                    PrintingBalance = sewingMaster.PrintingBalance,
                    H_FBalance = sewingMaster.H_FBalance,
                    EmbroideryBalance = sewingMaster.EmbroideryBalance,

                    CutBActualStartDate = sewingMaster.CutBActualStartDate,
                    CutBBalance = sewingMaster.CutBBalance,
                    AutoCut = sewingMaster.AutoCut,
                    LaserCut = sewingMaster.LaserCut,
                    HuasenCut = sewingMaster.HuasenCut,

                    IsSequenceUpdate = false,
                    IsSewingLineUpdate = false,
                    IsSewingStartDateUpdate = false,
                    IsSewingFinishDateUpdate = false,
                    IsSewingQuotaUpdate = false,
                    IsSewingPrepUpdate = false,

                    IsSewingActualStartDateUpdate = false,
                    IsSewingActualFinishDateUpdate = false,

                    IsSewingActualStartDateAutoUpdate = false,
                    IsSewingActualFinishDateAutoUpdate = false,

                    IsSewingBalanceUpdate = false,
                    IsCutAStartDateUpdate = false,
                    IsCutAFinishDateUpdate = false,
                    IsCutAQuotaUpdate = false,
                    IsCutAActualStartDateUpdate = false,
                    IsCutAActualFinishDateUpdate = false,
                    IsCutABalanceUpdate = false,
                    IsPrintingBalanceUpdate = false,
                    IsH_FBalanceUpdate = false,
                    IsEmbroideryBalanceUpdate = false,

                    IsCutBActualStartDateUpdate = false,
                    IsCutBBalanceUpdate = false,
                    IsAutoCutUpdate = false,
                    IsLaserCutUpdate = false,
                    IsHuasenCutUpdate = false
                };

                model.IsSequenceUpdate = isSequenceEditing;

                model.IsSewingLineUpdate = sewingLineUpdateList.Contains(productNo);
                model.IsSewingStartDateUpdate = lineSewingEditingList.Contains(sewingLine);
                model.IsSewingFinishDateUpdate = lineSewingEditingList.Contains(sewingLine);
                model.IsSewingQuotaUpdate = sewingQuotaUpdateList.Contains(productNo);

                model.IsSewingPrepUpdate = sewingPrepUpdateList.Contains(productNo);

                model.IsSewingActualStartDateUpdate = sewingActualStartDateUpdateList.Contains(productNo);
                model.IsSewingActualFinishDateUpdate = sewingActualFinishDateUpdateList.Contains(productNo);

                model.IsSewingActualStartDateAutoUpdate = sewingActualStartDateUpdateAutoList.Contains(productNo);
                model.IsSewingActualFinishDateAutoUpdate = sewingActualFinishDateUpdateAutoList.Contains(productNo);

                model.IsSewingBalanceUpdate = sewingBalanceUpdateList.Contains(productNo);

                model.IsCutAStartDateUpdate = lineCutPrepEditingList.Contains(sewingLine);
                model.IsCutAFinishDateUpdate = lineCutPrepEditingList.Contains(sewingLine);
                model.IsCutAQuotaUpdate = cutAQuotaUpdateList.Contains(productNo);
                model.IsCutAActualStartDateUpdate = cutAActualStartDateUpdateList.Contains(productNo);
                model.IsCutAActualFinishDateUpdate = cutAActualFinishDateUpdateList.Contains(productNo);
                model.IsCutABalanceUpdate = cutABalanceUpdateList.Contains(productNo);


                model.IsPrintingBalanceUpdate = printingBalanceUpdateList.Contains(productNo);
                model.IsH_FBalanceUpdate = h_fBalanceUpdateList.Contains(productNo);
                model.IsEmbroideryBalanceUpdate = embroideryBalanceUpdateList.Contains(productNo);
                model.IsCutBActualStartDateUpdate = cutBActualStartDateUpdateList.Contains(productNo);
                model.IsCutBBalanceUpdate = cutBBalanceUpdateList.Contains(productNo);
                model.IsAutoCutUpdate = autoCutUpdateList.Contains(productNo);
                model.IsLaserCutUpdate = laserCutUpdateList.Contains(productNo);
                model.IsHuasenCutUpdate = huasenCutUpdateList.Contains(productNo);

                if (model.IsSequenceUpdate == true ||
                    model.IsSewingLineUpdate == true ||
                    model.IsSewingStartDateUpdate == true ||
                    model.IsSewingFinishDateUpdate == true ||
                    model.IsSewingQuotaUpdate == true ||

                    model.IsSewingPrepUpdate == true ||

                    model.IsSewingActualStartDateUpdate == true ||
                    model.IsSewingActualFinishDateUpdate == true ||

                    model.IsSewingActualStartDateAutoUpdate == true ||
                    model.IsSewingActualFinishDateAutoUpdate == true ||

                    model.IsSewingBalanceUpdate == true ||
                    model.IsCutAStartDateUpdate == true ||
                    model.IsCutAFinishDateUpdate == true ||
                    model.IsCutAQuotaUpdate == true ||
                    model.IsCutAActualStartDateUpdate == true ||
                    model.IsCutAActualFinishDateUpdate == true ||
                    model.IsCutBActualStartDateUpdate == true ||
                    model.IsCutABalanceUpdate == true ||
                    model.IsPrintingBalanceUpdate == true ||
                    model.IsH_FBalanceUpdate == true ||
                    model.IsEmbroideryBalanceUpdate == true ||
                    model.IsCutBBalanceUpdate == true ||
                    model.IsAutoCutUpdate == true ||
                    model.IsLaserCutUpdate == true ||
                    model.IsHuasenCutUpdate == true)
                {
                    SewingMasterController.Insert_2(model);

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
            lineSewingEditingList.Clear();
            lineCutPrepEditingList.Clear();

            sewingLineUpdateList.Clear();
            sewingQuotaUpdateList.Clear();

            sewingPrepUpdateList.Clear();

            sewingActualStartDateUpdateList.Clear();
            sewingActualFinishDateUpdateList.Clear();

            sewingActualStartDateUpdateAutoList.Clear();
            sewingActualFinishDateUpdateAutoList.Clear();

            sewingBalanceUpdateList.Clear();

            cutAQuotaUpdateList.Clear();
            cutAActualStartDateUpdateList.Clear();
            cutAActualFinishDateUpdateList.Clear();
            cutABalanceUpdateList.Clear();

            printingBalanceUpdateList.Clear();
            h_fBalanceUpdateList.Clear();
            embroideryBalanceUpdateList.Clear();
            cutBActualStartDateUpdateList.Clear();
            cutBBalanceUpdateList.Clear();
            autoCutUpdateList.Clear();
            laserCutUpdateList.Clear();
            huasenCutUpdateList.Clear();
            
            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void dgSewingMaster_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isEditing = true;
        }

        private List<SewingMasterViewModel> sewingMasterViewSelectList = new List<SewingMasterViewModel>();
        private void dgSewingMaster_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popup.IsOpen = false;
            sewingMasterViewSelectList.Clear();
            var dataGrid = (DataGrid)sender;
            if (dataGrid != null)
            {
                foreach (DataGridCellInfo cellInfo in dataGrid.SelectedCells)
                {
                    sewingMasterViewSelectList.Add((SewingMasterViewModel)cellInfo.Item);
                }
                sewingMasterViewSelectList = sewingMasterViewSelectList.Distinct().ToList();
            }
        }

        private void dgSewingMaster_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && account.SewingMaster == true && isEditing == false)
            {
                var dataGrid = (DataGrid)sender;
                if (dataGrid != null)
                {
                    if (e.OriginalSource.GetType() == typeof(Thumb))
                    {
                        return;
                    }
                    if ((SewingMasterViewModel)dataGrid.CurrentItem != null && sewingMasterViewSelectList.Contains((SewingMasterViewModel)dataGrid.CurrentItem) == false)
                    {
                        sewingMasterViewSelectList.Add((SewingMasterViewModel)dataGrid.CurrentItem);
                    }
                    if (sewingMasterViewSelectList.Count > 0)
                    {
                        listView.ItemsSource = sewingMasterViewSelectList;
                        popup.PlacementTarget = lblPopup;
                        DragDrop.DoDragDrop(dataGrid, sewingMasterViewSelectList, DragDropEffects.Move);
                    }
                }
            }
        }

        private void dgSewingMaster_DragLeave(object sender, DragEventArgs e)
        {
            var frameworkElement = (FrameworkElement)e.OriginalSource;

            if (frameworkElement != null && frameworkElement.DataContext != null
                && frameworkElement.DataContext.GetType() == typeof(SewingMasterViewModel))
            {
                SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)frameworkElement.DataContext;
                dgSewingMaster.SelectedItem = sewingMasterView;
                dgSewingMaster.ScrollIntoView(sewingMasterView);
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
                var sewingMasterView = (SewingMasterViewModel)dataGrid.SelectedItem;
                int index = dataGrid.Items.IndexOf(sewingMasterView);
                int indexFirst = dataGrid.Items.IndexOf(sewingMasterViewSelectList.First());
                int indexLast = dataGrid.Items.IndexOf(sewingMasterViewSelectList.Last());
                if (index < indexFirst && index < indexLast)
                {
                    for (int i = sewingMasterViewSelectList.Count - 1; i >= 0; i = i - 1)
                    {
                        sewingMasterViewFindList.Remove(sewingMasterViewSelectList[i]);
                        sewingMasterViewFindList.Insert(index, sewingMasterViewSelectList[i]);
                        sewingMasterViewSelectList[i].Sequence = sewingMasterView.Sequence + i;
                    }
                    for (int i = index + sewingMasterViewSelectList.Count; i <= sewingMasterViewFindList.Count - 1; i++)
                    {
                        sewingMasterViewFindList[i].Sequence = sewingMasterViewFindList[i].Sequence + sewingMasterViewSelectList.Count;
                    }
                    isSequenceEditing = true;
                }
                else if (index > indexFirst && index > indexLast)
                {
                    for (int i = 0; i <= sewingMasterViewSelectList.Count - 1; i = i + 1)
                    {
                        sewingMasterViewFindList.Remove(sewingMasterViewSelectList[i]);
                        sewingMasterViewFindList.Insert(index - 1, sewingMasterViewSelectList[i]);
                        sewingMasterViewSelectList[i].Sequence = sewingMasterView.Sequence + i;
                    }
                    for (int i = index; i <= sewingMasterViewFindList.Count - 1; i++)
                    {
                        sewingMasterViewFindList[i].Sequence = sewingMasterViewFindList[i].Sequence + sewingMasterViewSelectList.Count;
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
            if ((isSequenceEditing == true || lineSewingEditingList.Count > 0 || lineCutPrepEditingList.Count > 0 ||

                sewingLineUpdateList.Count > 0 || sewingQuotaUpdateList.Count > 0 || sewingActualStartDateUpdateList.Count > 0 ||
                sewingActualFinishDateUpdateList.Count > 0 || sewingBalanceUpdateList.Count > 0 ||

                cutAQuotaUpdateList.Count > 0 || cutAActualStartDateUpdateList.Count > 0 || cutAActualFinishDateUpdateList.Count > 0 ||
                cutABalanceUpdateList.Count > 0 ||

                printingBalanceUpdateList.Count > 0 || h_fBalanceUpdateList.Count > 0 || embroideryBalanceUpdateList.Count > 0 ||
                cutBBalanceUpdateList.Count > 0 || autoCutUpdateList.Count > 0) && simulationMode == false)
            {
                MessageBoxResult result = MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (bwInsert.IsBusy == false)
                    {
                        e.Cancel = true;
                        this.Cursor = Cursors.Wait;
                        sewingMasterViewToInsertList = dgSewingMaster.Items.OfType<SewingMasterViewModel>().ToList();
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
                    SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)dataGridCellInfo.Item;
                    if (sewingMasterView != null)
                    {
                        sewingMasterView.ProductNoBackground = Brushes.Transparent;
                    }
                }
            }
            foreach(DataGridCellInfo dataGridCellInfo in e.AddedCells)
            {
                SewingMasterViewModel sewingMasterView = (SewingMasterViewModel)dataGridCellInfo.Item;
                if (sewingMasterView != null)
                {
                    sewingMasterView.ProductNoBackground = Brushes.RoyalBlue;
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

        private void miTranfer_Click(object sender, RoutedEventArgs e)
        {
            sewingMasterViewToInsertList = dgSewingMaster.SelectedItems.OfType<SewingMasterViewModel>().ToList();
            if (sewingMasterViewToInsertList.Count <= 0 ||
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

        bool simulationMode = false;
        string title = "";
        private void btnEnableSimulation_Click(object sender, RoutedEventArgs e)
        {
            dgSewingMaster.AlternatingRowBackground = Brushes.White;
            dgSewingMaster.RowBackground = Brushes.White;

            title = "Master Schedule - Sewing Simulation File";
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

                title = "Master Schedule - Sewing Master File";
                this.Title = title;

                ctmTranfer.Visibility = Visibility.Collapsed;
                simulationMode = false;

                //btnDisableSimulation.IsEnabled = false;

                btnSave.IsEnabled = false;
                btnCaculate.IsEnabled = false;

                sewingMasterViewList = new List<SewingMasterViewModel>();
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }
    }
}
