using MasterSchedule.Controllers;
using MasterSchedule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OutsoleInputMaterialDetailWindow.xaml
    /// </summary>
    public partial class OutsoleInputMaterialDetailWindow : Window
    {
        BackgroundWorker bwLoad;
        BackgroundWorker bwAddMore;
        BackgroundWorker bwInsert;
        BackgroundWorker bwLoadDetail;
        BackgroundWorker bwUpdateOutsoleMaterial;

        List<OutsoleMaterialModel> outsoleMaterialList;
        List<OutsoleMaterialModel> outsoleMaterialRejectUpdateList;
        List<OutsoleMaterialDetailModel> outsoleMaterialDetailList;
        List<OutsoleMaterialDetailModel> outsoleMaterialReloadList;
        List<OutsoleSuppliersModel> outsoleSupplierList;
        List<SizeRunModel> sizeRunList;
        OrdersModel orderSearch;
        AccountModel account;
        List<AccountModel> accountList;
        OutsoleSuppliersModel supplierClicked;

        DateTime dtDefault;

        DispatcherTimer timer;

        List<OutsoleMaterialDetailModel> outsoleMaterialDetailToInsertList;

        String _REJECT = "Hàng phế", _QUANTITY = "Số lượng kiểm", _ARRIVAL = "Hàng về", _TOTAL = "Tổng";

        public OutsoleInputMaterialDetailWindow(AccountModel account)
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwLoadDetail = new BackgroundWorker();
            bwLoadDetail.DoWork +=new DoWorkEventHandler(bwLoadDetail_DoWork);
            bwLoadDetail.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoadDetail_RunWorkerCompleted);

            bwAddMore = new BackgroundWorker();
            bwAddMore.DoWork += new DoWorkEventHandler(bwAddMore_DoWork);
            bwAddMore.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwAddMore_RunWorkerCompleted);

            bwInsert = new BackgroundWorker();
            bwInsert.DoWork +=new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);

            bwUpdateOutsoleMaterial = new BackgroundWorker();
            bwUpdateOutsoleMaterial.DoWork +=new DoWorkEventHandler(bwUpdateOutsoleMaterial_DoWork);
            bwUpdateOutsoleMaterial.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwUpdateOutsoleMaterial_RunWorkerCompleted);

            outsoleMaterialList = new List<OutsoleMaterialModel>();
            outsoleMaterialRejectUpdateList = new List<OutsoleMaterialModel>();
            outsoleMaterialDetailList = new List<OutsoleMaterialDetailModel>();
            outsoleMaterialReloadList = new List<OutsoleMaterialDetailModel>();
            outsoleSupplierList = new List<OutsoleSuppliersModel>();

            outsoleMaterialDetailToInsertList = new List<OutsoleMaterialDetailModel>();

            sizeRunList = new List<SizeRunModel>();
            accountList = new List<AccountModel>();

            orderSearch = new OrdersModel();
            supplierClicked = new OutsoleSuppliersModel();

            dtDefault = new DateTime(2000, 1, 1);

            this.account = account;

            timer = new DispatcherTimer();
            timer.Start();
            timer.Tick += new EventHandler(timer_Tick);

            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            txtUser.Text = string.Format("User : {0}", account.FullName);
            txtTimer.Text = string.Format("Clock: {0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
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
                btnSearch.IsEnabled = false;
                productNo = txtProductNo.Text.ToUpper().ToString();
                txtProductNo.Text = productNo;

                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            sizeRunList = SizeRunController.Select(productNo);
            outsoleMaterialList = OutsoleMaterialController.Select(productNo);
            outsoleSupplierList = OutsoleSuppliersController.Select();
            outsoleMaterialDetailList = OutsoleMaterialDetailController.Select(productNo);
            orderSearch = OrdersController.SelectTop1(productNo);
            accountList = AccountController.SelectAccount();
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnSearch.IsEnabled = true;

            txtIndexNo.IsEnabled = false;
            btnSave.IsEnabled = false;

            // Clear value
            stkSuppliers.Children.Clear();

            tblArticleNo.Text = "";
            tblStyle.Text = "";
            tblOSCode.Text = "";
            tblTotalPairs.Text = "";
            txtCreatedBy.Text = "";

            dgAdd.ItemsSource = null;
            dgAdd.Columns.Clear();

            spLoadDetail.Children.Clear();
            spTitle.Children.Clear();
            groupLoadDetail.Visibility = Visibility.Collapsed;

            btnAddIndexNo.IsEnabled = false;
            btnAddIndexNo.IsDefault = false;
            txtIndexNo.Clear();
            groupAddIndexNo.Header = "Supplier (Nhà Cung Cấp)";


            if (outsoleMaterialList.Count() == 0 || orderSearch == null)
            {
                MessageBox.Show("Not Found !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                txtProductNo.Focus();
                txtProductNo.SelectAll();
                return;
            }

            tblArticleNo.Text = orderSearch.ArticleNo;
            tblStyle.Text = orderSearch.ShoeName;
            tblOSCode.Text = orderSearch.OutsoleCode;
            tblTotalPairs.Text = orderSearch.Quantity.ToString();

            var outsoleMaterialSupplierIdPerPOList = outsoleMaterialList.OrderBy(o => o.OutsoleSupplierId).Select(s => s.OutsoleSupplierId).Distinct().ToList();

            // Create Supplier Button
            foreach (var supplierId in outsoleMaterialSupplierIdPerPOList)
            {
                var outsoleMaterialDetailPerSupplier = outsoleMaterialDetailList.Where(w => w.OutsoleSupplierId == supplierId).ToList();

                var supplier = outsoleSupplierList.FirstOrDefault(f => f.OutsoleSupplierId == supplierId);
                Button btnSupplier = new Button();


                btnSupplier.FontSize = 16;
                btnSupplier.MinHeight = 30;
                btnSupplier.Margin = new Thickness(0, 0, 10, 0);
                btnSupplier.ToolTip = "Supplier";
                btnSupplier.Tag = supplierId;
                btnSupplier.Content = supplier == null ? "" : supplier.Name;

                var style = FindResource("MyButton") as Style;
                if (outsoleMaterialDetailPerSupplier.Select(s => s.Quantity).Sum() >= sizeRunList.Select(s => s.Quantity).Sum())
                {
                    style = (Style)FindResource("MyButtonFinished");
                }
                else if (outsoleMaterialDetailPerSupplier.Select(s => s.Quantity).Sum() != 0 &&
                    outsoleMaterialDetailPerSupplier.Select(s => s.Quantity).Sum() < sizeRunList.Select(s => s.Quantity).Sum())
                {
                    style = (Style)FindResource("MyButtonInProcess");
                }
                btnSupplier.Style = style;

                btnSupplier.Click += new RoutedEventHandler(btnSupplier_Click);
                stkSuppliers.Children.Add(btnSupplier);
            }

            // Create Summary
            if (outsoleMaterialDetailList.Count > 0)
            {
                groupLoadDetail.Visibility = Visibility.Visible;
                // Title
                spTitle.Children.Add(CreateDataGridTitle());

                // Grid
                var suppliedIdList = outsoleMaterialDetailList.Select(s => s.OutsoleSupplierId).Distinct().ToList();
                foreach (var suppliedId in suppliedIdList)
                {
                    var supplierName = outsoleSupplierList.FirstOrDefault(f => f.OutsoleSupplierId == suppliedId) == null ? "" : outsoleSupplierList.FirstOrDefault(f => f.OutsoleSupplierId == suppliedId).Name;
                    var spTitleDetail = new StackPanel();
                    spTitleDetail.Orientation = Orientation.Horizontal;
                    spTitleDetail.Margin = new Thickness(0, 5, 0, 0);

                    var txtTitleDetail = new TextBlock();
                    txtTitleDetail.Text = String.Format("Supplier (Nhà cung cấp):  {0}", supplierName);
                    txtTitleDetail.FontWeight = FontWeights.Bold;
                    txtTitleDetail.FontStyle = FontStyles.Italic;
                    txtTitleDetail.FontSize = 14;

                    spTitleDetail.Children.Add(txtTitleDetail);

                    //Create grid
                    var dtSummary = new DataTable();
                    var dgSummary = new DataGrid();
                    CreateGridShowData(dgSummary, dtSummary);

                    // Fill Data
                    var drQuantity = dtSummary.NewRow();
                    var drReject = dtSummary.NewRow();
                    drQuantity["Status"] = _QUANTITY;
                    drReject["Status"] = _REJECT;

                    var outsoleMaterialDetailPerSupplierList = outsoleMaterialDetailList.Where(w => w.OutsoleSupplierId == suppliedId).ToList();
                    for (int i = 0; i <= sizeRunList.Count - 1; i++)
                    {
                        SizeRunModel sizeRun = sizeRunList[i];
                        var outsoleMaterialDetailPerSupplierPerSizeList = outsoleMaterialDetailPerSupplierList.Where(w => w.SizeNo == sizeRun.SizeNo).ToList();
                        if (outsoleMaterialDetailPerSupplierPerSizeList.Count == 0)
                            continue;
                        int qty = 0, reject = 0;
                        qty = outsoleMaterialDetailPerSupplierPerSizeList.Sum(s => s.Quantity);
                        reject = outsoleMaterialDetailPerSupplierPerSizeList.Sum(s => s.Reject);

                        if (qty > 0)
                            drQuantity[String.Format("Column{0}", i)] = qty;
                        if (reject > 0)
                            drReject[String.Format("Column{0}", i)] = reject;
                    }

                    string qtyTotalString = "", rejectTotalString = "";
                    if (outsoleMaterialDetailPerSupplierList.Sum(s => s.Quantity) > 0)
                        qtyTotalString = outsoleMaterialDetailPerSupplierList.Sum(s => s.Quantity).ToString();
                    if (outsoleMaterialDetailPerSupplierList.Sum(s => s.Reject) > 0)
                        rejectTotalString = outsoleMaterialDetailPerSupplierList.Sum(s => s.Reject).ToString();

                    drQuantity["Total"] = qtyTotalString;
                    drReject["Total"] = rejectTotalString;
                    dtSummary.Rows.Add(drQuantity);
                    dtSummary.Rows.Add(drReject);

                    dgSummary.ItemsSource = dtSummary.AsDataView();

                    spLoadDetail.Children.Add(spTitleDetail);
                    spLoadDetail.Children.Add(dgSummary);
                }
            }
        }

        private void btnSupplier_Click(object sender, RoutedEventArgs e)
        {
            var btnSupplierClicked = sender as Button;
            int supplierIdClicked = (int)btnSupplierClicked.Tag;
            supplierClicked = outsoleSupplierList.FirstOrDefault(f => f.OutsoleSupplierId == supplierIdClicked);

            groupAddIndexNo.Header = "Nhập Số Liệu Kiểm Kê:   " + supplierClicked.Name;
            btnAddIndexNo.IsEnabled = true;
            btnAddIndexNo.IsDefault = true;

            txtIndexNo.IsEnabled = true;
            txtIndexNo.Focus();

            txtCreatedBy.Text = "";
            dgAdd.Columns.Clear();
            dgAdd.ItemsSource = null;

            btnSave.IsEnabled = false;

            if (bwLoad.IsBusy == false)
            {
                this.Cursor = null;
                bwLoadDetail.RunWorkerAsync();
            }
        }

        string INDEXNO = "";
        private void btnAddIndexNo_Click(object sender, RoutedEventArgs e)
        {
            INDEXNO = txtIndexNo.Text.Trim().ToString();
            if (String.IsNullOrEmpty(INDEXNO) == true)
                return;

            popupAddMore.IsOpen = true;
            txtRound.Focus();
            txtRound.SelectAll();

            btnAddIndexNo.IsEnabled = false;
            btnAddMore.IsEnabled = true;
            btnAddMore.IsDefault = true;
        }

        int ROUND = 0;
        DataTable dtAdd = new DataTable();
        private void btnAddMore_Click(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(txtRound.Text.Trim().ToString(), out ROUND);
            if (ROUND == 0)
            {
                txtRound.Focus();
                return;
            }
            if (bwAddMore.IsBusy == false)
            {
                dtAdd = new DataTable();
                dgAdd.Columns.Clear();
                dgAdd.ItemsSource = null;

                this.Cursor = Cursors.Wait;
                bwAddMore.RunWorkerAsync();
            }
        }

        private void bwAddMore_DoWork(object sender, DoWorkEventArgs e)
        {
            // Excute Store.
            outsoleMaterialDetailList = OutsoleMaterialDetailController.Select(productNo).Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId && w.IndexNo == INDEXNO && w.Round == ROUND).ToList();

            // Create column
            Dispatcher.Invoke(new Action(() => {

                dtAdd.Columns.Add("Title", typeof(String));
                DataGridTextColumn colTitle = new DataGridTextColumn();
                colTitle.Header = String.Format("Nhà cung cấp\n{0}\nCông đoạn: {1}\nLần kiểm: {2}", supplierClicked.Name, INDEXNO, ROUND.ToString());
                colTitle.IsReadOnly = true;
                colTitle.Binding = new Binding("Title");
                dgAdd.Columns.Add(colTitle);

                dtAdd.Columns.Add("Status", typeof(String));
                DataGridTextColumn colStatus = new DataGridTextColumn();
                colStatus.Header = "Order Size\nO/S Size\nM/S Size\nSố lượng";
                colStatus.IsReadOnly = true;
                colStatus.Binding = new Binding("Status");
                dgAdd.Columns.Add(colStatus);

                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    SizeRunModel sizeRun = sizeRunList[i];
                    dtAdd.Columns.Add(String.Format("Column{0}", i, typeof(String)));
                    DataGridTextColumn col = new DataGridTextColumn();
                    col.SetValue(TagProperty, sizeRun.SizeNo);
                    col.Header = String.Format("{0}\n{1}\n{2}\n({3})", sizeRun.SizeNo, sizeRun.OutsoleSize, sizeRun.MidsoleSize, sizeRun.Quantity);
                    col.MinWidth = 40;
                    col.Binding = new Binding(String.Format("Column{0}", i));
                    dgAdd.Columns.Add(col);
                }

                DataGridTemplateColumn buttonColumn = new DataGridTemplateColumn();
                DataTemplate buttonTemplate = new DataTemplate();
                FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
                buttonTemplate.VisualTree = buttonFactory;
                buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(btnOK_Click));
                buttonFactory.SetValue(ContentProperty, "OK");
                buttonColumn.CellTemplate = buttonTemplate;
                dgAdd.Columns.Add(buttonColumn);

            }));
        }

        private void bwAddMore_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Fill Data
            DataRow drArrival = dtAdd.NewRow();
            DataRow drQuantity = dtAdd.NewRow();
            DataRow drReject = dtAdd.NewRow();

            drArrival["Status"] = _ARRIVAL;
            drQuantity["Status"] = _QUANTITY;
            drReject["Status"] = _REJECT;

            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                var sizeRun = sizeRunList[i];

                var outsoleMaterialPerSize = outsoleMaterialList.FirstOrDefault(f => f.SizeNo == sizeRun.SizeNo);
                if (outsoleMaterialPerSize != null && outsoleMaterialPerSize.Quantity != 0)
                {
                    drArrival[String.Format("Column{0}", i)] = outsoleMaterialPerSize.Quantity;
                }

                var outsoleMaterialDetailPerSize = outsoleMaterialDetailList.FirstOrDefault(f => f.SizeNo == sizeRun.SizeNo);
                if (outsoleMaterialDetailPerSize != null && outsoleMaterialDetailPerSize.Quantity != 0)
                {
                    drQuantity[String.Format("Column{0}", i)] = outsoleMaterialDetailPerSize.Quantity;
                }
                if (outsoleMaterialDetailPerSize != null && outsoleMaterialDetailPerSize.Reject != 0)
                {
                    drReject[String.Format("Column{0}", i)] = outsoleMaterialDetailPerSize.Reject;
                }
            }

            dtAdd.Rows.Add(drArrival);
            dtAdd.Rows.Add(drQuantity);
            dtAdd.Rows.Add(drReject);

            dgAdd.ItemsSource = dtAdd.AsDataView();

            // UI
            popupAddMore.IsOpen = false;
            btnAddMore.IsEnabled = false;
            txtIndexNo.Focus();

            btnSave.IsEnabled = true;
            this.Cursor = null;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var outsoleMaterialDetailQuantityList = new List<OutsoleMaterialDetailModel>();
            var outsoleMaterialDetailRejectList = new List<OutsoleMaterialDetailModel>();

            DataTable dtInsert = ((DataView)dgAdd.ItemsSource).ToTable();
            if (dtInsert == null)
                return;
            foreach (DataRow dr in dtInsert.Rows)
            {
                string status = dr["Status"].ToString();
                if (status.Contains(_ARRIVAL))
                    continue;

                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    var sizeRun = sizeRunList[i];
                    var materialDetailQty = new OutsoleMaterialDetailModel()
                    {
                        ProductNo = productNo,
                        OutsoleSupplierId = supplierClicked.OutsoleSupplierId,
                        SizeNo = sizeRun.SizeNo,
                        CreatedBy = account.UserName,
                        IndexNo = INDEXNO,
                        Round = ROUND,
                    };
                    int qty = 0, reject = 0;
                    if (status.Contains(_QUANTITY))
                    {
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out qty);
                        materialDetailQty.Quantity = qty;
                        outsoleMaterialDetailQuantityList.Add(materialDetailQty);
                    }
                    if (status.Contains(_REJECT))
                    {
                        Int32.TryParse(dr[String.Format("Column{0}", i)].ToString(), out reject);
                        materialDetailQty.Reject = reject;
                        outsoleMaterialDetailRejectList.Add(materialDetailQty);
                    }
                }
            }

            foreach (var outsoleMaterial in outsoleMaterialDetailQuantityList)
            {
                var outsoleMaterialDetailInsert = outsoleMaterial;
                var reject = outsoleMaterialDetailRejectList.FirstOrDefault(f => f.SizeNo == outsoleMaterial.SizeNo).Reject;
                var quantity = outsoleMaterial.Quantity;

                if (outsoleMaterial.Quantity == 0 && reject == 0)
                    continue;
                outsoleMaterialDetailInsert.Quantity = quantity;
                outsoleMaterialDetailInsert.Reject = reject;

                outsoleMaterialDetailToInsertList.Add(outsoleMaterialDetailInsert);
            }

            if (bwInsert.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnSave.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }
        }
        
        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var insertModel in outsoleMaterialDetailToInsertList)
            {
                OutsoleMaterialDetailController.Insert(insertModel, account);
            }
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnSave.IsEnabled = true;

            if (bwLoadDetail.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadDetail.RunWorkerAsync();
            }

            if (bwUpdateOutsoleMaterial.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwUpdateOutsoleMaterial.RunWorkerAsync();
            }
            if (e.Error == null)
                MessageBox.Show("Insert Successful (Đã Thêm) !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("An Error Occurred !", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void bwUpdateOutsoleMaterial_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMaterialRejectUpdateList = new List<OutsoleMaterialModel>();
            var outsoleMaterialDetailAferInsertList = OutsoleMaterialDetailController.Select(productNo).Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
            var sizeNoList = outsoleMaterialDetailAferInsertList.Select(s => s.SizeNo).Distinct().ToList();
            foreach (var sizeNo in sizeNoList)
            {
                var firstModel = outsoleMaterialDetailAferInsertList.FirstOrDefault();
                var outsoleMaterialUpdateReject = new OutsoleMaterialModel()
                {
                    ProductNo = firstModel.ProductNo,
                    OutsoleSupplierId = firstModel.OutsoleSupplierId,
                    SizeNo = sizeNo,
                    QuantityReject = outsoleMaterialDetailAferInsertList.Where(w => w.SizeNo == sizeNo).Sum(s => s.Reject),
                };
                outsoleMaterialRejectUpdateList.Add(outsoleMaterialUpdateReject);
            }
            foreach (var updateModel in outsoleMaterialRejectUpdateList)
            {
                OutsoleMaterialController.UpdateRejectFromOutsoleMaterialDetail(updateModel);
            }

            // Update Outsole RawMaterial. If Reject > 0 => Remove Actual Date
            var POAndSupplierHasRejectList = outsoleMaterialRejectUpdateList.Where(w => w.QuantityReject > 0).Select(s => new { ProductNo = s.ProductNo, OutsoleSupplierId = s.OutsoleSupplierId }).Distinct().ToList();
            foreach (var p in POAndSupplierHasRejectList)
            {
                var updateModel = new OutsoleRawMaterialModel() {
                    ProductNo = p.ProductNo,
                    OutsoleSupplierId = p.OutsoleSupplierId,
                    ActualDate = dtDefault
                };
                OutsoleRawMaterialController.UpdateActualDate(updateModel);
            }
        }

        private void bwUpdateOutsoleMaterial_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
        }

        private void bwLoadDetail_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleMaterialReloadList = OutsoleMaterialDetailController.Select(productNo).Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId).ToList();
        }

        private DataGrid CreateDataGridTitle()
        {
            var dgTitle = new DataGrid();
            var columnStatus = new DataGridTextColumn();
            columnStatus.Header = "Order Size\nOutsole Size\nMidsole Size\nSố lượng";
            columnStatus.Width = 100;
            dgTitle.Columns.Add(columnStatus);
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                var sizeRun = sizeRunList[i];
                DataGridTextColumn col = new DataGridTextColumn();
                col.Header = String.Format("{0}\n{1}\n{2}\n({3})", sizeRun.SizeNo, sizeRun.OutsoleSize, sizeRun.MidsoleSize, sizeRun.Quantity);
                col.MinWidth = 40;
                dgTitle.Columns.Add(col);
            }

            var columnTotal = new DataGridTextColumn();
            columnTotal.Header = String.Format("\n\n{0}\n{1}", _TOTAL, sizeRunList.Sum(s => s.Quantity));
            dgTitle.Columns.Add(columnTotal);

            dgTitle.AutoGenerateColumns = false;
            dgTitle.CanUserAddRows = false;

            return dgTitle;
        }

        private void CreateGridShowData(DataGrid dgInput, DataTable dtInput)
        {
            dgInput.AutoGenerateColumns = false;
            dgInput.CanUserAddRows = false;
            dgInput.SelectionUnit = DataGridSelectionUnit.CellOrRowHeader;
            dgInput.HeadersVisibility = DataGridHeadersVisibility.None;

            dtInput.Columns.Add("Status", typeof(String));
            DataGridTextColumn colStatus = new DataGridTextColumn();
            colStatus.Header = "Order Size\nOutsole Size\nMidsole Size\nSố lượng";
            colStatus.Width = 100;
            colStatus.IsReadOnly = true;
            colStatus.Binding = new Binding("Status");
            dgInput.Columns.Add(colStatus);

            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dtInput.Columns.Add(String.Format("Column{0}", i, typeof(String)));
                DataGridTextColumn col = new DataGridTextColumn();
                col.SetValue(TagProperty, sizeRun.SizeNo);
                col.Header = String.Format("{0}\n{1}\n{2}\n({3})", sizeRun.SizeNo, sizeRun.OutsoleSize, sizeRun.MidsoleSize, sizeRun.Quantity);
                col.MinWidth = 40;
                col.IsReadOnly = true;
                col.Binding = new Binding(String.Format("Column{0}", i));
                dgInput.Columns.Add(col);
            }

            dtInput.Columns.Add("Total", typeof(String));
            var colTotal = new DataGridTextColumn();
            colTotal.Header = _TOTAL;
            colTotal.IsReadOnly = true;
            colTotal.Binding = new Binding("Total");

            Style styleColTotal = new Style(typeof(DataGridCell));
            styleColTotal.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
            styleColTotal.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
            colTotal.CellStyle = styleColTotal;

            dgInput.Columns.Add(colTotal);
        }

        DataTable dtLoadDetail;
        private void bwLoadDetail_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            spLoadDetail.Children.Clear();
            spTitle.Children.Clear();

            if (outsoleMaterialReloadList.Count == 0)
            {
                groupLoadDetail.Visibility = Visibility.Collapsed;
                return;
            }
            groupLoadDetail.Visibility = Visibility.Visible;

            // Create GridTitle
            spTitle.Children.Add(CreateDataGridTitle());

            // Create UI LoadDetail
            var indexNoList = outsoleMaterialReloadList.OrderBy(o => o.IndexNo).Select(s => s.IndexNo).Distinct().ToList();
            foreach (var indexNo in indexNoList)
            {
                var roundList = outsoleMaterialReloadList.Where(w => w.IndexNo == indexNo).OrderBy(o => o.IndexNo).Select(s => s.Round).Distinct().ToList();
                foreach (var round in roundList)
                {
                    var outsoleMaterialDetailPerIndexPerRoundList = outsoleMaterialReloadList.Where(w => w.IndexNo == indexNo && w.Round == round).ToList();

                    StackPanel spTitleDetail = new StackPanel();
                    spTitleDetail.Orientation = Orientation.Horizontal;
                    spTitleDetail.Margin = new Thickness(0, 5, 0, 0);

                    var txtTitle = new TextBlock();
                    txtTitle.Text = String.Format("IndexNo (Công đoạn): {0}     Round (Lần kiểm): {1}", indexNo, round);
                    txtTitle.FontWeight = FontWeights.Bold;
                    txtTitle.FontStyle = FontStyles.Italic;
                    txtTitle.FontSize = 14;

                    spTitleDetail.Children.Add(txtTitle);

                    // Create grid
                    dtLoadDetail = new DataTable();
                    var dgLoadDetail = new DataGrid();
                    dgLoadDetail.MouseLeftButtonDown +=new MouseButtonEventHandler(dgLoadDetail_MouseLeftButtonDown);
                    dgLoadDetail.Tag = string.Format("{0},{1}", indexNo, round);
                    CreateGridShowData(dgLoadDetail, dtLoadDetail);

                    // Fill Data
                    var drLoadDetailQty = dtLoadDetail.NewRow();
                    var drLoadDetailReject = dtLoadDetail.NewRow();
                    drLoadDetailQty["Status"] = _QUANTITY;
                    drLoadDetailReject["Status"] = _REJECT;
                    for (int i = 0; i <= sizeRunList.Count - 1; i++)
                    {
                        var sizeRun = sizeRunList[i];
                        var outsoleMaterialDetailPerIndexPerRoundPerSizeList = outsoleMaterialDetailPerIndexPerRoundList.Where(w => w.SizeNo == sizeRun.SizeNo).FirstOrDefault();
                        if (outsoleMaterialDetailPerIndexPerRoundPerSizeList == null)
                            continue;
                        int qtyLoadDetail = 0, rejectLoadDetail = 0;
                        qtyLoadDetail = outsoleMaterialDetailPerIndexPerRoundPerSizeList.Quantity;
                        rejectLoadDetail = outsoleMaterialDetailPerIndexPerRoundPerSizeList.Reject;

                        if (qtyLoadDetail != 0)
                            drLoadDetailQty[String.Format("Column{0}", i)] = qtyLoadDetail;
                        if (rejectLoadDetail != 0)
                            drLoadDetailReject[String.Format("Column{0}", i)] = rejectLoadDetail;
                    }
                    string totalQtyString = "", totalRejectString = "";
                    if (outsoleMaterialDetailPerIndexPerRoundList.Sum(s => s.Quantity) > 0)
                        totalQtyString = outsoleMaterialDetailPerIndexPerRoundList.Sum(s => s.Quantity).ToString();
                    if (outsoleMaterialDetailPerIndexPerRoundList.Sum(s => s.Reject) > 0)
                        totalRejectString = outsoleMaterialDetailPerIndexPerRoundList.Sum(s => s.Reject).ToString();

                    drLoadDetailQty["Total"] = totalQtyString;
                    drLoadDetailReject["Total"] = totalRejectString;

                    dtLoadDetail.Rows.Add(drLoadDetailQty);
                    dtLoadDetail.Rows.Add(drLoadDetailReject);

                    dgLoadDetail.ItemsSource = dtLoadDetail.AsDataView();

                    spLoadDetail.Children.Add(spTitleDetail);
                    spLoadDetail.Children.Add(dgLoadDetail);
                    svLoadDetail.ScrollToBottom();
                }
            }
            var outsoleMaterialDetailReloadFirstDefault = outsoleMaterialReloadList.FirstOrDefault();
            if (outsoleMaterialDetailReloadFirstDefault != null)
            {
                string revisePeople = accountList.Where(w => w.UserName == outsoleMaterialDetailReloadFirstDefault.CreatedBy).FirstOrDefault().FullName;
                txtCreatedBy.Text = string.Format("Revised People: {0}      Revised Time: {1}", revisePeople, outsoleMaterialDetailReloadFirstDefault.UpdatedTime);
            }

            this.Cursor = null;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (dgAdd.CurrentItem == null)
            {
                return;
            }
            var dr = ((DataRowView)dgAdd.CurrentItem).Row;
            if (dr == null)
            {
                return;
            }
            if (dr["Status"].ToString().Contains(_REJECT) || dr["Status"].ToString().Contains(_ARRIVAL))
            {
                return;
            }
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                var sizeRun = sizeRunList[i];
                dr[String.Format("Column{0}", i)] = sizeRun.Quantity;
            }
            dgAdd.ItemsSource = null;
            dgAdd.ItemsSource = dtAdd.AsDataView();
        }

        private void dgAdd_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.GetValue(TagProperty) == null)
            {
                return;
            }
            string sizeNo = e.Column.GetValue(TagProperty).ToString();
            if (sizeRunList.Select(s => s.SizeNo).Contains(sizeNo) == false)
            {
                return;
            }

            var rowViewClickedStatus = ((DataRowView)e.Row.Item)["Status"].ToString();
            int qtyOld = 0;
            if (rowViewClickedStatus == _QUANTITY)
            {
                qtyOld = outsoleMaterialDetailList.Where(w => w.SizeNo == sizeNo).Sum(s => s.Quantity);
            }
            //else if (rowViewClickedStatus == _REJECT)
            //{
            //    qtyOld = outsoleMaterialDetailList.Where(w => w.SizeNo == sizeNo).Sum(s => s.Reject);
            //    //.Where(w => w.OutsoleSupplierId == supplierClicked.OutsoleSupplierId &&
            //    //                                         w.ProductNo == productNo &&
            //    //                                         w.IndexNo == INDEXNO &&
            //    //                                         w.Round == ROUND)
            //}
            int qtyOrder = sizeRunList.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity);
            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyNew = 0;
            if (int.TryParse(txtCurrent.Text, out qtyNew) == true)
            {
                txtCurrent.Text = (qtyOld + qtyNew).ToString();

                if (qtyOld + qtyNew < 0)
                {
                    txtCurrent.Text = qtyOld.ToString();
                }
            }
        }
        
        private void dgLoadDetail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dataGridClicked = sender as DataGrid;
            var cellClicked = dgAdd.CurrentCell;
            var itemClicked = dataGridClicked.CurrentItem;
            var rowViewClicked = (DataRowView)itemClicked;

            if (cellClicked == null || rowViewClicked == null)
                return;
            string statusRow = rowViewClicked.Row.ItemArray[0].ToString();

            string tagClicked = dataGridClicked.Tag as String;
            if (String.IsNullOrEmpty(tagClicked) == false && tagClicked.Split(',').Count() > 1)
            {
                string indexNo = tagClicked.Split(',')[0].ToString();
                string roundString = tagClicked.Split(',')[1].ToString();
                int round = Int32.Parse(roundString);
                if (statusRow.Contains(_REJECT))
                {
                    var window = new OutsoleMaterialInputRejectDetailWindow(productNo, supplierClicked, sizeRunList, account, accountList, indexNo, round, true);
                    window.ShowDialog();
                }
            }
        }

        private void dgAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //DataGridCellInfo cellClicked = dgAdd.CurrentCell;
            //var itemClicked = dgAdd.CurrentItem;
            //DataRowView rowViewClicked = (DataRowView)itemClicked;

            //if (cellClicked == null || rowViewClicked == null)
            //    return;
            //string statusRow = rowViewClicked.Row.ItemArray[1].ToString();
            //if (statusRow.Contains(_REJECT))
            //{
            //    //InputRejectDetailWindow(rowViewClicked);
            //    OutsoleMaterialInputRejectDetailWindow window = new OutsoleMaterialInputRejectDetailWindow(productNo, supplierClicked, sizeRunList, account, accountList, INDEXNO, ROUND);
            //    window.ShowDialog();

            //    var totalRejectInputList = window.rejectDetailFromTableList;
            //    if (totalRejectInputList == null)
            //        return;

            //    // Fill Row Reject
            //    for (int i = 0; i <= sizeRunList.Count - 1; i++)
            //    {
            //        rowViewClicked[String.Format("Column{0}", i)] = "";
            //        int rejectPerSize = totalRejectInputList.Where(w => w.SizeNo == sizeRunList[i].SizeNo).Select(s => s.QuantityReject).Sum();
            //        if (rejectPerSize > 0)
            //            rowViewClicked[String.Format("Column{0}", i)] = rejectPerSize.ToString();
            //    }
            //    dgAdd.ItemsSource = dtAdd.AsDataView();
            //}
        }

        private void dgAdd_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            string status = "";
            var rowViewClicked = dgAdd.CurrentItem as DataRowView;
            status = rowViewClicked.Row[1].ToString();
            if (rowViewClicked == null || status == "")
                return;

            if (status.Contains(_REJECT))
            {
                //InputRejectDetailWindow(rowViewClicked);
                OutsoleMaterialInputRejectDetailWindow window = new OutsoleMaterialInputRejectDetailWindow(productNo, supplierClicked, sizeRunList, account, accountList, INDEXNO, ROUND, false);
                window.ShowDialog();

                var totalRejectInputList = window.rejectDetailFromTableList;
                if (totalRejectInputList == null)
                    return;

                // Fill Row Reject
                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    rowViewClicked[String.Format("Column{0}", i)] = "";
                    int rejectPerSize = totalRejectInputList.Where(w => w.SizeNo == sizeRunList[i].SizeNo).Select(s => s.QuantityReject).Sum();
                    if (rejectPerSize > 0)
                        rowViewClicked[String.Format("Column{0}", i)] = rejectPerSize.ToString();
                }
                dgAdd.ItemsSource = dtAdd.AsDataView();
            }
        }

        private void txtProductNo_GotFocus(object sender, RoutedEventArgs e)
        {
            txtProductNo.Focus();
            btnSearch.IsDefault = true;

            btnAddMore.IsEnabled = false;

            txtIndexNo.IsEnabled = false;
            btnAddIndexNo.IsEnabled = false;

            btnSave.IsEnabled = false;
        }

        private void txtIndexNo_GotFocus(object sender, RoutedEventArgs e)
        {
            txtIndexNo.Focus();
            txtIndexNo.SelectAll();

            btnSearch.IsDefault = false;

            btnAddIndexNo.IsEnabled = true;
            btnAddIndexNo.IsDefault = true;
        }

        private void txtRound_GotFocus(object sender, RoutedEventArgs e)
        {
            txtRound.Focus();
            txtRound.SelectAll();

            btnSearch.IsDefault = false;

            btnAddMore.IsDefault = true;
        }
    }
}
