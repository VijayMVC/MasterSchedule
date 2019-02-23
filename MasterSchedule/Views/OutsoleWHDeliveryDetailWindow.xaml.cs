using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Data;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Input;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHInventoryWindow.xaml
    /// </summary>
    public partial class OutsoleWHDeliveryDetailWindow : Window
    {
        BackgroundWorker bwLoad;
        BackgroundWorker bwExportExcel;

        List<String> productNoList;
        List<OrdersModel> orderList;
        List<OutsoleMaterialModel> outsoleMaterialList;

        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        string outsoleCode;
        int supplierID;
        List<SizeRunModel> sizeRunList;
        List<String> sizeNoList;
        List<SewingMasterModel> sewingMasterList;

        DateTime dtDefault;
        public OutsoleWHDeliveryDetailWindow(   List<OrdersModel> orderList, 
                                                List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList, 
                                                List<String> productNoList, 
                                                List<OutsoleRawMaterialModel> outsoleRawMaterialList, 
                                                string outsoleCode, 
                                                int supplierID  )
        {
            this.supplierID = supplierID;
            this.outsoleCode = outsoleCode;
            this.orderList = orderList;
            this.outsoleReleaseMaterialList = outsoleReleaseMaterialList;
            this.productNoList = productNoList;
            this.outsoleRawMaterialList = outsoleRawMaterialList;

            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            sizeRunList = new List<SizeRunModel>();
            sewingMasterList = new List<SewingMasterModel>();
            sizeNoList = new List<String>();
            dtDefault = new DateTime(2000, 01, 01);

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwExportExcel = new BackgroundWorker();
            bwExportExcel.DoWork += new DoWorkEventHandler(bwExportExcel_DoWork);
            bwExportExcel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwExportExcel_RunWorkerCompleted);

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        DataTable dtExportExcel = new DataTable();
        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleSupplierList = OutsoleSuppliersController.Select();
            sizeRunList = SizeRunController.SelectByOutsoleRawMaterial();
            sewingMasterList = SewingMasterController.Select();
            outsoleMaterialList = OutsoleMaterialController.Select().Where(w => productNoList.Contains(w.ProductNo) && w.OutsoleSupplierId == supplierID).ToList();

            DataTable dt = new DataTable();
            Dispatcher.Invoke(new Action(() =>
            {
                dt.Columns.Add("ProductNo", typeof(String));
                DataGridTextColumn columnProductNo = new DataGridTextColumn();
                columnProductNo.Header = "PO No.";

                //Style
                Style styleColumnProductNo = new Style(typeof(DataGridCell));

                Setter setterColumnProductNoForeground = new Setter();
                setterColumnProductNoForeground.Property = DataGridCell.ForegroundProperty;
                setterColumnProductNoForeground.Value = Brushes.Black;
                styleColumnProductNo.Setters.Add(setterColumnProductNoForeground);

                Setter setterColumnProductNoBackground = new Setter();
                setterColumnProductNoBackground.Property = DataGridCell.BackgroundProperty;
                setterColumnProductNoBackground.Value = new Binding("ProductNoBackground");
                styleColumnProductNo.Setters.Add(setterColumnProductNoBackground);

                columnProductNo.CellStyle = styleColumnProductNo;

                columnProductNo.Binding = new Binding("ProductNo");
                columnProductNo.FontWeight = FontWeights.Bold;

                DataColumn columnProductNoBackground = new DataColumn("ProductNoBackground", typeof(SolidColorBrush));
                columnProductNoBackground.DefaultValue = Brushes.Transparent;
                dt.Columns.Add(columnProductNoBackground);

                dgInventory.Columns.Add(columnProductNo);

                Binding bindingWidth1 = new Binding();
                bindingWidth1.Source = columnProductNo;
                bindingWidth1.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1 = new ColumnDefinition();
                cd1.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1);
                gridTotal.ColumnDefinitions.Add(cd1);

                dt.Columns.Add("ArticleNo", typeof(String));
                DataGridTextColumn column1_1_1 = new DataGridTextColumn();
                column1_1_1.Header = "Article No.";
                column1_1_1.Binding = new Binding("ArticleNo");
                //column1_1_1.FontWeight = FontWeights.Bold;
                dgInventory.Columns.Add(column1_1_1);
                Binding bindingWidth1_1_1 = new Binding();
                bindingWidth1_1_1.Source = column1_1_1;
                bindingWidth1_1_1.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_1_1 = new ColumnDefinition();
                cd1_1_1.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_1_1);
                gridTotal.ColumnDefinitions.Add(cd1_1_1);

                dt.Columns.Add("OrderETD", typeof(DateTime));
                DataGridTextColumn column1_1 = new DataGridTextColumn();
                column1_1.Header = "Order EFD";
                // Style
                Style styleOrderETD = new Style(typeof(DataGridCell));
                styleOrderETD.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                column1_1.CellStyle = styleOrderETD;

                Binding binding = new Binding();
                binding.Path = new PropertyPath("OrderETD");
                binding.StringFormat = "dd-MMM";
                column1_1.Binding = binding;
                column1_1.FontWeight = FontWeights.Bold;
                dgInventory.Columns.Add(column1_1);
                Binding bindingWidth1_1 = new Binding();
                bindingWidth1_1.Source = column1_1;
                bindingWidth1_1.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_1 = new ColumnDefinition();
                cd1_1.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_1);
                gridTotal.ColumnDefinitions.Add(cd1_1);

                dt.Columns.Add("DeliveryETD", typeof(DateTime));
                DataGridTextColumn column1_1_A = new DataGridTextColumn();
                column1_1_A.Header = "Delivery EFD";
                Binding bindingEDTSupp = new Binding();
                bindingEDTSupp.Path = new PropertyPath("DeliveryETD");
                bindingEDTSupp.StringFormat = "dd-MMM";
                column1_1_A.Binding = bindingEDTSupp;
                column1_1_A.FontWeight = FontWeights.Bold;

                // Style
                Style styleDeliveryETD = new Style(typeof(DataGridCell));
                styleDeliveryETD.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                column1_1_A.CellStyle = styleDeliveryETD;

                dgInventory.Columns.Add(column1_1_A);
                Binding bindingWidth1_1_A = new Binding();
                bindingWidth1_1_A.Source = column1_1_A;
                bindingWidth1_1_A.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_1_A = new ColumnDefinition();
                cd1_1_A.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_1_A);
                gridTotal.ColumnDefinitions.Add(cd1_1_A);


                dt.Columns.Add("SewingStartDate", typeof(DateTime));
                DataGridTextColumn column1_1_B = new DataGridTextColumn();
                column1_1_B.Header = "Sewing Start";
                Binding bindingSewingStart = new Binding();
                bindingSewingStart.Path = new PropertyPath("SewingStartDate");
                bindingSewingStart.StringFormat = "dd-MMM";
                column1_1_B.Binding = bindingSewingStart;
                column1_1_B.FontWeight = FontWeights.Bold;

                // Style
                Style styleSewingStart = new Style(typeof(DataGridCell));
                styleDeliveryETD.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                column1_1_B.CellStyle = styleDeliveryETD;

                dgInventory.Columns.Add(column1_1_B);
                Binding bindingWidthSewingStart = new Binding();
                bindingWidthSewingStart.Source = column1_1_B;
                bindingWidthSewingStart.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cdSewingStart = new ColumnDefinition();
                cdSewingStart.SetBinding(ColumnDefinition.WidthProperty, bindingWidthSewingStart);
                gridTotal.ColumnDefinitions.Add(cdSewingStart);

                dt.Columns.Add("Quantity", typeof(Int32));
                DataGridTextColumn column1_2 = new DataGridTextColumn();
                column1_2.Header = "Quantity";
                Binding bindingQuantity = new Binding();
                bindingQuantity.Path = new PropertyPath("Quantity");
                //binding.StringFormat = "dd-MMM";
                column1_2.Binding = bindingQuantity;
                //column1_2.FontWeight = FontWeights.Bold;
                dgInventory.Columns.Add(column1_2);
                Binding bindingWidth1_2 = new Binding();
                bindingWidth1_2.Source = column1_2;
                bindingWidth1_2.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_2 = new ColumnDefinition();
                cd1_2.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_2);
                gridTotal.ColumnDefinitions.Add(cd1_2);


                dt.Columns.Add("Release", typeof(Int32));
                DataGridTextColumn column1_3 = new DataGridTextColumn();
                column1_3.Header = "Release";
                Binding bindingRelease = new Binding();
                bindingRelease.Path = new PropertyPath("Release");
                column1_3.Binding = bindingRelease;
                dgInventory.Columns.Add(column1_3);
                Binding bindingWidth1_3 = new Binding();
                bindingWidth1_3.Source = column1_3;
                bindingWidth1_3.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_3 = new ColumnDefinition();
                cd1_3.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_3);
                gridTotal.ColumnDefinitions.Add(cd1_3);


                dt.Columns.Add("QuantityDelivery", typeof(Int32));
                DataGridTextColumn column1_4 = new DataGridTextColumn();
                column1_4.Header = "Delivery Qty";
                // Style
                Style styleQtyDelivery = new Style(typeof(DataGridCell));
                styleQtyDelivery.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                Setter setterBackgroundDelivery = new Setter();
                setterBackgroundDelivery.Property = DataGridCell.ForegroundProperty;
                setterBackgroundDelivery.Value = new Binding("QuantityDeliveryForeground");
                styleQtyDelivery.Setters.Add(setterBackgroundDelivery);
                column1_4.CellStyle = styleQtyDelivery;

                Binding bindingQtyDelivery = new Binding();
                bindingQtyDelivery.Path = new PropertyPath("QuantityDelivery");
                column1_4.Binding = bindingQtyDelivery;
                dgInventory.Columns.Add(column1_4);
                Binding bindingWidth1_4 = new Binding();
                bindingWidth1_4.Source = column1_4;
                bindingWidth1_4.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_4 = new ColumnDefinition();
                cd1_4.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_4);
                gridTotal.ColumnDefinitions.Add(cd1_4);

                DataColumn columnDeliveryForeground = new DataColumn("QuantityDeliveryForeground", typeof(SolidColorBrush));
                columnDeliveryForeground.DefaultValue = Brushes.Black;
                dt.Columns.Add(columnDeliveryForeground);

                dgInventory.FrozenColumnCount = 8;

                var regex = new Regex("[a-z]|[A-Z]");
                sizeNoList = outsoleMaterialList.Select(s => s.SizeNo).Distinct().OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "")) : Double.Parse(s)).ToList();
                for (int i = 0; i <= sizeNoList.Count - 1; i++)
                {
                    //OutsoleSuppliersModel outsoleSupplier = outsoleSupplierList[i];
                    string sizeNoBinding = sizeNoList[i].Contains(".") ? sizeNoList[i].Replace(".", "") : sizeNoList[i];
                    dt.Columns.Add(String.Format("Column{0}", sizeNoBinding), typeof(Int32));
                    DataGridTextColumn column = new DataGridTextColumn();
                    //column.SetValue(TagProperty, sizeRun.SizeNo);
                    column.Header = sizeNoList[i];
                    column.Width = 35;
                    column.Binding = new Binding(String.Format("Column{0}", sizeNoBinding));

                    Style style = new Style(typeof(DataGridCell));
                    style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

                    Setter setterBackground = new Setter();
                    setterBackground.Property = DataGridCell.BackgroundProperty;
                    setterBackground.Value = new Binding(String.Format("Column{0}Background", sizeNoBinding));

                    Setter setterForeground = new Setter();
                    setterForeground.Property = DataGridCell.ForegroundProperty;
                    setterForeground.Value = new Binding(String.Format("Column{0}Foreground", sizeNoBinding));

                    Setter setterToolTip = new Setter();
                    setterToolTip.Property = DataGridCell.ToolTipProperty;
                    setterToolTip.Value = new Binding(String.Format("Column{0}ToolTip", sizeNoBinding));

                    style.Setters.Add(setterBackground);
                    style.Setters.Add(setterForeground);
                    style.Setters.Add(setterToolTip);

                    column.CellStyle = style;
                    dgInventory.Columns.Add(column);
                    Binding bindingWidth = new Binding();
                    bindingWidth.Source = column;
                    bindingWidth.Path = new PropertyPath("ActualWidth");
                    ColumnDefinition cd = new ColumnDefinition();
                    cd.SetBinding(ColumnDefinition.WidthProperty, bindingWidth);
                    gridTotal.ColumnDefinitions.Add(cd);

                    DataColumn columnBackground = new DataColumn(String.Format("Column{0}Background", sizeNoBinding), typeof(SolidColorBrush));
                    columnBackground.DefaultValue = Brushes.Transparent;

                    DataColumn columnForeground = new DataColumn(String.Format("Column{0}Foreground", sizeNoBinding), typeof(SolidColorBrush));
                    columnForeground.DefaultValue = Brushes.Black;

                    DataColumn columnToolTip = new DataColumn(String.Format("Column{0}ToolTip", sizeNoBinding), typeof(String));

                    dt.Columns.Add(columnBackground);
                    dt.Columns.Add(columnForeground);
                    dt.Columns.Add(columnToolTip);
                }

                dt.Columns.Add("TotalBalance", typeof(Int32));
                DataGridTextColumn columnTotalBalance = new DataGridTextColumn();
                columnTotalBalance.Header = "Total";
                columnTotalBalance.Binding = new Binding("TotalBalance");
                dgInventory.Columns.Add(columnTotalBalance);
                Binding bindingWidthTotalBalance = new Binding();
                bindingWidthTotalBalance.Source = columnTotalBalance;
                bindingWidthTotalBalance.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cdTotalBalance = new ColumnDefinition();
                cdTotalBalance.SetBinding(ColumnDefinition.WidthProperty, bindingWidthTotalBalance);
                gridTotal.ColumnDefinitions.Add(cdTotalBalance);
            }));

            foreach (string productNo in productNoList)
            {
                var outsoleMaterialDetailList = OutsoleMaterialDetailController.Select(productNo).Where(w => w.OutsoleSupplierId == supplierID).ToList();
                var order = orderList.FirstOrDefault(f => f.ProductNo == productNo);

                var dr = dt.NewRow();

                if (order == null)
                    continue;
                dr["ProductNo"] = productNo;

                //HighLightPO
                int qtyOutsoleMaterialDetail = outsoleMaterialDetailList.Sum(s => s.Quantity);
                if (qtyOutsoleMaterialDetail >= order.Quantity)
                    dr["ProductNoBackground"] = Brushes.Green;
                else if (qtyOutsoleMaterialDetail != 0 && qtyOutsoleMaterialDetail < order.Quantity)
                    dr["ProductNoBackground"] = Brushes.Yellow;

                dr["ArticleNo"] = order.ArticleNo;
                dr["Quantity"] = order.Quantity;
                dr["OrderETD"] = order.ETD;

                var sewingMasterModel = sewingMasterList.Where(w => w.ProductNo == productNo).FirstOrDefault();
                if (sewingMasterModel != null)
                {
                    dr["SewingStartDate"] = sewingMasterModel.SewingStartDate;
                }

                var outsoleRawMaterialModel = outsoleRawMaterialList.Where(w => w.ProductNo == productNo && w.OutsoleSupplierId == supplierID).FirstOrDefault();
                var dtTemp = new DateTime(2000, 01, 01);
                if (outsoleRawMaterialModel != null)
                {
                    dr["DeliveryETD"] = outsoleRawMaterialModel.ETD;
                    dtTemp = outsoleRawMaterialModel.ETD;
                }

                int qtyDelivery = 0;
                foreach (string sizeNo in sizeNoList)
                {
                    string sizeNoBinding = sizeNo.Contains(".") ? sizeNo.Replace(".", "") : sizeNo;
                    var outsoleMaterialRecieve = outsoleMaterialList.Where(w => w.ProductNo == productNo && w.SizeNo == sizeNo).FirstOrDefault();

                    int qtyDeliveryPerSize = 0;
                    int qtyRejectPerSize = 0;
                    if (outsoleMaterialRecieve != null)
                    {
                        qtyDeliveryPerSize = outsoleMaterialRecieve.Quantity;
                        qtyRejectPerSize = outsoleMaterialRecieve.QuantityReject;
                    }
                    int qtyPerSize = 0;
                    if (sizeRunList.Where(w => w.ProductNo == productNo).Count() > 0)
                    {
                        qtyPerSize = sizeRunList.Where(w => w.ProductNo == productNo && w.SizeNo == sizeNo).Select(s => s.Quantity).FirstOrDefault();
                    }

                    if (qtyRejectPerSize > 0)
                    {
                        dr[String.Format("Column{0}", sizeNoBinding)] = qtyRejectPerSize;
                        dr[String.Format("Column{0}Foreground", sizeNoBinding)] = Brushes.Red;
                    }

                    int qtyBalance = qtyPerSize - qtyDeliveryPerSize;
                    if (qtyBalance > 0 && dtTemp != dtDefault)
                    {
                        dr[String.Format("Column{0}", sizeNoBinding)] = qtyBalance + qtyRejectPerSize;
                        var dtNow = DateTime.Now;
                        if (outsoleMaterialRecieve != null)
                        {
                            if (outsoleMaterialRecieve.ModifiedTime.Date < dtNow.Date)
                            {
                                dr[String.Format("Column{0}Background", sizeNoBinding)] = Brushes.Tomato;
                            }
                            if (outsoleMaterialRecieve.ModifiedTime.Date == dtNow.Date)
                            {
                                dr[String.Format("Column{0}Background", sizeNoBinding)] = Brushes.Green;
                            }
                            dr[String.Format("Column{0}Foreground", sizeNoBinding)] = Brushes.Black;
                        }

                        if (qtyRejectPerSize > 0)
                        {
                            dr[String.Format("Column{0}Background", sizeNoBinding)] = Brushes.Yellow;
                            dr[String.Format("Column{0}ToolTip", sizeNoBinding)] = String.Format("Balance: {0}\nReject: {1}", qtyBalance, qtyRejectPerSize);
                        }

                    }
                    qtyDelivery += qtyDeliveryPerSize;
                }
                int totalRelease = outsoleReleaseMaterialList.Where(w => w.ProductNo == productNo).Sum(s => s.Quantity);

                if (order.Quantity != qtyDelivery && qtyDelivery != 0)
                {
                    dr["QuantityDeliveryForeground"] = Brushes.Red;
                }

                dr["Release"] = totalRelease;
                dr["QuantityDelivery"] = qtyDelivery;


                int totalQty = outsoleMaterialList.Where(w => w.ProductNo == productNo).Sum(s => s.Quantity);
                int totalReject = outsoleMaterialList.Where(w => w.ProductNo == productNo).Sum(s => s.QuantityReject);

                if (order.Quantity - totalQty + totalReject > 0)
                {
                    dr["TotalBalance"] = order.Quantity - totalQty + totalReject;
                }

                dt.Rows.Add(dr);
            }

            Dispatcher.Invoke(new Action(() =>
            {
                TextBlock lblTotal = new TextBlock();
                lblTotal.Text = "TOTAL";
                lblTotal.Margin = new Thickness(1, 0, 0, 0);
                lblTotal.FontWeight = FontWeights.Bold;
                Border bdrTotal = new Border();
                Grid.SetColumn(bdrTotal, 2);
                Grid.SetColumnSpan(bdrTotal, 5);
                bdrTotal.BorderThickness = new Thickness(1, 0, 1, 1);
                bdrTotal.BorderBrush = Brushes.Black;
                bdrTotal.Child = lblTotal;
                gridTotal.Children.Add(bdrTotal);

                TextBlock lblQuantityTotal = new TextBlock();
                lblQuantityTotal.Text = dt.Compute("Sum(Quantity)", "").ToString();
                lblQuantityTotal.Margin = new Thickness(1, 0, 0, 0);
                lblQuantityTotal.FontWeight = FontWeights.Bold;
                lblQuantityTotal.TextAlignment = TextAlignment.Center;

                Border bdrQuantityTotal = new Border();
                Grid.SetColumn(bdrQuantityTotal, 7);
                bdrQuantityTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrQuantityTotal.BorderBrush = Brushes.Black;
                bdrQuantityTotal.Child = lblQuantityTotal;
                gridTotal.Children.Add(bdrQuantityTotal);
                dgInventory.ItemsSource = dt.AsDataView();

                TextBlock lblReleaseTotal = new TextBlock();
                lblReleaseTotal.Text = dt.Compute("Sum(Release)", "").ToString();
                lblReleaseTotal.Margin = new Thickness(1, 0, 0, 0);
                lblReleaseTotal.FontWeight = FontWeights.Bold;
                lblReleaseTotal.TextAlignment = TextAlignment.Center;

                Border bdrReleaseTotal = new Border();
                Grid.SetColumn(bdrReleaseTotal, 8);
                bdrReleaseTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrReleaseTotal.BorderBrush = Brushes.Black;
                bdrReleaseTotal.Child = lblReleaseTotal;
                gridTotal.Children.Add(bdrReleaseTotal);
                dgInventory.ItemsSource = dt.AsDataView();

                TextBlock lblQuantityDelivery = new TextBlock();
                lblQuantityDelivery.Text = dt.Compute("Sum(QuantityDelivery)", "").ToString();
                lblQuantityDelivery.Margin = new Thickness(1, 0, 0, 0);
                lblQuantityDelivery.FontWeight = FontWeights.Bold;
                lblQuantityDelivery.TextAlignment = TextAlignment.Center;

                Border bdrQuantityDeliveryTotal = new Border();
                Grid.SetColumn(bdrQuantityDeliveryTotal, 9);
                bdrQuantityDeliveryTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrQuantityDeliveryTotal.BorderBrush = Brushes.Black;
                bdrQuantityDeliveryTotal.Child = lblQuantityDelivery;
                gridTotal.Children.Add(bdrQuantityDeliveryTotal);
                dgInventory.ItemsSource = dt.AsDataView();

                for (int i = 0; i <= sizeNoList.Count - 1; i++)
                {
                    Border brTemp = new Border();
                    Grid.SetColumn(brTemp, 10 + i);
                    gridTotal.Children.Add(brTemp);
                }

                TextBlock txtTotalBalance = new TextBlock();
                txtTotalBalance.Text = dt.Compute("Sum(TotalBalance)", "").ToString();
                txtTotalBalance.Margin = new Thickness(1, 0, 0, 0);
                txtTotalBalance.FontWeight = FontWeights.Bold;
                txtTotalBalance.TextAlignment = TextAlignment.Center;

                Border brTotalBalance = new Border();
                Grid.SetColumn(brTotalBalance, 11 + sizeNoList.Count());
                brTotalBalance.BorderThickness = new Thickness(1, 0, 1, 1);
                brTotalBalance.BorderBrush = Brushes.Black;
                brTotalBalance.Child = txtTotalBalance;
                gridTotal.Children.Add(brTotalBalance);
                dgInventory.ItemsSource = dt.AsDataView();
            }));
            dtExportExcel = dt;
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnExcelFile.IsEnabled = true;
        }

        private void btnExcelFile_Click(object sender, RoutedEventArgs e)
        {
            if (bwExportExcel.IsBusy == false)
            {
                btnExcelFile.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                bwExportExcel.RunWorkerAsync();
            }
        }

        private void bwExportExcel_DoWork(object sender, DoWorkEventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            var supplierName = outsoleSupplierList.Where(w => w.OutsoleSupplierId == supplierID).FirstOrDefault().Name;

            Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    worksheet = workbook.ActiveSheet;
                    worksheet.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    worksheet.Cells.Font.Name = "Arial";
                    worksheet.Cells.Font.Size = 10;

                    worksheet.Name = String.Format("{0} - {1}", outsoleCode, supplierName);

                    for (int i = 0; i < dtExportExcel.Rows.Count; i++)
                    {
                        DataRow dr = dtExportExcel.Rows[i];

                        var valueList = new List<Object>();
                        var headerList = new List<String>();
                        var colorCodeList = new List<String>();

                        var productNo = dr["ProductNo"].ToString();
                        valueList.Add(productNo);
                        headerList.Add("ProductNo");
                        colorCodeList.Add("");

                        var article = dr["ArticleNo"].ToString();
                        valueList.Add(article);
                        headerList.Add("ArticleNo");
                        colorCodeList.Add("");

                        var orderETD = dr["OrderETD"].ToString().Split(' ')[0].ToString();
                        valueList.Add(orderETD);
                        headerList.Add("Order ETD");
                        colorCodeList.Add("");

                        var deliveryETD = dr["DeliveryETD"].ToString().Split(' ')[0].ToString();
                        valueList.Add(deliveryETD);
                        headerList.Add("Delivery ETD");
                        colorCodeList.Add("");

                        var sewingStartDate = dr["SewingStartDate"].ToString().Split(' ')[0].ToString();
                        valueList.Add(sewingStartDate);
                        headerList.Add("SewingStartDate");
                        colorCodeList.Add("");

                        var quantity = dr["Quantity"].ToString();
                        valueList.Add(quantity);
                        headerList.Add("Quantity");
                        colorCodeList.Add("");

                        var release = dr["Release"].ToString();
                        valueList.Add(release);
                        headerList.Add("Release");
                        colorCodeList.Add("");

                        var qtyDelivery = dr["QuantityDelivery"].ToString();
                        string qtyDeliveryForegroundString = "";
                        var qtyDeliveryForeground = dr["QuantityDeliveryForeground"] as SolidColorBrush;
                        if (qtyDeliveryForeground.ToString() == Brushes.Red.ToString())
                            qtyDeliveryForegroundString = qtyDeliveryForeground.ToString();

                        valueList.Add(qtyDelivery);
                        headerList.Add("Quantity Delivery");
                        colorCodeList.Add(qtyDeliveryForegroundString);

                        for (int j = 0; j <= sizeNoList.Count - 1; j++)
                        {
                            string sizeNoBinding = sizeNoList[j].Contains(".") ? sizeNoList[j].Replace(".", "") : sizeNoList[j];
                            var qtyPerSize = dr[String.Format("Column{0}", sizeNoBinding)].ToString();

                            string foregroundString = "";
                            var foregroundValue = dr[String.Format("Column{0}Foreground", sizeNoBinding)] as SolidColorBrush;
                            if (foregroundValue.ToString() == Brushes.Red.ToString())
                                foregroundString = foregroundValue.ToString();

                            valueList.Add(qtyPerSize);
                            headerList.Add(sizeNoList[j]);
                            colorCodeList.Add(foregroundString);
                        }

                        var total = dr["TotalBalance"].ToString();
                        valueList.Add(total);
                        headerList.Add("Total");
                        colorCodeList.Add("");

                        worksheet.Cells.Rows[1].Font.FontStyle = "Bold";

                        // Header
                        for (int headerColumn = 0; headerColumn < headerList.Count; headerColumn++)
                        {
                            worksheet.Cells[1, headerColumn + 1] = headerList[headerColumn];
                        }

                        for (int valueColumn = 0; valueColumn < valueList.Count(); valueColumn++)
                        {
                            worksheet.Cells[i + 2, valueColumn + 1] = valueList[valueColumn].ToString();
                            if (colorCodeList[valueColumn] != "")
                                worksheet.Cells[i + 2, valueColumn + 1].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                //worksheet.Cells.Rows[i + 2, index + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        }
                    }
                    if (workbook != null)
                    {
                        var sfd = new System.Windows.Forms.SaveFileDialog();
                        sfd.Filter = "Excel Documents (*.xls)|*.xls";
                        sfd.FileName = String.Format("Outsole Delivery Detail Report For {0} - {1}", outsoleCode, supplierName);
                        if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            workbook.SaveAs(sfd.FileName);
                            MessageBox.Show("Export Successful !", "Master-Schedule Export Excel", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Master-Schedule Export Excel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    excel.Quit();
                    workbook = null;
                    excel = null;
                }
            }));
        }

        private void bwExportExcel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnExcelFile.IsEnabled = true;
        }
    }
}
