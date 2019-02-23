using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Data;
using System.Windows.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WinForms;

using MasterSchedule.Models;
using MasterSchedule.Controllers;
using MasterSchedule.DataSets;
using MasterSchedule.Helpers;


namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleBalanceWindow.xaml
    /// </summary>
    public partial class OutsoleOutputBalanceWindow : Window
    {
        BackgroundWorker bwLoad;
        DataTable dt;
        List<OutsoleMasterModel> outsoleMasterList;
        List<OrdersModel> orderList;
        List<OutsoleOutputModel> outsoleOutputList;
        List<SizeRunModel> sizeRunList;
        List<String> productNoList;
        List<String> sizeNoList;

        List<OutsoleOutputBalanceViewModel> outsoleOutputBalanceViewModelList;

        public OutsoleOutputBalanceWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            outsoleMasterList = new List<OutsoleMasterModel>();
            orderList = new List<OrdersModel>();
            outsoleOutputList = new List<OutsoleOutputModel>();
            sizeRunList = new List<SizeRunModel>();

            productNoList = new List<String>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpETDStart.SelectedDate = DateTime.Now.Date;
            dpETDEnd.SelectedDate = DateTime.Now.Date;

            if (bwLoad.IsBusy == false)
            {
                btnPreview.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            orderList = OrdersController.Select();
            productNoList = orderList.Select(s => s.ProductNo).Distinct().ToList();

            outsoleMasterList = OutsoleMasterController.Select();
            outsoleOutputList = OutsoleOutputController.SelectByIsEnable();
            sizeRunList = SizeRunController.SelectIsEnable();

            var regex = new Regex("[a-z]|[A-Z]");
            sizeNoList = new List<String>();

            var sizeNoIsDouble = sizeRunList.Where(w => regex.IsMatch(w.SizeNo) == false).Select(s => s.SizeNo).Distinct().ToList();
            sizeNoIsDouble = sizeNoIsDouble.OrderBy(s => Double.Parse(s)).ToList();
            sizeNoList.AddRange(sizeNoIsDouble);

            var sizeNoHasCharacter = sizeRunList.Where(w => regex.IsMatch(w.SizeNo)).OrderBy(o => o.SizeNo).Select(s => s.SizeNo).Distinct().ToList();
            sizeNoHasCharacter = sizeNoHasCharacter.OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "")) : Double.Parse(s)).ToList();
            sizeNoList.AddRange(sizeNoHasCharacter);

            // Collect Data
            outsoleOutputBalanceViewModelList = new List<OutsoleOutputBalanceViewModel>();
            foreach (var po in productNoList)
            {
                var order = orderList.Where(w => w.ProductNo == po).FirstOrDefault();
                var outsoleMaster_PO = outsoleMasterList.Where(w => w.ProductNo == po).FirstOrDefault();
                var sizeRun_POList = sizeRunList.Where(w => w.ProductNo == po).ToList();
                var outsoleOutput_POList = outsoleOutputList.Where(w => w.ProductNo == po).ToList();

                if (order == null || outsoleMaster_PO == null || sizeRun_POList.Count == 0)
                    continue;

                if (outsoleOutput_POList.Sum(s => s.Quantity) >= sizeRun_POList.Sum(s => s.Quantity))
                    continue;

                var outsoleOutputBalanceViewModel = new OutsoleOutputBalanceViewModel()
                {
                    ProductNo = po,
                    Country = order.Country,
                    ShoeName = order.ShoeName,
                    ArticleNo = order.ArticleNo,
                    ETD = order.ETD,
                    OutsoleLine = outsoleMaster_PO.OutsoleLine,
                    OutsoleCode = order.OutsoleCode
                };

                var outsoleOutputBalanceValueList = new List<OutsoleOutputBalanceValue>();
                for (int i = 0; i <= sizeNoList.Count - 1; i++)
                {
                    var sizeNoBinding = sizeNoList[i].Contains(".") == true ? sizeNoList[i].Replace(".", "@") : sizeNoList[i];

                    var outsoleOutputBalanceValue = new OutsoleOutputBalanceValue();
                    outsoleOutputBalanceValue.SizeNo = sizeNoList[i];
                    outsoleOutputBalanceValue.ForeColor = Brushes.Black;

                    var sizeRun_Size = sizeRun_POList.Where(w => w.SizeNo == sizeNoList[i]).FirstOrDefault();
                    if (sizeRun_Size == null)
                        continue;

                    //PO not yet make
                    if (outsoleOutput_POList.Count == 0)
                    {
                        outsoleOutputBalanceValue.Value = sizeRun_Size.Quantity;
                    }

                    else
                    {
                        var outsoleOutput_PO_SizeNo = outsoleOutput_POList.Where(w => w.SizeNo == sizeNoList[i]).FirstOrDefault();
                        int qtyShow = sizeRun_Size.Quantity - outsoleOutput_PO_SizeNo.Quantity;
                        if (qtyShow > 0)
                        {
                            outsoleOutputBalanceValue.Value = qtyShow;
                        }
                        if (outsoleOutput_PO_SizeNo.Quantity > 0)
                        {
                            outsoleOutputBalanceValue.ForeColor = Brushes.Red;
                        }
                    }
                    if (outsoleOutputBalanceValue.Value > 0)
                        outsoleOutputBalanceValueList.Add(outsoleOutputBalanceValue);
                }
                outsoleOutputBalanceViewModel.Values = outsoleOutputBalanceValueList;
                outsoleOutputBalanceViewModelList.Add(outsoleOutputBalanceViewModel);
            }
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnPreview.IsEnabled = true;
        }


        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            var outsoleOutputBalanceFilterList = outsoleOutputBalanceViewModelList.ToList();
            if (chboETD.IsChecked == true)
            {
                DateTime etdStart = dpETDStart.SelectedDate.Value;
                DateTime etdEnd = dpETDEnd.SelectedDate.Value;
                outsoleOutputBalanceFilterList = outsoleOutputBalanceFilterList.Where(w => etdStart.Date <= w.ETD.Date && w.ETD.Date <= etdEnd.Date).ToList();
            }

            string country = txtCountry.Text;
            if (string.IsNullOrEmpty(country) == false)
            {
                outsoleOutputBalanceFilterList = outsoleOutputBalanceFilterList.Where(w => w.Country.ToLower().Contains(country.ToLower()) == true).ToList();
            }

            string articleNo = txtArticleNo.Text;
            if (string.IsNullOrEmpty(articleNo) == false)
            {
                outsoleOutputBalanceFilterList = outsoleOutputBalanceFilterList.Where(w => w.ArticleNo.ToLower().Contains(articleNo.ToLower()) == true).ToList();
            }

            string shoeName = txtStyle.Text;
            if (string.IsNullOrEmpty(shoeName) == false)
            {
                outsoleOutputBalanceFilterList = outsoleOutputBalanceFilterList.Where(w => w.ShoeName.ToLower().Contains(shoeName.ToLower()) == true).ToList();
            }

            string outsoleLine = txtOutsoleLine.Text;
            if (string.IsNullOrEmpty(outsoleLine) == false)
            {
                outsoleOutputBalanceFilterList = outsoleOutputBalanceFilterList.Where(w => w.OutsoleLine.ToLower().Contains(outsoleLine.ToLower()) == true).ToList();
            }

            var SizeNoRemoveList = new List<String>();
            var sizeNoListFilter = sizeNoList;

            var valuesList = outsoleOutputBalanceFilterList.Select(s => s.Values).ToList();

            foreach (var sizeNo in sizeNoList)
            {
                var valuesList_SizeNo = valuesList.Where(w => w.Select(s => s.SizeNo).Contains(sizeNo)).ToList();
                if (valuesList_SizeNo.Count == 0)
                    SizeNoRemoveList.Add(sizeNo);
            }

            sizeNoListFilter = sizeNoList.Where(w => SizeNoRemoveList.Contains(w) == false).ToList();


            // Create DataGrid
            Dispatcher.Invoke(new Action(() =>
            {
                dt = new DataTable();
                dt.Columns.Clear();
                dgOutsoleBalance.ItemsSource = null;
                dgOutsoleBalance.Columns.Clear();

                dt.Columns.Add("ProductNo", typeof(String));
                var colProductNo = new DataGridTextColumn();
                colProductNo.Header = "Product No";
                colProductNo.Binding = new Binding("ProductNo");
                dgOutsoleBalance.Columns.Add(colProductNo);

                dt.Columns.Add("OutsoleCode", typeof(String));
                var colOutsoleCode = new DataGridTextColumn();
                colOutsoleCode.Header = "O/S Code";
                colOutsoleCode.Binding = new Binding("OutsoleCode");
                dgOutsoleBalance.Columns.Add(colOutsoleCode);

                dt.Columns.Add("Country", typeof(String));
                var colCountry = new DataGridTextColumn();
                colCountry.Header = "Country";
                colCountry.Binding = new Binding("Country");
                dgOutsoleBalance.Columns.Add(colCountry);

                dt.Columns.Add("Style", typeof(String));
                var colStyle = new DataGridTextColumn();
                colStyle.Header = "Style";
                colStyle.Binding = new Binding("Style");
                dgOutsoleBalance.Columns.Add(colStyle);

                dt.Columns.Add("ArticleNo", typeof(String));
                var colArticleNo = new DataGridTextColumn();
                colArticleNo.Header = "ArticleNo";
                colArticleNo.Binding = new Binding("ArticleNo");
                dgOutsoleBalance.Columns.Add(colArticleNo);

                dt.Columns.Add("ETD", typeof(DateTime));
                var colETD = new DataGridTextColumn();
                colETD.Header = "EFD";
                colETD.Binding = new Binding("ETD");
                colETD.Binding.StringFormat = "MM/dd";
                dgOutsoleBalance.Columns.Add(colETD);

                dt.Columns.Add("OutsoleLine", typeof(String));
                var colOutsoleLine = new DataGridTextColumn();
                colOutsoleLine.Header = "Outsole Line";
                colOutsoleLine.Binding = new Binding("OutsoleLine");
                dgOutsoleBalance.Columns.Add(colOutsoleLine);

                dgOutsoleBalance.FrozenColumnCount = 7;

                for (int i = 0; i <= sizeNoListFilter.Count - 1; i++)
                {
                    var sizeNoBinding = sizeNoListFilter[i].Contains(".") == true ? sizeNoListFilter[i].Replace(".", "@") : sizeNoListFilter[i];

                    dt.Columns.Add(String.Format("Column{0}", sizeNoBinding, typeof(String)));
                    DataGridTextColumn col = new DataGridTextColumn();
                    col.Header = String.Format("{0}", sizeNoListFilter[i]);
                    col.MinWidth = 40;
                    col.Binding = new Binding(String.Format("Column{0}", sizeNoBinding));

                    Style style = new Style(typeof(DataGridCell));
                    Setter setterForeground = new Setter();
                    setterForeground.Property = DataGridCell.ForegroundProperty;
                    setterForeground.Value = new Binding(String.Format("Column{0}Foreground", sizeNoBinding));

                    style.Setters.Add(setterForeground);

                    col.CellStyle = style;

                    DataColumn columnForeground = new DataColumn(String.Format("Column{0}Foreground", sizeNoBinding), typeof(SolidColorBrush));
                    columnForeground.DefaultValue = Brushes.Black;

                    dt.Columns.Add(columnForeground);

                    dgOutsoleBalance.Columns.Add(col);
                }
            }));

            // Fill Data
            foreach (var T in outsoleOutputBalanceFilterList)
            {
                var dr = dt.NewRow();
                dr["ProductNo"] = T.ProductNo;
                dr["Country"] = T.Country;
                dr["Style"] = T.ShoeName;
                dr["ArticleNo"] = T.ArticleNo;
                dr["ETD"] = T.ETD;
                dr["OutsoleLine"] = T.OutsoleLine;
                dr["OutsoleCode"] = T.OutsoleCode;

                foreach (var TT in T.Values)
                {
                    var sizeNoBinding = TT.SizeNo.Contains(".") == true ? TT.SizeNo.Replace(".", "@") : TT.SizeNo;
                    if (TT.Value > 0)
                        dr[String.Format("Column{0}", sizeNoBinding)] = TT.Value;
                    dr[String.Format("Column{0}Foreground", sizeNoBinding)] = TT.ForeColor;
                }

                dt.Rows.Add(dr);
            }
            dgOutsoleBalance.ItemsSource = dt.AsDataView();
        }

        private void btnExcelFile_Click(object sender, RoutedEventArgs e)
        {
            //OutsoleOutputBalanceReportWindow window = new OutsoleOutputBalanceReportWindow(dt, sizeNoList);
            //window.ShowDialog();
        }

        private void bwExportExcel_DoWork(object sender, DoWorkEventArgs e)
        {
            #region EXCEL
            //Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            //Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            //Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            //Dispatcher.Invoke(new Action(() =>
            //{
            //    try
            //    {
            //        worksheet = workbook.ActiveSheet;
            //        worksheet.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //        worksheet.Cells.Font.Name = "Arial";
            //        worksheet.Cells.Font.Size = 10;

            //        worksheet.Name = String.Format("{0}", "Outsole Output Balance");

            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            DataRow dr = dt.Rows[i];

            //            var headerList = new List<String>();
            //            var valueList = new List<Object>();
            //            var colorCodeList = new List<String>();

            //            var productNo = dr["ProductNo"].ToString();
            //            valueList.Add(productNo);
            //            headerList.Add("ProductNo");
            //            colorCodeList.Add("");

            //            var country = dr["Country"].ToString();
            //            valueList.Add(country);
            //            headerList.Add("Country");
            //            colorCodeList.Add("");

            //            var style = dr["Style"].ToString();
            //            valueList.Add(style);
            //            headerList.Add("Style");
            //            colorCodeList.Add("");

            //            var articleNo = dr["ArticleNo"].ToString();
            //            valueList.Add(articleNo);
            //            headerList.Add("ArticleNo");
            //            colorCodeList.Add("");

            //            var ETD = dr["ETD"].ToString().Split(' ')[0].ToString();
            //            valueList.Add(ETD);
            //            headerList.Add("EFD");
            //            colorCodeList.Add("");

            //            var outsoleLine = dr["OutsoleLine"].ToString();
            //            valueList.Add(outsoleLine);
            //            headerList.Add("OutsoleLine");
            //            colorCodeList.Add("");

            //            for (int j = 0; j < sizeNoList.Count - 1; j++)
            //            {
            //                var balancePerSize = dr[String.Format("Column{0}", j)].ToString();

            //                string foregroundString = "";
            //                var foregroundValue = dr[String.Format("Column{0}Foreground", j)] as SolidColorBrush;
            //                if (foregroundValue.ToString() == Brushes.Red.ToString())
            //                    foregroundString = foregroundValue.ToString();

            //                headerList.Add(sizeNoList[j]);
            //                valueList.Add(balancePerSize);
            //                colorCodeList.Add(foregroundString);
            //            }

            //            worksheet.Cells.Rows[1].Font.FontStyle = "Bold";
            //            // Header
            //            for (int indexHeader = 0; indexHeader < headerList.Count; indexHeader++)
            //            {
            //                worksheet.Cells[1, indexHeader + 1] = headerList[indexHeader];
            //            }

            //            for (int index = 0; index < valueList.Count(); index++)
            //            {
            //                worksheet.Cells[i + 2, index + 1] = valueList[index].ToString();
            //                if (colorCodeList[index] != "")
            //                    worksheet.Cells[i + 2, index + 1].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
            //            }
            //        }
            //        if (workbook != null)
            //        {
            //            var sfd = new System.Windows.Forms.SaveFileDialog();
            //            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            //            sfd.FileName = String.Format("Outsole Output Balance");
            //            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //            {
            //                workbook.SaveAs(sfd.FileName);
            //                MessageBox.Show("Export Successful !", "Master-Schedule Export Excel", MessageBoxButton.OK, MessageBoxImage.Information);
            //            }
            //        }
            //    }
            //    catch (System.Exception ex)
            //    {
            //        MessageBox.Show(ex.Message, "Master-Schedule Export Excel", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //    finally
            //    {
            //        excel.Quit();
            //        workbook = null;
            //        excel = null;
            //    }
            //}));
            #endregion

            #region REPORT
            

            #endregion
        }

        private void bwExportExcel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            //btnExcelFile.IsEnabled = true;
        }

        class OutsoleOutputBalanceViewModel
        {
            public string ProductNo { get; set; }
            public string Country { get; set; }
            public string ShoeName { get; set; }
            public string ArticleNo { get; set; }
            public DateTime ETD { get; set; }
            public string OutsoleLine { get; set; }
            public string OutsoleCode { get; set; }

            public List<OutsoleOutputBalanceValue> Values { get; set; }
        }

        class OutsoleOutputBalanceValue
        {
            public string SizeNo { get; set; }
            public int Value { get; set; }
            public SolidColorBrush ForeColor { get; set; }
        }

    }
}
