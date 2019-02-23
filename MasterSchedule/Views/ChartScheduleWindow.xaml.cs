using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Data;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using MasterSchedule.ViewModels;
using MasterSchedule.Helpers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ChartScheduleWindow.xaml
    /// </summary>
    public partial class ChartScheduleWindow : Window
    {
        BackgroundWorker threadLoad;
        List<OrdersModel> orderList;

        List<SewingMasterModel> sewingMasterList;
        List<AssemblyMasterModel> assemblyMasterList;
        List<OutsoleMasterModel> outsoleMasterList;

        List<RawMaterialModel> rawMaterialList;
        List<OffDayModel> offDateList;
        DataTable dt;
        DateTime dateDefault;
        string sewing = "S", assembly = "A", outsole = "O";
        public ChartScheduleWindow()
        {
            InitializeComponent();

            threadLoad = new BackgroundWorker();
            threadLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadLoad_RunWorkerCompleted);
            threadLoad.DoWork += new DoWorkEventHandler(threadLoad_DoWork);

            orderList = new List<OrdersModel>();
            sewingMasterList = new List<SewingMasterModel>();
            assemblyMasterList = new List<AssemblyMasterModel>();
            outsoleMasterList = new List<OutsoleMasterModel>();
            rawMaterialList = new List<RawMaterialModel>();


            dateDefault = new DateTime(2000, 1, 1);
        }

        private void threadLoad_DoWork(object sender, DoWorkEventArgs e)
        {

            //Get offdate
            offDateList = OffDayController.SelectDate();

            int[] materialIdUpperArray = { 1, 2, 3, 4, 10 };
            int[] materialIdAssemblyArray = { 8 };
            int[] materialIdOutsoleArray = { 6 };

            object[] args = e.Argument as object[];
            DateTime dateFrom = (args[0] as DateTime?).Value;
            DateTime dateTo = (args[1] as DateTime?).Value;
            string sectionId = args[2] as string;
            if (orderList.Count <= 0)
            {
                orderList = OrdersController.Select();
            }

            if (sectionId == sewing && sewingMasterList.Count <= 0)
            {
                sewingMasterList = SewingMasterController.Select();
            }
            if (sectionId == assembly && assemblyMasterList.Count <= 0)
            {
                assemblyMasterList = AssemblyMasterController.Select();
            }
            if (sectionId == outsole && outsoleMasterList.Count <= 0)
            {
                outsoleMasterList = OutsoleMasterController.Select();
            }

            if (rawMaterialList.Count <= 0)
            {
                rawMaterialList = RawMaterialController.Select();
            }

            string[] lineArray = null;
            string[] tempArray = null;
            if (sectionId == sewing)
            {
                //lineArray = sewingMasterList.Select(s => s.SewingLine).Distinct().OrderBy(s => s).ToArray();
                tempArray = sewingMasterList.Select(s => s.SewingLine).Distinct().ToArray();
                SortArray(tempArray);
                lineArray = tempArray;
            }
            if (sectionId == assembly)
            {
                //lineArray = assemblyMasterList.Select(s => s.AssemblyLine).Distinct().OrderBy(s => s).ToArray();
                tempArray = assemblyMasterList.Select(s => s.AssemblyLine).Distinct().ToArray();
                SortArray(tempArray);
                lineArray = tempArray;
            }
            if (sectionId == outsole)
            {
                lineArray = outsoleMasterList.Select(s => s.OutsoleLine).Distinct().OrderBy(s => s).ToArray();
            }

            foreach (string line in lineArray)
            {
                DataRow dr = dt.NewRow();
                dr["Line"] = line;
                if (sectionId == sewing)
                {
                    List<SewingMasterModel> sewingMasterTempList = sewingMasterList.Where(s => s.SewingLine == line && ((dateFrom <= s.SewingStartDate && s.SewingStartDate <= dateTo) || (dateFrom <= s.SewingFinishDate && s.SewingFinishDate <= dateTo))).ToList();
                    foreach (SewingMasterModel sewingMaster in sewingMasterTempList)
                    {
                        OrdersModel order = orderList.Where(o => o.ProductNo == sewingMaster.ProductNo).FirstOrDefault();
                        CompareDate(order, dateFrom, dateTo, sewingMaster.SewingStartDate, sewingMaster.SewingFinishDate, ref dr, materialIdUpperArray);
                    }
                }

                if (sectionId == assembly)
                {
                    List<AssemblyMasterModel> sewingMasterTempList = assemblyMasterList.Where(s => s.AssemblyLine == line && ((dateFrom <= s.AssemblyStartDate && s.AssemblyStartDate <= dateTo) || (dateFrom <= s.AssemblyFinishDate && s.AssemblyFinishDate <= dateTo))).ToList();
                    foreach (AssemblyMasterModel sewingMaster in sewingMasterTempList)
                    {
                        OrdersModel order = orderList.Where(o => o.ProductNo == sewingMaster.ProductNo).FirstOrDefault();
                        CompareDate(order, dateFrom, dateTo, sewingMaster.AssemblyStartDate, sewingMaster.AssemblyFinishDate, ref dr, materialIdAssemblyArray);
                    }
                }

                if (sectionId == outsole)
                {
                    List<OutsoleMasterModel> sewingMasterTempList = outsoleMasterList.Where(s => s.OutsoleLine == line && ((dateFrom <= s.OutsoleStartDate && s.OutsoleStartDate <= dateTo) || (dateFrom <= s.OutsoleFinishDate && s.OutsoleFinishDate <= dateTo))).ToList();
                    foreach (OutsoleMasterModel sewingMaster in sewingMasterTempList)
                    {                       
                        OrdersModel order = orderList.Where(o => o.ProductNo == sewingMaster.ProductNo).FirstOrDefault();
                        CompareDate(order, dateFrom, dateTo, sewingMaster.OutsoleStartDate, sewingMaster.OutsoleFinishDate, ref dr, materialIdOutsoleArray);
                    }
                }

                dt.Rows.Add(dr);
            }
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

        private void CompareDate(OrdersModel order, DateTime dateFrom, DateTime dateTo, DateTime startDate, DateTime finishDate, ref DataRow dataRow, int[] materialIdArray)
        {

            if (order == null)
            {
                return;
            }

            MaterialArrivalViewModel materialArrival = MaterialArrival(order.ProductNo, materialIdArray);
            List<DateTime> dateOffList = offDateList.Where(o => String.IsNullOrEmpty(o.Date.ToString()) == false).Select(p => p.Date).ToList();

            //DateTime dateColor = new DateTime(1992, 11, 16);
            DateTime dateColor = startDate;

            // Green Color
            while (dateColor <= finishDate)
            {
                if (dateColor >= dateFrom && dateColor <= dateTo)
                {
                    if (dataRow[String.Format("Day_{0:d-M}_Background", dateColor)] == Brushes.Red)
                    {
                        dataRow[String.Format("Day_{0:d-M}_Background", dateColor)] = Brushes.Red;
                    }
                    // addition code
                    //if (dataRow[String.Format("Day_{0:d-M}_Background", dateColor)] == Brushes.Yellow)
                    //{
                    //    dataRow[String.Format("Day_{0:d-M}_Background", dateColor)] = Brushes.Yellow;
                    //}
                    if (dataRow[String.Format("Day_{0:d-M}_Background", dateColor)] == Brushes.Orange)
                    {
                        dataRow[String.Format("Day_{0:d-M}_Background", dateColor)] = Brushes.Orange;
                    }

                    if (dataRow[String.Format("Day_{0:d-M}_Background", dateColor)] == Brushes.White)
                    {
                       dataRow[String.Format("Day_{0:d-M}_Background", dateColor)] = Brushes.Green;
                       dataRow[String.Format("Day_{0:d-M}_Tooltip", dateColor)] = string.Format(" ProductNo: {0}\nArticle: {1}\nPM: {2}\nShoe Name: {3}\nEFD: {4:MM/dd/yyyy}\nUpper Mat's Arrival: {5}", order.ProductNo, order.ArticleNo, order.PatternNo, order.ShoeName, order.ETD, materialArrival != null ? materialArrival.Date.ToShortDateString() : "");
                    }

                    // Yellow Color
                    if (materialArrival != null && dateColor > materialArrival.Date && (dateFrom <= dateColor && dateColor <= dateTo))
                    {
                        //int range = TimeHelper.CalculateDate(materialArrival.Date, dateColor);
                        int range = (Int32)(dateColor - materialArrival.Date).TotalDays;
                        if (range >= 0 && range <= 7)
                        {
                            if (dataRow[String.Format("Day_{0:d-M}_Background", dateColor)] == Brushes.Green)
                            {
                                dataRow[String.Format("Day_{0:d-M}_Background", dateColor)] = Brushes.Yellow;
                                dataRow[String.Format("Day_{0:d-M}_Tooltip", dateColor)] =
                                    string.Format("Product No: {0}\nShoe Name: {1}\nMaterial Arrival: {2:MM/dd/yyyy}\nStart Date: {3:MM/dd/yyyy}", order.ProductNo, order.ShoeName, materialArrival.Date, dateColor);
                            }
                        }
                    }

                    if (dateOffList.Contains(dateColor))
                    {
                        dataRow[String.Format("Day_{0:d-M}_Background", dateColor)] = Brushes.White;
                        dataRow[String.Format("Day_{0:d-M}_Tooltip", dateColor)] = null;
                    }
                }
                dateColor = dateColor.AddDays(1);
            }

            // Orange Color
            if (materialArrival != null && startDate < materialArrival.Date && (dateFrom <= startDate && startDate <= dateTo))
            {
                dataRow[String.Format("Day_{0:d-M}_Background", startDate)] = Brushes.Orange;
                if (dateOffList.Contains(startDate))
                {
                    dataRow[String.Format("Day_{0:d-M}_Background", startDate)] = Brushes.White;
                }
                dataRow[String.Format("Day_{0:d-M}_Tooltip", startDate)] =
                    string.Format("Product No: {0}\nShoe Name: {1}\nMaterial Arrival: {2:MM/dd/yyyy}\nStart Date: {3:MM/dd/yyyy}", order.ProductNo, order.ShoeName, materialArrival.Date, startDate);
            }


            // Red Color
            if (finishDate > order.ETD && (dateFrom <= finishDate && finishDate <= dateTo))
            {
                dataRow[String.Format("Day_{0:d-M}_Background", finishDate)] = Brushes.Red;
                dataRow[String.Format("Day_{0:d-M}_Tooltip", finishDate)] =
                    string.Format("Product No: {0}\nShoe Name: {1}\nETD: {2:MM/dd/yyyy}\nFinish Date: {3:MM/dd/yyyy}", order.ProductNo, order.ShoeName, order.ETD, finishDate);
            }
            
        }

        private void threadLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnViewResult.IsEnabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            dgAssemblyChart.ItemsSource = dt.AsDataView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpDateFrom.SelectedDate = DateTime.Now;
            dpDateTo.SelectedDate = DateTime.Now;
        }

        private void btnViewResult_Click(object sender, RoutedEventArgs e)
        {
            dgAssemblyChart.Columns.Clear();
            DateTime dateFrom = dpDateFrom.SelectedDate.Value.Date;
            DateTime dateTo = dpDateTo.SelectedDate.Value.Date;
            ComboBoxItem sectionItem = cboSection.SelectedItem as ComboBoxItem;
            string sectionId = sectionItem.Name;
            string sectionName = sectionItem.Content as string;
            //Create Columns.

            lblTitle.Text = string.Format("{0} SCHEDULE CHART", sectionName.ToUpper());

            dt = new DataTable();

            DataGridTextColumn dgcLine = new DataGridTextColumn();
            dgcLine.Header = "Line";
            dgcLine.Binding = new Binding("Line");
            dgcLine.CanUserSort = false;

            dgAssemblyChart.Columns.Add(dgcLine);

            dt.Columns.Add("Line", typeof(String));

            for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
            {
                DataGridTextColumn dgcDate = new DataGridTextColumn();
                dgcDate.Header = string.Format("{0:d/M}", date);
                dgcDate.Binding = new Binding(string.Format("Day_{0:d-M}", date));
                dgcDate.CanUserSort = false;


                Style style = new Style(typeof(DataGridCell));
                
                style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                
                Setter setter = new Setter();
                setter.Property = DataGridCell.BackgroundProperty;
                setter.Value = new Binding(String.Format("Day_{0:d-M}_Background", date));
                style.Setters.Add(setter);

                Setter setterTooltip = new Setter();
                setterTooltip.Property = ToolTipService.ToolTipProperty;
                setterTooltip.Value = new Binding(String.Format("Day_{0:d-M}_Tooltip", date));
                style.Setters.Add(setterTooltip);

                dgcDate.CellStyle = style;

                dgAssemblyChart.Columns.Add(dgcDate);

                dt.Columns.Add(String.Format("Day_{0:d-M}", date), typeof(String));

                DataColumn columnBackground = new DataColumn(String.Format("Day_{0:d-M}_Background", date), typeof(SolidColorBrush));
                columnBackground.DefaultValue = Brushes.White;
                dt.Columns.Add(columnBackground);

                dt.Columns.Add(String.Format("Day_{0:d-M}_Tooltip", date), typeof(String));
            }

            if (threadLoad.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            btnViewResult.IsEnabled = false;
            object[] args = { dateFrom, dateTo, sectionId };
            threadLoad.RunWorkerAsync(args);
        }

        private MaterialArrivalViewModel MaterialArrival(string productNo, int[] materialIdArray)
        {
            List<RawMaterialModel> rawMaterialTypeList = rawMaterialList.Where(r => r.ProductNo == productNo && materialIdArray.Contains(r.MaterialTypeId)).ToList();
            rawMaterialTypeList.RemoveAll(r => r.ETD.Date == dateDefault.Date);
            MaterialArrivalViewModel materialArrivalView = new MaterialArrivalViewModel();
            if (rawMaterialTypeList.Select(r => r.ActualDate).Count() > 0 && rawMaterialTypeList.Select(r => r.ActualDate.Date).Contains(dateDefault.Date) == false)
            {
                materialArrivalView.Date = rawMaterialTypeList.Select(r => r.ActualDate).Max();
                materialArrivalView.Foreground = Brushes.Blue;
                materialArrivalView.Background = Brushes.Transparent;
            }
            else
            {
                if (rawMaterialTypeList.Select(r => r.ETD).Count() > 0 && rawMaterialTypeList.Where(r => r.ETD.Date != dateDefault.Date).Count() > 0)
                {
                    materialArrivalView.Date = rawMaterialTypeList.Where(r => r.ActualDate.Date == dateDefault.Date).Select(r => r.ETD).Max();
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
        
    }
}
