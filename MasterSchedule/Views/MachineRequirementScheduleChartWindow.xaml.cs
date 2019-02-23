using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Data;
using System.ComponentModel;
using MasterSchedule.Helpers;
using MasterSchedule.Models;
using MasterSchedule.ViewModels;
using MasterSchedule.Controllers;
// primitives nguyenthuy

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for MachineRequirementScheduleChartWindow.xaml
    /// </summary>
    public partial class MachineRequirementScheduleChartWindow : Window
    {
        int dateBefore = -5;
        string cutting = "C", prep = "P", sewing = "S", stockfit = "SF", assembly = "A";
        //string sewingLine = "E", stockfitLine = "G", assemblyLine = "F";//cutPrepLine = "B", 
        string workerSewingIdStart = "E", workerStockfitIdStart = "G", workerAssemblyIdStart = "F"; //workerCutIdStart = "B", workerPrepIdStart = "C", 
        string cutPosition = "0", cellPosition = "1", printingPosition = "2", hsPosition = "3";
        string[] cuttingMode    = { "Worker", "Arm Clicker", "Beam", "Cut-Strap", "Laser", "Puncher Hole", "Skiving" };
        string[] prepMode       = { "Worker", "Verticle HF", "Horizontal HF", "Online Heat Press", "Auto HF", "Inye", "Hotmelt Machine" };
        string[] sewingMode     = { "Worker", "Small Computer", "Big Computer", "Ultrasonic", "4Needle Flat", "4Needle Post", "Long Table", "Eyeleting", "ZZ Binding", "Hotmelt Machine", "HandHeld Hotmelt", "Stationary HHHotmelt" };
        string[] stockfitMode   = { "Worker", "Vertical Buffing", "Horizontal Buffing", "Side Buffing", "Outsole Stitching", "Auto Buffing", "Hydraulic Cutting", "Pad Printing" };
        string[] assemblyMode   = { "Worker", "Toe Lasting", "Side Lasting", "Heel Lasting", "Side Press", "Top Down", "Hotmelt Machine", "Sockliner Hotmelt", "V Wrinkle Remover" };
        //string[] antuongList = { "antuong 1", "antuong 1.2", "antuong 1.23", "antuong 1.3", "antuong 1.3.4", "antuong 1.4.5", "antuong 2", "antuong 3", "antuong 3.4", "antuong 4", "antuong 4.5", "antuong 5", "antuong 5.6", "antuong 6", "o/s", "\b", "" };
        // array cellcolor
        SolidColorBrush[] cellColor = { Brushes.Green, Brushes.Tomato, Brushes.DarkGoldenrod, Brushes.MediumPurple, Brushes.Gray };

        Button btnCuttingMode, btnPrepMode, btnSewingMode, btnStockfitMode, btnAssemblyMode;
        DataTable dt;
        DataTable dtTotal;
        //thread
        BackgroundWorker threadLoad;
        BackgroundWorker threadLoadData;
        //Lists
        List<OrdersModel> orderList;
        List<SewingMasterModel> sewingMasterList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<OutsoleMasterModel> outsoleMasterList;
        List<SockliningMasterModel> sockliningMasterList;
        List<String> productNoList;
        List<RawMaterialModel> rawMaterialList;
        //List<OffDayModel> offDateList;
        List<MachineRequirementAssemblyViewModel> assemblyRequirementList;
        List<MachineRequirementModel> machineRequirementList;
        // ListTotal
        List<int> totalList;

        public MachineRequirementScheduleChartWindow()
        {
            InitializeComponent();
            threadLoad = new BackgroundWorker();
            threadLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadLoad_RunWorkerCompleted);
            threadLoad.DoWork += new DoWorkEventHandler(threadLoad_DoWork);

            orderList = new List<OrdersModel>();
            sewingMasterList = new List<SewingMasterModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            sockliningMasterList = new List<SockliningMasterModel>();
            rawMaterialList = new List<RawMaterialModel>();
            productNoList = new List<string>();

            totalList = new List<int>();

            threadLoadData = new BackgroundWorker();
            threadLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadLoadData_RunWorkerCompleted);
            threadLoadData.DoWork += new DoWorkEventHandler(threadLoadData_DoWork);

        }
        // thread 1. click btnCreateSchedule
        private void threadLoad_DoWork (object sender, DoWorkEventArgs e)
        {
            object[] args = e.Argument as object[];
            DateTime dateFrom = (args[0] as DateTime?).Value;
            DateTime dateTo = (args[1] as DateTime?).Value;
            string sectionId = args[2] as string;

            int availableMachineValue = (int)args[3];
            // default load data worker
            DisplayData(dateFrom, dateTo, sectionId, 0);
            //DisplayRowTotal(dateFrom, dateTo, availableMachineValue);
        }
        private void threadLoad_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnViewResult.IsEnabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            dgMachineRequirementChart.ItemsSource = dt.AsDataView();
            dgvTotal.ItemsSource = dtTotal.AsDataView();
        }
        private void Window_Loaded (object sender, RoutedEventArgs e)
        {
            dpDateFrom.SelectedDate = DateTime.Now;
            dpDateTo.SelectedDate = DateTime.Now;
        }
        private void btnViewResult_Click (object sender, RoutedEventArgs e)
        {
            dgvTotal.Columns.Clear();
            dgvTotal.Visibility = Visibility.Visible;
            dgMachineRequirementChart.Columns.Clear();
            //gridTotal.Children.Clear();
            //btnTotal.IsEnabled = true;
            lblMachineAvailable.Visibility = Visibility.Hidden;
            txtMachineAvailable.Visibility = Visibility.Hidden;
            DateTime dateFrom = dpDateFrom.SelectedDate.Value.Date;
            DateTime dateTo = dpDateTo.SelectedDate.Value.Date;
            ComboBoxItem sectionItem = cboSection.SelectedItem as ComboBoxItem;
            string sectionName = sectionItem.Content as string;
            string sectionId = sectionItem.Name;

            // addition code
            lblTitle.Text = string.Format("{0} MACHINE REQUIREMENT SCHEDULE CHART", sectionName.ToUpper());

            #region CreateButton
            // Create Button List
            if (sectionId == cutting)
            {
                gridLine.Children.Clear();
                for (int i = 0; i < cuttingMode.Count(); i++)
                {
                    btnCuttingMode = new Button();
                    btnCuttingMode.Tag = i;
                    Grid.SetColumn(btnCuttingMode, i);
                    btnCuttingMode.Content = String.Format("{0}", cuttingMode[i]);
                    btnCuttingMode.Click += new RoutedEventHandler(btnCuttingMode_Click);
                    gridLine.Children.Add(btnCuttingMode);
                }
            }
            if (sectionId == prep)
            {
                gridLine.Children.Clear();
                for (int i = 0; i < prepMode.Count(); i++)
                {
                    btnPrepMode = new Button();
                    btnPrepMode.Tag = i;
                    Grid.SetColumn(btnPrepMode, i);
                    btnPrepMode.Content = String.Format("{0}", prepMode[i]);
                    btnPrepMode.Click += new RoutedEventHandler(btnPrepMode_Click);
                    gridLine.Children.Add(btnPrepMode);
                }
            }

            if (sectionId == sewing)
            {
                gridLine.Children.Clear();
                for (int i = 0; i < sewingMode.Count(); i++)
                {
                    btnSewingMode = new Button();
                    btnSewingMode.Tag = i;
                    Grid.SetColumn(btnSewingMode, i);
                    btnSewingMode.Content = String.Format("{0}", sewingMode[i]);
                    btnSewingMode.Click += new RoutedEventHandler(btnSewingMode_Click);
                    gridLine.Children.Add(btnSewingMode);
                }
            }

            if (sectionId == stockfit)
            {
                gridLine.Children.Clear();
                for (int i = 0; i < stockfitMode.Count(); i++)
                {
                    btnStockfitMode = new Button();
                    btnStockfitMode.Tag = i;
                    Grid.SetColumn(btnStockfitMode, i);
                    btnStockfitMode.Content = String.Format("{0}", stockfitMode[i]);
                    btnStockfitMode.Click += new RoutedEventHandler(btnStockfitMode_Click);
                    gridLine.Children.Add(btnStockfitMode);
                }
            }

            if (sectionId == assembly)
            {
                gridLine.Children.Clear();
                for (int i = 0; i < assemblyMode.Count(); i++)
                {
                    btnAssemblyMode = new Button();
                    btnAssemblyMode.Tag = i;
                    Grid.SetColumn(btnAssemblyMode, i);
                    btnAssemblyMode.Content = String.Format("{0}", assemblyMode[i]);
                    btnAssemblyMode.Click += new RoutedEventHandler(btnAssemblyMode_Click);
                    gridLine.Children.Add(btnAssemblyMode);
                }
            }
            #endregion
            // Create Column
            CreateColumn(dateFrom, dateTo);
            CreateTotalRow(dateFrom, dateTo);

            if (threadLoad.IsBusy == true)
            {
                return;
            }

            // addition code availableMachineWorkerValue
            int availableMachineValue = 9999;

            this.Cursor = Cursors.Wait;
            btnViewResult.IsEnabled = false;
            object[] args = { dateFrom, dateTo, sectionId, availableMachineValue };
            threadLoad.RunWorkerAsync(args);
        }

        // thread 2. click btnModeSection
        private void threadLoadData_DoWork (object sender, DoWorkEventArgs e)
        {
            object[] args = e.Argument as object[];
            DateTime dateFrom = (args[0] as DateTime?).Value;
            DateTime dateTo = (args[1] as DateTime?).Value;
            string sectionId = args[2] as string;
            int sectionModeId = (int)args[3];
            int availableMachineValue = (int)args[4];

            DisplayData(dateFrom, dateTo, sectionId, sectionModeId);
            DisplayRowTotal(dateFrom, dateTo, availableMachineValue);

        }

        private void threadLoadData_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            dgMachineRequirementChart.ItemsSource = dt.AsDataView();
            dgvTotal.ItemsSource = dtTotal.AsDataView();
        }

        private void btnCuttingMode_Click (object sender, RoutedEventArgs e)
        {
            Button btnCuttingMode = (Button)sender as Button;
            int tag = (int)btnCuttingMode.Tag;
            dgvTotal.Columns.Clear();
            dgMachineRequirementChart.Columns.Clear();
            DateTime dateFrom = dpDateFrom.SelectedDate.Value.Date;
            DateTime dateTo = dpDateTo.SelectedDate.Value.Date;
            ComboBoxItem sectionItem = cboSection.SelectedItem as ComboBoxItem;
            string sectionName = sectionItem.Content as string;
            string sectionId = sectionItem.Name;
            int sectionModeId = tag;

            // Create column
            CreateColumn(dateFrom, dateTo);
            CreateTotalRow(dateFrom, dateTo);
            DisplayMachineAvailable(sectionId, sectionModeId, dateFrom);

            // addtion code availableMachineValue
            int availableMachineValue;
            Int32.TryParse(txtMachineAvailable.Text.ToString(), out availableMachineValue);

            if (threadLoadData.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            object[] args = { dateFrom, dateTo, sectionId, sectionModeId, availableMachineValue };
            threadLoadData.RunWorkerAsync(args);
        }
        private void btnPrepMode_Click (object sender, RoutedEventArgs e)
        {
            Button btnPrepMode = (Button)sender as Button;
            int tag = (int)btnPrepMode.Tag;
            dgvTotal.Columns.Clear();
            dgMachineRequirementChart.Columns.Clear();
            DateTime dateFrom = dpDateFrom.SelectedDate.Value.Date;
            DateTime dateTo = dpDateTo.SelectedDate.Value.Date;
            ComboBoxItem sectionItem = cboSection.SelectedItem as ComboBoxItem;
            string sectionName = sectionItem.Content as string;
            string sectionId = sectionItem.Name;
            int sectionModeId = tag;

            // Create column
            CreateColumn(dateFrom, dateTo);
            CreateTotalRow(dateFrom, dateTo);

            DisplayMachineAvailable(sectionId, sectionModeId, dateFrom);

            // addtion code availableMachineValue
            int availableMachineValue;
            Int32.TryParse(txtMachineAvailable.Text.ToString(), out availableMachineValue);

            if (threadLoadData.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            object[] args = { dateFrom, dateTo, sectionId, sectionModeId, availableMachineValue };
            threadLoadData.RunWorkerAsync(args);
        }
        private void btnSewingMode_Click (object sender, RoutedEventArgs e)
        {
            Button btnSewingMode = (Button)sender as Button;
            int tag = (int)btnSewingMode.Tag;
            dgvTotal.Columns.Clear();
            dgMachineRequirementChart.Columns.Clear();
            DateTime dateFrom = dpDateFrom.SelectedDate.Value.Date;
            DateTime dateTo = dpDateTo.SelectedDate.Value.Date;
            ComboBoxItem sectionItem = cboSection.SelectedItem as ComboBoxItem;
            string sectionName = sectionItem.Content as string;
            string sectionId = sectionItem.Name;
            int sectionModeId = tag;


            // create column
            CreateColumn(dateFrom, dateTo);
            CreateTotalRow(dateFrom, dateTo);

            DisplayMachineAvailable(sectionId, sectionModeId, dateFrom);

            // addtion code availableMachineValue
            int availableMachineValue;
            Int32.TryParse(txtMachineAvailable.Text.ToString(), out availableMachineValue);

            if (threadLoadData.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            object[] args = { dateFrom, dateTo, sectionId, sectionModeId, availableMachineValue };
            threadLoadData.RunWorkerAsync(args);
        }
        private void btnStockfitMode_Click (object sender, RoutedEventArgs e)
        {
            Button btnStockfitMode = (Button)sender as Button;
            int tag = (int)btnStockfitMode.Tag;
            dgvTotal.Columns.Clear();
            dgMachineRequirementChart.Columns.Clear();
            DateTime dateFrom = dpDateFrom.SelectedDate.Value.Date;
            DateTime dateTo = dpDateTo.SelectedDate.Value.Date;
            ComboBoxItem sectionItem = cboSection.SelectedItem as ComboBoxItem;
            int sectionModeId = tag;
            string sectionId = sectionItem.Name;

            // Create column date
            CreateColumn(dateFrom, dateTo);
            CreateTotalRow(dateFrom, dateTo);

            DisplayMachineAvailable(sectionId, sectionModeId, dateFrom);

            // addtion code availableMachineValue
            int availableMachineValue;
            Int32.TryParse(txtMachineAvailable.Text.ToString(), out availableMachineValue);

            if (threadLoadData.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            object[] args = { dateFrom, dateTo, sectionId, sectionModeId, availableMachineValue };
            threadLoadData.RunWorkerAsync(args);
        }
        private void btnAssemblyMode_Click (object sender, RoutedEventArgs e)
        {
            Button btnAssemblyMode = (Button)sender as Button;
            int tag = (int)btnAssemblyMode.Tag;
            dgvTotal.Columns.Clear();
            dgMachineRequirementChart.Columns.Clear();
            DateTime dateFrom = dpDateFrom.SelectedDate.Value.Date;
            DateTime dateTo = dpDateTo.SelectedDate.Value.Date;
            ComboBoxItem sectionItem = cboSection.SelectedItem as ComboBoxItem;
            int sectionModeId = tag;
            string sectionId = sectionItem.Name;

            // Create column date
            CreateColumn(dateFrom, dateTo);
            CreateTotalRow(dateFrom, dateTo);

            DisplayMachineAvailable(sectionId, sectionModeId, dateFrom);

            // addtion code availableMachineValue
            int availableMachineValue;
            Int32.TryParse(txtMachineAvailable.Text.ToString(), out availableMachineValue);

            if (threadLoadData.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            object[] args = { dateFrom, dateTo, sectionId, sectionModeId, availableMachineValue };
            threadLoadData.RunWorkerAsync(args);
        }

        // Create cloumn when you click create button, or click button mode in section
        private void CreateColumn (DateTime dateFrom, DateTime dateTo)
        {
            dt = new DataTable();
            dt.Columns.Add("Line", typeof(String));
            DataGridTextColumn dgcLine = new DataGridTextColumn();
            dgcLine.Header = "Line";
            dgcLine.CanUserSort = false;
            dgcLine.Width = 100;
            dgcLine.Binding = new Binding("Line");
            dgMachineRequirementChart.Columns.Add(dgcLine);

            for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
            {
                dt.Columns.Add(String.Format("Day_{0:d-M}", date), typeof(string));
                DataGridTextColumn dgcDate = new DataGridTextColumn();
                dgcDate.Header = string.Format(" {0:d/M} ", date);
                dgcDate.Binding = new Binding(String.Format("Day_{0:d-M}", date));
                //dgcDate.Width = 38;
                dgcDate.CanUserSort = false;

                Style style = new Style(typeof(DataGridCell));
                style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

                //Style headerStyle = new Style(typeof(DataGridColumnHeader));
                //headerStyle.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty,"Center"));

                Setter setterBackground = new Setter();
                setterBackground.Property = DataGridCell.BackgroundProperty;
                setterBackground.Value = new Binding(String.Format("Day_{0:d-M}_Background", date));
                style.Setters.Add(setterBackground);

                Setter setterTooltip = new Setter();
                setterTooltip.Property = ToolTipService.ToolTipProperty;
                setterTooltip.Value = new Binding(String.Format("Day_{0:d-M}_Tooltip", date));
                style.Setters.Add(setterTooltip);

                Setter setterForeground = new Setter();
                setterForeground.Property = DataGridCell.ForegroundProperty;
                setterForeground.Value = new Binding(String.Format("Day_{0:d-M}_Forceground", date));
                style.Setters.Add(setterForeground);

                dgcDate.CellStyle = style;
                //dgcDate.HeaderStyle = headerStyle;

                dgMachineRequirementChart.Columns.Add(dgcDate);

                DataColumn columnBackground = new DataColumn(String.Format("Day_{0:d-M}_Background", date), typeof(SolidColorBrush));
                DataColumn columnForceground = new DataColumn(String.Format("Day_{0:d-M}_Forceground", date), typeof(SolidColorBrush));
                DataColumn columnToolTip = new DataColumn(String.Format("Day_{0:d-M}_Tooltip", date), typeof(string));
                columnBackground.DefaultValue = Brushes.White;
                columnForceground.DefaultValue = Brushes.Black;

                dt.Columns.Add(columnBackground);
                dt.Columns.Add(columnForceground);
                dt.Columns.Add(columnToolTip);
            }

        }
        // Create rowtotal
        private void CreateTotalRow (DateTime dateFrom, DateTime dateTo)
        {
            dtTotal = new DataTable();
            dtTotal.Columns.Add("Line", typeof(String));

            DataGridTextColumn totalLine = new DataGridTextColumn();
            totalLine.Width = 100;
            totalLine.Binding = new Binding("Line");
            dgvTotal.Columns.Add(totalLine);

            for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
            {
                dtTotal.Columns.Add(String.Format("Day_{0:d-M}", date), typeof(Int32));
                DataGridTextColumn totalDate = new DataGridTextColumn();

                totalDate.Header = string.Format(" {0:d/M} ", date);
                totalDate.Binding = new Binding(String.Format("Day_{0:d-M}", date));
                //totalDate.Width = 38;

                Style style = new Style(typeof(DataGridCell));
                style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

                Setter setterForeground = new Setter();
                setterForeground.Property = DataGridCell.ForegroundProperty;
                setterForeground.Value = new Binding(String.Format("Day_{0:d-M}_Forceground", date));
                style.Setters.Add(setterForeground);

                totalDate.CellStyle = style;
                dgvTotal.Columns.Add(totalDate);

                DataColumn columnForeground = new DataColumn(String.Format("Day_{0:d-M}_Forceground", date), typeof(SolidColorBrush));
                columnForeground.DefaultValue = Brushes.Black;

                
                dtTotal.Columns.Add(columnForeground);
            }
        }
        // after that. data going to display gridview
        private void DisplayData (DateTime dateFrom, DateTime dateTo, string sectionId, int sectionModeId)
        {
            if (orderList.Count <= 0)
            {
                orderList = OrdersController.SelectSubString();
            }

            if (sectionId == cutting || sectionId == prep && sockliningMasterList.Count <= 0)
            {
                sockliningMasterList = SockliningMasterController.Select();
                sewingMasterList = SewingMasterController.Select();
            }
            if (sectionId == sewing && sewingMasterList.Count <= 0)
            {
                sewingMasterList = SewingMasterController.Select();
            }
            if (sectionId == stockfit && outsoleMasterList.Count <= 0)
            {
                outsoleMasterList = OutsoleMasterController.Select();
            }
            if (sectionId == assembly && assemblyMasterList.Count <= 0)
            {
                assemblyMasterList = AssemblyMasterController.Select();
            }

            if (rawMaterialList.Count <= 0)
            {
                rawMaterialList = RawMaterialController.Select();
            }

            assemblyRequirementList = MachineRequirementController.SelectAssembly();
            machineRequirementList = MachineRequirementController.Select();
            List<String> articleNoList = machineRequirementList.Where(o => String.IsNullOrEmpty(o.ArticleNo) == false).Select(p => p.ArticleNo).ToList();

            string[] lineArray = null;
            string[] sortArray = null;
            if (sectionId == cutting || sectionId == prep)
            {
                //lineArray = sewingMasterList.Select(s => s.SewingLine).Distinct().OrderBy(s => s).ToArray();
                ////lineArray = sockliningMasterList.Select(s => s.SockliningLine).Distinct().OrderBy(s => s).ToArray();  
                sortArray = sewingMasterList.Select(s => s.SewingLine).Distinct().ToArray();
                SortArray(sortArray);
                lineArray = sortArray;
            }
            if (sectionId == sewing )
            {
                sortArray = sewingMasterList.Select(s => s.SewingLine).Distinct().ToArray();
                SortArray(sortArray);
                lineArray = sortArray;
            }
            if (sectionId == stockfit)
            {
                lineArray = outsoleMasterList.Select(s => s.OutsoleLine).Distinct().OrderBy(s => s).ToArray();
            }
            if (sectionId == assembly)
            {
                sortArray = assemblyMasterList.Select(s => s.AssemblyLine).Distinct().ToArray();
                SortArray(sortArray);
                lineArray = sortArray;
            }

            foreach (string line in lineArray)
            {
                DataRow dr = dt.NewRow();
                dr["Line"] = line;
                #region when you click any button
                
                if (sectionId == cutting)
                {
                    List<SewingMasterModel> cuttingTempList = sewingMasterList.Where(p => p.SewingLine == line && ((dateFrom <= p.SewingStartDate && p.SewingStartDate <= dateTo) || (dateFrom <= p.SewingFinishDate && p.SewingFinishDate <= dateTo))).ToList();
                    foreach (SewingMasterModel cuttingTemp in cuttingTempList)
                    {
                        float inputDb = (float)cuttingTemp.SewingQuota;
                        DateTime startDate = cuttingTemp.SewingStartDate.AddDays(dateBefore);
                        DateTime finishDate = cuttingTemp.SewingFinishDate.AddDays(dateBefore);

                        OrdersModel order = orderList.Where(o => o.ProductNo == cuttingTemp.ProductNo).FirstOrDefault();
                        if (order != null)
                        {
                            string articleNo = order.ArticleNo.Substring(0, 6);
                            MachineRequirementModel cuttingValue = machineRequirementList.Where(o => o.ArticleNo.Contains(articleNo)).FirstOrDefault();
                            float inputEx, ratio = 0;
                            float.TryParse(cuttingValue.CuttingQuantity, out inputEx);
                            if (sectionModeId == 0)
                            {
                                float.TryParse(cuttingValue.CuttingWorker, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 1)
                            {
                                float.TryParse(cuttingValue.CuttingArmClicker, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 2)
                            {
                                float.TryParse(cuttingValue.CuttingBeam, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 3)
                            {
                                float.TryParse(cuttingValue.CuttingCutStrap, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 4)
                            {
                                float.TryParse(cuttingValue.CuttingLaser, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 5)
                            {
                                float.TryParse(cuttingValue.CuttingPuncherHole, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 6)
                            {
                                float.TryParse(cuttingValue.CuttingSkiving, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                        }
                    }
                }

                if (sectionId == prep)
                {
                    List<SewingMasterModel> prepTempList = sewingMasterList.Where(p => p.SewingLine == line && ((dateFrom <= p.SewingStartDate && p.SewingStartDate <= dateTo) || (dateFrom <= p.SewingFinishDate && p.SewingFinishDate <= dateTo))).ToList();
                    //List<SockliningMasterModel> sockliningMasterTempList = sockliningMasterList.Where(p => p.SockliningLine == line && ((dateFrom <= p.SockliningStartDate && p.SockliningStartDate <= dateTo) || (dateFrom <= p.SockliningFinishDate && p.SockliningFinishDate <= dateTo))).ToList();

                    foreach (SewingMasterModel prepTemp in prepTempList)
                    {
                        float inputDb = (float)prepTemp.SewingQuota;
                        DateTime startDate = prepTemp.SewingStartDate.AddDays(dateBefore);
                        DateTime finishDate = prepTemp.SewingFinishDate.AddDays(dateBefore);

                        OrdersModel order = orderList.Where(o => o.ProductNo == prepTemp.ProductNo).FirstOrDefault();
                        if (order != null)
                        {
                            string articleNo = order.ArticleNo.Substring(0, 6);
                            MachineRequirementModel prepValue = machineRequirementList.Where(o => o.ArticleNo.Contains(articleNo)).FirstOrDefault();
                            float inputEx, ratio = 0;
                            float.TryParse(prepValue.CuttingQuantity, out inputEx);
                            if (sectionModeId == 0)
                            {
                                float.TryParse(prepValue.PrepWorker, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 1)
                            {
                                float.TryParse(prepValue.PrepVerticalHF, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 2)
                            {
                                float.TryParse(prepValue.PrepHorizontalHF, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 3)
                            {
                                float.TryParse(prepValue.PrepOnlineHeatPress, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 4)
                            {
                                float.TryParse(prepValue.PrepAutoHF, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 5)
                            {
                                float.TryParse(prepValue.PrepInye, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 6)
                            {
                                float.TryParse(prepValue.PrepHotmeltMachine, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                        }
                    }
                }

                if (sectionId == sewing)
                {
                    List<SewingMasterModel> sewingMasterTempList = sewingMasterList.Where(p => p.SewingLine == line && ((dateFrom <= p.SewingStartDate && p.SewingStartDate <= dateTo) || (dateFrom <= p.SewingFinishDate && p.SewingFinishDate <= dateTo))).ToList();
                    foreach (SewingMasterModel sewingTemp in sewingMasterTempList)
                    {
                        float inputDb = (float)sewingTemp.SewingQuota;
                        DateTime startDate = sewingTemp.SewingStartDate;
                        DateTime finishDate = sewingTemp.SewingFinishDate;
                        OrdersModel order = orderList.Where(o => o.ProductNo == sewingTemp.ProductNo).FirstOrDefault();
                        if (order != null)
                        {
                            string articleNo = order.ArticleNo.Substring(0, 6);
                            MachineRequirementModel sewingValue = machineRequirementList.Where(o => o.ArticleNo.Contains(articleNo)).FirstOrDefault();
                            float inputEx, ratio = 0;
                            float.TryParse(sewingValue.SewingQuantity, out inputEx);
                            if (sectionModeId == 0)
                            {
                                float.TryParse(sewingValue.SewingWorker, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 1)
                            {
                                float.TryParse(sewingValue.SewingSmallComputer, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 2)
                            {
                                float.TryParse(sewingValue.SewingBigComputer, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 3)
                            {
                                float.TryParse(sewingValue.SewingUltrasonic, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 4)
                            {
                                float.TryParse(sewingValue.Sewing4NeedleFlat, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 5)
                            {
                                float.TryParse(sewingValue.Sewing4NeedlePost, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 6)
                            {
                                float.TryParse(sewingValue.SewingLongTable, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 7)
                            {
                                float.TryParse(sewingValue.SewingEyeleting, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 8)
                            {
                                float.TryParse(sewingValue.SewingZZBinding, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 9)
                            {
                                float.TryParse(sewingValue.SewingHotmeltMachine, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 10)
                            {
                                float.TryParse(sewingValue.SewingHandHeldHotmelt, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 11)
                            {
                                float.TryParse(sewingValue.SewingStationaryHHHotmelt, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                        }
                    }
                }

                if (sectionId == stockfit)
                {
                    List<OutsoleMasterModel> outsoleMasterTempList = outsoleMasterList.Where(o => o.OutsoleLine == line && ((dateFrom <= o.OutsoleStartDate && o.OutsoleStartDate <= dateTo) || (dateFrom <= o.OutsoleFinishDate && o.OutsoleFinishDate <= dateTo))).ToList();
                    foreach (OutsoleMasterModel outsoleTemp in outsoleMasterTempList)
                    {
                        float inputDb = (float)outsoleTemp.OutsoleQuota;
                        DateTime startDate = outsoleTemp.OutsoleStartDate;
                        DateTime finishDate = outsoleTemp.OutsoleFinishDate;
                        OrdersModel order = orderList.Where(p => p.ProductNo == outsoleTemp.ProductNo).FirstOrDefault();
                        if (order != null)
                        {
                            string articleNo = order.ArticleNo.Substring(0, 6);
                            MachineRequirementModel outsoleValue = machineRequirementList.Where(o => o.ArticleNo.Contains(articleNo)).FirstOrDefault();
                            float inputEx, ratio = 0;
                            float.TryParse(outsoleValue.StockfitQuantity, out inputEx);
                            if (sectionModeId == 0)
                            {
                                float.TryParse(outsoleValue.StockfitWorker, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 1)
                            {
                                float.TryParse(outsoleValue.StockfitVerticalBuffing, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 2)
                            {
                                float.TryParse(outsoleValue.StockfitHorizontalBuffing, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 3)
                            {
                                float.TryParse(outsoleValue.StockfitSideBuffing, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 4)
                            {
                                float.TryParse(outsoleValue.StockfitOutsoleStitching, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 5)
                            {
                                float.TryParse(outsoleValue.StockfitAutoBuffing, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 6)
                            {
                                float.TryParse(outsoleValue.StockfitHydraulicCutting, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 7)
                            {
                                float.TryParse(outsoleValue.StockfitPadPrinting, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                        }
                    }
                }

                if (sectionId == assembly)
                {
                    List<AssemblyMasterModel> assemblyMasterTempList = assemblyMasterList.Where(s => s.AssemblyLine == line && ((dateFrom <= s.AssemblyStartDate && s.AssemblyStartDate <= dateTo) || (dateFrom <= s.AssemblyFinishDate && s.AssemblyFinishDate <= dateTo))).ToList();
                    foreach (AssemblyMasterModel assemblyTemp in assemblyMasterTempList)
                    {

                        float inputDb = (float)assemblyTemp.AssemblyQuota;
                        DateTime startDate = assemblyTemp.AssemblyStartDate;
                        DateTime finishDate = assemblyTemp.AssemblyFinishDate;
                        OrdersModel order = orderList.Where(o => o.ProductNo == assemblyTemp.ProductNo).FirstOrDefault();
                        List<String> productNoList = orderList.Where(o => o.ProductNo == assemblyTemp.ProductNo).Select(p => p.ProductNo).ToList();
                        if (order != null)
                        {
                            string articleNo = order.ArticleNo.Substring(0, 6);
                            
                            MachineRequirementModel assemblyQuantityValue = machineRequirementList.Where(o => o.ArticleNo.Contains(articleNo)).FirstOrDefault();
                            float inputEx, ratio = 0;
                            float.TryParse(assemblyQuantityValue.AssemblyQuantity, out inputEx);
                            if (sectionModeId == 0)
                            {
                                float.TryParse(assemblyQuantityValue.AssemblyWorker, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 1)
                            {
                                float.TryParse(assemblyQuantityValue.AssemblyToeLasting, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 2)
                            {
                                float.TryParse(assemblyQuantityValue.AssemblySideLasting, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 3)
                            {
                                float.TryParse(assemblyQuantityValue.AssemblyHeelLasting, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 4)
                            {
                                float.TryParse(assemblyQuantityValue.AssemblySidePress, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 5)
                            {
                                float.TryParse(assemblyQuantityValue.AssemblyTopDown, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 6)
                            {
                                float.TryParse(assemblyQuantityValue.AssemblyHotmeltMachine, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 7)
                            {
                                float.TryParse(assemblyQuantityValue.AssemblySocklinerHotmelt, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                            if (sectionModeId == 8)
                            {
                                float.TryParse(assemblyQuantityValue.AssemblyVWrinkleRemover, out ratio);
                                CalculateAndChangeColor(articleNoList, articleNo, order, dateFrom, dateTo, startDate, finishDate, ref dr, inputEx, inputDb, ratio);
                            }
                        }
                    }
                }
                #endregion
                dt.Rows.Add(dr);
            }
            CompareInLine(dateFrom, dateTo);
            
        }

        // Sort array
        private static void SortArray(string[] inputArray)
        {
            string[] outputArray;
            for (int i = 0; i < inputArray.Count(); i++)
            {
                // Sewinggggg
                if (inputArray[i] == "sewing 1")
                {
                    inputArray[i] = "sewing 01";
                }
                if (inputArray[i] == "sewing 1A")
                {
                    inputArray[i] = "sewing 01A";
                }
                if (inputArray[i] == "sewing 1B")
                {
                    inputArray[i] = "sewing 01B";
                }

                if (inputArray[i] == "sewing 10")
                {
                    inputArray[i] = "sewing100";
                }
                if (inputArray[i] == "sewing 10A")
                {
                    inputArray[i] = "sewing100A";
                }
                if (inputArray[i] == "sewing 10B")
                {
                    inputArray[i] = "sewing100B";
                }

                if (inputArray[i] == "sewing 11")
                {
                    inputArray[i] = "sewing110";
                }
                if (inputArray[i] == "sewing 11A")
                {
                    inputArray[i] = "sewing110A";
                }
                if (inputArray[i] == "sewing 11B")
                {
                    inputArray[i] = "sewing110B";
                }

                if (inputArray[i] == "sewing 12")
                {
                    inputArray[i] = "sewing120";
                }
                if (inputArray[i] == "sewing 12A")
                {
                    inputArray[i] = "sewing120A";
                }
                if (inputArray[i] == "sewing 12B")
                {
                    inputArray[i] = "sewing120B";
                }

                // Assemblyyyyyy
                if (inputArray[i] == "assy 10")
                {
                    inputArray[i] = "assy10";
                }
                if (inputArray[i] == "assy 11")
                {
                    inputArray[i] = "assy11";
                }
                if (inputArray[i] == "assy 12A")
                {
                    inputArray[i] = "assy12A";
                }
                if (inputArray[i] == "assy 12B")
                {
                    inputArray[i] = "assy12B";
                }
            }
            Array.Sort(inputArray);
            outputArray = inputArray;
            for (int i = 0; i < outputArray.Count(); i++)
            {
                // Sewingggg
                if (outputArray[i] == "sewing 01")
                {
                    outputArray[i] = "sewing 1";
                }
                if (outputArray[i] == "sewing 01A")
                {
                    outputArray[i] = "sewing 1A";
                }
                if (outputArray[i] == "sewing 01B")
                {
                    outputArray[i] = "sewing 1B";
                }

                if (outputArray[i] == "sewing100")
                {
                    outputArray[i] = "sewing 10";
                }
                if (outputArray[i] == "sewing100A")
                {
                    outputArray[i] = "sewing 10A";
                }
                if (outputArray[i] == "sewing100B")
                {
                    outputArray[i] = "sewing 10B";
                }

                if (outputArray[i] == "sewing110")
                {
                    outputArray[i] = "sewing 11";
                }
                if (outputArray[i] == "sewing110A")
                {
                    outputArray[i] = "sewing 11A";
                }
                if (outputArray[i] == "sewing110B")
                {
                    outputArray[i] = "sewing 11B";
                }

                if (outputArray[i] == "sewing120")
                {
                    outputArray[i] = "sewing 12";
                }
                if (outputArray[i] == "sewing120A")
                {
                    outputArray[i] = "sewing 12A";
                }
                if (outputArray[i] == "sewing120B")
                {
                    outputArray[i] = "sewing 12B";
                }

                // Assemblyyyy
                // Assemblyyyyyy
                if (outputArray[i] == "assy10")
                {
                    outputArray[i] = "assy 10";
                }
                if (outputArray[i] == "assy11")
                {
                    outputArray[i] = "assy 11";
                }
                if (outputArray[i] == "assy12A")
                {
                    outputArray[i] = "assy 12A";
                }
                if (outputArray[i] == "assy12B")
                {
                    outputArray[i] = "assy 12B";
                }
            }
        }

        // Calculate follow line
        private void CompareInLine(DateTime dateFrom, DateTime dateTo)
        {
            for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
            {
                #region sewing1
                int sewing1Position = -1, sewing1APosition = -1, sewing1BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 1")
                    {
                        sewing1Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 1A")
                    {
                        sewing1APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 1B")
                    {
                        sewing1BPosition = i;
                    }
                }
                int sewing1 = 0, sewing1A = 0, sewing1B = 0, sewing1AB = 0;
                if (sewing1Position != -1 && sewing1APosition != -1 && sewing1BPosition != -1)
                {
                    dynamic valueSewing1 = dt.Rows[sewing1Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing1))
                    {
                        sewing1 = Convert.ToInt32(valueSewing1);
                    }
                    dynamic valueSewing1A = dt.Rows[sewing1APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing1A))
                    {
                        sewing1A = Convert.ToInt32(valueSewing1A);
                    }
                    dynamic valueSewing1B = dt.Rows[sewing1BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing1B))
                    {
                        sewing1B = Convert.ToInt32(valueSewing1B);
                    }
                    sewing1AB = sewing1A + sewing1B;
                }
                if (sewing1 > sewing1AB)
                {
                    dt.Rows[sewing1Position][String.Format("Day_{0:d-M}", date)] = sewing1;
                    dt.Rows[sewing1APosition][String.Format("Day_{0:d-M}", date)] = 0;
                    dt.Rows[sewing1BPosition][String.Format("Day_{0:d-M}", date)] = 0;
                }
                if (sewing1 < sewing1AB)
                {
                    dt.Rows[sewing1Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing1APosition][String.Format("Day_{0:d-M}", date)] = sewing1A.ToString();
                    if (sewing1A == 0)
                    {
                        dt.Rows[sewing1APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing1BPosition][String.Format("Day_{0:d-M}", date)] = sewing1B.ToString();
                    if (sewing1B == 0)
                    {
                        dt.Rows[sewing1BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion

                #region sewing2
                int sewing2Position = -1, sewing2APosition = -1, sewing2BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 2")
                    {
                        sewing2Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 2A")
                    {
                        sewing2APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 2B")
                    {
                        sewing2BPosition = i;
                    }
                }
                int sewing2 = 0, sewing2A = 0, sewing2B = 0, sewing2AB = 0;
                if (sewing2Position != -1 && sewing2APosition != -1 && sewing2BPosition != -1)
                {
                    dynamic valueSewing2 = dt.Rows[sewing2Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing2))
                    {
                        sewing2 = Convert.ToInt32(valueSewing2);
                    }
                    dynamic valueSewing2A = dt.Rows[sewing2APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing2A))
                    {
                        sewing2A = Convert.ToInt32(valueSewing2A);
                    }
                    dynamic valueSewing2B = dt.Rows[sewing2BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing2B))
                    {
                        sewing2B = Convert.ToInt32(valueSewing2B);
                    }
                    sewing2AB = sewing2A + sewing2B;
                }
                if (sewing2 > sewing2AB)
                {
                    dt.Rows[sewing2Position][String.Format("Day_{0:d-M}", date)] = sewing2.ToString();
                    dt.Rows[sewing2APosition][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing2BPosition][String.Format("Day_{0:d-M}", date)] = "";
                }
                if (sewing2 < sewing2AB)
                {
                    dt.Rows[sewing2Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing2APosition][String.Format("Day_{0:d-M}", date)] = sewing2A.ToString();
                    if (sewing2A == 0)
                    {
                        dt.Rows[sewing2APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing2BPosition][String.Format("Day_{0:d-M}", date)] = sewing2B.ToString();
                    if (sewing2B == 0)
                    {
                        dt.Rows[sewing2BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion

                #region sewing3
                int sewing3Position = -1, sewing3APosition = -1, sewing3BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 3")
                    {
                        sewing3Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 3A")
                    {
                        sewing3APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 3B")
                    {
                        sewing3BPosition = i;
                    }
                }
                int sewing3 = 0, sewing3A = 0, sewing3B = 0, sewing3AB = 0;
                if (sewing3Position != -1 && sewing3APosition != -1 && sewing3BPosition != -1)
                {
                    dynamic valueSewing3 = dt.Rows[sewing3Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing3))
                    {
                        sewing3 = Convert.ToInt32(valueSewing3);
                    }
                    dynamic valueSewing3A = dt.Rows[sewing3APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing3A))
                    {
                        sewing3A = Convert.ToInt32(valueSewing3A);
                    }
                    dynamic valueSewing3B = dt.Rows[sewing3BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing3B))
                    {
                        sewing3B = Convert.ToInt32(valueSewing3B);
                    }
                    sewing3AB = sewing3A + sewing3B;
                }
                if (sewing3 > sewing3AB)
                {
                    dt.Rows[sewing3Position][String.Format("Day_{0:d-M}", date)] = sewing3.ToString();
                    dt.Rows[sewing3APosition][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing3BPosition][String.Format("Day_{0:d-M}", date)] = "";
                }
                if (sewing3 < sewing3AB)
                {
                    dt.Rows[sewing3Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing3APosition][String.Format("Day_{0:d-M}", date)] = sewing3A.ToString();
                    if (sewing3A == 0)
                    {
                        dt.Rows[sewing3APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing3BPosition][String.Format("Day_{0:d-M}", date)] = sewing3B.ToString();
                    if (sewing3B == 0)
                    {
                        dt.Rows[sewing3BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion

                #region sewing4
                int sewing4Position = -1, sewing4APosition = -1, sewing4BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 4")
                    {
                        sewing4Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 4A")
                    {
                        sewing4APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 4B")
                    {
                        sewing4BPosition = i;
                    }
                }
                int sewing4 = 0, sewing4A = 0, sewing4B = 0, sewing4AB = 0;
                if (sewing4Position != -1 && sewing4APosition != -1 && sewing4BPosition != -1)
                {
                    dynamic valueSewing4 = dt.Rows[sewing4Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing4))
                    {
                        sewing4 = Convert.ToInt32(valueSewing4);
                    }
                    dynamic valueSewing4A = dt.Rows[sewing4APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing4A))
                    {
                        sewing4A = Convert.ToInt32(valueSewing4A);
                    }
                    dynamic valueSewing4B = dt.Rows[sewing4BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing4B))
                    {
                        sewing4B = Convert.ToInt32(valueSewing4B);
                    }
                    sewing4AB = sewing4A + sewing4B;
                }
                if (sewing4 > sewing4AB)
                {
                    dt.Rows[sewing4Position][String.Format("Day_{0:d-M}", date)] = sewing4.ToString();
                    dt.Rows[sewing4APosition][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing4BPosition][String.Format("Day_{0:d-M}", date)] = "";
                }
                if (sewing4 < sewing4AB)
                {
                    dt.Rows[sewing4Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing4APosition][String.Format("Day_{0:d-M}", date)] = sewing4A.ToString();
                    if (sewing4A == 0)
                    {
                        dt.Rows[sewing4APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing4BPosition][String.Format("Day_{0:d-M}", date)] = sewing4B.ToString();
                    if (sewing4B == 0)
                    {
                        dt.Rows[sewing4BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion

                #region sewing5
                int sewing5Position = -1, sewing5APosition = -1, sewing5BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 5")
                    {
                        sewing5Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 5A")
                    {
                        sewing5APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 5B")
                    {
                        sewing5BPosition = i;
                    }
                }
                int sewing5 = 0, sewing5A = 0, sewing5B = 0, sewing5AB = 0;
                if (sewing5Position != -1 && sewing5APosition != -1 && sewing5BPosition != -1)
                {
                    dynamic valueSewing5 = dt.Rows[sewing5Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing5))
                    {
                        sewing5 = Convert.ToInt32(valueSewing5);
                    }
                    dynamic valueSewing5A = dt.Rows[sewing5APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing5A))
                    {
                        sewing5A = Convert.ToInt32(valueSewing5A);
                    }
                    dynamic valueSewing5B = dt.Rows[sewing5BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valueSewing5B))
                    {
                        sewing5B = Convert.ToInt32(valueSewing5B);
                    }
                    sewing5AB = sewing5A + sewing5B;
                }
                if (sewing5 > sewing5AB)
                {
                    dt.Rows[sewing5Position][String.Format("Day_{0:d-M}", date)] = sewing5.ToString();
                    dt.Rows[sewing5APosition][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing5BPosition][String.Format("Day_{0:d-M}", date)] = "";
                }
                if (sewing5 < sewing5AB)
                {
                    dt.Rows[sewing5Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing5APosition][String.Format("Day_{0:d-M}", date)] = sewing5A.ToString();
                    if (sewing5A == 0)
                    {
                        dt.Rows[sewing5APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing5BPosition][String.Format("Day_{0:d-M}", date)] = sewing5B.ToString();
                    if (sewing5B == 0)
                    {
                        dt.Rows[sewing5BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion

                #region sewing6
                int sewing6Position = -1, sewing6APosition = -1, sewing6BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 6")
                    {
                        sewing6Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 6A")
                    {
                        sewing6APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 6B")
                    {
                        sewing6BPosition = i;
                    }
                }
                int sewing6 = 0, sewing6A = 0, sewing6B = 0, sewing6AB = 0;
                if (sewing6Position != -1 && sewing6APosition != -1 && sewing6BPosition != -1)
                {
                    dynamic valuesewing6 = dt.Rows[sewing6Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing6))
                    {
                        sewing6 = Convert.ToInt32(valuesewing6);
                    }
                    dynamic valuesewing6A = dt.Rows[sewing6APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing6A))
                    {
                        sewing6A = Convert.ToInt32(valuesewing6A);
                    }
                    dynamic valuesewing6B = dt.Rows[sewing6BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing6B))
                    {
                        sewing6B = Convert.ToInt32(valuesewing6B);
                    }
                    sewing6AB = sewing6A + sewing6B;
                }
                if (sewing6 > sewing6AB)
                {
                    dt.Rows[sewing6Position][String.Format("Day_{0:d-M}", date)] = sewing6.ToString();
                    dt.Rows[sewing6APosition][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing6BPosition][String.Format("Day_{0:d-M}", date)] = "";
                }
                if (sewing6 < sewing6AB)
                {
                    dt.Rows[sewing6Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing6APosition][String.Format("Day_{0:d-M}", date)] = sewing6A.ToString();
                    if (sewing6A == 0)
                    {
                        dt.Rows[sewing6APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing6BPosition][String.Format("Day_{0:d-M}", date)] = sewing6B.ToString();
                    if (sewing6B == 0)
                    {
                        dt.Rows[sewing6BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion

                #region sewing7
                int sewing7Position = -1, sewing7APosition = -1, sewing7BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 7")
                    {
                        sewing7Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 7A")
                    {
                        sewing7APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 7B")
                    {
                        sewing7BPosition = i;
                    }
                }
                int sewing7 = 0, sewing7A = 0, sewing7B = 0, sewing7AB = 0;
                if (sewing7Position != -1 && sewing7APosition != -1 && sewing7BPosition != -1)
                {
                    dynamic valuesewing7 = dt.Rows[sewing7Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing7))
                    {
                        sewing7 = Convert.ToInt32(valuesewing7);
                    }
                    dynamic valuesewing7A = dt.Rows[sewing7APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing7A))
                    {
                        sewing7A = Convert.ToInt32(valuesewing7A);
                    }
                    dynamic valuesewing7B = dt.Rows[sewing7BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing7B))
                    {
                        sewing7B = Convert.ToInt32(valuesewing7B);
                    }
                    sewing7AB = sewing7A + sewing7B;
                }
                if (sewing7 > sewing7AB)
                {
                    dt.Rows[sewing7Position][String.Format("Day_{0:d-M}", date)] = sewing7.ToString();
                    dt.Rows[sewing7APosition][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing7BPosition][String.Format("Day_{0:d-M}", date)] = "";
                }
                if (sewing7 < sewing7AB)
                {
                    dt.Rows[sewing7Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing7APosition][String.Format("Day_{0:d-M}", date)] = sewing7A.ToString();
                    if (sewing7A == 0)
                    {
                        dt.Rows[sewing7APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing7BPosition][String.Format("Day_{0:d-M}", date)] = sewing7B.ToString();
                    if (sewing7B == 0)
                    {
                        dt.Rows[sewing7BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion

                #region sewing8
                int sewing8Position = -1, sewing8APosition = -1, sewing8BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 8")
                    {
                        sewing8Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 8A")
                    {
                        sewing8APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 8B")
                    {
                        sewing8BPosition = i;
                    }
                }
                int sewing8 = 0, sewing8A = 0, sewing8B = 0, sewing8AB = 0;
                if (sewing8Position != -1 && sewing8APosition != -1 && sewing8BPosition != -1)
                {
                    dynamic valuesewing8 = dt.Rows[sewing8Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing8))
                    {
                        sewing8 = Convert.ToInt32(valuesewing8);
                    }
                    dynamic valuesewing8A = dt.Rows[sewing8APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing8A))
                    {
                        sewing8A = Convert.ToInt32(valuesewing8A);
                    }
                    dynamic valuesewing8B = dt.Rows[sewing8BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing8B))
                    {
                        sewing8B = Convert.ToInt32(valuesewing8B);
                    }
                    sewing8AB = sewing8A + sewing8B;
                }
                if (sewing8 > sewing8AB)
                {
                    dt.Rows[sewing8Position][String.Format("Day_{0:d-M}", date)] = sewing8.ToString();
                    dt.Rows[sewing8APosition][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing8BPosition][String.Format("Day_{0:d-M}", date)] = "";
                }
                if (sewing8 < sewing8AB)
                {
                    dt.Rows[sewing8Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing8APosition][String.Format("Day_{0:d-M}", date)] = sewing8A.ToString();
                    if (sewing8A == 0)
                    {
                        dt.Rows[sewing8APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing8BPosition][String.Format("Day_{0:d-M}", date)] = sewing8B.ToString();
                    if (sewing8B == 0)
                    {
                        dt.Rows[sewing8BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion

                #region sewing9
                int sewing9Position = -1, sewing9APosition = -1, sewing9BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 9")
                    {
                        sewing9Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 9A")
                    {
                        sewing9APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 9B")
                    {
                        sewing9BPosition = i;
                    }
                }
                int sewing9 = 0, sewing9A = 0, sewing9B = 0, sewing9AB = 0;
                if (sewing9Position != -1 && sewing9APosition != -1 && sewing9BPosition != -1)
                {
                    dynamic valuesewing9 = dt.Rows[sewing9Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing9))
                    {
                        sewing9 = Convert.ToInt32(valuesewing9);
                    }
                    dynamic valuesewing9A = dt.Rows[sewing9APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing9A))
                    {
                        sewing9A = Convert.ToInt32(valuesewing9A);
                    }
                    dynamic valuesewing9B = dt.Rows[sewing9BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing9B))
                    {
                        sewing9B = Convert.ToInt32(valuesewing9B);
                    }
                    sewing9AB = sewing9A + sewing9B;
                }
                if (sewing9 > sewing9AB)
                {
                    dt.Rows[sewing9Position][String.Format("Day_{0:d-M}", date)] = sewing9.ToString();
                    dt.Rows[sewing9APosition][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing9BPosition][String.Format("Day_{0:d-M}", date)] = "";
                }
                if (sewing9 < sewing9AB)
                {
                    dt.Rows[sewing9Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing9APosition][String.Format("Day_{0:d-M}", date)] = sewing9A.ToString();
                    if (sewing9A == 0)
                    {
                        dt.Rows[sewing9APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing9BPosition][String.Format("Day_{0:d-M}", date)] = sewing9B.ToString();
                    if (sewing9B == 0)
                    {
                        dt.Rows[sewing9BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion

                #region sewing10
                int sewing10Position = -1, sewing10APosition = -1, sewing10BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 10")
                    {
                        sewing10Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 10A")
                    {
                        sewing10APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 10B")
                    {
                        sewing10BPosition = i;
                    }
                }
                int sewing10 = 0, sewing10A = 0, sewing10B = 0, sewing10AB = 0;
                if (sewing10Position != -1 && sewing10APosition != -1 && sewing10BPosition != -1)
                {
                    dynamic valuesewing10 = dt.Rows[sewing10Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing10))
                    {
                        sewing10 = Convert.ToInt32(valuesewing10);
                    }
                    dynamic valuesewing10A = dt.Rows[sewing10APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing10A))
                    {
                        sewing10A = Convert.ToInt32(valuesewing10A);
                    }
                    dynamic valuesewing10B = dt.Rows[sewing10BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing10B))
                    {
                        sewing10B = Convert.ToInt32(valuesewing10B);
                    }
                    sewing10AB = sewing10A + sewing10B;
                }
                if (sewing10 > sewing10AB)
                {
                    dt.Rows[sewing10Position][String.Format("Day_{0:d-M}", date)] = sewing10.ToString();
                    dt.Rows[sewing10APosition][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing10BPosition][String.Format("Day_{0:d-M}", date)] = "";
                }
                if (sewing10 < sewing10AB)
                {
                    dt.Rows[sewing10Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing10APosition][String.Format("Day_{0:d-M}", date)] = sewing10A.ToString();
                    if (sewing10A == 0)
                    {
                        dt.Rows[sewing10APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing10BPosition][String.Format("Day_{0:d-M}", date)] = sewing10B.ToString();
                    if (sewing10B == 0)
                    {
                        dt.Rows[sewing10BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion

                #region sewing11
                int sewing11Position = -1, sewing11APosition = -1, sewing11BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 11")
                    {
                        sewing11Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 11A")
                    {
                        sewing11APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 11B")
                    {
                        sewing11BPosition = i;
                    }
                }
                int sewing11 = 0, sewing11A = 0, sewing11B = 0, sewing11AB = 0;
                if (sewing11Position != -1 && sewing11APosition != -1 && sewing11BPosition != -1)
                {
                    dynamic valuesewing11 = dt.Rows[sewing11Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing11))
                    {
                        sewing11 = Convert.ToInt32(valuesewing11);
                    }
                    dynamic valuesewing11A = dt.Rows[sewing11APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing11A))
                    {
                        sewing11A = Convert.ToInt32(valuesewing11A);
                    }
                    dynamic valuesewing11B = dt.Rows[sewing11BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing11B))
                    {
                        sewing11B = Convert.ToInt32(valuesewing11B);
                    }
                    sewing11AB = sewing11A + sewing11B;
                }
                if (sewing11 > sewing11AB)
                {
                    dt.Rows[sewing11Position][String.Format("Day_{0:d-M}", date)] = sewing11.ToString();
                    dt.Rows[sewing11APosition][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing11BPosition][String.Format("Day_{0:d-M}", date)] = "";
                }
                if (sewing11 < sewing11AB)
                {
                    dt.Rows[sewing11Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing11APosition][String.Format("Day_{0:d-M}", date)] = sewing11A.ToString();
                    if (sewing11A == 0)
                    {
                        dt.Rows[sewing11APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing11BPosition][String.Format("Day_{0:d-M}", date)] = sewing11B.ToString();
                    if (sewing11B == 0)
                    {
                        dt.Rows[sewing11BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion

                #region sewing12
                int sewing12Position = -1, sewing12APosition = -1, sewing12BPosition = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["line"].ToString() == "sewing 12")
                    {
                        sewing12Position = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 12A")
                    {
                        sewing12APosition = i;
                    }
                    if (dt.Rows[i]["line"].ToString() == "sewing 12B")
                    {
                        sewing12BPosition = i;
                    }
                }
                int sewing12 = 0, sewing12A = 0, sewing12B = 0, sewing12AB = 0;
                if (sewing12Position != -1 && sewing12APosition != -1 && sewing12BPosition != -1)
                {
                    dynamic valuesewing12 = dt.Rows[sewing12Position][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing12))
                    {
                        sewing12 = Convert.ToInt32(valuesewing12);
                    }
                    dynamic valuesewing12A = dt.Rows[sewing12APosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing12A))
                    {
                        sewing12A = Convert.ToInt32(valuesewing12A);
                    }
                    dynamic valuesewing12B = dt.Rows[sewing12BPosition][String.Format("Day_{0:d-M}", date)].ToString();
                    if (!string.IsNullOrEmpty(valuesewing12B))
                    {
                        sewing12B = Convert.ToInt32(valuesewing12B);
                    }
                    sewing12AB = sewing12A + sewing12B;
                }
                if (sewing12 > sewing12AB)
                {
                    dt.Rows[sewing12Position][String.Format("Day_{0:d-M}", date)] = sewing12.ToString();
                    dt.Rows[sewing12APosition][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing12BPosition][String.Format("Day_{0:d-M}", date)] = "";
                }
                if (sewing12 < sewing12AB)
                {
                    dt.Rows[sewing12Position][String.Format("Day_{0:d-M}", date)] = "";
                    dt.Rows[sewing12APosition][String.Format("Day_{0:d-M}", date)] = sewing12A.ToString();
                    if (sewing12A == 0)
                    {
                        dt.Rows[sewing12APosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                    dt.Rows[sewing12BPosition][String.Format("Day_{0:d-M}", date)] = sewing12B.ToString();
                    if (sewing12B == 0)
                    {
                        dt.Rows[sewing12BPosition][String.Format("Day_{0:d-M}", date)] = "";
                    }
                }
                #endregion
            }

        }

        // Display rowtotal
        private void DisplayRowTotal (DateTime dateFrom, DateTime dateTo, int availableMachineValue)
        {
            // antuonglist
            List<string> antuongList = new List<string>();
            List<SewingMasterModel> antuongMasterList = SewingMasterController.SelectAnTuongList();
            antuongList = antuongMasterList.Select(s => s.SewingLine).Distinct().ToList();
            DataRow drTotal = dtTotal.NewRow();
            drTotal["Line"] = "Total";
            // calculate sum row
            for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
            {
                int sum = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    // fix Total row
                    string line = dr["Line"].ToString();
                    if (!antuongList.Contains(line))
                    {
                        dynamic value = dr[String.Format("Day_{0:d-M}", date)].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            sum += Convert.ToInt32(value);
                        }
                    }
                }
                drTotal[String.Format("Day_{0:d-M}", date)] = sum;
                // addition code: forceground color red
                if (sum > availableMachineValue)
                {
                    if (drTotal[String.Format("Day_{0:d-M}_Forceground", date)] == Brushes.Black)
                    {
                        drTotal[String.Format("Day_{0:d-M}_Forceground", date)] = Brushes.Red;
                    }
                    else
                    {
                        drTotal[String.Format("Day_{0:d-M}_Forceground", date)] = Brushes.Red;
                    }
                }
            }
            dtTotal.Rows.Add(drTotal);
        }

        // Display machine available
        private void DisplayMachineAvailable (string sectionId, int sectionModeId, DateTime dateSelect)
        {
            List<AvailableWorker> workerTotalInDate = new List<AvailableWorker>();
            int _year, _month, _day;
            int cutWorker, prepWorker, sewingWorker, stockfitWorker, assemblyWorker;
            _year = dateSelect.Year;
            _month = dateSelect.Month;
            _day = dateSelect.Day;

            workerTotalInDate = AvailableMachineController.SelectWorker(_year, _month, _day);

            AvailableMachineModel availableMachine = new AvailableMachineModel();
            availableMachine = AvailableMachineController.Select();
            string title = " Available: ";

            if (sectionId == cutting)
            {
                if (sectionModeId == 0)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    // Count cutting worker by workerId
                    cutWorker = workerTotalInDate.Where(w => w.DetailPositionId.Contains(cutPosition)).Select(s => s.WorkerId).Count();
                    lblMachineAvailable.Content = "Worker Available: ";
                    txtMachineAvailable.Text = cutWorker.ToString();
                    //
                }

                if (sectionModeId == 1)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = cuttingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.CuttingArmClicker;
                }
                if (sectionModeId == 2)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = cuttingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.CuttingBeam;
                }
                if (sectionModeId == 3)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = cuttingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.CuttingCutStrap;
                }
                if (sectionModeId == 4)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = cuttingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.CuttingLaser;
                }
                if (sectionModeId == 5)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = cuttingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.CuttingPuncherHole;
                }
                if (sectionModeId == 6)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = cuttingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.CuttingSkiving;
                }
            }
            if (sectionId == prep)
            {
                if (sectionModeId == 0)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    // Count prep worker by workerId
                    prepWorker = workerTotalInDate.Where(w => w.DetailPositionId.Contains(cellPosition) || w.DetailPositionId.Contains(printingPosition) || w.DetailPositionId.Contains(hsPosition)).Select(s => s.WorkerId).Count();
                    lblMachineAvailable.Content = "Worker Available: ";
                    txtMachineAvailable.Text = prepWorker.ToString();
                    //
                }

                if (sectionModeId == 1)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = prepMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.PrepVerticalHF;
                }
                if (sectionModeId == 2)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = prepMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.PrepHorizontalHF;
                }
                if (sectionModeId == 3)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = prepMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.PrepOnlineHeatPress;
                }
                if (sectionModeId == 4)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = prepMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.PrepAutoHF;
                }
                if (sectionModeId == 5)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = prepMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.PrepInye;
                }
                if (sectionModeId == 6)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = prepMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.PrepHotmeltMachine;
                }
            }
            if (sectionId == sewing)
            {
                if (sectionModeId == 0)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    // Count sewing Worker
                    sewingWorker = workerTotalInDate.Where(w => w.WorkerId.Contains(workerSewingIdStart)).Select(s => s.WorkerId).Count();
                    lblMachineAvailable.Content = "Worker Available: ";
                    txtMachineAvailable.Text = sewingWorker.ToString();
                    //
                }

                if (sectionModeId == 1)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = sewingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.SewingSmallComputer;
                }
                if (sectionModeId == 2)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = sewingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.SewingBigComputer;
                }
                if (sectionModeId == 3)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = sewingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.SewingUltrasonic;
                }
                if (sectionModeId == 4)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = sewingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.Sewing4NeedleFlat;
                }
                if (sectionModeId == 5)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = sewingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.Sewing4NeedlePost;
                }
                if (sectionModeId == 6)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = sewingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.SewingLongTable;
                }
                if (sectionModeId == 7)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = sewingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.SewingEyeleting;
                }
                if (sectionModeId == 8)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = sewingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.SewingZZBinding;
                }
                if (sectionModeId == 9)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = sewingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.SewingHotmeltMachine;
                }
                if (sectionModeId == 10)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = sewingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.SewingHandHeldHotmelt;
                }
                if (sectionModeId == 11)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = sewingMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.SewingStationaryHHHotmelt;
                }
            }

            if (sectionId == stockfit)
            {

                if (sectionModeId == 0)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;

                    // Count stockfitWorker Worker
                    stockfitWorker = workerTotalInDate.Where(w => w.WorkerId.Contains(workerStockfitIdStart)).Select(s => s.WorkerId).Count();
                    lblMachineAvailable.Content = "Worker Available: ";
                    txtMachineAvailable.Text = stockfitWorker.ToString();
                    //
                }

                if (sectionModeId == 1)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = stockfitMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.StockfitVerticalBuffing;
                }
                if (sectionModeId == 2)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = stockfitMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.StockfitHorizontalBuffing;
                }
                if (sectionModeId == 3)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = stockfitMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.StockfitSideBuffing;
                }
                if (sectionModeId == 4)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = stockfitMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.StockfitOutsoleStitching;
                }
                if (sectionModeId == 5)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = stockfitMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.StockfitAutoBuffing;
                }
                if (sectionModeId == 6)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = stockfitMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.StockfitHydraulicCutting;
                }
                if (sectionModeId == 7)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = stockfitMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.StockfitPadPrinting;
                }
            }

            if (sectionId == assembly)
            {
                if (sectionModeId == 0)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    // Count assembly Worker by workerId
                    assemblyWorker = workerTotalInDate.Where(w => w.WorkerId.Contains(workerAssemblyIdStart)).Select(s => s.WorkerId).Count();
                    lblMachineAvailable.Content = "Worker Available: ";
                    txtMachineAvailable.Text = assemblyWorker.ToString();
                    //
                }

                if (sectionModeId == 1)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = assemblyMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.AssemblyToeLasting;
                }
                if (sectionModeId == 2)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = assemblyMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.AssemblySideLasting;
                }
                if (sectionModeId == 3)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = assemblyMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.AssemblyHeelLasting;
                }
                if (sectionModeId == 4)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = assemblyMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.AssemblySidePress;
                }
                if (sectionModeId == 5)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = assemblyMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.AssemblyTopDown;
                }
                if (sectionModeId == 6)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = assemblyMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.AssemblyHotmeltMachine;
                }
                if (sectionModeId == 7)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = assemblyMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.AssemblySocklinerHotmelt;
                }
                if (sectionModeId == 8)
                {
                    lblMachineAvailable.Visibility = Visibility.Visible;
                    txtMachineAvailable.Visibility = Visibility.Visible;
                    lblMachineAvailable.Content = assemblyMode[sectionModeId].ToString() + title;
                    txtMachineAvailable.Text = availableMachine.AssemblyVWrinkleRemover;
                }
            }
        }

        // Calculate and fill color cell
        private void CalculateAndChangeColor(List<String> articleNoList, string article, OrdersModel order, DateTime dateFrom, DateTime dateTo, DateTime startDate, DateTime finishDate, ref DataRow dataRow, float inputEx, float inputDb, float ratio)
        {
            OrdersModel shoeName = OrdersController.SelectByArticleNo6(article);
            DateTime dateDo = startDate;
            int colorValue = 0;
            if (order != null)
            {
                while (dateDo <= finishDate)
                {
                    if (dateFrom <= dateDo && dateDo <= dateTo)
                    {
                        float result = CalculateHelper.DivideFloat(inputDb, inputEx) * ratio;
                        int resultPrint = (int)Math.Round(result);
                        if (resultPrint == 0)
                        {
                            dataRow[String.Format("Day_{0:d-M}", dateDo)] = "";
                        }
                        dataRow[String.Format("Day_{0:d-M}", dateDo)] = String.Format("{0}", resultPrint.ToString());
                        dataRow[String.Format("Day_{0:d-M}_Tooltip", dateDo)] = shoeName.ShoeName;
                        for (int i = 0; i < articleNoList.Count; i++)
                        {
                            if (articleNoList[i].Contains(article))
                            {
                                colorValue = i % 5;
                                dataRow[String.Format("Day_{0:d-M}_Background", dateDo)] = cellColor[colorValue];
                            }
                        }
                    }
                    dateDo = dateDo.AddDays(1);
                }
            }
        }
        
        private void dgMachineRequirementChart_ScrollChanged (object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0)
            {
                scrHeader.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }

    }
}