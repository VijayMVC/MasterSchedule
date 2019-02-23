using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Win32;
using MasterSchedule.Models;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using MasterSchedule.Controllers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ImportAvailableMachineWindow.xaml
    /// </summary>
    public partial class ImportAvailableMachineWindow : Window
    {
        string filePath;
        List<AvailableMachineModel> availableMachineList;
        BackgroundWorker bwLoad;
        BackgroundWorker bwImport;
        public ImportAvailableMachineWindow()
        {
            filePath = "";
            availableMachineList = new List<AvailableMachineModel>();
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            bwImport = new BackgroundWorker();
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import Available Machine";
            openFileDialog.Filter = "EXCEL Files (*.xls, *.xlsx)|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                if (bwLoad.IsBusy == false)
                {
                    this.Cursor = Cursors.Wait;
                    availableMachineList.Clear();
                    lblStatus.Text = "Reading....";
                    bwLoad.RunWorkerAsync();
                }
                else
                {
                    this.Close();
                }
            }
        }
        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            Excel.Application excelApplication = new Excel.Application();
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(filePath);
            //excelApplication.Visible = true;
            Excel.Worksheet excelWorksheet;
            Excel.Range excelRange;
            try
            {
                excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets[1];
                excelRange = excelWorksheet.UsedRange;
                progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = excelRange.Rows.Count));
                //int[] column = {2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50 };
                for (int i = 3; i <= excelRange.Rows.Count; i++)
                {
                    AvailableMachineModel availableMachine = new AvailableMachineModel();
                    var id = (excelRange.Cells[i, 1] as Excel.Range).Value2;
                    if (id != null)
                    {
                        string idValue = id.ToString();
                        availableMachine.Id = idValue;

                        //float cuttingArmCliker = 0;
                        var cuttingArmClikerValue = (excelRange.Cells[i, 3] as Excel.Range).Value2;
                        string cuttingArmCliker = "";
                        if (cuttingArmClikerValue != null)
                        {
                            cuttingArmCliker = cuttingArmClikerValue.ToString();
                        }
                        availableMachine.CuttingArmClicker = cuttingArmCliker;

                        //float cuttingBeam = 0;
                        var cuttingBeamValue = (excelRange.Cells[i, 4] as Excel.Range).Value2;
                        string cuttingBeam = "";
                        if (cuttingBeamValue != null)
                        {
                            cuttingBeam = cuttingBeamValue.ToString();
                        }
                        availableMachine.CuttingBeam = cuttingBeam;

                        //float cuttingCutStrap = 0;
                        var cuttingCutStrapValue = (excelRange.Cells[i, 5] as Excel.Range).Value2;
                        string cuttingCutStrap = "";
                        if (cuttingCutStrapValue != null)
                        {
                            cuttingCutStrap = cuttingCutStrapValue.ToString();
                        }
                        availableMachine.CuttingCutStrap = cuttingCutStrap;

                        //float cuttingLaser = 0;
                        var cuttingLaserValue = (excelRange.Cells[i, 6] as Excel.Range).Value2;
                        string cuttingLaser = "";
                        if (cuttingLaserValue != null)
                        {
                            cuttingLaser = cuttingLaserValue.ToString();
                        }
                        availableMachine.CuttingLaser = cuttingLaser;

                        var cuttingPuncherHoleValue = (excelRange.Cells[i, 7] as Excel.Range).Value2;
                        string cuttingPuncherHole = "";
                        if (cuttingPuncherHoleValue != null)
                        {
                            cuttingPuncherHole = cuttingPuncherHoleValue.ToString();
                        }
                        availableMachine.CuttingPuncherHole = cuttingPuncherHole;

                        //float cuttingSkiving = 0;
                        var cuttingSkivingValue = (excelRange.Cells[i, 8] as Excel.Range).Value2;
                        string cuttingSkiving = "";
                        if (cuttingSkivingValue != null)
                        {
                            cuttingSkiving = cuttingSkivingValue.ToString();
                        }
                        availableMachine.CuttingSkiving = cuttingSkiving;


                        // Prep
                        //float prepVerticalHF = 0;
                        var prepVerticalHFValue = (excelRange.Cells[i, 9] as Excel.Range).Value2;
                        string prepVerticalHF = "";
                        if (prepVerticalHFValue != null)
                        {
                            prepVerticalHF = prepVerticalHFValue.ToString();
                        }
                        availableMachine.PrepVerticalHF = prepVerticalHF;

                        //float prepHorizontalHF = 0;
                        var prepHorizontalHFValue = (excelRange.Cells[i, 10] as Excel.Range).Value2;
                        string prepHorizontalHF = "";
                        if (prepHorizontalHFValue != null)
                        {
                            prepHorizontalHF = prepHorizontalHFValue.ToString();
                        }
                        availableMachine.PrepHorizontalHF = prepHorizontalHF;

                        var prepOnlineHeatPressValue = (excelRange.Cells[i, 11] as Excel.Range).Value2;
                        string prepOnlineHeatPress = "";
                        if (prepOnlineHeatPressValue != null)
                        {
                            prepOnlineHeatPress = prepOnlineHeatPressValue.ToString();
                        }
                        availableMachine.PrepOnlineHeatPress = prepOnlineHeatPress;

                        //float prepAutoHF = 0;
                        var prepAutoHFValue = (excelRange.Cells[i, 12] as Excel.Range).Value2;
                        string prepAutoHF = "";
                        if (prepAutoHFValue != null)
                        {
                            prepAutoHF = prepAutoHFValue.ToString();
                        }
                        availableMachine.PrepAutoHF = prepAutoHF;

                        //float prepInye = 0;
                        var prepInyeValue = (excelRange.Cells[i, 13] as Excel.Range).Value2;
                        string prepInye = "";
                        if (prepInyeValue != null)
                        {
                            prepInye = prepInyeValue.ToString();
                        }
                        availableMachine.PrepInye = prepInye;

                        //float prepHotmeltMachine = 0;
                        var prepHotmeltMachineValue = (excelRange.Cells[i, 14] as Excel.Range).Value2;
                        string prepHotmeltMachine = "";
                        if (prepHotmeltMachineValue != null)
                        {
                            prepHotmeltMachine = prepHotmeltMachineValue.ToString();
                        }
                        availableMachine.PrepHotmeltMachine = prepHotmeltMachine;

                        // Sewing
                        //float sewingSmallComputer = 0;
                        var sewingSmallComputerValue = (excelRange.Cells[i, 15] as Excel.Range).Value2;
                        string sewingSmallComputer = "";
                        if (sewingSmallComputerValue != null)
                        {
                            sewingSmallComputer = sewingSmallComputerValue.ToString();
                        }
                        availableMachine.SewingSmallComputer = sewingSmallComputer;

                        //float sewingBigComputer = 0;
                        var sewingBigComputerValue = (excelRange.Cells[i, 16] as Excel.Range).Value2;
                        string sewingBigComputer = "";
                        if (sewingBigComputerValue != null)
                        {
                            sewingBigComputer = sewingBigComputerValue.ToString();
                        }
                        availableMachine.SewingBigComputer = sewingBigComputer;

                        //float sewingUltrasonic = 0;
                        var sewingUltrasonicValue = (excelRange.Cells[i, 17] as Excel.Range).Value2;
                        string sewingUltrasonic = "";
                        if (sewingUltrasonicValue != null)
                        {
                            sewingUltrasonic = sewingUltrasonicValue.ToString();
                        }
                        availableMachine.SewingUltrasonic = sewingUltrasonic;

                        //float sewingFourNeeldleFlat = 0;
                        var sewing4NeeldleFlatValue = (excelRange.Cells[i, 18] as Excel.Range).Value2;
                        string sewing4NeeldleFlat = "";
                        if (sewing4NeeldleFlatValue != null)
                        {
                            sewing4NeeldleFlat = sewing4NeeldleFlatValue.ToString();
                        }
                        availableMachine.Sewing4NeedleFlat = sewing4NeeldleFlat;

                        //float sewingFourNeedlePost = 0;
                        var sewing4NeedlePostValue = (excelRange.Cells[i, 19] as Excel.Range).Value2;
                        string sewing4NeedlePost = "";
                        if (sewing4NeedlePostValue != null)
                        {
                            sewing4NeedlePost = sewing4NeedlePostValue.ToString();
                        }
                        availableMachine.Sewing4NeedlePost = sewing4NeedlePost;

                        //float sewingLongTable = 0;
                        var sewingLongTableValue = (excelRange.Cells[i, 20] as Excel.Range).Value2;
                        string sewingLongTable = "";
                        if (sewingLongTableValue != null)
                        {
                            sewingLongTable = sewingLongTableValue.ToString();
                        }
                        availableMachine.SewingLongTable = sewingLongTable;

                        //float sewingEyeleting = 0;
                        var sewingEyeletingValue = (excelRange.Cells[i, 21] as Excel.Range).Value2;
                        string sewingEyeleting = "";
                        if (sewingEyeletingValue != null)
                        {
                            sewingEyeleting = sewingEyeletingValue.ToString();
                        }
                        availableMachine.SewingEyeleting = sewingEyeleting;

                        //float sewingZZBinding = 0;
                        var sewingZZBindingVlaue = (excelRange.Cells[i, 22] as Excel.Range).Value2;
                        string sewingZZBinding = "";
                        if (sewingZZBindingVlaue != null)
                        {
                            sewingZZBinding = sewingZZBindingVlaue.ToString();
                        }
                        availableMachine.SewingZZBinding = sewingZZBinding;

                        //float sewingHotmeltMachine = 0;
                        var sewingHotmeltMachineValue = (excelRange.Cells[i, 23] as Excel.Range).Value2;
                        string sewingHotmeltMachine = "";
                        if (sewingHotmeltMachineValue != null)
                        {
                            sewingHotmeltMachine = sewingHotmeltMachineValue.ToString();
                        }
                        availableMachine.SewingHotmeltMachine = sewingHotmeltMachine;

                        //float sewingHotmeltHeldMachine = 0;
                        var sewingHotmeltHeldMachineValue = (excelRange.Cells[i, 24] as Excel.Range).Value2;
                        string sewingHotmeltHeldMachine = "";
                        if (sewingHotmeltHeldMachineValue != null)
                        {
                            sewingHotmeltHeldMachine = sewingHotmeltHeldMachineValue.ToString();
                        }
                        availableMachine.SewingHandHeldHotmelt = sewingHotmeltHeldMachine;

                        //float sewingStationaryHHHotmelt = 0;
                        var sewingStationaryHHHotmeltValue = (excelRange.Cells[i, 25] as Excel.Range).Value2;
                        string sewingStationaryHHHotmelt = "";
                        if (sewingStationaryHHHotmeltValue != null)
                        {
                            sewingStationaryHHHotmelt = sewingStationaryHHHotmeltValue.ToString();
                        }
                        availableMachine.SewingStationaryHHHotmelt = sewingStationaryHHHotmelt;

                        // Stockfit
                        //float stockfitVerticalBuffing = 0;
                        var stockfitVerticalBuffingValue = (excelRange.Cells[i, 26] as Excel.Range).Value2;
                        string stockfitVerticalBuffing = "";
                        if (stockfitVerticalBuffingValue != null)
                        {
                            stockfitVerticalBuffing = stockfitVerticalBuffingValue.ToString();
                        }
                        availableMachine.StockfitVerticalBuffing = stockfitVerticalBuffing;

                        //float stockfitHorizontalBuffing = 0;
                        var stockfitHorizontalBuffingValue = (excelRange.Cells[i, 27] as Excel.Range).Value2;
                        string stockfitHorizontalBuffing = "";
                        if (stockfitHorizontalBuffingValue != null)
                        {
                            stockfitHorizontalBuffing = stockfitHorizontalBuffingValue.ToString();
                        }
                        availableMachine.StockfitHorizontalBuffing = stockfitHorizontalBuffing;

                        //float stockfitSideBuffing = 0;
                        var stockfitSideBuffingValue = (excelRange.Cells[i, 28] as Excel.Range).Value2;
                        string stockfitSideBuffing = "";
                        if (stockfitSideBuffingValue != null)
                        {
                            stockfitSideBuffing = stockfitSideBuffingValue.ToString();
                        }
                        availableMachine.StockfitSideBuffing = stockfitSideBuffing;

                        //float stockfitOutsoleStitching = 0;
                        var stockfitOutsoleStitchingValue = (excelRange.Cells[i, 29] as Excel.Range).Value2;
                        string stockfitOutsoleStitching = "";
                        if (stockfitOutsoleStitchingValue != null)
                        {
                            stockfitOutsoleStitching = stockfitOutsoleStitchingValue.ToString();
                        }
                        availableMachine.StockfitOutsoleStitching = stockfitOutsoleStitching;

                        //float stockfitAutoBuffing = 0;
                        var stockfitAutoBuffingValue = (excelRange.Cells[i, 30] as Excel.Range).Value2;
                        string stockfitAutoBuffing = "";
                        if (stockfitAutoBuffingValue != null)
                        {
                            stockfitAutoBuffing = stockfitAutoBuffingValue.ToString();
                        }
                        availableMachine.StockfitAutoBuffing = stockfitAutoBuffing;

                        //float stockfitHydraulicCutting = 0;
                        var stockfitHydraulicCuttingValue = (excelRange.Cells[i, 31] as Excel.Range).Value2;
                        string stockfitHydraulicCutting = "";
                        if (stockfitHydraulicCuttingValue != null)
                        {
                            stockfitHydraulicCutting = stockfitHydraulicCuttingValue.ToString();
                        }
                        availableMachine.StockfitHydraulicCutting = stockfitHydraulicCutting;

                        //float stockfitPadPrinting = 0;
                        var stockfitPadPrintingValue = (excelRange.Cells[i, 32] as Excel.Range).Value2;
                        string stockfitPadPrinting = "";
                        if (stockfitPadPrintingValue != null)
                        {
                            stockfitPadPrinting = stockfitPadPrintingValue.ToString();
                        }
                        availableMachine.StockfitPadPrinting = stockfitPadPrinting;

                        // Assembly
                        //float assemblyToeLasting = 0;
                        var assemblyToeLastingValue = (excelRange.Cells[i, 33] as Excel.Range).Value2;
                        string assemblyToeLasting = "";
                        if (assemblyToeLastingValue != null)
                        {
                            assemblyToeLasting = assemblyToeLastingValue.ToString();
                        }
                        availableMachine.AssemblyToeLasting = assemblyToeLasting;

                        //float assemblySideLasting = 0;
                        var assemblySideLastingValue = (excelRange.Cells[i, 34] as Excel.Range).Value2;
                        string assemblySideLasting = "";
                        if (assemblySideLastingValue != null)
                        {
                            assemblySideLasting = assemblySideLastingValue.ToString();
                        }
                        availableMachine.AssemblySideLasting = assemblySideLasting;

                        //float assemblyHeelLasting = 0;
                        var assemblyHeelLastingValue = (excelRange.Cells[i, 35] as Excel.Range).Value2;
                        string assemblyHeelLasting = "";
                        if (assemblyHeelLastingValue != null)
                        {
                            assemblyHeelLasting = assemblyHeelLastingValue.ToString();
                        }
                        availableMachine.AssemblyHeelLasting = assemblyHeelLasting;

                        //float assemblySidePress = 0;
                        var assemblySidePressValue = (excelRange.Cells[i, 36] as Excel.Range).Value2;
                        string assemblySidePress = "";
                        if (assemblySidePressValue != null)
                        {
                            assemblySidePress = assemblySidePressValue.ToString();
                        }
                        availableMachine.AssemblySidePress = assemblySidePress;

                        //float assemblyTopDown = 0;
                        var assemblyTopDownValue = (excelRange.Cells[i, 37] as Excel.Range).Value2;
                        string assemblyTopDown = "";
                        if (assemblyTopDownValue != null)
                        {
                            assemblyTopDown = assemblyTopDownValue.ToString();
                        }
                        availableMachine.AssemblyTopDown = assemblyTopDown;

                        //float assemblyHotmeltMachine = 0;
                        var assemblyHotmeltMachineValue = (excelRange.Cells[i, 38] as Excel.Range).Value2;
                        string assemblyHotmeltMachine = "";
                        if (assemblyHotmeltMachineValue != null)
                        {
                            assemblyHotmeltMachine = assemblyHotmeltMachineValue.ToString();
                        }
                        availableMachine.AssemblyHotmeltMachine = assemblyHotmeltMachine;

                        //float assemblySocklinerHotmelt = 0;
                        var assemblySocklinerHotmeltValue = (excelRange.Cells[i, 39] as Excel.Range).Value2;
                        string assemblySocklinerHotmelt = "";
                        if (assemblySocklinerHotmeltValue != null)
                        {
                            assemblySocklinerHotmelt = assemblySocklinerHotmeltValue.ToString();
                        }
                        availableMachine.AssemblySocklinerHotmelt = assemblySocklinerHotmelt;

                        //float assemblyVWrinkleRemover = 0;
                        var assemblyVWrinkleRemoverValue = (excelRange.Cells[i, 40] as Excel.Range).Value2;
                        string assemblyVWrinkleRemover = "";
                        if (assemblyVWrinkleRemoverValue != null)
                        {
                            assemblyVWrinkleRemover = assemblyVWrinkleRemoverValue.ToString();
                        }
                        availableMachine.AssemblyVWrinkleRemover = assemblyVWrinkleRemover;

                        // Add model to List
                        availableMachineList.Add(availableMachine);

                    }
                    progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = i));

                }
            }
            catch
            {
                availableMachineList.Clear();
            }
            finally
            {
                excelWorkbook.Close(false, Missing.Value, Missing.Value);
                excelApplication.Quit();
            }

        }
        //private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    Excel.Application excelApplication = new Excel.Application();
        //    Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(filePath);
        //    Excel.Worksheet excelWorksheet;
        //    Excel.Range excelRange;
        //    try
        //    {
        //        excelWorksheet = (Excel.Worksheet)excelWorkbook.Worksheets[1];
        //        excelRange = excelWorksheet.UsedRange;
        //        progressBar.Dispatcher.Invoke((Action)(() => progressBar.Maximum = excelRange.Rows.Count));
        //        for (int i = 3; i <= excelRange.Rows.Count; i++)
        //        {
        //            AvailableMachineModel availableMachine = new AvailableMachineModel();

        //            string id = (excelRange.Cells[i, 1] as Excel.Range).Value2;
        //            if (id != null)
        //            {
        //                string idValue = id.ToString();
        //                availableMachine.Id = idValue;

        //                // CUTTING
        //                string cuttingArmClicker = (excelRange.Cells[i, 3] as Excel.Range).Value2;
        //                availableMachine.CuttingArmClicker = CheckNull(cuttingArmClicker);

        //                string cuttingBeam = (excelRange.Cells[i, 4] as Excel.Range).Value2;
        //                availableMachine.CuttingBeam = CheckNull(cuttingBeam);

        //                string cuttingCutStrap = (excelRange.Cells[i, 5] as Excel.Range).Value2;
        //                availableMachine.CuttingCutStrap = CheckNull(cuttingCutStrap);

        //                string cuttingLaser = (excelRange.Cells[i, 6] as Excel.Range).Value2;
        //                availableMachine.CuttingLaser = CheckNull(cuttingLaser);

        //                string cuttingPuncherHole = (excelRange.Cells[i, 7] as Excel.Range).Value2;
        //                availableMachine.CuttingPuncherHole = CheckNull(cuttingPuncherHole);

        //                string cuttingSkiving = (excelRange.Cells[i, 8] as Excel.Range).Value2;
        //                availableMachine.CuttingSkiving = CheckNull(cuttingSkiving);

        //                // PREP
        //                string prepVerticalHF = (excelRange.Cells[i, 9] as Excel.Range).Value2;
        //                availableMachine.PrepVerticalHF = CheckNull(prepVerticalHF);

        //                string prepHorizontalHF = (excelRange.Cells[i, 10] as Excel.Range).Value2;
        //                availableMachine.PrepHorizontalHF = CheckNull(prepHorizontalHF);

        //                string prepOnlineHeatPress = (excelRange.Cells[i, 11] as Excel.Range).Value2;
        //                availableMachine.PrepOnlineHeatPress = CheckNull(prepOnlineHeatPress);

        //                string prepAutoHF = (excelRange.Cells[i, 12] as Excel.Range).Value2;
        //                availableMachine.PrepAutoHF = CheckNull(prepAutoHF);

        //                string prepInye = (excelRange.Cells[i, 13] as Excel.Range).Value2;
        //                availableMachine.PrepInye = CheckNull(prepInye);

        //                string prepHotmeltMachine = (excelRange.Cells[i, 14] as Excel.Range).Value2;
        //                availableMachine.PrepHotmeltMachine = CheckNull(prepHotmeltMachine);

        //                // SEWING
        //                string sewingSmallComputer = (excelRange.Cells[i, 15] as Excel.Range).Value2;
        //                availableMachine.SewingSmallComputer = CheckNull(sewingSmallComputer);

        //                string sewingBigComputer = (excelRange.Cells[i, 16] as Excel.Range).Value2;
        //                availableMachine.SewingBigComputer = CheckNull(sewingBigComputer);

        //                string sewingUltrasonic = (excelRange.Cells[i, 17] as Excel.Range).Value2;
        //                availableMachine.SewingUltrasonic = CheckNull(sewingUltrasonic);

        //                string sewing4NeedleFlat = (excelRange.Cells[i, 18] as Excel.Range).Value2;
        //                availableMachine.Sewing4NeedleFlat = CheckNull(sewing4NeedleFlat);

        //                string sewing4NeedlePost = (excelRange.Cells[i, 19] as Excel.Range).Value2;
        //                availableMachine.Sewing4NeedlePost = CheckNull(sewing4NeedlePost);

        //                string sewingLongTable = (excelRange.Cells[i, 20] as Excel.Range).Value2;
        //                availableMachine.SewingLongTable = CheckNull(sewingLongTable);

        //                string sewingEyeLeting = (excelRange.Cells[i, 21] as Excel.Range).Value2;
        //                availableMachine.SewingEyeleting = CheckNull(sewingEyeLeting);

        //                string sewingZZBinding = (excelRange.Cells[i, 22] as Excel.Range).Value2;
        //                availableMachine.SewingZZBinding = CheckNull(sewingZZBinding);

        //                string sewingHotmeltMachine = (excelRange.Cells[i, 23] as Excel.Range).Value2;
        //                availableMachine.SewingHotmeltMachine = CheckNull(sewingHotmeltMachine);

        //                string sewingHandHeldHotmelt = (excelRange.Cells[i, 24] as Excel.Range).Value2;
        //                availableMachine.SewingHandHeldHotmelt = CheckNull(sewingHandHeldHotmelt);

        //                string sewingStationaryHHHotmelt = (excelRange.Cells[i, 25] as Excel.Range).Value2;
        //                availableMachine.SewingStationaryHHHotmelt = CheckNull(sewingStationaryHHHotmelt);

        //                // STOCKFIT
        //                string stockfitVerticalBuffing = (excelRange.Cells[i, 26] as Excel.Range).Value2;
        //                availableMachine.StockfitVerticalBuffing = CheckNull(stockfitVerticalBuffing);

        //                string stockfitHorizontalBuffing = (excelRange.Cells[i, 27] as Excel.Range).Value2;
        //                availableMachine.StockfitHorizontalBuffing = CheckNull(stockfitHorizontalBuffing);

        //                string stockfitSideBuffing = (excelRange.Cells[i, 28] as Excel.Range).Value2;
        //                availableMachine.StockfitSideBuffing = CheckNull(stockfitSideBuffing);

        //                string stockfitOutsoleStitching = (excelRange.Cells[i, 29] as Excel.Range).Value2;
        //                availableMachine.StockfitOutsoleStitching = CheckNull(stockfitOutsoleStitching);

        //                string stockfitAutoBuffing = (excelRange.Cells[i, 30] as Excel.Range).Value2;
        //                availableMachine.StockfitAutoBuffing = CheckNull(stockfitAutoBuffing);

        //                string stockfitHydraulicCutting = (excelRange.Cells[i, 31] as Excel.Range).Value2;
        //                availableMachine.StockfitHydraulicCutting = CheckNull(stockfitHydraulicCutting);

        //                string stockfitPadPrinting = (excelRange.Cells[i, 32] as Excel.Range).Value2;
        //                availableMachine.StockfitPadPrinting = CheckNull(stockfitPadPrinting);

        //                // ASSEMBLY
        //                string assemblyToeLasting = (excelRange.Cells[i, 33] as Excel.Range).Value2;
        //                availableMachine.AssemblyToeLasting = CheckNull(assemblyToeLasting);

        //                string assemblySideLasting = (excelRange.Cells[i, 34] as Excel.Range).Value2;
        //                availableMachine.AssemblySideLasting = CheckNull(assemblySideLasting);

        //                string assemblyHeelLasting = (excelRange.Cells[i, 35] as Excel.Range).Value2;
        //                availableMachine.AssemblyHeelLasting = CheckNull(assemblyHeelLasting);

        //                string assemblySidePress = (excelRange.Cells[i, 36] as Excel.Range).Value2;
        //                availableMachine.AssemblySidePress = CheckNull(assemblySidePress);

        //                string assemblyTopDown = (excelRange.Cells[i, 37] as Excel.Range).Value2;
        //                availableMachine.AssemblyTopDown = CheckNull(assemblyTopDown);

        //                string assemblyHotmeltMachine = (excelRange.Cells[i, 38] as Excel.Range).Value2;
        //                availableMachine.AssemblyHotmeltMachine = CheckNull(assemblyHotmeltMachine);

        //                string assemblySocklinerHotmelt = (excelRange.Cells[i, 39] as Excel.Range).Value2;
        //                availableMachine.AssemblySocklinerHotmelt = CheckNull(assemblySocklinerHotmelt);

        //                string assemblyVWrinkleRemover = (excelRange.Cells[i, 40] as Excel.Range).Value2;
        //                availableMachine.AssemblyVWrinkleRemover = CheckNull(assemblyVWrinkleRemover);

        //                // add model to list
        //                availableMachineList.Add(availableMachine);
        //            }
                    
        //            // display progressbar
        //            progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = i));
        //        }

        //    }
        //    catch
        //    {
        //        availableMachineList.Clear();
        //    }
        //    finally
        //    {
        //        excelWorkbook.Close(false, Missing.Value, Missing.Value);
        //        excelApplication.Quit();
        //    }
        //}
        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            this.Cursor = null;
            lblStatus.Text = "Completed!";
            if (availableMachineList.Count() > 0)
            {
                dgAvailableMachine.ItemsSource = availableMachineList;
                btnImport.IsEnabled = true;
                MessageBox.Show(string.Format("Read Completed. {0} Row", availableMachineList.Count()), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Excel File Error. Try Again!", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (bwImport.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnImport.IsEnabled = false;
                //machineRequirementImportList = dgMachineRequirement.Items.OfType<MachineRequirementModel>().ToList();
                bwImport.RunWorkerAsync();
            }
        }
        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (AvailableMachineModel availableMachine in availableMachineList)
            {
                AvailableMachineController.Insert(availableMachine);
            }
        }
        private void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnImport.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Insert Completed!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        } 

        private void dgAvailableMachine_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0)
            {
                scrHeader.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }

        // function not work..
        private string CheckNull(dynamic input)
        {
            string output;
            if (input.Value2 == null)
            {
                output = "0";
            }
            else
            {
                output = input.ToString();
            }
            return output;
        }
        
    }
}
