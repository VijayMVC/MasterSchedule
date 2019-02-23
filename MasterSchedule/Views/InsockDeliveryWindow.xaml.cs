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
    /// Interaction logic for InsockDeliveryWindow.xaml
    /// </summary>
    public partial class InsockDeliveryWindow : Window
    {
        BackgroundWorker bwLoadData;
        List<InsockMaterialModel> insockMaterialList;
        List<OrdersModel> orderList;
        List<InsockSuppliersModel> insockSupplierList;
        List<Int32> suppIdShowInGridviewList;
        List<InsockRawMaterialModel> insockRawMaterialList;

        public InsockDeliveryWindow()
        {
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);

            insockMaterialList = new List<InsockMaterialModel>();
            orderList = new List<OrdersModel>();
            insockSupplierList = new List<InsockSuppliersModel>();
            suppIdShowInGridviewList = new List<int>();
            insockRawMaterialList = new List<InsockRawMaterialModel>();

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
            insockMaterialList = InsockMaterialController.Select();

            orderList = OrdersController.SelectByOutsoleMaterial();
            insockSupplierList = InsockSuppliersController.Select();
            insockRawMaterialList = InsockRawMaterialController.Select();

            List<String> lastCodeList = orderList.Select(o => o.LastCode).Distinct().ToList();
            foreach (string lastCode in lastCodeList)
            {
                var productNoList = orderList.Where(w => w.LastCode == lastCode).Select(s => s.ProductNo).Distinct().ToList();
                var insockMaterialList_D1 = insockMaterialList.Where(w => productNoList.Contains(w.ProductNo)).ToList();
                var supplierPerLastCode = insockMaterialList_D1.Select(s => s.InsockSupplierId).Distinct().ToList();
                suppIdShowInGridviewList.AddRange(supplierPerLastCode);
            }

            suppIdShowInGridviewList = suppIdShowInGridviewList.Distinct().OrderBy(s => s).ToList();

            DataTable dt = new DataTable();

            Dispatcher.Invoke(new Action(() => {
                dt.Columns.Add("LastCode", typeof(String));
                DataGridTextColumn columnLastCode = new DataGridTextColumn();
                columnLastCode.Header = "Last Code";
                columnLastCode.Binding = new Binding("LastCode");
                columnLastCode.FontWeight = FontWeights.Bold;
                dgInsockDelivery.Columns.Add(columnLastCode);
                Binding bindingWidth1 = new Binding();
                bindingWidth1.Source = columnLastCode;
                bindingWidth1.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd1 = new ColumnDefinition();
                cd1.SetBinding(ColumnDefinition.WidthProperty, bindingWidth1);
                gridTotal.ColumnDefinitions.Add(cd1);


                dt.Columns.Add("Quantity", typeof(Int32));
                DataGridTextColumn columnQuantity = new DataGridTextColumn();
                columnQuantity.Header = "Quantity";
                columnQuantity.Binding = new Binding("Quantity");
                dgInsockDelivery.Columns.Add(columnQuantity);
                Binding bindingWidth2 = new Binding();
                bindingWidth2.Source = columnQuantity;
                bindingWidth2.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd2 = new ColumnDefinition();
                cd2.SetBinding(ColumnDefinition.WidthProperty, bindingWidth2);
                gridTotal.ColumnDefinitions.Add(cd2);

                dt.Columns.Add("Reject", typeof(Int32));
                DataGridTextColumn column4 = new DataGridTextColumn();
                column4.Header = "Reject";
                column4.Binding = new Binding("Reject");
                dgInsockDelivery.Columns.Add(column4);
                Binding bindingWidth4 = new Binding();
                bindingWidth4.Source = column4;
                bindingWidth4.Path = new PropertyPath("ActualWidth");
                ColumnDefinition cd4 = new ColumnDefinition();
                cd4.SetBinding(ColumnDefinition.WidthProperty, bindingWidth4);
                gridTotal.ColumnDefinitions.Add(cd4);

                dgInsockDelivery.FrozenColumnCount = 3;

                for (int i = 0; i < suppIdShowInGridviewList.Count; i++)
                {
                    var insockSupplier = insockSupplierList.Where(w => w.InsockSupplierId == suppIdShowInGridviewList[i]).FirstOrDefault();
                    if (insockSupplier != null)
                    {
                        dt.Columns.Add(String.Format("Column{0}", suppIdShowInGridviewList[i]), typeof(String));
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.Header = insockSupplier.InsockSupplierName;
                        column.Binding = new Binding(String.Format("Column{0}", suppIdShowInGridviewList[i]));
                        column.FontWeight = FontWeights.Bold;

                        Style style = new Style(typeof(DataGridCell));
                        style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

                        Setter setterBackground = new Setter();
                        setterBackground.Property = DataGridCell.BackgroundProperty;
                        setterBackground.Value = new Binding(String.Format("Column{0}Background", suppIdShowInGridviewList[i]));
                        style.Setters.Add(setterBackground);

                        Setter setterToolTip = new Setter();
                        setterToolTip.Property = DataGridCell.ToolTipProperty;
                        setterToolTip.Value = new Binding(String.Format("Column{0}ToolTip", suppIdShowInGridviewList[i]));
                        style.Setters.Add(setterToolTip);

                        column.CellStyle = style;

                        dgInsockDelivery.Columns.Add(column);
                        Binding bindingWidth = new Binding();
                        bindingWidth.Source = column;
                        bindingWidth.Path = new PropertyPath("ActualWidth");
                        ColumnDefinition cd = new ColumnDefinition();
                        cd.SetBinding(ColumnDefinition.WidthProperty, bindingWidth);
                        gridTotal.ColumnDefinitions.Add(cd);

                        DataColumn columnBackground = new DataColumn(String.Format("Column{0}Background", suppIdShowInGridviewList[i]), typeof(SolidColorBrush));
                        DataColumn columnToolTip = new DataColumn(String.Format("Column{0}ToolTip", suppIdShowInGridviewList[i]), typeof(String));
                        columnBackground.DefaultValue = Brushes.White;
                        dt.Columns.Add(columnBackground);
                        dt.Columns.Add(columnToolTip);
                    }
                }
            }));

            foreach (var lastCode in lastCodeList)
            {
                DataRow dr = dt.NewRow();
                dr["LastCode"] = lastCode;

                var productNoList = orderList.Where(w => w.LastCode == lastCode).Select(s => s.ProductNo).Distinct().ToList();
                var insockMaterialList_D1 = insockMaterialList.Where(w => productNoList.Contains(w.ProductNo)).ToList();
                var insockSupplierIdList = insockMaterialList_D1.Select(s => s.InsockSupplierId).Distinct().ToList();
                var qtyTotalList = new List<Int32>();
                var qtyRejectList = new List<Int32>();

                foreach (var insockSupplierId in insockSupplierIdList)
                {
                    var outsoleMaterialList_D2 = insockMaterialList_D1.Where(o => o.InsockSupplierId == insockSupplierId).ToList();
                    int qtyTotal = 0;
                    int qtyRejectTotal = 0;
                    foreach (string productNo in productNoList)
                    {
                        var insockMaterialList_D3 = outsoleMaterialList_D2.Where(w => w.ProductNo == productNo).ToList();
                        var sizeNoList = insockMaterialList.Where(w => w.ProductNo == productNo).Select(s => s.SizeNo).Distinct().ToList();
                        foreach (var sizeNo in sizeNoList)
                        {
                            int qtyMax = insockMaterialList_D3.Where(w => w.SizeNo == sizeNo).Sum(s => s.Quantity - s.QuantityReject);
                            int qtyReject = insockMaterialList_D3.Where(w => w.SizeNo == sizeNo).Sum(s => s.QuantityReject);

                            qtyTotal += qtyMax;
                            qtyRejectTotal += qtyReject;
                        }
                    }
                    dr[String.Format("Column{0}", insockSupplierId)] = "x";
                    dr[String.Format("Column{0}Background", insockSupplierId)] = Brushes.Green;
                    var suppName = insockSupplierList.Where(w => w.InsockSupplierId == insockSupplierId).Select(s => s.InsockSupplierName).FirstOrDefault();
                    if (suppName != null)
                    {
                        dr[String.Format("Column{0}ToolTip", insockSupplierId)] = string.Format("{0}{1}", suppName, qtyRejectTotal > 0 ? String.Format("\nQtyReject: {0}", qtyRejectTotal.ToString()) : "");
                    }
                    qtyTotalList.Add(qtyTotal);
                    qtyRejectList.Add(qtyRejectTotal);
                }

                if (qtyRejectList.Count > 0)
                    dr["Reject"] = qtyRejectList.Max();
                if (qtyTotalList.Count > 0)
                    dr["Quantity"] = qtyTotalList.Max();
                dt.Rows.Add(dr);
            }

            Dispatcher.Invoke(new Action(() =>
            {
                dgInsockDelivery.ItemsSource = dt.AsDataView();
                // Row Tail
                TextBlock lblTotal = new TextBlock();
                lblTotal.Text = "TOTAL";
                lblTotal.Margin = new Thickness(1, 0, 0, 0);
                lblTotal.FontWeight = FontWeights.Bold;
                Border bdrTotal = new Border();
                Grid.SetColumn(bdrTotal, 2);
                Grid.SetColumnSpan(bdrTotal, 1);
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
                dgInsockDelivery.ItemsSource = dt.AsDataView();


                TextBlock lblRejectMaximunTotal = new TextBlock();
                lblRejectMaximunTotal.Text = dt.Compute("Sum(Reject)", "").ToString();
                lblRejectMaximunTotal.Margin = new Thickness(1, 0, 0, 0);
                lblRejectMaximunTotal.FontWeight = FontWeights.Bold;
                Border bdrRejectMaximumTotal = new Border();
                Grid.SetColumn(bdrRejectMaximumTotal, 4);
                bdrRejectMaximumTotal.BorderThickness = new Thickness(0, 0, 1, 1);
                bdrRejectMaximumTotal.BorderBrush = Brushes.Black;
                bdrRejectMaximumTotal.Child = lblRejectMaximunTotal;
                gridTotal.Children.Add(bdrRejectMaximumTotal);
                dgInsockDelivery.ItemsSource = dt.AsDataView();
            }));

        }
        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }

        private void dgInsockDelivery_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataRowView dataRowClick = dgInsockDelivery.CurrentItem as DataRowView;
            var dataColumnClick = dgInsockDelivery.CurrentColumn;

            if (dataRowClick == null || dataColumnClick == null)
            {
                return;
            }

            int supplierIndex = -1;
            if (dataColumnClick.DisplayIndex > 2)
            {
                if (dataRowClick.Row.ItemArray[(dataColumnClick.DisplayIndex - 2) * 3].ToString() == "x")
                {
                    supplierIndex = dataColumnClick.DisplayIndex - 3;
                }
            }
            if (supplierIndex != -1)
            {
                var lastCodeTranfer = dataRowClick.Row.ItemArray[0].ToString();
                var supplierIdTranfer = suppIdShowInGridviewList[supplierIndex];
                var orderListTranfer = orderList.Where(w => w.LastCode == lastCodeTranfer).ToList();
                var productNoList = orderListTranfer.Select(s => s.ProductNo).Distinct().ToList();

                var insockMaterialListTranfer = insockMaterialList.Where(w => productNoList.Contains(w.ProductNo) && w.InsockSupplierId == supplierIdTranfer).ToList();

                InsockDeliveryDetailWindow window = new InsockDeliveryDetailWindow(
                    orderListTranfer,
                    insockMaterialListTranfer,
                    insockRawMaterialList,
                    lastCodeTranfer,
                    supplierIdTranfer);
                window.Title = String.Format("{0} for {1} - {2}", window.Title, lastCodeTranfer, insockSupplierList.Where(w => w.InsockSupplierId == supplierIdTranfer).Select(s => s.InsockSupplierName).FirstOrDefault());
                window.Show();
                window.Focus();
            }
        }
    }
}
