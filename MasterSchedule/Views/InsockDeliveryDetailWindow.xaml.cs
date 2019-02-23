using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Text.RegularExpressions;

using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Data;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for InsockDeliveryDetailWindow.xaml
    /// </summary>
    public partial class InsockDeliveryDetailWindow : Window
    {
        List<OrdersModel> orderList;
        List<InsockMaterialModel> insockMaterialList;
        List<InsockRawMaterialModel> insockRawMaterialList;
        string lastCode;
        int insockSupplierId;

        List<SizeRunModel> sizeRunList;
        List<SewingMasterModel> sewingMasterList;
        List<String> sizeNoList;
        DateTime dtDefault;

        BackgroundWorker bwLoad;

        public InsockDeliveryDetailWindow(List<OrdersModel> orderList, List<InsockMaterialModel> insockMaterialList, List<InsockRawMaterialModel> insockRawMaterialList, string lastCode, int insockSupplierId)
        {
            this.orderList = orderList;
            this.insockMaterialList = insockMaterialList;
            this.insockRawMaterialList = insockRawMaterialList;
            this.lastCode = lastCode;
            this.insockSupplierId = insockSupplierId;

            sizeRunList = new List<SizeRunModel>();
            sewingMasterList = new List<SewingMasterModel>();
            sizeNoList = new List<String>();
            dtDefault = new DateTime(2000, 01, 01);

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

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
        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            sizeRunList = SizeRunController.SelectByInsockRawMaterial();
            sewingMasterList = SewingMasterController.Select();

            var dt = new DataTable();

            Dispatcher.Invoke(new Action(() =>
            {
                dt.Columns.Add("ProductNo", typeof(String));
                DataGridTextColumn column1 = new DataGridTextColumn();
                column1.Header = "PO No.";
                column1.Binding = new Binding("ProductNo");
                column1.FontWeight = FontWeights.Bold;
                dgInsockDeliveryDetail.Columns.Add(column1);
                Binding bindingWidth1 = new Binding();
                bindingWidth1.Source = column1;
                bindingWidth1.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1 = new ColumnDefinition();
                cd1.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1);
                gridTotal.ColumnDefinitions.Add(cd1);

                dt.Columns.Add("ArticleNo", typeof(String));
                DataGridTextColumn column1_1_1 = new DataGridTextColumn();
                column1_1_1.Header = "Article No.";
                column1_1_1.Binding = new Binding("ArticleNo");
                //column1_1_1.FontWeight = FontWeights.Bold;
                dgInsockDeliveryDetail.Columns.Add(column1_1_1);
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
                Style styleCenter = new Style(typeof(DataGridCell));
                styleCenter.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                column1_1.CellStyle = styleCenter;

                Binding binding = new Binding();
                binding.Path = new PropertyPath("OrderETD");
                binding.StringFormat = "dd-MMM";
                column1_1.Binding = binding;
                column1_1.FontWeight = FontWeights.Bold;
                dgInsockDeliveryDetail.Columns.Add(column1_1);
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
                
                column1_1_A.CellStyle = styleCenter;

                dgInsockDeliveryDetail.Columns.Add(column1_1_A);
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
                column1_1_B.CellStyle = styleCenter;

                dgInsockDeliveryDetail.Columns.Add(column1_1_B);
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
                dgInsockDeliveryDetail.Columns.Add(column1_2);
                Binding bindingWidth1_2 = new Binding();
                bindingWidth1_2.Source = column1_2;
                bindingWidth1_2.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_2 = new ColumnDefinition();
                cd1_2.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_2);
                gridTotal.ColumnDefinitions.Add(cd1_2);


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
                dgInsockDeliveryDetail.Columns.Add(column1_4);
                Binding bindingWidth1_4 = new Binding();
                bindingWidth1_4.Source = column1_4;
                bindingWidth1_4.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_4 = new ColumnDefinition();
                cd1_4.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_4);
                gridTotal.ColumnDefinitions.Add(cd1_4);

                DataColumn columnDeliveryForeground = new DataColumn("QuantityDeliveryForeground", typeof(SolidColorBrush));
                columnDeliveryForeground.DefaultValue = Brushes.Black;
                dt.Columns.Add(columnDeliveryForeground);

                dgInsockDeliveryDetail.FrozenColumnCount = 7;

                var regex = new Regex("[a-z]|[A-Z]");
                sizeNoList = insockMaterialList.Select(s => s.SizeNo).Distinct().OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "")) : Double.Parse(s)).ToList();
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
                    dgInsockDeliveryDetail.Columns.Add(column);
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
                dgInsockDeliveryDetail.Columns.Add(columnTotalBalance);
                Binding bindingWidthTotalBalance = new Binding();
                bindingWidthTotalBalance.Source = columnTotalBalance;
                bindingWidthTotalBalance.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cdTotalBalance = new ColumnDefinition();
                cdTotalBalance.SetBinding(ColumnDefinition.WidthProperty, bindingWidthTotalBalance);
                gridTotal.ColumnDefinitions.Add(cdTotalBalance);
            }));

            var productNoList = insockMaterialList.Select(s => s.ProductNo).Distinct().ToList();
            foreach (string productNo in productNoList)
            {
                DataRow dr = dt.NewRow();
                dr["ProductNo"] = productNo;
                OrdersModel order = orderList.Where(w => w.ProductNo == productNo).FirstOrDefault();
                if (order != null)
                {
                    dr["ArticleNo"] = order.ArticleNo;
                    dr["Quantity"] = order.Quantity;
                    dr["OrderETD"] = order.ETD;
                }

                var sewingMasterModel = sewingMasterList.Where(w => w.ProductNo == productNo).FirstOrDefault();
                if (sewingMasterModel != null)
                {
                    dr["SewingStartDate"] = sewingMasterModel.SewingStartDate;
                }

                var insockRawMaterialModel = insockRawMaterialList.Where(w => w.ProductNo == productNo && w.InsockSupplierId == insockSupplierId).FirstOrDefault();
                DateTime dtTemp = new DateTime(2000, 01, 01);
                if (insockRawMaterialModel != null)
                {
                    dr["DeliveryETD"] = insockRawMaterialModel.ETD;
                    dtTemp = insockRawMaterialModel.ETD;
                }

                int qtyDelivery = 0;
                foreach (string sizeNo in sizeNoList)
                {
                    string sizeNoBinding = sizeNo.Contains(".") ? sizeNo.Replace(".", "") : sizeNo;
                    var insockMaterialRecieve = insockMaterialList.Where(w => w.ProductNo == productNo && w.SizeNo == sizeNo).FirstOrDefault();

                    int qtyDeliveryPerSize = 0;
                    int qtyRejectPerSize = 0;
                    if (insockMaterialRecieve != null)
                    {
                        qtyDeliveryPerSize = insockMaterialRecieve.Quantity;
                        qtyRejectPerSize = insockMaterialRecieve.QuantityReject;
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
                        DateTime dtNow = DateTime.Now;
                        if (insockMaterialRecieve != null)
                        {
                            if (insockMaterialRecieve.ModifiedTime.Date < dtNow.Date)
                            {
                                dr[String.Format("Column{0}Background", sizeNoBinding)] = Brushes.Tomato;
                            }
                            if (insockMaterialRecieve.ModifiedTime.Date == dtNow.Date)
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
                //int totalRelease = outsoleReleaseMaterialList.Where(w => w.ProductNo == productNo).Sum(s => s.Quantity);

                if (order.Quantity != qtyDelivery && qtyDelivery != 0)
                {
                    dr["QuantityDeliveryForeground"] = Brushes.Red;
                }

                //dr["Release"] = totalRelease;
                dr["QuantityDelivery"] = qtyDelivery;

                int totalQty = insockMaterialList.Where(w => w.ProductNo == productNo).Sum(s => s.Quantity);
                int totalReject = insockMaterialList.Where(w => w.ProductNo == productNo).Sum(s => s.QuantityReject);

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
                dgInsockDeliveryDetail.ItemsSource = dt.AsDataView();

                
                TextBlock lblQuantityDelivery = new TextBlock();
                lblQuantityDelivery.Text = dt.Compute("Sum(QuantityDelivery)", "").ToString();
                lblQuantityDelivery.Margin = new Thickness(1, 0, 0, 0);
                lblQuantityDelivery.FontWeight = FontWeights.Bold;
                lblQuantityDelivery.TextAlignment = TextAlignment.Center;

                Border bdrQuantityDeliveryTotal = new Border();
                Grid.SetColumn(bdrQuantityDeliveryTotal, 8);
                bdrQuantityDeliveryTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrQuantityDeliveryTotal.BorderBrush = Brushes.Black;
                bdrQuantityDeliveryTotal.Child = lblQuantityDelivery;
                gridTotal.Children.Add(bdrQuantityDeliveryTotal);
                dgInsockDeliveryDetail.ItemsSource = dt.AsDataView();

                for (int i = 0; i <= sizeNoList.Count - 1; i++)
                {
                    Border brTemp = new Border();
                    Grid.SetColumn(brTemp, 9 + i);
                    gridTotal.Children.Add(brTemp);
                }

                TextBlock txtTotalBalance = new TextBlock();
                txtTotalBalance.Text = dt.Compute("Sum(TotalBalance)", "").ToString();
                txtTotalBalance.Margin = new Thickness(1, 0, 0, 0);
                txtTotalBalance.FontWeight = FontWeights.Bold;
                txtTotalBalance.TextAlignment = TextAlignment.Center;

                Border brTotalBalance = new Border();
                Grid.SetColumn(brTotalBalance, 10 + sizeNoList.Count());
                brTotalBalance.BorderThickness = new Thickness(1, 0, 1, 1);
                brTotalBalance.BorderBrush = Brushes.Black;
                brTotalBalance.Child = txtTotalBalance;
                gridTotal.Children.Add(brTotalBalance);
                dgInsockDeliveryDetail.ItemsSource = dt.AsDataView();
            }));
        }
        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }
    }
}
