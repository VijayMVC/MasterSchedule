using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Data;
using System.ComponentModel;
using System.Windows.Input;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHInventoryWindow.xaml
    /// </summary>
    public partial class OutsoleWHInventoryDetailWindow : Window
    {
        List<String> productNoList;
        List<OrdersModel> orderList;
        List<OutsoleMaterialModel> outsoleMaterialList;

        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList;
        List<OutsoleSuppliersModel> outsoleSupplierList;

        BackgroundWorker bwLoad;

        public OutsoleWHInventoryDetailWindow(List<String> productNoList, List<OrdersModel> orderList, List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList, List<OutsoleSuppliersModel> outsoleSupplierList)
        {
            this.productNoList = productNoList;
            this.orderList = orderList;
            //this.outsoleMaterialList = outsoleMaterialList;
            this.outsoleReleaseMaterialList = outsoleReleaseMaterialList;
            this.outsoleSupplierList = outsoleSupplierList;

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
            outsoleMaterialList = OutsoleMaterialController.Select().Where(w => productNoList.Contains(w.ProductNo)).ToList();

            Dispatcher.Invoke(new Action(() => {
                DataTable dt = new DataTable();
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

                dt.Columns.Add("ETD", typeof(DateTime));
                DataGridTextColumn column1_1 = new DataGridTextColumn();
                column1_1.Header = "EFD";
                Binding binding = new Binding();
                binding.Path = new PropertyPath("ETD");
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
                //binding.StringFormat = "dd-MMM";
                column1_3.Binding = bindingRelease;
                //column1_2.FontWeight = FontWeights.Bold;
                dgInventory.Columns.Add(column1_3);
                Binding bindingWidth1_3 = new Binding();
                bindingWidth1_3.Source = column1_3;
                bindingWidth1_3.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1_3 = new ColumnDefinition();
                cd1_3.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1_3);
                gridTotal.ColumnDefinitions.Add(cd1_3);

                for (int i = 0; i <= outsoleSupplierList.Count - 1; i++)
                {
                    OutsoleSuppliersModel outsoleSupplier = outsoleSupplierList[i];
                    dt.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                    DataGridTextColumn column = new DataGridTextColumn();
                    //column.SetValue(TagProperty, sizeRun.SizeNo);
                    column.Header = outsoleSupplier.Name;
                    column.Binding = new Binding(String.Format("Column{0}", i));

                    Style style = new Style(typeof(DataGridCell));
                    style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

                    Setter setterForeground = new Setter();
                    setterForeground.Property = DataGridCell.ForegroundProperty;
                    setterForeground.Value = Brushes.Black;
                    style.Setters.Add(setterForeground);

                    Setter setterBackground = new Setter();
                    setterBackground.Property = DataGridCell.BackgroundProperty;
                    setterBackground.Value = new Binding(String.Format("Column{0}Background", i));
                    style.Setters.Add(setterBackground);

                    column.CellStyle = style;

                    dgInventory.Columns.Add(column);

                    Binding bindingWidth = new Binding();
                    bindingWidth.Source = column;
                    bindingWidth.Path = new PropertyPath("ActualWidth");
                    ColumnDefinition cd = new ColumnDefinition();
                    cd.SetBinding(ColumnDefinition.WidthProperty, bindingWidth);
                    gridTotal.ColumnDefinitions.Add(cd);

                    DataColumn columnBackground = new DataColumn(String.Format("Column{0}Background", i), typeof(SolidColorBrush));
                    columnBackground.DefaultValue = Brushes.Transparent;

                    dt.Columns.Add(columnBackground);
                }

                dt.Columns.Add("Matching", typeof(Int32));
                DataGridTextColumn column2 = new DataGridTextColumn();
                column2.Header = "Matching";
                column2.Binding = new Binding("Matching");
                dgInventory.Columns.Add(column2);
                Binding bindingWidth2 = new Binding();
                bindingWidth2.Source = column2;
                bindingWidth2.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd2 = new ColumnDefinition();
                cd2.SetBinding(ColumnDefinition.WidthProperty, bindingWidth2);
                gridTotal.ColumnDefinitions.Add(cd2);

                foreach (string productNo in productNoList)
                {
                    var outsoleMaterialDetailPerPOList = OutsoleMaterialDetailController.Select(productNo).ToList();

                    if (productNo == "105-5900")
                    { }
                    List<OutsoleMaterialModel> outsoleMaterialList_D1 = outsoleMaterialList.Where(o => o.ProductNo == productNo).ToList();
                    List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList_D1 = outsoleReleaseMaterialList.Where(o => o.ProductNo == productNo).ToList();

                    DataRow dr = dt.NewRow();
                    dr["ProductNo"] = productNo;
                    OrdersModel order = orderList.Where(o => o.ProductNo == productNo).FirstOrDefault();

                    if (order != null)
                    {
                        dr["ETD"] = order.ETD;
                        dr["ArticleNo"] = order.ArticleNo;
                        dr["Quantity"] = order.Quantity;
                    }

                    List<String> sizeNoList = outsoleMaterialList.Where(o => o.ProductNo == productNo).Select(o => o.SizeNo).Distinct().ToList();
                    int qtyMaterialTotalToCheck = 0;

                    for (int i = 0; i <= outsoleSupplierList.Count - 1; i++)
                    {
                        OutsoleSuppliersModel outsoleSupplier = outsoleSupplierList[i];
                        List<OutsoleMaterialModel> outsoleMaterialList_D2 = outsoleMaterialList_D1.Where(o => o.OutsoleSupplierId == outsoleSupplier.OutsoleSupplierId).ToList();

                        int qtyMaterialTotal = 0;
                        int qtyReleaseTotal = 0;
                        foreach (string sizeNo in sizeNoList)
                        {
                            int qtyMax = outsoleMaterialList_D2.Where(o => o.SizeNo == sizeNo).Sum(o => (o.Quantity - o.QuantityReject));
                            int qtyRelease = outsoleReleaseMaterialList_D1.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity);

                            int qtyMaterial = qtyMax - qtyRelease;
                            if (qtyMaterial < 0)
                            {
                                qtyMaterial = 0;
                            }
                            qtyMaterialTotal += qtyMaterial;
                            qtyMaterialTotalToCheck += qtyMaterial;
                            qtyReleaseTotal += qtyRelease;
                        }
                        dr["Release"] = qtyReleaseTotal;
                        dr[String.Format("Column{0}", i)] = qtyMaterialTotal;

                        var outsoleMaterialDetailPerPOPerSupplierList = outsoleMaterialDetailPerPOList.Where(w => w.OutsoleSupplierId == outsoleSupplierList[i].OutsoleSupplierId).ToList();
                        if (outsoleMaterialDetailPerPOPerSupplierList.Count > 0)
                        {
                            int qtyMaterialDetail = outsoleMaterialDetailPerPOPerSupplierList.Sum(s => s.Quantity);
                            if (qtyMaterialDetail != 0 && qtyMaterialDetail < order.Quantity)
                                dr[String.Format("Column{0}Background", i)] = Brushes.Yellow;
                            if (qtyMaterialDetail != 0 && qtyMaterialDetail >= order.Quantity)
                                dr[String.Format("Column{0}Background", i)] = Brushes.Green;
                        }
                    }
                    int qtyMatchingTotal = 0;
                    foreach (string sizeNo in sizeNoList)
                    {
                        int qtyMin = outsoleMaterialList_D1.Where(o => o.SizeNo == sizeNo).Select(o => (o.Quantity - o.QuantityReject)).Min();
                        int qtyRelease = outsoleReleaseMaterialList_D1.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity);
                        int qtyMatching = qtyMin - qtyRelease;
                        if (qtyMatching < 0)
                        {
                            qtyMatching = 0;
                        }
                        qtyMatchingTotal += qtyMatching;
                    }
                    dr["Matching"] = qtyMatchingTotal;
                    if (qtyMaterialTotalToCheck != 0)
                    {
                        dt.Rows.Add(dr);
                    }
                }

                TextBlock lblTotal = new TextBlock();
                lblTotal.Text = "TOTAL";
                lblTotal.Margin = new Thickness(1, 0, 0, 0);
                lblTotal.FontWeight = FontWeights.Bold;
                Border bdrTotal = new Border();
                Grid.SetColumn(bdrTotal, 2);
                Grid.SetColumnSpan(bdrTotal, 3);
                bdrTotal.BorderThickness = new Thickness(1, 0, 1, 1);
                bdrTotal.BorderBrush = Brushes.Black;
                bdrTotal.Child = lblTotal;
                gridTotal.Children.Add(bdrTotal);

                TextBlock lblQuantityTotal = new TextBlock();
                lblQuantityTotal.Text = dt.Compute("Sum(Quantity)", "").ToString();
                lblQuantityTotal.Margin = new Thickness(1, 0, 0, 0);
                lblQuantityTotal.FontWeight = FontWeights.Bold;
                Border bdrQuantityTotal = new Border();
                Grid.SetColumn(bdrQuantityTotal, 5);
                bdrQuantityTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrQuantityTotal.BorderBrush = Brushes.Black;
                bdrQuantityTotal.Child = lblQuantityTotal;
                gridTotal.Children.Add(bdrQuantityTotal);
                dgInventory.ItemsSource = dt.AsDataView();

                TextBlock lblReleaseTotal = new TextBlock();
                lblReleaseTotal.Text = dt.Compute("Sum(Release)", "").ToString();
                lblReleaseTotal.Margin = new Thickness(1, 0, 0, 0);
                lblReleaseTotal.FontWeight = FontWeights.Bold;
                Border bdrReleaseTotal = new Border();
                Grid.SetColumn(bdrReleaseTotal, 6);
                bdrReleaseTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrReleaseTotal.BorderBrush = Brushes.Black;
                bdrReleaseTotal.Child = lblReleaseTotal;
                gridTotal.Children.Add(bdrReleaseTotal);
                dgInventory.ItemsSource = dt.AsDataView();

                for (int i = 0; i <= outsoleSupplierList.Count - 1; i++)
                {
                    TextBlock lblSupplierTotal = new TextBlock();
                    lblSupplierTotal.Text = dt.Compute(String.Format("Sum(Column{0})", i), "").ToString();
                    lblSupplierTotal.Margin = new Thickness(1, 0, 0, 0);
                    lblSupplierTotal.FontWeight = FontWeights.Bold;
                    Border bdrSupplierTotal = new Border();
                    Grid.SetColumn(bdrSupplierTotal, 7 + i);
                    bdrSupplierTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                    bdrSupplierTotal.BorderBrush = Brushes.Black;
                    bdrSupplierTotal.Child = lblSupplierTotal;
                    gridTotal.Children.Add(bdrSupplierTotal);
                }

                TextBlock lblMatchingTotal = new TextBlock();
                lblMatchingTotal.Text = dt.Compute("Sum(Matching)", "").ToString();
                lblMatchingTotal.Margin = new Thickness(1, 0, 0, 0);
                lblMatchingTotal.FontWeight = FontWeights.Bold;
                Border bdrMatchingTotal = new Border();
                Grid.SetColumn(bdrMatchingTotal, 7 + outsoleSupplierList.Count());
                bdrMatchingTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrMatchingTotal.BorderBrush = Brushes.Black;
                bdrMatchingTotal.Child = lblMatchingTotal;
                gridTotal.Children.Add(bdrMatchingTotal);
                dgInventory.ItemsSource = dt.AsDataView();
            }));
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }
    }
}
