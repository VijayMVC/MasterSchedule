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
    /// Interaction logic for UpperComponentDeliveryWindow.xaml
    /// </summary>
    public partial class UpperComponentWHDeliveryWindow : Window
    {
        BackgroundWorker bwLoad;
        List<OrdersModel> orderList;
        List<UpperComponentModel> upperComponentList;
        List<UpperComponentMaterialModel> upperComponentMaterialList;
        List<UpperComponentRawMaterialModel> upperComponentRawMaterialList;
        List<Int32> upperComponentIDShowInGridviewList;

        public UpperComponentWHDeliveryWindow()
        {
            orderList = new List<OrdersModel>();
            upperComponentList = new List<UpperComponentModel>();
            upperComponentMaterialList = new List<UpperComponentMaterialModel>();
            upperComponentRawMaterialList = new List<UpperComponentRawMaterialModel>();
            upperComponentIDShowInGridviewList = new List<Int32>();

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
            upperComponentMaterialList = UpperComponentMaterialController.Select();
            //outsoleReleaseMaterialList = OutsoleReleaseMaterialController.SelectByOutsoleMaterial();
            // lay ra nhung order co outsolematerial roi.
            orderList = OrdersController.SelectByOutsoleMaterial();
            upperComponentList = UpperComponentController.Select();
            upperComponentRawMaterialList = UpperComponentRawMaterialController.Select();


            List<String> outsoleCodeList = orderList.Select(o => o.OutsoleCode).Distinct().ToList();
            foreach (string outsoleCode in outsoleCodeList)
            {
                List<String> productNoList = orderList.Where(o => o.OutsoleCode == outsoleCode).Select(o => o.ProductNo).Distinct().ToList();
                List<UpperComponentMaterialModel> upperComponentMaterialList_D1 = upperComponentMaterialList.Where(o => productNoList.Contains(o.ProductNo)).ToList();
                List<Int32> upperComponentPerOutsole = upperComponentMaterialList_D1.Select(o => o.UpperComponentID).Distinct().ToList();
                upperComponentIDShowInGridviewList.AddRange(upperComponentPerOutsole);
            }
            upperComponentIDShowInGridviewList = upperComponentIDShowInGridviewList.Distinct().OrderBy(s => s).ToList();

            DataTable dt = new DataTable();

            Dispatcher.Invoke(new Action(() =>
            {
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

                for (int i = 0; i < upperComponentIDShowInGridviewList.Count; i++)
                {
                    UpperComponentModel upperComponent = upperComponentList.Where(w => w.UpperComponentID == upperComponentIDShowInGridviewList[i]).FirstOrDefault();
                    if (upperComponent != null)
                    {
                        dt.Columns.Add(String.Format("Column{0}", upperComponentIDShowInGridviewList[i]), typeof(String));
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.Width = 30;
                        column.Binding = new Binding(String.Format("Column{0}", upperComponentIDShowInGridviewList[i]));
                        column.FontWeight = FontWeights.Bold;

                        Style style = new Style(typeof(DataGridCell));
                        style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                        Setter setterBackground = new Setter();
                        setterBackground.Property = DataGridCell.BackgroundProperty;
                        setterBackground.Value = new Binding(String.Format("Column{0}Background", upperComponentIDShowInGridviewList[i]));
                        style.Setters.Add(setterBackground);

                        Setter setterToolTip = new Setter();
                        setterToolTip.Property = DataGridCell.ToolTipProperty;
                        setterToolTip.Value = new Binding(String.Format("Column{0}ToolTip", upperComponentIDShowInGridviewList[i]));
                        style.Setters.Add(setterToolTip);

                        column.CellStyle = style;

                        dgInventory.Columns.Add(column);
                        Binding bindingWidth = new Binding();
                        bindingWidth.Source = column;
                        bindingWidth.Path = new PropertyPath("ActualWidth");
                        ColumnDefinition cd = new ColumnDefinition();
                        cd.SetBinding(ColumnDefinition.WidthProperty, bindingWidth);
                        gridTotal.ColumnDefinitions.Add(cd);

                        DataColumn columnBackground = new DataColumn(String.Format("Column{0}Background", upperComponentIDShowInGridviewList[i]), typeof(SolidColorBrush));
                        DataColumn columnToolTip = new DataColumn(String.Format("Column{0}ToolTip", upperComponentIDShowInGridviewList[i]), typeof(String));
                        columnBackground.DefaultValue = Brushes.White;
                        dt.Columns.Add(columnBackground);
                        dt.Columns.Add(columnToolTip);
                    }
                }
            }));


            foreach (string outsoleCode in outsoleCodeList)
            {
                DataRow dr = dt.NewRow();
                dr["OutsoleCode"] = outsoleCode;
                List<String> productNoList = orderList.Where(o => o.OutsoleCode == outsoleCode).Select(o => o.ProductNo).Distinct().ToList();
                List<UpperComponentMaterialModel> upperComponentMaterialList_D1 = upperComponentMaterialList.Where(o => productNoList.Contains(o.ProductNo)).ToList();
                //List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList_D1 = outsoleReleaseMaterialList.Where(o => productNoList.Contains(o.ProductNo)).ToList();
                List<Int32> upperComponentIDList = upperComponentMaterialList_D1.Select(o => o.UpperComponentID).Distinct().ToList();
                List<Int32> qtyTotalList = new List<Int32>();
                List<Int32> qtyRejectList = new List<Int32>();

                foreach (Int32 upperComponentID in upperComponentIDList)
                {

                    List<UpperComponentMaterialModel> upperComponentMaterialList_D2 = upperComponentMaterialList_D1.Where(o => o.UpperComponentID == upperComponentID).ToList();
                    int qtyTotal = 0;
                    int qtyRejectTotal = 0;

                    foreach (string productNo in productNoList)
                    {
                        List<UpperComponentMaterialModel> upperComponentMaterialList_D3 = upperComponentMaterialList_D2.Where(o => o.ProductNo == productNo).ToList();
                        //List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList_D3 = outsoleReleaseMaterialList_D1.Where(o => o.ProductNo == productNo).ToList();

                        List<String> sizeNoList = upperComponentMaterialList.Where(o => o.ProductNo == productNo).Select(o => o.SizeNo).Distinct().ToList();
                        foreach (string sizeNo in sizeNoList)
                        {
                            int qtyMax = upperComponentMaterialList_D3.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity - o.QuantityReject);
                            int qtyReject = upperComponentMaterialList_D3.Where(o => o.SizeNo == sizeNo).Sum(o => o.QuantityReject);
                            //int qtyRelease = outsoleReleaseMaterialList_D3.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity);
                            //int qty = qtyMax - qtyRelease;
                            int qty = qtyMax;
                            if (qty < 0)
                            {
                                qty = 0;
                            }
                            qtyTotal += qty;
                            qtyRejectTotal += qtyReject;
                        }
                    }
                    dr[String.Format("Column{0}", upperComponentID)] = "x";
                    dr[String.Format("Column{0}Background", upperComponentID)] = Brushes.Green;
                    var upperComponentName = upperComponentList.Where(w => w.UpperComponentID == upperComponentID).Select(s => s.UpperComponentName).FirstOrDefault();
                    if (upperComponentName != null)
                    {
                        dr[String.Format("Column{0}ToolTip", upperComponentID)] = string.Format("{0}{1}", upperComponentName, qtyRejectTotal > 0 ? String.Format("\nQtyReject: {0}", qtyRejectTotal.ToString()) : "");
                            //upperComponentName;
                    }
                    qtyTotalList.Add(qtyTotal);
                    qtyRejectList.Add(qtyRejectTotal);
                }

                int qtyMatchTotal = 0;
                foreach (string productNo in productNoList)
                {
                    List<UpperComponentMaterialModel> upperComponentMaterialList_D2 = upperComponentMaterialList_D1.Where(o => o.ProductNo == productNo).ToList();
                    //List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList_D2 = outsoleReleaseMaterialList_D1.Where(o => o.ProductNo == productNo).ToList();
                    List<String> sizeNoList = upperComponentMaterialList.Where(o => o.ProductNo == productNo).Select(o => o.SizeNo).Distinct().ToList();
                    foreach (string sizeNo in sizeNoList)
                    {
                        int qtyMin = upperComponentMaterialList_D2.Where(o => o.SizeNo == sizeNo).Select(o => o.Quantity - o.QuantityReject).Min();
                        //int qtyRelease = outsoleReleaseMaterialList_D2.Where(o => o.SizeNo == sizeNo).Sum(o => o.Quantity);
                        //int qtyMatch = qtyMin - qtyRelease;
                        int qtyMatch = qtyMin;
                        if (qtyMatch < 0)
                        {
                            qtyMatch = 0;
                        }
                        qtyMatchTotal += qtyMatch;
                    }
                }

                if (qtyRejectList.Count > 0)
                {
                    dr["RejectMaximum"] = qtyRejectList.Max();
                }

                if (qtyTotalList.Count > 0)
                {
                    dr["Quantity"] = qtyTotalList.Max();
                    dr["Matching"] = qtyMatchTotal;
                    dt.Rows.Add(dr);
                }
            }

            Dispatcher.Invoke(new Action(() =>
            {
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
        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                int upperComponentIDTranfer = upperComponentIDShowInGridviewList[suppIndex];
                List<OrdersModel> orderListTranfer = orderList.Where(w => w.OutsoleCode == outsoleCodeTranfer).ToList();
                List<String> productNoList = orderListTranfer.Select(s => s.ProductNo).Distinct().ToList();
                List<UpperComponentMaterialModel> upperComponentMaterialListTranfer = upperComponentMaterialList.Where(w => productNoList.Contains(w.ProductNo) && w.UpperComponentID == upperComponentIDTranfer).ToList();
                List<String> productNoListTranfer = upperComponentMaterialListTranfer.Select(s => s.ProductNo).Distinct().ToList();
                //List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialListTranfer = outsoleReleaseMaterialList.Where(w => productNoListTranfer.Contains(w.ProductNo)).ToList();
                UpperComponentWHDeliveryDetailWindow window = new UpperComponentWHDeliveryDetailWindow(orderListTranfer,
                                                                                        upperComponentMaterialListTranfer,
                                                                                        productNoListTranfer,
                                                                                        upperComponentRawMaterialList,
                                                                                        outsoleCodeTranfer,
                                                                                        upperComponentIDTranfer);
                window.Title = String.Format("{0} for {1} - {2}", window.Title, outsoleCodeTranfer, upperComponentList.Where(w => w.UpperComponentID == upperComponentIDTranfer).Select(s => s.UpperComponentName).FirstOrDefault());
                window.Show();
                window.Focus();
            }
        }
    }
}
