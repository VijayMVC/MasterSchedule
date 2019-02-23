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
    /// Interaction logic for ImportMachineRequirementWindow.xaml
    /// </summary>
    public partial class ImportMachineRequirementWindow : Window
    {
        string filePath;
        List<MachineRequirementModel> machineRequirementList;
        List<MachineRequirementModel> machineRequirementImportList;
        BackgroundWorker bwLoad;
        BackgroundWorker bwImport;
        public ImportMachineRequirementWindow()
        {
            filePath = "";
            machineRequirementList = new List<MachineRequirementModel>();
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            bwImport = new BackgroundWorker();
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);
            machineRequirementImportList = new List<MachineRequirementModel>();
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import Machine Requirement";
            openFileDialog.Filter = "EXCEL Files (*.xls, *.xlsx)|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                if (bwLoad.IsBusy == false)
                {
                    this.Cursor = Cursors.Wait;
                    machineRequirementList.Clear();
                    lblStatus.Text = "Reading...";
                    bwLoad.RunWorkerAsync();
                }
            }
            else
            {
                this.Close();
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
                    #region
                    // read each excel column
                    MachineRequirementModel machineRequirement = new MachineRequirementModel();
                    var articleNoValue = (excelRange.Cells[i, 1] as Excel.Range).Value2;
                    if (articleNoValue != null)
                    {
                        string articleNo = articleNoValue.ToString();
                        machineRequirement.ArticleNo = articleNo;

                        var shoeNameValue = (excelRange.Cells[i, 2] as Excel.Range).Value2;
                        string shoeName = "";
                        if (shoeNameValue != null)
                        {
                            shoeName = shoeNameValue.ToString();
                        }
                        machineRequirement.ShoeName = shoeName;

                        var pMValue = (excelRange.Cells[i, 3] as Excel.Range).Value2;
                        string pM = "";
                        if (pMValue != null)
                        {
                            pM = pMValue.ToString();
                        }
                        machineRequirement.PM = pM;

                        // Cutting
                        //float cuttingQuantity = 0;
                        var cuttingQuantityValue = (excelRange.Cells[i, 4] as Excel.Range).Value2;
                        string cuttingQuantity = "";
                        if (cuttingQuantityValue != null)
                        {
                            cuttingQuantity = cuttingQuantityValue.ToString();
                        }
                        machineRequirement.CuttingQuantity = cuttingQuantity;

                        //float cuttingWorker = 0;
                        var cuttingWorkerValue = (excelRange.Cells[i, 5] as Excel.Range).Value2;
                        string cuttingWorker = "";
                        if (cuttingWorkerValue != null)
                        {
                            cuttingWorker = cuttingWorkerValue.ToString();
                        }
                        machineRequirement.CuttingWorker = cuttingWorker;

                        //float cuttingArmCliker = 0;
                        var cuttingArmClikerValue = (excelRange.Cells[i, 6] as Excel.Range).Value2;
                        string cuttingArmClicker = "";
                        if (cuttingArmClikerValue != null)
                        {
                            cuttingArmClicker = cuttingArmClikerValue.ToString();
                        }
                        machineRequirement.CuttingArmClicker = cuttingArmClicker;

                        //float cuttingBeam = 0;
                        var cuttingBeamValue = (excelRange.Cells[i, 7] as Excel.Range).Value2;
                        string cuttingBeam = "";
                        if (cuttingBeamValue != null)
                        {
                            cuttingBeam = cuttingBeamValue.ToString();
                        }
                        machineRequirement.CuttingBeam = cuttingBeam;

                        //float cuttingCutStrap = 0;
                        var cuttingCutStrapValue = (excelRange.Cells[i, 8] as Excel.Range).Value2;
                        string cuttingCutStrap = "";
                        if (cuttingCutStrapValue != null)
                        {
                            cuttingCutStrap = cuttingCutStrapValue.ToString();
                        }
                        machineRequirement.CuttingCutStrap = cuttingCutStrap;

                        //float cuttingLaser = 0;
                        var cuttingLaserValue = (excelRange.Cells[i, 9] as Excel.Range).Value2;
                        string cuttingLaser = "";
                        if (cuttingLaserValue != null)
                        {
                            cuttingLaser = cuttingLaserValue.ToString();
                        }
                        machineRequirement.CuttingLaser = cuttingLaser;


                        var cuttingPuncherHoleValue = (excelRange.Cells[i, 10] as Excel.Range).Value2;
                        string cuttingPuncherHole = "";
                        if(cuttingPuncherHoleValue != null)
                        {
                            cuttingPuncherHole = cuttingPuncherHoleValue.ToString();
                        }
                        machineRequirement.CuttingPuncherHole = cuttingPuncherHole;

                        //float cuttingSkiving = 0;
                        var cuttingSkivingValue = (excelRange.Cells[i, 11] as Excel.Range).Value2;
                        string cuttingSkiving = "";
                        if (cuttingSkivingValue != null)
                        {
                            cuttingSkiving = cuttingSkivingValue.ToString();
                        }
                        machineRequirement.CuttingSkiving = cuttingSkiving;


                        // Prep
                        //float prepWorker = 0;
                        var prepWorkerValue = (excelRange.Cells[i, 12] as Excel.Range).Value2;
                        string prepWorker = "";
                        if (prepWorkerValue != null)
                        {
                            prepWorker = prepWorkerValue.ToString();
                        }
                        machineRequirement.PrepWorker = prepWorker;

                        //float prepVerticalHF = 0;
                        var prepVerticalHFValue = (excelRange.Cells[i, 13] as Excel.Range).Value2;
                        string prepVerticalHF = "";
                        if (prepVerticalHFValue != null)
                        {
                            prepVerticalHF = prepVerticalHFValue.ToString();
                        }
                        machineRequirement.PrepVerticalHF = prepVerticalHF;

                        //float prepHorizontalHF = 0;
                        var prepHorizontalHFValue = (excelRange.Cells[i, 14] as Excel.Range).Value2;
                        string prepHorizontalHF = "";
                        if (prepHorizontalHFValue != null)
                        {
                            prepHorizontalHF = prepHorizontalHFValue.ToString();
                        }
                        machineRequirement.PrepHorizontalHF = prepHorizontalHF;

                        var prepOnlineHeatPressValue = (excelRange.Cells[i, 15] as Excel.Range).Value2;
                        string prepOnlineHeatPress = "";
                        if (prepOnlineHeatPressValue != null)
                        {
                            prepOnlineHeatPress = prepOnlineHeatPressValue.ToString();
                        }
                        machineRequirement.PrepOnlineHeatPress = prepOnlineHeatPress;

                        //float prepAutoHF = 0;
                        var prepAutoHFValue= (excelRange.Cells[i, 16] as Excel.Range).Value2;
                        string prepAutoHF = "";
                        if (prepAutoHFValue != null)
                        {
                            prepAutoHF = prepAutoHFValue.ToString();
                        }
                        machineRequirement.PrepAutoHF = prepAutoHF;

                        //float prepInye = 0;
                        var prepInyeValue = (excelRange.Cells[i, 17] as Excel.Range).Value2;
                        string prepInye = "";
                        if (prepInyeValue != null)
                        {
                            prepInye = prepInyeValue.ToString();
                        }
                        machineRequirement.PrepInye = prepInye;

                        //float prepHotmeltMachine = 0;
                        var prepHotmeltMachineValue = (excelRange.Cells[i, 18] as Excel.Range).Value2;
                        string prepHotmeltMachine = "";
                        if (prepHotmeltMachineValue != null)
                        {
                            prepHotmeltMachine = prepHotmeltMachineValue.ToString();
                        }
                        machineRequirement.PrepHotmeltMachine = prepHotmeltMachine;

                        // Sewing
                        var sewingQuantityValue = (excelRange.Cells[i, 19] as Excel.Range).Value2;
                        string sewingQuantity = "";
                        if (sewingQuantityValue != null)
                        {
                            sewingQuantity = sewingQuantityValue.ToString();
                        }
                        machineRequirement.SewingQuantity = sewingQuantity;

                        //float sewingWorker = 0;
                        var sewingWorkerValue = (excelRange.Cells[i, 20] as Excel.Range).Value2;
                        string sewingWorker = "";
                        if (sewingWorkerValue != null)
                        {
                            sewingWorker = sewingWorkerValue.ToString();
                        }
                        machineRequirement.SewingWorker = sewingWorker;

                        //float sewingSmallComputer = 0;
                        var sewingSmallComputerValue = (excelRange.Cells[i, 21] as Excel.Range).Value2;
                        string sewingSmallComputer = "";
                        if (sewingSmallComputerValue != null)
                        {
                            sewingSmallComputer = sewingSmallComputerValue.ToString();
                        }
                        machineRequirement.SewingSmallComputer = sewingSmallComputer;

                        //float sewingBigComputer = 0;
                        var sewingBigComputerValue = (excelRange.Cells[i, 22] as Excel.Range).Value2;
                        string sewingBigComputer = "";
                        if (sewingBigComputerValue != null)
                        {
                            sewingBigComputer = sewingBigComputerValue.ToString();
                        }
                        machineRequirement.SewingBigComputer = sewingBigComputer;

                        //float sewingUltrasonic = 0;
                        var sewingUltrasonicValue = (excelRange.Cells[i, 23] as Excel.Range).Value2;
                        string sewingUltrasonic = "";
                        if (sewingUltrasonicValue != null)
                        {
                            sewingUltrasonic = sewingUltrasonicValue.ToString();
                        }
                        machineRequirement.SewingUltrasonic = sewingUltrasonic;

                        //float sewingFourNeeldleFlat = 0;
                        var sewing4NeeldleFlatValue = (excelRange.Cells[i, 24] as Excel.Range).Value2;
                        string sewing4NeeldleFlat = "";
                        if (sewing4NeeldleFlatValue != null)
                        {
                            sewing4NeeldleFlat = sewing4NeeldleFlatValue.ToString();
                        }
                        machineRequirement.Sewing4NeedleFlat = sewing4NeeldleFlat;

                        //float sewingFourNeedlePost = 0;
                        var sewing4NeedlePostValue = (excelRange.Cells[i, 25] as Excel.Range).Value2;
                        string sewing4NeedlePost = "";
                        if (sewing4NeedlePostValue != null)
                        {
                            sewing4NeedlePost = sewing4NeedlePostValue.ToString();
                        }
                        machineRequirement.Sewing4NeedlePost = sewing4NeedlePost;

                        //float sewingLongTable = 0;
                        var sewingLongTableValue = (excelRange.Cells[i, 26] as Excel.Range).Value2;
                        string sewingLongTable = "";
                        if (sewingLongTableValue != null)
                        {
                            sewingLongTable = sewingLongTableValue.ToString();
                        }
                        machineRequirement.SewingLongTable = sewingLongTable;

                        //float sewingEyeleting = 0;
                        var sewingEyeletingValue = (excelRange.Cells[i, 27] as Excel.Range).Value2;
                        string sewingEyeleting = "";
                        if (sewingEyeletingValue != null)
                        {
                            sewingEyeleting = sewingEyeletingValue.ToString();
                        }
                        machineRequirement.SewingEyeleting = sewingEyeleting;

                        //float sewingZZBinding = 0;
                        var sewingZZBindingValue = (excelRange.Cells[i, 28] as Excel.Range).Value2;
                        string sewingZZBinding = "";
                        if (sewingZZBindingValue != null)
                        {
                            sewingZZBinding = sewingZZBindingValue.ToString();
                        }
                        machineRequirement.SewingZZBinding = sewingZZBinding;

                        //float sewingHotmeltMachine = 0;
                        var sewingHotmeltMachineValue = (excelRange.Cells[i, 29] as Excel.Range).Value2;
                        string sewingHotmeltMachine = "";
                        if (sewingHotmeltMachineValue != null)
                        {
                            sewingHotmeltMachine = sewingHotmeltMachineValue.ToString();
                        }
                        machineRequirement.SewingHotmeltMachine = sewingHotmeltMachine;

                        //float sewingHotmeltHeldMachine = 0;
                        var sewingHotmeltHeldMachineValue = (excelRange.Cells[i, 30] as Excel.Range).Value2;
                        string sewingHotmeltHeldMachine = "";
                        if (sewingHotmeltHeldMachineValue != null)
                        {
                            sewingHotmeltHeldMachine = sewingHotmeltHeldMachineValue.ToString();
                        }
                        machineRequirement.SewingHandHeldHotmelt = sewingHotmeltHeldMachine;

                        //float sewingStationaryHHHotmelt = 0;
                        var sewingStationaryHHHotmeltValue = (excelRange.Cells[i, 31] as Excel.Range).Value2;
                        string sewingStationaryHHHotmelt = "";
                        if (sewingStationaryHHHotmeltValue != null)
                        {
                            sewingStationaryHHHotmelt = sewingStationaryHHHotmeltValue.ToString();
                        }
                        machineRequirement.SewingStationaryHHHotmelt = sewingStationaryHHHotmelt;
                        
                        // Stockfit
                        //float stockfitQuantity = 0;
                        var stockfitQuantityValue = (excelRange.Cells[i, 32] as Excel.Range).Value2;
                        string stockfitQuantity = "";
                        if (stockfitQuantityValue != null)
                        {
                            stockfitQuantity = stockfitQuantityValue.ToString();
                        }
                        machineRequirement.StockfitQuantity = stockfitQuantity;

                        //float stockfitWorker = 0;
                        var stockfitWorkerValue = (excelRange.Cells[i, 33] as Excel.Range).Value2;
                        string stockfitWorker = "";
                        if (stockfitWorkerValue != null)
                        {
                            stockfitWorker = stockfitWorkerValue.ToString();
                        }
                        machineRequirement.StockfitWorker = stockfitWorker;

                        //float stockfitVerticalBuffing = 0;
                        var stockfitVerticalBuffingValue = (excelRange.Cells[i, 34] as Excel.Range).Value2;
                        string stockfitVerticalBuffing = "";
                        if (stockfitVerticalBuffingValue != null)
                        {
                            stockfitVerticalBuffing = stockfitVerticalBuffingValue.ToString();
                        }
                        machineRequirement.StockfitVerticalBuffing = stockfitVerticalBuffing;

                        //float stockfitHorizontalBuffing = 0;
                        var stockfitHorizontalBuffingValue = (excelRange.Cells[i, 35] as Excel.Range).Value2;
                        string stockfitHorizontalBuffing = "";
                        if (stockfitHorizontalBuffingValue != null)
                        {
                            stockfitHorizontalBuffing = stockfitHorizontalBuffingValue.ToString();
                        }
                        machineRequirement.StockfitHorizontalBuffing = stockfitHorizontalBuffing;

                        //float stockfitSideBuffing = 0;
                        var stockfitSideBuffingValue = (excelRange.Cells[i, 36] as Excel.Range).Value2;
                        string stockfitSideBuffing = "";
                        if (stockfitSideBuffingValue != null)
                        {
                            stockfitSideBuffing = stockfitSideBuffingValue.ToString();
                        }
                        machineRequirement.StockfitSideBuffing = stockfitSideBuffing;

                        //float stockfitOutsoleStitching = 0;
                        var stockfitOutsoleStitchingValue = (excelRange.Cells[i, 37] as Excel.Range).Value2;
                        string stockfitOutsoleStitching = "";
                        if (stockfitOutsoleStitchingValue != null)
                        {
                            stockfitOutsoleStitching = stockfitOutsoleStitchingValue.ToString();
                        }
                        machineRequirement.StockfitOutsoleStitching = stockfitOutsoleStitching;

                        //float stockfitAutoBuffing = 0;
                        var stockfitAutoBuffingValue = (excelRange.Cells[i, 38] as Excel.Range).Value2;
                        string stockfitAutoBuffing = "";
                        if (stockfitAutoBuffingValue != null)
                        {
                            stockfitAutoBuffing = stockfitAutoBuffingValue.ToString();
                        }
                        machineRequirement.StockfitAutoBuffing = stockfitAutoBuffing;

                        //float stockfitHydraulicCutting = 0;
                        var stockfitHydraulicCuttingValue = (excelRange.Cells[i, 39] as Excel.Range).Value2;
                        string stockfitHydraulicCutting = "";
                        if (stockfitHydraulicCuttingValue != null)
                        {
                            stockfitHydraulicCutting = stockfitHydraulicCuttingValue.ToString();
                        }
                        machineRequirement.StockfitHydraulicCutting = stockfitHydraulicCutting;

                        //float stockfitPadPrinting = 0;
                        var stockfitPadPrintingValue = (excelRange.Cells[i, 40] as Excel.Range).Value2;
                        string stockfitPadPrinting = "";
                        if (stockfitPadPrintingValue != null)
                        {
                            stockfitPadPrinting = stockfitPadPrintingValue.ToString();
                        }
                        machineRequirement.StockfitPadPrinting = stockfitPadPrinting;

                        // Assembly
                        //float assemblyQuantity = 0;
                        var assemblyQuantityValue = (excelRange.Cells[i, 41] as Excel.Range).Value2;
                        string assemblyQuantity = "";
                        if (assemblyQuantityValue != null)
                        {
                            assemblyQuantity = assemblyQuantityValue.ToString();
                        }
                        machineRequirement.AssemblyQuantity = assemblyQuantity;

                        //float assemblyWorker = 0;
                        var assemblyWorkerValue = (excelRange.Cells[i, 42] as Excel.Range).Value2;
                        string assemblyWorker = "";
                        if (assemblyWorkerValue != null)
                        {
                            assemblyWorker = assemblyWorkerValue.ToString();
                        }
                        machineRequirement.AssemblyWorker = assemblyWorker;

                        //float assemblyToeLasting = 0;

                        var assemblyToeLastingValue = (excelRange.Cells[i, 43] as Excel.Range).Value2;
                        string assemblyToeLasting = "";
                        if (assemblyToeLastingValue != null)
                        {
                            assemblyToeLasting = assemblyToeLastingValue.ToString();
                        }
                        machineRequirement.AssemblyToeLasting = assemblyToeLasting;

                        //float assemblySideLasting = 0;
                        var assemblySideLastingValue = (excelRange.Cells[i, 44] as Excel.Range).Value2;
                        string assemblySideLasting = "";
                        if (assemblySideLastingValue != null)
                        {
                            assemblySideLasting = assemblySideLastingValue.ToString();
                        }
                        machineRequirement.AssemblySideLasting = assemblySideLasting;

                        //float assemblyHeelLasting = 0;
                        var assemblyHeelLastingValue = (excelRange.Cells[i, 45] as Excel.Range).Value2;
                        string assemblyHeelLasting = "";
                        if (assemblyHeelLastingValue != null)
                        {
                            assemblyHeelLasting = assemblyHeelLastingValue.ToString();
                        }
                        machineRequirement.AssemblyHeelLasting = assemblyHeelLasting;

                        //float assemblySidePress = 0;
                        var assemblySidePressValue = (excelRange.Cells[i, 46] as Excel.Range).Value2;
                        string assemblySidePress = "";
                        if (assemblySidePressValue != null)
                        {
                            assemblySidePress = assemblySidePressValue.ToString();
                        }
                        machineRequirement.AssemblySidePress = assemblySidePress;

                        //float assemblyTopDown = 0;
                        var assemblyTopDownValue = (excelRange.Cells[i, 47] as Excel.Range).Value2;
                        string assemblyTopDown = "";
                        if (assemblyTopDownValue != null)
                        {
                            assemblyTopDown = assemblyTopDownValue.ToString();
                        }
                        machineRequirement.AssemblyTopDown = assemblyTopDown;

                        //float assemblyHotmeltMachine = 0;
                        var assemblyHotmeltMachineValue = (excelRange.Cells[i, 48] as Excel.Range).Value2;
                        string assemblyHotmeltMachine = "";
                        if (assemblyHotmeltMachineValue != null)
                        {
                            assemblyHotmeltMachine = assemblyHotmeltMachineValue.ToString();
                        }
                        machineRequirement.AssemblyHotmeltMachine = assemblyHotmeltMachine;

                        //float assemblySocklinerHotmelt = 0;
                        var assemblySocklinerHotmeltValue = (excelRange.Cells[i, 49] as Excel.Range).Value2;
                        string assemblySocklinerHotmelt = "";
                        if (assemblySocklinerHotmeltValue != null)
                        {
                            assemblySocklinerHotmelt = assemblySocklinerHotmeltValue.ToString();
                        }
                        machineRequirement.AssemblySocklinerHotmelt = assemblySocklinerHotmelt;

                        //float assemblyVWrinkleRemover = 0;

                        var assemblyVWrinkleRemoverValue = (excelRange.Cells[i, 50] as Excel.Range).Value2;
                        string assemblyVWrinkleRemover = "";
                        if (assemblyVWrinkleRemoverValue != null)
                        {
                            assemblyVWrinkleRemover = assemblyVWrinkleRemoverValue.ToString();
                        }
                        machineRequirement.AssemblyVWrinkleRemover = assemblyVWrinkleRemover;

                        // Add model to List
                        machineRequirementList.Add(machineRequirement);

                    }
                    progressBar.Dispatcher.Invoke((Action)(() => progressBar.Value = i));
                    #endregion
                }
            }
            catch
            {
                machineRequirementList.Clear();
            }
            finally
            {
                excelWorkbook.Close(false, Missing.Value, Missing.Value);
                excelApplication.Quit();
            }

        }


        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            this.Cursor = null;
            lblStatus.Text = "Completed!";
            if (machineRequirementList.Count() > 0)
            {
                dgMachineRequirement.ItemsSource = machineRequirementList;
                btnImport.IsEnabled = true;
                MessageBox.Show(string.Format("Read Completed. {0} AricleNo.!", machineRequirementList.Count()), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
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
            foreach (MachineRequirementModel machineRequirement in machineRequirementList)
            {
                MachineRequirementController.Insert(machineRequirement);
                //dgMachineRequirement.Dispatcher.Invoke((Action)(() =>
                //{
                //    dgMachineRequirement.SelectedItem = machineRequirement;
                //    dgMachineRequirement.ScrollIntoView(machineRequirement);
                //}));
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




        private void dgMachineRequirement_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0)
            {
                scrHeader.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }
    }
}
