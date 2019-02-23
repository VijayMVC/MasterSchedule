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

using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Data;


namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHInventoryWindow.xaml
    /// </summary>

    public partial class OutsoleWHDeliveryWindow : Window
    {
        BackgroundWorker bwLoadData;
        List<OutsoleMaterialModel> outsoleMaterialList;
        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList;
        List<OrdersModel> orderList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<Int32> supplierIdShowList;
        List<OutsoleRawMaterialModel> outsoleRawMaterialList;

        public OutsoleWHDeliveryWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            outsoleMaterialList = new List<OutsoleMaterialModel>();
            outsoleReleaseMaterialList = new List<OutsoleReleaseMaterialModel>();
            orderList = new List<OrdersModel>();
            outsoleSupplierList = new List<OutsoleSuppliersModel>();
            supplierIdShowList = new List<Int32>();
            outsoleRawMaterialList = new List<OutsoleRawMaterialModel>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMaterialList = OutsoleMaterialController.Select();
            outsoleReleaseMaterialList = OutsoleReleaseMaterialController.SelectByOutsoleMaterial();
            //lay ra nhung order co outsolematerial roi

            orderList = OrdersController.SelectByOutsoleMaterial();
            outsoleSupplierList = OutsoleSuppliersController.Select();
            outsoleRawMaterialList = OutsoleRawMaterialController.Select();

            var outsoleCodeList = orderList.Select(o => o.OutsoleCode).Distinct().ToList();

            // Group By ProductNo
            //var outsoleMaterialGroupBy_ProductNo = from p in outsoleMaterialList
            //                                       group p.Quantity by p.ProductNo into g
            //                                       select new { ProductNo = g.Key, Quantities = g.ToList() };

            //var productNoList_Test = outsoleMaterialGroupBy_ProductNo.OrderBy(o => o.ProductNo).Select(s => s.ProductNo).ToList();

            //// PO has Quantity > 0
            //var productNoList = outsoleMaterialGroupBy_ProductNo.Where(w => w.Quantities.Sum() > 0).OrderBy(o => o.ProductNo).Select(s => s.ProductNo).Distinct().ToList();
            //var outsoleCodeList = orderList.Where(w => productNoList.Contains(w.ProductNo)).Select(s => s.OutsoleCode).Distinct().ToList();

            foreach (string outsoleCode in outsoleCodeList)
            {
                var productNoList = orderList.Where(w => w.OutsoleCode == outsoleCode).Select(s => s.ProductNo).Distinct().ToList();
                var outsoleMaterialList_D1 = outsoleMaterialList.Where(o => productNoList.Contains(o.ProductNo)).ToList();
                var supplierId_OutsoleCode = outsoleMaterialList_D1.Select(o => o.OutsoleSupplierId).Distinct().ToList();
                supplierIdShowList.AddRange(supplierId_OutsoleCode);
            }

            supplierIdShowList = supplierIdShowList.Distinct().OrderBy(s => s).ToList();

            var dt = new DataTable();

            Dispatcher.Invoke(new Action(() => {
                dt.Columns.Add("OutsoleCode", typeof(String));
                DataGridTextColumn column1 = new DataGridTextColumn();
                column1.Header = "O/S Code";
                column1.Binding = new Binding("OutsoleCode");
                column1.FontWeight = FontWeights.Bold;
                dgInventory.Columns.Add(column1);
                Binding bindingWidth1 = new Binding();
                bindingWidth1.Source = column1;
                bindingWidth1.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1 = new ColumnDefinition();
                cd1.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1);
                gridTotal.ColumnDefinitions.Add(cd1);

                dt.Columns.Add("Quantity", typeof(Int32));
                DataGridTextColumn column2 = new DataGridTextColumn();
                column2.Header = "Quantity";
                column2.Binding = new Binding("Quantity");
                dgInventory.Columns.Add(column2);
                Binding bindingWidth2 = new Binding();
                bindingWidth2.Source = column2;
                bindingWidth2.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd2 = new ColumnDefinition();
                cd2.SetBinding(ColumnDefinition.WidthProperty, bindingWidth2);
                gridTotal.ColumnDefinitions.Add(cd2);

                dt.Columns.Add("Matching", typeof(Int32));
                DataGridTextColumn column3 = new DataGridTextColumn();
                column3.Header = "Matching";
                column3.Binding = new Binding("Matching");
                dgInventory.Columns.Add(column3);
                Binding bindingWidth3 = new Binding();
                bindingWidth3.Source = column3;
                bindingWidth3.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd3 = new ColumnDefinition();
                cd3.SetBinding(ColumnDefinition.WidthProperty, bindingWidth3);
                gridTotal.ColumnDefinitions.Add(cd3);

                dt.Columns.Add("RejectMaximum", typeof(Int32));
                DataGridTextColumn column4 = new DataGridTextColumn();
                column4.Header = "Reject";
                column4.Binding = new Binding("RejectMaximum");
                dgInventory.Columns.Add(column4);

                Binding bindingWidth4 = new Binding();
                bindingWidth4.Source = column4;
                bindingWidth4.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd4 = new ColumnDefinition();
                cd4.SetBinding(ColumnDefinition.WidthProperty, bindingWidth4);
                gridTotal.ColumnDefinitions.Add(cd4);

                dgInventory.FrozenColumnCount = 4;

                for (int i = 0; i < supplierIdShowList.Count; i++)
                {
                    var outsoleSupplier = outsoleSupplierList.Where(w => w.OutsoleSupplierId == supplierIdShowList[i]).FirstOrDefault();
                    if (outsoleSupplier != null)
                    {
                        dt.Columns.Add(String.Format("Column{0}", supplierIdShowList[i]), typeof(String));
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.Width = 30;
                        column.Binding = new Binding(String.Format("Column{0}", supplierIdShowList[i]));
                        column.FontWeight = FontWeights.Bold;

                        Style style = new Style(typeof(DataGridCell));
                        style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

                        Setter setterBackground = new Setter();
                        setterBackground.Property = DataGridCell.BackgroundProperty;
                        setterBackground.Value = new Binding(String.Format("Column{0}Background", supplierIdShowList[i]));
                        style.Setters.Add(setterBackground);

                        Setter setterToolTip = new Setter();
                        setterToolTip.Property = DataGridCell.ToolTipProperty;
                        setterToolTip.Value = new Binding(String.Format("Column{0}ToolTip", supplierIdShowList[i]));
                        style.Setters.Add(setterToolTip);

                        column.CellStyle = style;

                        dgInventory.Columns.Add(column);
                        Binding bindingWidth = new Binding();
                        bindingWidth.Source = column;
                        bindingWidth.Path = new PropertyPath("ActualWidth");
                        ColumnDefinition cd = new ColumnDefinition();
                        cd.SetBinding(ColumnDefinition.WidthProperty, bindingWidth);
                        gridTotal.ColumnDefinitions.Add(cd);

                        DataColumn columnBackground = new DataColumn(String.Format("Column{0}Background", supplierIdShowList[i]), typeof(SolidColorBrush));
                        DataColumn columnToolTip = new DataColumn(String.Format("Column{0}ToolTip", supplierIdShowList[i]), typeof(String));
                        columnBackground.DefaultValue = Brushes.Transparent;

                        dt.Columns.Add(columnBackground);
                        dt.Columns.Add(columnToolTip);
                    }
                }
            }));

            // Load Data
            foreach (string outsoleCode in outsoleCodeList)
            {
                var dr = dt.NewRow();

                dr["OutsoleCode"] = outsoleCode;
                var productNoList_OutsoleCode = orderList.Where(o => o.OutsoleCode == outsoleCode).Select(o => o.ProductNo).Distinct().ToList();
                var outsoleMaterialList_D1 = outsoleMaterialList.Where(o => productNoList_OutsoleCode.Contains(o.ProductNo)).ToList();
                var outsoleReleaseMaterialList_D1 = outsoleReleaseMaterialList.Where(o => productNoList_OutsoleCode.Contains(o.ProductNo)).ToList();
                var supplierIdList = outsoleMaterialList_D1.Select(o => o.OutsoleSupplierId).Distinct().ToList();
                var qtyTotalList = new List<Int32>();
                var qtyRejectList = new List<Int32>();

                foreach (Int32 supplierId in supplierIdList)
                {
                    var outsoleMaterialList_D2 = outsoleMaterialList_D1.Where(o => o.OutsoleSupplierId == supplierId).ToList();
                    int qtyTotal = 0;
                    int qtyRejectTotal = 0;
                    foreach (string productNo in productNoList_OutsoleCode)
                    {
                        var outsoleMaterialList_D3 = outsoleMaterialList_D2.Where(o => o.ProductNo == productNo).ToList();
                        var outsoleReleaseMaterialList_D3 = outsoleReleaseMaterialList_D1.Where(o => o.ProductNo == productNo).ToList();
                        var sizeNoList = outsoleMaterialList.Where(o => o.ProductNo == productNo).Select(o => o.SizeNo).Distinct().ToList();
                        foreach (string sizeNo in sizeNoList)
                        {
                            int qtyMax = outsoleMaterialList_D3.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity - o.QuantityReject);
                            int qtyRelease = outsoleReleaseMaterialList_D3.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity);
                            int qtyReject = outsoleMaterialList_D3.Where(o => o.SizeNo == sizeNo).Sum(s => s.QuantityReject);
                            int qty = qtyMax - qtyRelease;
                            if (qty < 0)
                            {
                                qty = 0;
                            }
                            qtyTotal += qty;
                            qtyRejectTotal += qtyReject;
                        }
                    }
                    dr[String.Format("Column{0}", supplierId)] = "x";
                    dr[String.Format("Column{0}Background", supplierId)] = Brushes.Green;
                    var suppName = outsoleSupplierList.Where(w => w.OutsoleSupplierId == supplierId).Select(s => s.Name).FirstOrDefault();
                    if (suppName != null)
                    {
                        dr[String.Format("Column{0}ToolTip", supplierId)] =
                            string.Format("{0}{1}", suppName, qtyRejectTotal > 0 ? String.Format("\nQtyReject: {0}", qtyRejectTotal.ToString()) : "");
                        //suppName + "/n" + "Qty Reject: " + qtyRejectTotal.ToString();
                    }
                    qtyTotalList.Add(qtyTotal);
                    qtyRejectList.Add(qtyRejectTotal);
                }

                int qtyMatchTotal = 0;
                foreach (string productNo in productNoList_OutsoleCode)
                {
                    var outsoleMaterialList_D2 = outsoleMaterialList_D1.Where(o => o.ProductNo == productNo).ToList();
                    var outsoleReleaseMaterialList_D2 = outsoleReleaseMaterialList_D1.Where(o => o.ProductNo == productNo).ToList();
                    var sizeNoList = outsoleMaterialList.Where(o => o.ProductNo == productNo).Select(o => o.SizeNo).Distinct().ToList();
                    foreach (string sizeNo in sizeNoList)
                    {
                        int qtyMin = outsoleMaterialList_D2.Where(o => o.SizeNo == sizeNo).Select(o => o.Quantity - o.QuantityReject).Min();
                        int qtyRelease = outsoleReleaseMaterialList_D2.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity);
                        int qtyMatch = qtyMin - qtyRelease;
                        if (qtyMatch < 0)
                        {
                            qtyMatch = 0;
                        }
                        qtyMatchTotal += qtyMatch;
                    }
                }

                if (qtyTotalList.Sum() == 0 && qtyMatchTotal == 0 && qtyRejectList.Sum() == 0)
                    continue;

                dr["RejectMaximum"] = qtyRejectList.Max();
                dr["Quantity"] = qtyTotalList.Max();
                dr["Matching"] = qtyMatchTotal;

                dt.Rows.Add(dr);
            }

            Dispatcher.Invoke(new Action(() => {
                dgInventory.ItemsSource = dt.AsDataView();

                TextBlock lblTotal = new TextBlock();
                lblTotal.Text = "TOTAL";
                lblTotal.Margin = new Thickness(1, 0, 0, 0);
                lblTotal.FontWeight = FontWeights.Bold;
                Border bdrTotal = new Border();
                Grid.SetColumn(bdrTotal, 1);
                Grid.SetColumnSpan(bdrTotal, 2);
                bdrTotal.BorderThickness = new Thickness(1, 0, 1, 1);
                bdrTotal.BorderBrush = Brushes.Black;
                bdrTotal.Child = lblTotal;
                gridTotal.Children.Add(bdrTotal);

                TextBlock lblQuantityTotal = new TextBlock();
                lblQuantityTotal.Text = dt.Compute("Sum(Quantity)", "").ToString();
                lblQuantityTotal.Margin = new Thickness(1, 0, 0, 0);
                lblQuantityTotal.FontWeight = FontWeights.Bold;
                Border bdrQuantityTotal = new Border();
                Grid.SetColumn(bdrQuantityTotal, 3);
                bdrQuantityTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrQuantityTotal.BorderBrush = Brushes.Black;
                bdrQuantityTotal.Child = lblQuantityTotal;
                gridTotal.Children.Add(bdrQuantityTotal);
                dgInventory.ItemsSource = dt.AsDataView();

                TextBlock lblMatchingTotal = new TextBlock();
                lblMatchingTotal.Text = dt.Compute("Sum(Matching)", "").ToString();
                lblMatchingTotal.Margin = new Thickness(1, 0, 0, 0);
                lblMatchingTotal.FontWeight = FontWeights.Bold;
                Border bdrMatchingTotal = new Border();
                Grid.SetColumn(bdrMatchingTotal, 4);
                bdrMatchingTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrMatchingTotal.BorderBrush = Brushes.Black;
                bdrMatchingTotal.Child = lblMatchingTotal;
                gridTotal.Children.Add(bdrMatchingTotal);
                dgInventory.ItemsSource = dt.AsDataView();

                TextBlock lblRejectMaximunTotal = new TextBlock();
                lblRejectMaximunTotal.Text = dt.Compute("Sum(RejectMaximum)", "").ToString();
                lblRejectMaximunTotal.Margin = new Thickness(1, 0, 0, 0);
                lblRejectMaximunTotal.FontWeight = FontWeights.Bold;
                Border bdrRejectMaximumTotal = new Border();
                Grid.SetColumn(bdrRejectMaximumTotal, 5);
                bdrRejectMaximumTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrRejectMaximumTotal.BorderBrush = Brushes.Black;
                bdrRejectMaximumTotal.Child = lblRejectMaximunTotal;
                gridTotal.Children.Add(bdrRejectMaximumTotal);
                dgInventory.ItemsSource = dt.AsDataView();
            }));
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }

        private void dgInventory_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataRowView dataRowClick = dgInventory.CurrentItem as DataRowView;
            var dataColumnClick = dgInventory.CurrentColumn;

            if (dataRowClick == null || dataColumnClick == null)
            {
                return;
            }

            int suppIndex = -1;
            if (dataColumnClick.DisplayIndex > 3)
            {
                if (dataRowClick.Row.ItemArray[(dataColumnClick.DisplayIndex - 3) * 3 + 1].ToString() == "x")
                {
                    suppIndex = dataColumnClick.DisplayIndex - 4;
                }
            }

            if (suppIndex != -1)
            {
                string outsoleCodeTranfer = dataRowClick.Row.ItemArray[0].ToString();
                int supplierIdTranfer = supplierIdShowList[suppIndex];
                var orderListTranfer = orderList.Where(w => w.OutsoleCode == outsoleCodeTranfer).ToList();
                var productNoList = orderListTranfer.Select(s => s.ProductNo).Distinct().ToList();
                var outsoleMaterialListTranfer = outsoleMaterialList.Where(w => productNoList.Contains(w.ProductNo) && w.OutsoleSupplierId == supplierIdTranfer).ToList();
                var productNoListTranfer = outsoleMaterialListTranfer.Select(s => s.ProductNo).Distinct().ToList();
                var outsoleReleaseMaterialListTranfer = outsoleReleaseMaterialList.Where(w => productNoList.Contains(w.ProductNo)).ToList();
                var window = new OutsoleWHDeliveryDetailWindow( orderListTranfer,
                                                                outsoleReleaseMaterialListTranfer,
                                                                productNoListTranfer,
                                                                outsoleRawMaterialList,
                                                                outsoleCodeTranfer,
                                                                supplierIdTranfer);
                window.Title = String.Format("{0} for {1} - {2}", window.Title, outsoleCodeTranfer, outsoleSupplierList.Where(w => w.OutsoleSupplierId == supplierIdTranfer).Select(s => s.Name).FirstOrDefault());
                window.Show();
                window.Focus();
            }
        }
    }
}

