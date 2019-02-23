using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Data;
using System.Text.RegularExpressions;
using System.ComponentModel;


namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpperComponentWHDeliveryDetailWindow.xaml
    /// </summary>
    public partial class UpperComponentWHDeliveryDetailWindow : Window
    {
        List<String> productNoList;
        List<OrdersModel> orderList;
        List<SizeRunModel> sizeRunList;
        List<UpperComponentMaterialModel> upperComponentMaterialList;
        List<UpperComponentRawMaterialModel> upperComponentRawMaterialList;
        string outsoleCode;
        int upperComponentID;
        BackgroundWorker bwLoad;
        public UpperComponentWHDeliveryDetailWindow(List<OrdersModel> orderList,
                                                List<UpperComponentMaterialModel> upperComponentMaterialList,
                                                List<String> productNoList,
                                                List<UpperComponentRawMaterialModel> upperComponentRawMaterialList,
                                                string outsoleCode,
                                                int upperComponentID)
        {
            this.upperComponentID = upperComponentID;
            this.orderList = orderList;
            this.outsoleCode = outsoleCode;
            this.upperComponentMaterialList = upperComponentMaterialList;
            this.upperComponentRawMaterialList = upperComponentRawMaterialList;
            this.productNoList = productNoList;
            sizeRunList = new List<SizeRunModel>();
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
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
            DataTable dt = new DataTable();
            var regex = new Regex("[a-z]|[A-Z]");
            List<String> sizeNoList = upperComponentMaterialList.Select(s => s.SizeNo).Distinct().OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "")) : Double.Parse(s)).ToList();

            DateTime dtDefault = new DateTime(2000, 01, 01);
            var sewingMasterList = SewingMasterController.Select().ToList();

            Dispatcher.Invoke(new Action(() =>
            {
                dt.Columns.Add("ProductNo", typeof(String));
                DataGridTextColumn column1 = new DataGridTextColumn();
                column1.Header = "PO No.";
                column1.Binding = new Binding("ProductNo");
                column1.FontWeight = FontWeights.Bold;
                dgInventory.Columns.Add(column1);
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


                //dt.Columns.Add("Release", typeof(Int32));
                //DataGridTextColumn column1_3 = new DataGridTextColumn();
                //column1_3.Header = "Release";
                //Binding bindingRelease = new Binding();
                //bindingRelease.Path = new PropertyPath("Release");
                //column1_3.Binding = bindingRelease;
                //dgInventory.Columns.Add(column1_3);
                //Binding bindingWidth1_3 = new Binding();
                //bindingWidth1_3.Source = column1_3;
                //bindingWidth1_3.Path = new PropertyPath("ActualWidth");
                //ColumnDefinition cd1_3 = new ColumnDefinition();
                //cd1_3.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_3);
                //gridTotal.ColumnDefinitions.Add(cd1_3);


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



                //dt.Columns.Add("Matching", typeof(Int32));
                //DataGridTextColumn column2 = new DataGridTextColumn();
                //column2.Header = "Matching";
                //column2.Binding = new Binding("Matching");
                //dgInventory.Columns.Add(column2);
                //Binding bindingWidth2 = new Binding();
                //bindingWidth2.Source = column2;
                //bindingWidth2.Path = new PropertyPath("ActualWidth");
                //ColumnDefinition cd2 = new ColumnDefinition();
                //cd2.SetBinding(ColumnDefinition.WidthProperty, bindingWidth2);
                //gridTotal.ColumnDefinitions.Add(cd2);

                dgInventory.FrozenColumnCount = 8;
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
                    style.Setters.Add(setterBackground);
                    column.CellStyle = style;

                    Setter setterForeground = new Setter();
                    setterForeground.Property = DataGridCell.ForegroundProperty;
                    setterForeground.Value = new Binding(String.Format("Column{0}Foreground", sizeNoBinding));
                    style.Setters.Add(setterForeground);

                    Setter setterToolTip = new Setter();
                    setterToolTip.Property = DataGridCell.ToolTipProperty;
                    setterToolTip.Value = new Binding(String.Format("Column{0}ToolTip", sizeNoBinding));
                    style.Setters.Add(setterToolTip);

                    dgInventory.Columns.Add(column);
                    Binding bindingWidth = new Binding();
                    bindingWidth.Source = column;
                    bindingWidth.Path = new PropertyPath("ActualWidth");
                    ColumnDefinition cd = new ColumnDefinition();
                    cd.SetBinding(ColumnDefinition.WidthProperty, bindingWidth);
                    gridTotal.ColumnDefinitions.Add(cd);

                    DataColumn columnBackground = new DataColumn(String.Format("Column{0}Background", sizeNoBinding), typeof(SolidColorBrush));
                    columnBackground.DefaultValue = Brushes.White;

                    DataColumn columnForeground = new DataColumn(String.Format("Column{0}Foreground", sizeNoBinding), typeof(SolidColorBrush));
                    columnForeground.DefaultValue = Brushes.Black;

                    DataColumn columnToolTip = new DataColumn(String.Format("Column{0}ToolTip", sizeNoBinding), typeof(String));

                    dt.Columns.Add(columnBackground);
                    dt.Columns.Add(columnForeground);
                    dt.Columns.Add(columnToolTip);
                }
            }));

            foreach (string productNo in productNoList)
            {
                sizeRunList = SizeRunController.Select(productNo);
                var upperComponentMaterialList_D1 = upperComponentMaterialList.Where(w => w.ProductNo == productNo).ToList();
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

                UpperComponentRawMaterialModel upperComponentRawMaterialModel = upperComponentRawMaterialList.Where(w => w.ProductNo == productNo && w.UpperComponentID == upperComponentID).FirstOrDefault();
                DateTime dtTemp = new DateTime(2000, 01, 01);
                if (upperComponentRawMaterialModel != null)
                {
                    dr["DeliveryETD"] = upperComponentRawMaterialModel.ETD;
                    dtTemp = upperComponentRawMaterialModel.ETD;
                }

                int qtyDelivery = 0;
                //int qtyMatchTotal = 0;
                foreach (string sizeNo in sizeNoList)
                {
                    string sizeNoBinding = sizeNo.Contains(".") ? sizeNo.Replace(".", "") : sizeNo;
                    int qtyPerSize = 0;
                    if (sizeRunList.Count > 0)
                    {
                        qtyPerSize = sizeRunList.Where(w => w.SizeNo == sizeNo).Select(s => s.Quantity).FirstOrDefault();
                    }

                    var upperComponentRecieve = upperComponentMaterialList.Where(w => w.ProductNo == productNo && w.SizeNo == sizeNo).FirstOrDefault();
                    int qtyRejectPerSize = 0;
                    int qtyDeliveryPerSize = 0;
                    if (upperComponentRecieve != null)
                    {
                        qtyDeliveryPerSize = upperComponentRecieve.Quantity;
                        qtyRejectPerSize = upperComponentRecieve.QuantityReject;
                    }

                    if (qtyRejectPerSize > 0)
                    {
                        dr[String.Format("Column{0}", sizeNoBinding)] = qtyRejectPerSize;
                        dr[String.Format("Column{0}Foreground", sizeNoBinding)] = Brushes.Red;
                    }

                    //int qtyDeliveryPerSize = upperComponentMaterialList.Where(w => w.ProductNo == productNo && w.SizeNo == sizeNo).Select(s => s.Quantity).FirstOrDefault();
                    int qtyBalance = qtyPerSize - qtyDeliveryPerSize;
                    if (qtyBalance > 0 && dtTemp != dtDefault)
                    {
                        dr[String.Format("Column{0}", sizeNoBinding)] = qtyBalance + qtyRejectPerSize;
                        DateTime dtNow = DateTime.Now;
                        UpperComponentMaterialModel upperComponentMaterialRecieve = upperComponentMaterialList.Where(w => w.ProductNo == productNo && w.SizeNo == sizeNo).FirstOrDefault();
                        if (upperComponentMaterialRecieve != null)
                        {
                            if (upperComponentMaterialRecieve.ModifiedTime.Date < dtNow.Date)
                            {
                                dr[String.Format("Column{0}Background", sizeNoBinding)] = Brushes.Tomato;
                            }
                            if (upperComponentMaterialRecieve.ModifiedTime.Date == dtNow.Date)
                            {
                                dr[String.Format("Column{0}Background", sizeNoBinding)] = Brushes.Green;
                            }
                        }
                        if (qtyRejectPerSize > 0)
                        {
                            dr[String.Format("Column{0}Background", sizeNoBinding)] = Brushes.Yellow;
                            dr[String.Format("Column{0}ToolTip", sizeNoBinding)] = String.Format("Balance: {0}\nReject: {1}", qtyBalance, qtyRejectPerSize);
                        }
                    }
                    qtyDelivery += qtyDeliveryPerSize;

                    // Matching
                    //if (upperComponentMaterialList_D1.Count > 0)
                    //{
                    //    int qtyMin = upperComponentMaterialList_D1.Where(o => o.SizeNo == sizeNo).Select(o => o.Quantity - o.QuantityReject).Min();
                    //    //int qtyRelease = outsoleReleaseMaterialList_D2.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity);
                    //    //int qtyMatch = qtyMin - qtyRelease;
                    //    int qtyMatch = qtyMin;
                    //    if (qtyMatch < 0)
                    //    {
                    //        qtyMatch = 0;
                    //    }
                    //    qtyMatchTotal += qtyMatch;
                    //}
                }
                int totalQty = upperComponentMaterialList.Where(w => w.ProductNo == productNo).Sum(s => s.Quantity - s.QuantityReject);
                //int totalRelease = outsoleReleaseMaterialList.Where(w => w.ProductNo == productNo).Sum(s => s.Quantity);

                if (order.Quantity != qtyDelivery && qtyDelivery != 0)
                {
                    dr["QuantityDeliveryForeground"] = Brushes.Red;
                }

                //dr["Release"] = totalRelease;
                dr["QuantityDelivery"] = qtyDelivery;
                //dr["Matching"] = qtyMatchTotal;
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
                Border bdrQuantityTotal = new Border();
                Grid.SetColumn(bdrQuantityTotal, 7);
                bdrQuantityTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrQuantityTotal.BorderBrush = Brushes.Black;
                bdrQuantityTotal.Child = lblQuantityTotal;
                gridTotal.Children.Add(bdrQuantityTotal);
                dgInventory.ItemsSource = dt.AsDataView();

                //TextBlock lblReleaseTotal = new TextBlock();
                //lblReleaseTotal.Text = dt.Compute("Sum(Release)", "").ToString();
                //lblReleaseTotal.Margin = new Thickness(1, 0, 0, 0);
                //lblReleaseTotal.FontWeight = FontWeights.Bold;
                //Border bdrReleaseTotal = new Border();
                //Grid.SetColumn(bdrReleaseTotal, 8);
                //bdrReleaseTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                //bdrReleaseTotal.BorderBrush = Brushes.Black;
                //bdrReleaseTotal.Child = lblReleaseTotal;
                //gridTotal.Children.Add(bdrReleaseTotal);
                //dgInventory.ItemsSource = dt.AsDataView();

                TextBlock lblQuantityDelivery = new TextBlock();
                lblQuantityDelivery.Text = dt.Compute("Sum(QuantityDelivery)", "").ToString();
                lblQuantityDelivery.Margin = new Thickness(1, 0, 0, 0);
                lblQuantityDelivery.FontWeight = FontWeights.Bold;
                Border bdrQuantityDeliveryTotal = new Border();
                Grid.SetColumn(bdrQuantityDeliveryTotal, 8);
                bdrQuantityDeliveryTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrQuantityDeliveryTotal.BorderBrush = Brushes.Black;
                bdrQuantityDeliveryTotal.Child = lblQuantityDelivery;
                gridTotal.Children.Add(bdrQuantityDeliveryTotal);
                dgInventory.ItemsSource = dt.AsDataView();

                //for (int i = 0; i <= outsoleSupplierList.Count - 1; i++)
                //{
                //    TextBlock lblSupplierTotal = new TextBlock();
                //    lblSupplierTotal.Text = dt.Compute(String.Format("Sum(Column{0})", i), "").ToString();
                //    lblSupplierTotal.Margin = new Thickness(1, 0, 0, 0);
                //    lblSupplierTotal.FontWeight = FontWeights.Bold;
                //    Border bdrSupplierTotal = new Border();
                //    Grid.SetColumn(bdrSupplierTotal, 7 + i);
                //    bdrSupplierTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                //    bdrSupplierTotal.BorderBrush = Brushes.Black;
                //    bdrSupplierTotal.Child = lblSupplierTotal;
                //    gridTotal.Children.Add(bdrSupplierTotal);
                //}
            }));
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }
    }
}
