using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Data;
using System.Text.RegularExpressions;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpdateOrdersWindow.xaml
    /// </summary>
    public partial class UpdateOrdersWindow : Window
    {
        BackgroundWorker bwSearch;
        OrdersModel orders;
        List<SizeRunModel> sizeRunList;
        string productNo;
        DataTable dt;
        BackgroundWorker bwUpdateOrders;
        OrdersModel ordersToUpdate;
        BackgroundWorker bwUpdateSizeRun;
        List<SizeRunModel> sizeRunToUpdateList;
        List<SizeRunModel> sizeRunMapToUpdateList;

        BackgroundWorker bwDelete;
        List<OrdersModel> orderList;
        List<SizeRunModel> sizeRunPerOutsoleCodeList;
        List<SizeRunModel> sizeRunPerArticleList;

        BackgroundWorker bwSearchOutsoleCode;
        BackgroundWorker bwUpdateSizeMap;

        BackgroundWorker bwCheckOutsoleCode;

        BackgroundWorker bwSearchArticleNo;
        BackgroundWorker bwUpdateSizeMapByArticleNo;

        public UpdateOrdersWindow()
        {
            bwSearch = new BackgroundWorker();
            bwSearch.DoWork += new DoWorkEventHandler(bwSearch_DoWork);
            bwSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSearch_RunWorkerCompleted);

            orders = new OrdersModel();
            sizeRunList = new List<SizeRunModel>();
            dt = new DataTable();

            bwUpdateOrders = new BackgroundWorker();
            bwUpdateOrders.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwUpdateOrders_RunWorkerCompleted);
            bwUpdateOrders.DoWork += new DoWorkEventHandler(bwUpdateOrders_DoWork);

            ordersToUpdate = new OrdersModel();

            bwUpdateSizeRun = new BackgroundWorker();
            bwUpdateSizeRun.DoWork += new DoWorkEventHandler(bwUpdateSizeRun_DoWork);
            bwUpdateSizeRun.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwUpdateSizeRun_RunWorkerCompleted);

            sizeRunToUpdateList = new List<SizeRunModel>();

            bwDelete = new BackgroundWorker();
            bwDelete.DoWork += new DoWorkEventHandler(bwDelete_DoWork);
            bwDelete.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwDelete_RunWorkerCompleted);

            bwSearchOutsoleCode = new BackgroundWorker();
            bwSearchOutsoleCode.DoWork += new DoWorkEventHandler(bwSearchOutsoleCode_DoWork);
            bwSearchOutsoleCode.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSearchOutsoleCode_RunWorkerCompleted);

            bwUpdateSizeMap = new BackgroundWorker();
            bwUpdateSizeMap.DoWork += new DoWorkEventHandler(bwUpdateSizeMap_DoWork);
            bwUpdateSizeMap.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwUpdateSizeMap_RunWorkerCompleted);

            bwCheckOutsoleCode = new BackgroundWorker();
            bwCheckOutsoleCode.DoWork += new DoWorkEventHandler(bwCheckOutsoleCode_DoWork);
            bwCheckOutsoleCode.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwCheckOutsoleCode_RunWorkerCompleted);

            bwSearchArticleNo = new BackgroundWorker();
            bwSearchArticleNo.DoWork += new DoWorkEventHandler(bwSearchArticleNo_DoWork);
            bwSearchArticleNo.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSearchArticleNo_RunWorkerCompleted);

            bwUpdateSizeMapByArticleNo = new BackgroundWorker();
            bwUpdateSizeMapByArticleNo.DoWork += new DoWorkEventHandler(bwUpdateSizeMapByArticleNo_DoWork);
            bwUpdateSizeMapByArticleNo.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwUpdateSizeMapByArticleNo_RunWorkerCompleted);

            orderList = new List<OrdersModel>();

            sizeRunPerOutsoleCodeList = new List<SizeRunModel>();
            sizeRunPerArticleList = new List<SizeRunModel>();
            sizeRunMapToUpdateList = new List<SizeRunModel>();

            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtProductNo.Text) == true)
            {
                return;
            }
            if (bwSearch.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                productNo = txtProductNo.Text;
                btnSearch.IsEnabled = false;
                btnDelete.IsEnabled = false;
                orders = null;
                btnUpdateOrder.IsEnabled = false;
                dt = new DataTable();
                btnUpdateSizeRun.IsEnabled = false;
                bwSearch.RunWorkerAsync();
            }
        }

        private void bwSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            orders = OrdersController.SelectTop1(productNo);
            sizeRunList = SizeRunController.Select(productNo);
        }

        private void bwSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgOrder.ItemsSource = null;
            if (orders != null)
            {
                dgOrder.ItemsSource = new List<OrdersModel>() { orders, };
                btnUpdateOrder.IsEnabled = true;
            }
            dgSizeRun.ItemsSource = null;
            dgSizeRun.Columns.Clear();
            if (sizeRunList.Count >= 1)
            {
                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    var sizeRun = sizeRunList[i];
                    dt.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                    DataGridTextColumn column = new DataGridTextColumn();
                    column.Header = sizeRun.SizeNo;
                    column.MinWidth = 40;
                    column.Binding = new Binding(String.Format("Column{0}", i));
                    dgSizeRun.Columns.Add(column);
                    btnUpdateSizeRun.IsEnabled = true;
                }
                DataRow dr = dt.NewRow();
                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    var sizeRun = sizeRunList[i];
                    dr[String.Format("Column{0}", i)] = sizeRun.Quantity;
                }
                dt.Rows.Add(dr);
                dgSizeRun.ItemsSource = dt.AsDataView();
            }
            btnSearch.IsEnabled = true;
            btnDelete.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnUpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            if (bwUpdateOrders.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnUpdateOrder.IsEnabled = false;
                ordersToUpdate = dgOrder.Items.OfType<OrdersModel>().FirstOrDefault();
                bwUpdateOrders.RunWorkerAsync();
            }
        }

        private void bwUpdateOrders_DoWork(object sender, DoWorkEventArgs e)
        {
            ordersToUpdate.ProductNo = productNo;
            OrdersController.Update(ordersToUpdate);
        }

        private void bwUpdateOrders_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnUpdateOrder.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Updated!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnUpdateSizeRun_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            sizeRunToUpdateList.Clear();
            dt = ((DataView)dgSizeRun.ItemsSource).ToTable();
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    string sizeNo = sizeRunList[i].SizeNo;
                    int quantity = (Int32)dr[String.Format("Column{0}", i)];
                    if (quantity >= 0)
                    {
                        var model = new SizeRunModel
                        {
                            ProductNo = productNo,
                            SizeNo = sizeNo,
                            Quantity = quantity,
                        };
                        sizeRunToUpdateList.Add(model);
                    }
                }
            }
            if (bwUpdateSizeRun.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnUpdateSizeRun.IsEnabled = false;
                bwUpdateSizeRun.RunWorkerAsync();
            }
        }

        private void bwUpdateSizeRun_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var model in sizeRunToUpdateList)
            {
                SizeRunController.Insert(model);
            }
        }

        private void bwUpdateSizeRun_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnUpdateSizeRun.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Updated!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtProductNo.Text) == true ||
                MessageBox.Show("Confirm Delete?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            if (bwDelete.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                productNo = txtProductNo.Text;
                btnSearch.IsEnabled = false;
                btnDelete.IsEnabled = false;
                bwDelete.RunWorkerAsync();
            }
        }

        private void bwDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            OrdersController.Delete(productNo);
        }

        private void bwDelete_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnDelete.IsEnabled = true;
            btnSearch.IsEnabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            orders = null;
            dgOrder.ItemsSource = null;
            btnUpdateOrder.IsEnabled = false;
            dgSizeRun.ItemsSource = null;
            btnUpdateSizeRun.IsEnabled = false;
            MessageBox.Show("Deleted!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        string outsoleCode = "";
        private void btnSearchOutsoleCode_Click(object sender, RoutedEventArgs e)
        {
            outsoleCode = txtOutsoleCode.Text.Trim().ToUpper().ToString();
            txtOutsoleCode.Text = outsoleCode;
            if (String.IsNullOrEmpty(outsoleCode) == true)
            {
                return;
            }
            if (bwSearchOutsoleCode.IsBusy == false)
            {
                dgSizeMap.ItemsSource = null;
                dtSizeMap = new DataTable();
                this.Cursor = Cursors.Wait;
                btnSearchOutsoleCode.IsEnabled = false;
                btnUpdateSizeMap.IsEnabled = false;
                prgStatusUpdateOutsoleCode.Value = 0;

                bwSearchOutsoleCode.RunWorkerAsync();
            }
        }

        DataTable dtSizeMap = new DataTable();
        private void bwSearchOutsoleCode_DoWork(object sender, DoWorkEventArgs e)
        {
            orderList = OrdersController.Select();
            sizeRunPerOutsoleCodeList = SizeRunController.SelectPerOutsoleCode(outsoleCode).Where(w => w.UpdateOutsoleSizeByArticle == false).ToList();
            var outsoleCodeList = orderList.Select(s => s.OutsoleCode).Distinct().ToList();

            Dispatcher.Invoke(new Action(() =>
            {

                dgSizeMap.Columns.Clear();
                if (outsoleCodeList.Contains(outsoleCode) == false)
                {
                    MessageBox.Show("Not Found !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    txtOutsoleCode.SelectAll();
                    txtOutsoleCode.Focus();
                    return;
                }
                FillDataSearch(dtSizeMap, dgSizeMap, sizeRunPerOutsoleCodeList);
            }));
        }

        private void bwSearchOutsoleCode_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgSizeMap.ItemsSource = dtSizeMap.AsDataView();
            this.Cursor = null;
            btnSearchOutsoleCode.IsEnabled = true;
            btnUpdateSizeMap.IsEnabled = true;
        }

        private void FillDataSearch(DataTable dt, DataGrid dg, List<SizeRunModel> sizeRunList)
        {
            var regex = new Regex("[a-z]|[A-Z]");
            var sizeNoList = sizeRunList.Select(s => s.SizeNo).Distinct().OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "")) : Double.Parse(s)).ToList();

            // Create Column
            dt.Columns.Add("TypeSize", typeof(string));
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Order Size";
            column.MinWidth = 60;
            column.Binding = new Binding("TypeSize");
            column.IsReadOnly = true;
            dg.Columns.Add(column);

            for (int i = 0; i < sizeNoList.Count; i++)
            {
                string sizeNoBinding = sizeNoList[i].Contains(".") ? sizeNoList[i].Replace(".", "@") : sizeNoList[i];

                dt.Columns.Add(String.Format("Column{0}", sizeNoBinding), typeof(String));
                var columnSize = new DataGridTextColumn();
                columnSize.Header = sizeNoList[i];
                columnSize.MinWidth = 40;
                columnSize.Binding = new Binding(String.Format("Column{0}", sizeNoBinding));

                Style style = new Style(typeof(DataGridCell));
                style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                columnSize.CellStyle = style;

                dg.Columns.Add(columnSize);
            }

            // Fill Data
            DataRow drOutsoleSize = dt.NewRow();
            DataRow drMidsoleSize = dt.NewRow();
            drOutsoleSize["TypeSize"] = "Outsole Size";
            drMidsoleSize["TypeSize"] = "Midsole Size";
            for (int i = 0; i < sizeNoList.Count; i++)
            {
                var sizeRun = sizeRunList.Where(w => w.SizeNo == sizeNoList[i]).FirstOrDefault();
                string sizeNoBinding = sizeNoList[i].Contains(".") ? sizeNoList[i].Replace(".", "@") : sizeNoList[i];
                if (sizeRun != null)
                {
                    drOutsoleSize[String.Format("Column{0}", sizeNoBinding)] = sizeRun.OutsoleSize;
                    drMidsoleSize[String.Format("Column{0}", sizeNoBinding)] = sizeRun.MidsoleSize;
                }
            }
            dt.Rows.Add(drOutsoleSize);
            dt.Rows.Add(drMidsoleSize);
        }

        DataTable dtSizeMapUpdate;
        private void btnUpdateSizeMap_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            if (bwUpdateSizeMap.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                dtSizeMapUpdate = new DataTable();
                sizeRunMapToUpdateList = new List<SizeRunModel>();
                btnUpdateSizeMap.IsEnabled = false;
                prgStatusUpdateOutsoleCode.Value = 0;

                bwUpdateSizeMap.RunWorkerAsync();
            }
        }

        private void bwUpdateSizeMap_DoWork(object sender, DoWorkEventArgs e)
        {
            GetDataFromTable(dgSizeMap, sizeRunPerOutsoleCodeList, false);

            Dispatcher.Invoke(new Action(() =>
            {
                prgStatusUpdateOutsoleCode.Maximum = sizeRunMapToUpdateList.Count;
            }));
            int indexProgressBar = 1;
            foreach (var sizeRunMapUpdate in sizeRunMapToUpdateList)
            {
                SizeRunController.UpdateSizeMap(sizeRunMapUpdate);
                Dispatcher.Invoke(new Action(() =>
                {
                    prgStatusUpdateOutsoleCode.Value = indexProgressBar;
                }));
                indexProgressBar++;
            }
        }

        private void bwUpdateSizeMap_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnUpdateSizeMap.IsEnabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Updating Result", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Updated!", "Updating Result", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnCheckOutsoleCodeNotYetUpdateSizeOutsole_Click(object sender, RoutedEventArgs e)
        {
            if (bwCheckOutsoleCode.IsBusy == false)
            {
                btnCheckOutsoleCodeNotYetUpdateSizeOutsole.IsEnabled = false;
                this.Cursor = Cursors.Wait;

                bwCheckOutsoleCode.RunWorkerAsync();
            }
        }

        private void bwCheckOutsoleCode_DoWork(object sender, DoWorkEventArgs e)
        {
            var sizeRunList = SizeRunController.SelectIsEnable().Where(w => String.IsNullOrEmpty(w.OutsoleSize) == true).ToList();
            var orderList = OrdersController.Select();
            Dispatcher.Invoke(new Action(() =>
            {
                if (sizeRunList.Count == 0)
                {
                    MessageBox.Show("Empty !", "Checking Result", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Cursor = null;
                    btnCheckOutsoleCodeNotYetUpdateSizeOutsole.IsEnabled = true;
                    return;
                }

                var productNoList = sizeRunList.Select(s => s.ProductNo).Distinct().ToList();
                var outsoleCodeList = orderList.Where(w => productNoList.Contains(w.ProductNo)).Select(s => s.OutsoleCode).Distinct().ToList();
                MessageBox.Show(String.Format("Outsole Code List:\n{0}", String.Join("\n", outsoleCodeList)), "Checking Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }));
        }

        private void bwCheckOutsoleCode_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnCheckOutsoleCodeNotYetUpdateSizeOutsole.IsEnabled = true;
        }

        private void txtProductNo_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            btnSearchOutsoleCode.IsDefault = false;
            btnSearchArticle.IsDefault = false;
            btnSearch.IsDefault = true;
        }

        private void txtOutsoleCode_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            btnSearch.IsDefault = false;
            btnSearchArticle.IsDefault = false;
            btnSearchOutsoleCode.IsDefault = true;
        }

        private void txtArticleNo_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            btnSearch.IsDefault = false;
            btnSearchArticle.IsDefault = true;
            btnSearchOutsoleCode.IsDefault = false;
        }

        string articleNo = "";
        DataTable dtSizeMapByArticle;
        private void btnSearchArticle_Click(object sender, RoutedEventArgs e)
        {
            articleNo = txtArticleNo.Text.Trim().ToString();
            if (String.IsNullOrEmpty(articleNo) == true)
            {
                return;
            }
            if (bwSearchArticleNo.IsBusy == false)
            {
                dgSizeMapByArticle.ItemsSource = null;
                dtSizeMapByArticle = new DataTable();
                this.Cursor = Cursors.Wait;

                btnSearchArticle.IsEnabled = false;
                btnUpdateSizeMapByArticle.IsEnabled = false;
                prgStatusUpdateOutsoleCode.Value = 0;

                bwSearchArticleNo.RunWorkerAsync();
            }
        }

        private void bwSearchArticleNo_DoWork(object sender, DoWorkEventArgs e)
        {
            orderList = OrdersController.Select();
            sizeRunPerArticleList = SizeRunController.SelectPerArticle(articleNo);
            var ordersList_Article = orderList.Where(w => w.ArticleNo.Contains("-") ? w.ArticleNo.Split('-')[0].ToString().Contains(articleNo) == true : w.ArticleNo.Contains(articleNo) == true).ToList();
            
            Dispatcher.Invoke(new Action(() =>
            {
                dgSizeMapByArticle.Columns.Clear();
                if (ordersList_Article.Count == 0)
                {
                    MessageBox.Show("Not Found !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    txtArticleNo.SelectAll();
                    txtArticleNo.Focus();
                    return;
                }
                FillDataSearch(dtSizeMapByArticle, dgSizeMapByArticle, sizeRunPerArticleList);
            }));
        }

        private void bwSearchArticleNo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgSizeMapByArticle.ItemsSource = dtSizeMapByArticle.AsDataView();
            this.Cursor = null;
            btnSearchArticle.IsEnabled = true;
            btnUpdateSizeMapByArticle.IsEnabled = true;
        }

        private void btnUpdateSizeMapByArticle_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            if (bwUpdateSizeMap.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                dtSizeMapByArticle = new DataTable();
                sizeRunMapToUpdateList = new List<SizeRunModel>();
                btnUpdateSizeMapByArticle.IsEnabled = false;
                prgStatusUpdateOutsoleCode.Value = 0;

                bwUpdateSizeMapByArticleNo.RunWorkerAsync();
            }
        }

        private void bwUpdateSizeMapByArticleNo_DoWork(object sender, DoWorkEventArgs e)
        {
            GetDataFromTable(dgSizeMapByArticle, sizeRunPerArticleList, true);
            Dispatcher.Invoke(new Action(() =>
            {
                prgStatusUpdateOutsoleCode.Maximum = sizeRunMapToUpdateList.Count;
            }));
            int indexProgressBar = 1;
            foreach (var sizeRunMapUpdate in sizeRunMapToUpdateList)
            {
                SizeRunController.UpdateSizeMap(sizeRunMapUpdate);
                Dispatcher.Invoke(new Action(() =>
                {
                    prgStatusUpdateOutsoleCode.Value = indexProgressBar;
                }));
                indexProgressBar++;
            }
        }

        private void bwUpdateSizeMapByArticleNo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnUpdateSizeMapByArticle.IsEnabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Updating Result", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Updated!", "Updating Result", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GetDataFromTable(DataGrid dg, List<SizeRunModel> sizeRunList, bool updateOutsoleSizeByArticle)
        {
            var dt = new DataTable();
            dt = ((DataView)(dg.ItemsSource)).ToTable();
            var regex = new Regex("[a-z]|[A-Z]");
            var sizeNoList = sizeRunList.Select(s => s.SizeNo).Distinct().OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "")) : Double.Parse(s)).ToList();
            var productNoListUpdate = sizeRunList.Select(s => s.ProductNo).Distinct().ToList();

            var outsoleSizeRunList = new List<SizeRunModel>();
            var midsoleSizeRunList = new List<SizeRunModel>();
            foreach (DataRow dr in dt.Rows)
            {
                string typeSize = dr["TypeSize"].ToString();
                foreach (var productNo in productNoListUpdate)
                {
                    var sizeRunPerPO = sizeRunList.Where(w => w.ProductNo == productNo).ToList();
                    for (int i = 0; i < sizeNoList.Count; i++)
                    {
                        string sizeNoBinding = sizeNoList[i].Contains(".") ? sizeNoList[i].Replace(".", "@") : sizeNoList[i];
                        var sizeRunPerSize = sizeRunPerPO.Where(w => w.SizeNo == sizeNoList[i]).FirstOrDefault();
                        if (sizeRunPerSize == null)
                        {
                            continue;
                        }
                        string outsoleSize = "", midsoleSize = "";
                        if (typeSize.Contains("Outsole Size"))
                        {
                            outsoleSize = dr[String.Format("Column{0}", sizeNoBinding)].ToString();
                            var outsoleSizeRun = new SizeRunModel()
                            {
                                ProductNo = productNo,
                                SizeNo = sizeNoList[i],
                                OutsoleSize = outsoleSize,
                            };
                            outsoleSizeRunList.Add(outsoleSizeRun);
                        }
                        if (typeSize.Contains("Midsole Size"))
                        {
                            midsoleSize = dr[String.Format("Column{0}", sizeNoBinding)].ToString();
                            var midsoleSizeRun = new SizeRunModel()
                            {
                                ProductNo = productNo,
                                SizeNo = sizeNoList[i],
                                MidsoleSize = midsoleSize,
                            };
                            midsoleSizeRunList.Add(midsoleSizeRun);
                        }
                    }
                }
            }
            foreach (var sizeRun in outsoleSizeRunList)
            {
                var sizeRunMapUpdate = new SizeRunModel()
                {
                    ProductNo = sizeRun.ProductNo,
                    SizeNo = sizeRun.SizeNo,
                    MidsoleSize = midsoleSizeRunList.Where(w => w.ProductNo == sizeRun.ProductNo && w.SizeNo == sizeRun.SizeNo).FirstOrDefault().MidsoleSize,
                    OutsoleSize = sizeRun.OutsoleSize,
                    UpdateOutsoleSizeByArticle = updateOutsoleSizeByArticle
                };
                sizeRunMapToUpdateList.Add(sizeRunMapUpdate);
            }
        }
    }
}
