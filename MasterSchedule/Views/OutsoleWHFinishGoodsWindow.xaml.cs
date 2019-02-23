using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MasterSchedule.Controllers;
using MasterSchedule.Models;
using System.Data;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleWHFinishGoodsWindow.xaml
    /// </summary>
    public partial class OutsoleWHFinishGoodsWindow : Window
    {
        BackgroundWorker bwLoad;
        BackgroundWorker bwInsert;
        List<SizeRunModel> sizeRunList;
        List<OutsoleWHFGModel> outsoleWHFGList;
        List<OutsoleWHFGModel> outsoleWHFGFromTableList;
        DataTable dt;

        DateTime dtInput;
        public OutsoleWHFinishGoodsWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);

            sizeRunList = new List<SizeRunModel>();
            outsoleWHFGList = new List<OutsoleWHFGModel>();
            dtInput = DateTime.Now;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProductNo.Focus();
        }

        string productNo = "";
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;

                productNo = txtProductNo.Text.ToUpper().ToString();
                txtProductNo.Text = productNo;

                dt = new DataTable();
                dgOutsoleWHFG.Columns.Clear();
                dgOutsoleWHFG.ItemsSource = null;

                btnSearch.IsEnabled = false;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            sizeRunList = SizeRunController.Select(productNo);
            outsoleWHFGList = OutsoleWHFGController.Select(productNo).OrderBy(o => o.CreatedDate).ToList();
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSearch.IsEnabled = true;
            btnSearch.IsDefault = true;
            this.Cursor = null;

            if (sizeRunList.Count() == 0)
            {
                MessageBox.Show("Not Found !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                txtProductNo.Focus();
                txtProductNo.SelectAll();

                return;
            }

            // Created Column
            dt.Columns.Add("CreatedDate", typeof(DateTime));
            DataGridTextColumn columnCreatedDate = new DataGridTextColumn();
            columnCreatedDate.Header = "Date";
            columnCreatedDate.Binding = new Binding("CreatedDate");
            columnCreatedDate.Binding.StringFormat = "MM/dd";
            dgOutsoleWHFG.Columns.Add(columnCreatedDate);

            dt.Columns.Add("Status", typeof(String));
            DataGridTextColumn columnStatus = new DataGridTextColumn();
            columnStatus.Header = productNo;
            columnStatus.Binding = new Binding("Status");

            columnStatus.IsReadOnly = true;
            columnStatus.MinWidth = 80;

            dgOutsoleWHFG.Columns.Add(columnStatus);

            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dt.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                DataGridTextColumn column = new DataGridTextColumn();
                column.SetValue(TagProperty, sizeRun.SizeNo);
                column.Header = String.Format("{0}\n({1})", sizeRun.SizeNo, sizeRun.Quantity);
                column.MinWidth = 40;
                column.Binding = new Binding(String.Format("Column{0}", i)) { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.LostFocus };
                dgOutsoleWHFG.Columns.Add(column);
            }

            dt.Columns.Add("Total", typeof(Int32));
            DataGridTextColumn columnTotal = new DataGridTextColumn();
            columnTotal.Header = string.Format("Total\n{0}", sizeRunList.Select(s => s.Quantity).Sum());
            columnTotal.Binding = new Binding("Total") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.LostFocus };
            columnTotal.MinWidth = 50;
            columnTotal.IsReadOnly = true;
            dgOutsoleWHFG.Columns.Add(columnTotal);

            DataGridTemplateColumn buttonColumn = new DataGridTemplateColumn();
            DataTemplate buttonTemplate = new DataTemplate();
            FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
            buttonTemplate.VisualTree = buttonFactory;
            buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(btnOK_Click));
            buttonFactory.SetValue(ContentProperty, "OK");
            buttonColumn.CellTemplate = buttonTemplate;
            dgOutsoleWHFG.Columns.Add(buttonColumn);


            Style style = new Style(typeof(DataGridCell));
            style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
            dgOutsoleWHFG.CellStyle = style;

            // Fill data
            if (outsoleWHFGList.Count() > 0)
            {
                var createdDateList = outsoleWHFGList.Select(s => s.CreatedDate).Distinct().ToList();
                foreach (var createdDate in createdDateList)
                {
                    DataRow dr = dt.NewRow();
                    dr["CreatedDate"] = createdDate;
                    dr["Status"] = "Quantity";
                    var sizeNoAndQtyList = outsoleWHFGList.Where(w => w.CreatedDate == createdDate).Select(s => new { SizeNo = s.SizeNo, Quantity = s.Quantity }).ToList();
                    for (int i = 0; i < sizeNoAndQtyList.Count; i++)
                    {
                        dr[String.Format("Column{0}", i)] = sizeNoAndQtyList[i].Quantity;
                    }
                    dr["Total"] = outsoleWHFGList.Where(w => w.CreatedDate == createdDate).Select(s => s.Quantity).Sum();

                    dt.Rows.Add(dr);
                }
            }

            dgOutsoleWHFG.ItemsSource = dt.AsDataView();

            btnAddRow.IsEnabled = true;
            btnBalance.IsEnabled = true;
        }

        private void btnAddRow_Click(object sender, RoutedEventArgs e)
        {
            bool addRow = false;
            var dtCurrent = ((DataView)dgOutsoleWHFG.ItemsSource).ToTable();
            if (dtCurrent.Rows.Count == 0)
            {
                addRow = true;
            }
            var createdDateList = new List<DateTime>();
            for (int i = 0; i < dtCurrent.Rows.Count; i++)
            {
                var drCurrent = dtCurrent.Rows[i];
                createdDateList.Add((DateTime)drCurrent.ItemArray[0]);
            }
            var lastCreatedDate = createdDateList.Select(s => s.Date).Distinct().OrderBy(s => s.Date).LastOrDefault().Date;
            if (dtInput.Date != lastCreatedDate)
            {
                addRow = true;
            }

            if (addRow == true)
            {
                DataRow dr = dt.NewRow();
                dr["CreatedDate"] = dtInput;
                dr["Status"] = "Quantity";
                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    dr[String.Format("Column{0}", i)] = 0;
                }
                dr["Total"] = 0;

                dt.Rows.Add(dr);
                dgOutsoleWHFG.ItemsSource = dt.AsDataView();
            }
        }

        private void btnBalance_Click(object sender, RoutedEventArgs e)
        {
            int drRemoveAt = 0;
            bool addBalance = true;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drCheck = dt.Rows[i];
                if (drCheck["Status"].ToString().Contains("Balance"))
                {
                    addBalance = false;
                    drRemoveAt = i;
                }
            }

            DataRow drBalance = dt.NewRow();
            drBalance["CreatedDate"] = dtInput;
            drBalance["Status"] = "Balance";
            int totalBalance = 0;
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                int qtyOrder = sizeRunList[i].Quantity;
                int qtyTotal = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Status"].ToString().Contains("Quantity") == true)
                    {
                        int qtyPerSize = 0;
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out qtyPerSize);
                        qtyTotal += qtyPerSize;
                    }
                }
                int balancePerSize = qtyOrder - qtyTotal;
                drBalance[String.Format("Column{0}", i)] = balancePerSize;
                totalBalance += balancePerSize;
            }
            drBalance["Total"] = totalBalance;

            if (addBalance == true)
            {
                dt.Rows.Add(drBalance);
            }
            else
            {
                dt.Rows.RemoveAt(drRemoveAt);
                dt.Rows.Add(drBalance);
            }
            dgOutsoleWHFG.ItemsSource = dt.AsDataView();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (dgOutsoleWHFG.CurrentItem == null)
            {
                return;
            }
            DataRow dr = ((DataRowView)dgOutsoleWHFG.CurrentItem).Row;
            if (dr == null)
            {
                return;
            }
            if (dr["Status"].ToString().Contains("Balance"))
            {
                return;
            }
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dr[String.Format("Column{0}", i)] = sizeRun.Quantity;
            }
            dr["Total"] = sizeRunList.Select(s => s.Quantity).Sum();
            dgOutsoleWHFG.ItemsSource = null;
            dgOutsoleWHFG.ItemsSource = dt.AsDataView();
        }

        private void dgOutsoleWHFG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (dgOutsoleWHFG.CurrentItem == null)
            {
                return;
            }
            DataRow drCurrent = ((DataRowView)dgOutsoleWHFG.CurrentItem).Row;

            // date validate
            //if (e.Column.GetValue(TagProperty) == "ChangeDate")
            //{
            //    TextBox txtChangeDate = (TextBox)e.EditingElement;
            //}

            if (e.EditAction != DataGridEditAction.Commit)
            {
                return;
            }
            if (e.Column.GetValue(TagProperty) == null)
            {
                return;
            }
            string sizeNo = e.Column.GetValue(TagProperty).ToString();
            if (sizeRunList.Select(s => s.SizeNo).Contains(sizeNo) == false)
            {
                return;
            }

            int qtyOrder = sizeRunList.Where(s => s.SizeNo == sizeNo).Select(s => s.Quantity).FirstOrDefault();

            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyNew = 0;

            // Calculate Total
            if (int.TryParse(txtCurrent.Text, out qtyNew) == true)
            {
                if (qtyNew > qtyOrder)
                {
                    txtCurrent.BorderBrush = Brushes.Red;
                    txtCurrent.BorderThickness = new Thickness(1, 1, 1, 1);
                    txtCurrent.Foreground = Brushes.Red;
                    txtCurrent.Text = "!";
                    txtCurrent.SelectAll();
                }
                else
                {
                    int qtyTotal = 0;
                    for (int i = 0; i <= sizeRunList.Count - 1; i++)
                    {
                        if (sizeRunList[i].SizeNo != sizeNo)
                        {
                            int qtyOld = (Int32)drCurrent[String.Format("Column{0}", i)];
                            qtyTotal += qtyOld;
                        }
                    }
                    drCurrent["Total"] = qtyTotal + qtyNew;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (bwInsert.IsBusy == true || dt == null)
            {
                return;
            }

            outsoleWHFGFromTableList = new List<OutsoleWHFGModel>();
            dt = ((DataView)dgOutsoleWHFG.ItemsSource).ToTable();
            if (dt.Rows.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (dr["Status"].ToString().Contains("Balance"))
                {
                    continue;
                }
                for (int j = 0; j <= sizeRunList.Count - 1; j++)
                {
                    OutsoleWHFGModel outsoleWHFG = new OutsoleWHFGModel();
                    outsoleWHFG.ProductNo = productNo;
                    outsoleWHFG.SizeNo = sizeRunList[j].SizeNo;

                    int qty = 0;
                    if (dr["Status"].ToString().Contains("Quantity"))
                    {
                        Int32.TryParse(dr[String.Format("Column{0}", j)].ToString(), out qty);
                    }
                    outsoleWHFG.Quantity = qty;
                    outsoleWHFG.CreatedDate = (DateTime)dr.ItemArray[0];

                    outsoleWHFGFromTableList.Add(outsoleWHFG);
                }
            }

            this.Cursor = Cursors.Wait;
            btnSave.IsEnabled = false;
            bwInsert.RunWorkerAsync();
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var intsert in outsoleWHFGFromTableList)
            {
                OutsoleWHFGController.Insert(intsert);
            }
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Saved !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.Cursor = null;
            btnSave.IsEnabled = true;
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = dgOutsoleWHFG.SelectedItems;
            if (selectedItems.Count == 0 || MessageBox.Show("Confirm Delete?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            List<DataRow> deletedRowList = new List<DataRow>();
            foreach (var item in selectedItems)
            {
                DataRowView drSelected = (DataRowView)item;
                DateTime dateSelected = (DateTime)drSelected.Row.ItemArray[0];
                string status = drSelected.Row.ItemArray[1].ToString();
                if (status.Contains("Balance") == false)
                {
                    OutsoleWHFGController.Delete(productNo, dateSelected);
                    deletedRowList.Add((DataRow)drSelected.Row);
                }
            }

            foreach (var deletedRow in deletedRowList)
            {
                dt.Rows.Remove(deletedRow);
            }
            dgOutsoleWHFG.ItemsSource = dt.AsDataView();

            MessageBox.Show("Deleted!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
