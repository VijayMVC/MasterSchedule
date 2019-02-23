using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Data;
using MasterSchedule.DataSets;
using MasterSchedule.Helpers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ReleaseToStockfitWindow.xaml
    /// </summary>
    public partial class UpdateAssemblyReleaseWindow : Window
    {
        AccountModel account;
        BackgroundWorker bwLoadData;
        BackgroundWorker bwAddMore;
        OrdersModel orderSearch;
        List<OrdersModel> orderAllList;
        List<AssemblyReleaseModel> assemblyReleaseAllList;
        List<SewingOutputModel> sewingOutputAllList;
        List<OutsoleOutputModel> outsoleOutputAllList;
        List<SizeRunModel> sizeRunAllList;
        List<SizeRunModel> sizeRunSearchList;
        List<AssemblyReleaseModel> assemblyReleaseSearchList;
        List<OutsoleOutputModel> outsoleOutputSearchList;
        List<SewingOutputModel> sewingOutputSearchList;
        List<AssemblyReleaseModel> assemblyReleaseToInsertList;
        BackgroundWorker bwInsert;
        BackgroundWorker bwRemove;
        string productNoToRemove;
        string reportId;
        public UpdateAssemblyReleaseWindow(AccountModel account, string reportId)
        {
            this.account = account;
            this.reportId = reportId;
            orderAllList = new List<OrdersModel>();
            assemblyReleaseAllList = new List<AssemblyReleaseModel>();
            sewingOutputAllList = new List<SewingOutputModel>();
            outsoleOutputAllList = new List<OutsoleOutputModel>();
            sizeRunAllList = new List<SizeRunModel>();
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            bwAddMore = new BackgroundWorker();
            bwAddMore.DoWork += new DoWorkEventHandler(bwAddMore_DoWork);
            bwAddMore.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwAddMore_RunWorkerCompleted);
            orderSearch = new OrdersModel();
            sizeRunAllList = new List<SizeRunModel>();
            sizeRunSearchList = new List<SizeRunModel>();
            assemblyReleaseSearchList = new List<AssemblyReleaseModel>();
            sewingOutputSearchList = new List<SewingOutputModel>();
            outsoleOutputSearchList = new List<OutsoleOutputModel>();
            assemblyReleaseToInsertList = new List<AssemblyReleaseModel>();
            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);
            bwRemove = new BackgroundWorker();
            bwRemove.DoWork += new DoWorkEventHandler(bwRemove_DoWork);
            bwRemove.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwRemove_RunWorkerCompleted);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //reportId = String.Format("RPTA{0}", Math.Round(DateTime.Now.ToOADate() * 100000, 0));        
            lblReportId.Text = reportId;
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            assemblyReleaseAllList = AssemblyReleaseController.Select(reportId, null);
            orderAllList = OrdersController.SelectByAssemblyRelease(reportId);
            sewingOutputAllList = SewingOutputController.SelectByAssemblyRelease(reportId);
            outsoleOutputAllList = OutsoleOutputController.SelectByAssemblyRelease(reportId);
            sizeRunAllList = SizeRunController.SelectByAssemblyRelease(reportId);
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnAddMore.IsEnabled = true;
            if (account.AssemblyMaster == true)
            {
                btnRelease.IsEnabled = true;
            }
            btnExport.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<String> productNoList = assemblyReleaseAllList.Where(o => o.ReportId == reportId).OrderBy(o => o.Cycle).Select(o => o.ProductNo).Distinct().ToList();
            if (productNoList.Count <= 0)
            {
                this.Close();
            }
            foreach (string productNo in productNoList)
            {
                AddMore(orderAllList.Where(o => o.ProductNo == productNo).FirstOrDefault(),
                    sizeRunAllList.Where(o => o.ProductNo == productNo).ToList(),
                    assemblyReleaseAllList.Where(o => o.ProductNo == productNo).ToList(),
                    sewingOutputAllList.Where(o => o.ProductNo == productNo).ToList(),
                    outsoleOutputAllList.Where(o => o.ProductNo == productNo).ToList());
            }
            
        }

        private void btnAddMore_Click(object sender, RoutedEventArgs e)
        {
            if (popupAddMore.IsOpen == true)
            {
                popupAddMore.IsOpen = false;
                return;
            }
            popupAddMore.PlacementTarget = btnAddMore;
            popupAddMore.IsOpen = true;
        }

        private void btnAddMoreOk_Click(object sender, RoutedEventArgs e)
        {
            if (bwAddMore.IsBusy == false && String.IsNullOrEmpty(txtProductNo.Text) == false)
            {
                if (sizeRunAllList.Where(s => s.ProductNo == txtProductNo.Text).Count() > 0)
                {
                    return;
                }
                this.Cursor = Cursors.Wait;
                orderSearch = null;
                sizeRunSearchList.Clear();
                assemblyReleaseSearchList.Clear();
                sewingOutputSearchList.Clear();
                outsoleOutputSearchList.Clear();
                btnAddMoreOk.IsEnabled = false;
                bwAddMore.RunWorkerAsync();
            }
        }

        private void bwAddMore_DoWork(object sender, DoWorkEventArgs e)
        {
            string productNo = "";
            txtProductNo.Dispatcher.Invoke((Action)(() =>
                {
                    productNo = txtProductNo.Text;
                }));
            orderSearch = OrdersController.SelectTop1(productNo);
            sizeRunSearchList = SizeRunController.Select(productNo);
            assemblyReleaseSearchList = AssemblyReleaseController.Select(productNo);
            sewingOutputSearchList = SewingOutputController.Select(productNo);
            outsoleOutputSearchList = OutsoleOutputController.Select(productNo);
        }

        private void bwAddMore_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (orderSearch == null || orderSearch.IsEnable == false || sizeRunSearchList.Count < 1)
            {
                btnAddMoreOk.IsEnabled = true;
                this.Cursor = null;
                popupAddMore.IsOpen = false;
                return;
            }
            btnAddMoreOk.IsEnabled = true;
            this.Cursor = null;           
            //if (e.Error != null)
            //{
            //    MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            popupAddMore.IsOpen = false;
            orderAllList.Add(orderSearch);
            sizeRunAllList.AddRange(sizeRunSearchList);
            AddMore(orderSearch, sizeRunSearchList, assemblyReleaseSearchList, sewingOutputSearchList, outsoleOutputSearchList);
            
        }

        public void AddMore(OrdersModel order, List<SizeRunModel> sizeRunList, List<AssemblyReleaseModel> assemblyReleaseList,
            List<SewingOutputModel> sewingOutputList, List<OutsoleOutputModel> outsoleOutputList)
        {
            if (order == null)
            {
                return;
            }

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Vertical;
            sp.Margin = new Thickness(0, 10, 0, 0);

            Grid grid = new Grid();
            for (int i = 1; i <= 10; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(1, GridUnitType.Auto);

                grid.ColumnDefinitions.Add(cd);
            }
            ColumnDefinition cd1 = new ColumnDefinition();
            cd1.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(cd1);

            TextBlock lbl1 = new TextBlock();
            Grid.SetColumn(lbl1, 0);
            lbl1.Text = "PO No.:";
            lbl1.VerticalAlignment = VerticalAlignment.Bottom;


            TextBlock lbl1_1 = new TextBlock();
            Grid.SetColumn(lbl1_1, 1);
            lbl1_1.Margin = new Thickness(5, 0, 0, 0);
            lbl1_1.MinWidth = 33;
            lbl1_1.Text = order.ProductNo;
            lbl1_1.FontWeight = FontWeights.Bold;
            lbl1_1.VerticalAlignment = VerticalAlignment.Bottom;

            TextBlock lbl2 = new TextBlock();
            Grid.SetColumn(lbl2, 2);
            lbl2.Margin = new Thickness(10, 0, 0, 0);
            lbl2.Text = "Style:";
            lbl2.VerticalAlignment = VerticalAlignment.Bottom;

            TextBlock lbl3 = new TextBlock();
            Grid.SetColumn(lbl3, 3);
            lbl3.Margin = new Thickness(5, 0, 0, 0);
            lbl3.MinWidth = 33;
            lbl3.Text = order.ShoeName;
            lbl3.FontWeight = FontWeights.Bold;
            lbl3.VerticalAlignment = VerticalAlignment.Bottom;

            TextBlock lbl4 = new TextBlock();
            Grid.SetColumn(lbl4, 4);
            lbl4.Margin = new Thickness(10, 0, 0, 0);
            lbl4.Text = "Article No.:";
            lbl4.VerticalAlignment = VerticalAlignment.Bottom;


            TextBlock lbl5 = new TextBlock();
            Grid.SetColumn(lbl5, 5);
            lbl5.Margin = new Thickness(5, 0, 0, 0);
            lbl5.MinWidth = 33;
            lbl5.Text = order.ArticleNo;
            lbl5.FontWeight = FontWeights.Bold;
            lbl5.VerticalAlignment = VerticalAlignment.Bottom;

            TextBlock lbl5_1 = new TextBlock();
            Grid.SetColumn(lbl5_1, 6);
            lbl5_1.Margin = new Thickness(10, 0, 0, 0);
            lbl5_1.Text = "Last Code:";
            lbl5_1.VerticalAlignment = VerticalAlignment.Bottom;


            TextBlock lbl5_2 = new TextBlock();
            Grid.SetColumn(lbl5_2, 7);
            lbl5_2.Margin = new Thickness(5, 0, 0, 0);
            lbl5_2.MinWidth = 33;
            lbl5_2.Text = order.LastCode;
            lbl5_2.FontWeight = FontWeights.Bold;
            lbl5_2.VerticalAlignment = VerticalAlignment.Bottom;

            TextBlock lbl6 = new TextBlock();
            Grid.SetColumn(lbl6, 8);
            lbl6.Margin = new Thickness(10, 0, 0, 0);
            lbl6.Text = "Released Qty:";
            lbl6.VerticalAlignment = VerticalAlignment.Bottom;

            int qtyReleasedTotal = assemblyReleaseList.Where(a => a.ReportId != reportId).Sum(o => o.Quantity);

            TextBlock lbl7 = new TextBlock();
            Grid.SetColumn(lbl7, 9);
            lbl7.Margin = new Thickness(5, 0, 0, 0);
            lbl7.MinWidth = 33;
            lbl7.Text = String.Format("{0}/{1}", qtyReleasedTotal, order.Quantity);
            lbl7.FontWeight = FontWeights.Bold;
            lbl7.VerticalAlignment = VerticalAlignment.Bottom;

            Button btnRemove = new Button();
            Grid.SetColumn(btnRemove, 10);
            btnRemove.Content = " X ";
            btnRemove.HorizontalAlignment = HorizontalAlignment.Right;
            btnRemove.Tag = order.ProductNo;
            btnRemove.IsEnabled = false;
            if (account.AssemblyMaster == true)
            {
                btnRemove.IsEnabled = true;
            }
            btnRemove.Click += new RoutedEventHandler(btnRemove_Click);

            grid.Children.Add(lbl1);
            grid.Children.Add(lbl1_1);
            grid.Children.Add(lbl2);
            grid.Children.Add(lbl3);
            grid.Children.Add(lbl4);
            grid.Children.Add(lbl5);
            grid.Children.Add(lbl5_1);
            grid.Children.Add(lbl5_2);
            grid.Children.Add(lbl6);
            grid.Children.Add(lbl7);
            grid.Children.Add(btnRemove);

            List<Object> tags = new List<Object>();
            tags.Add(order.ProductNo);
            tags.Add(qtyReleasedTotal);
            DataTable dt = new DataTable();
            DataGrid dg = new DataGrid();
            Grid.SetColumn(dg, 0);
            Grid.SetColumnSpan(dg, 6);
            Grid.SetRow(dg, 1);
            dg.Margin = new Thickness(0, 5, 0, 0);
            dg.SelectionUnit = DataGridSelectionUnit.CellOrRowHeader;
            dg.AutoGenerateColumns = false;
            dg.CanUserDeleteRows = false;
            dg.Tag = tags;
            dg.CellEditEnding += new EventHandler<DataGridCellEditEndingEventArgs>(dg_CellEditEnding);

            dt.Columns.Add("Cycle", typeof(Int32));
            DataGridTextColumn column1 = new DataGridTextColumn();
            column1.Header = String.Format("{0}\n{1}\n{2}\n{3}\n{4}\n\n{5}", "Size", "Quantity", "Upper In", "Outsole In", "Current Matching", "Cycle");
            column1.MinWidth = 100;
            column1.Binding = new Binding(String.Format("Cycle"));
            dg.Columns.Add(column1);
            int qtyTotal = 0;
            int qtyUpperInTotal = 0;
            int qtyOutsoleInTotal = 0;
            int qtyMatchTotal = 0;
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                dt.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                SizeRunModel sizeRun = sizeRunList[i];

                int qtyMatch = 0;
                //if (sewingOutputList.Where(o => o.SizeNo == sizeRun.SizeNo).Count() >= 1)
                //{
                    int qtyUpperIn = sewingOutputList.Where(o => o.SizeNo == sizeRun.SizeNo).Sum(o => o.Quantity);
                    int qtyOutsoleIn = outsoleOutputList.Where(o => o.SizeNo == sizeRun.SizeNo).Sum(o => o.Quantity);
                    int qtyReleased = assemblyReleaseList.Where(o => o.SizeNo == sizeRun.SizeNo && o.ReportId != reportId).Sum(o => o.Quantity);
                    qtyMatch = MatchingHelper.Calculate(qtyUpperIn, qtyOutsoleIn, sizeRun.SizeNo) - qtyReleased;
                    if (qtyMatch < 0)
                    {
                        qtyMatch = 0;
                    }
                    qtyTotal += sizeRun.Quantity;
                    qtyUpperInTotal += qtyUpperIn;
                    qtyOutsoleInTotal += qtyOutsoleIn;
                    qtyMatchTotal += qtyMatch;
                //}

                DataGridTextColumn column = new DataGridTextColumn();
                column.SetValue(TagProperty, new Int32[] { i, qtyMatch });
                column.Header = String.Format("{0}\n({1})\n({2})\n({3})\n({4})\n\n{5}", sizeRun.SizeNo, sizeRun.Quantity, qtyUpperIn, qtyOutsoleIn, qtyMatch, "");
                column.MinWidth = 40;
                Binding binding = new Binding();
                binding.Path = new PropertyPath(String.Format("Column{0}", i));
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                column.Binding = binding;
                dg.Columns.Add(column);
            }

            dt.Columns.Add("Total", typeof(Int32));
            DataGridTextColumn column2 = new DataGridTextColumn();
            //column2.Header = String.Format("{0}\n({1})", "Total Qty", qtyMatchTotal);
            column2.Header = String.Format("{0}\n({1})\n({2})\n({3})\n({4})\n\n{5}", "Total Qty", qtyTotal, qtyUpperInTotal, qtyOutsoleInTotal, qtyMatchTotal, "");
            column2.MinWidth = 80;
            column2.Binding = new Binding(String.Format("Total"));
            column2.FontWeight = FontWeights.Bold;
            column2.IsReadOnly = true;
            dg.Columns.Add(column2);

            List<Int32> cycleList = assemblyReleaseList.Where(o => o.ReportId == reportId).Select(o => o.Cycle).Distinct().OrderBy(o => o).ToList();
            foreach (int cycle in cycleList)
            {
                DataRow dr1 = dt.NewRow();
                dr1["Cycle"] = cycle.ToString();
                int qtyReleaseTotal = 0;
                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    SizeRunModel sizeRun = sizeRunList[i];
                    int qtyRelease = assemblyReleaseList.Where(o => o.Cycle == cycle && o.SizeNo == sizeRun.SizeNo && o.ReportId == reportId).Sum(o => o.Quantity);
                    qtyReleaseTotal += qtyRelease;
                    dr1[String.Format("Column{0}", i)] = qtyRelease;
                }
                dr1["Total"] = qtyReleaseTotal;
                dt.Rows.Add(dr1);
            }

            dg.ItemsSource = dt.AsDataView();

            sp.Children.Add(grid);
            sp.Children.Add(dg);

            spMain.Children.Add(sp);

            svMain.ScrollToBottom();
        }

        private void dg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Int32[] tag = (Int32[])(e.Column).GetValue(TagProperty);
            if (tag == null)
            {
                return;
            }
            DataGrid dg = (DataGrid)sender;
            DataTable dt = ((DataView)dg.ItemsSource).ToTable();
            int qtySumBySize = 0;
            Int32.TryParse(dt.Compute(String.Format("Sum(Column{0})", tag[0]), "").ToString(), out qtySumBySize);
            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyNew = 0;
            if (int.TryParse(txtCurrent.Text, out qtyNew) == true)
            {
                if (qtyNew < 0 || qtySumBySize > tag[1])
                {
                    qtyNew = 0;
                    txtCurrent.Text = qtyNew.ToString();
                }

                DataRowView dataRowView = e.Row.Item as DataRowView;
                DataRow dr = dataRowView.Row;
                Object[] itemArray = dr.ItemArray as Object[];
                int qtyReleaseTotal = 0;
                for (int i = 1; i <= itemArray.Count() - 2; i++)
                {
                    int qtyRelease = 0;
                    Int32.TryParse(itemArray[i].ToString(), out qtyRelease);
                    qtyReleaseTotal += qtyRelease;
                }
                dr["Total"] = qtyReleaseTotal;
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Remove?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            Button btnRemove = (Button)sender;
            productNoToRemove = btnRemove.Tag.ToString();
            Grid grid = (Grid)btnRemove.Parent;
            if (grid != null)
            {
                StackPanel sp = (StackPanel)grid.Parent;
                if (sp != null)
                {
                    if (bwRemove.IsBusy == false && String.IsNullOrEmpty(productNoToRemove) == false)
                    {
                        spMain.Children.Remove(sp);
                        sizeRunAllList.RemoveAll(s => s.ProductNo == productNoToRemove);
                        this.Cursor = Cursors.Wait;
                        bwRemove.RunWorkerAsync();
                    }
                }
            }
        }

        private void bwRemove_DoWork(object sender, DoWorkEventArgs e)
        {
            AssemblyReleaseController.Delete(reportId, productNoToRemove);
        }

        private void bwRemove_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Removed!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Release?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            assemblyReleaseToInsertList.Clear();
            foreach (StackPanel sp in spMain.Children)
            {
                foreach (UIElement ui in sp.Children)
                {
                    if (ui.GetType() == typeof(DataGrid))
                    {
                        DataGrid dg = (DataGrid)ui;
                        List<Object> tags = dg.Tag as List<Object>;
                        string productNo = tags[0] as String;
                        List<SizeRunModel> sizeRunList = sizeRunAllList.Where(s => s.ProductNo == productNo).ToList();
                        DataTable dt = ((DataView)dg.ItemsSource).ToTable();
                        foreach (DataRow dr in dt.Rows)
                        {
                            int cycle = 0;
                            if (int.TryParse(dr["Cycle"].ToString(), out cycle) == true)
                            {
                                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                                {
                                    SizeRunModel sizeRun = sizeRunList[i];
                                    int qtyRelease = 0;
                                    int.TryParse(dr[String.Format("Column{0}", i)].ToString(), out qtyRelease);
                                    AssemblyReleaseModel assemblyRelease = new AssemblyReleaseModel
                                    {
                                        ReportId = reportId,
                                        ProductNo = productNo,
                                        Cycle = cycle,
                                        SizeNo = sizeRun.SizeNo,
                                        Quantity = qtyRelease,
                                    };
                                    assemblyReleaseToInsertList.Add(assemblyRelease);
                                }
                            }
                        }
                    }
                }
            }
            if (assemblyReleaseToInsertList.Count > 0 && bwInsert.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnRelease.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (AssemblyReleaseModel model in assemblyReleaseToInsertList)
            {
                AssemblyReleaseController.Insert(model);
            }
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnRelease.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Released!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            DataTable dtToExport = new AssemblyReleaseDataSet().Tables["AssemblyReleaseTable"];
            foreach (StackPanel sp in spMain.Children)
            {
                foreach (UIElement ui in sp.Children)
                {
                    if (ui.GetType() == typeof(DataGrid))
                    {
                        DataGrid dg = (DataGrid)ui;
                        List<Object> tags = dg.Tag as List<Object>;
                        string productNo = tags[0] as String;
                        int? qtyReleasedTotal = tags[1] as Int32?;
                        OrdersModel order = orderAllList.Where(o => o.ProductNo == productNo).FirstOrDefault();
                        List<SizeRunModel> sizeRunList = sizeRunAllList.Where(s => s.ProductNo == productNo).ToList();
                        DataTable dt = ((DataView)dg.ItemsSource).ToTable();
                        foreach (DataRow dr in dt.Rows)
                        {
                            int cycle = 0;
                            if (int.TryParse(dr["Cycle"].ToString(), out cycle) == true)
                            {
                                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                                {
                                    SizeRunModel sizeRun = sizeRunList[i];
                                    int qtyRelease = 0;
                                    int.TryParse(dr[String.Format("Column{0}", i)].ToString(), out qtyRelease);
                                    DataRow drToExport = dtToExport.NewRow();
                                    drToExport["ProductNo"] = productNo;
                                    drToExport["OrderQuantity"] = sizeRun.Quantity;
                                    if (order != null)
                                    {
                                        drToExport["ShoeName"] = order.ShoeName;
                                        drToExport["ArticleNo"] = order.ArticleNo;
                                        drToExport["LastCode"] = order.LastCode;
                                        drToExport["TotalOrderQuantity"] = order.Quantity;
                                    }
                                    drToExport["TotalReleasedQuantity"] = qtyReleasedTotal.Value;
                                    drToExport["Cycle"] = cycle;
                                    drToExport["SizeNo"] = sizeRun.SizeNo;
                                    drToExport["Quantity"] = qtyRelease;
                                    dtToExport.Rows.Add(drToExport);
                                }
                            }
                        }
                    }
                }
            }

            if (dtToExport.Rows.Count <= 0)
            {
                return;
            }
            AssemblyReleaseReportWindow window = new AssemblyReleaseReportWindow(reportId, dtToExport);
            window.ShowDialog();
        }
    }
}
