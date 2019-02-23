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

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for InputOutsoleMarterialWindow.xaml
    /// </summary>
    public partial class OutsoleInputOutputWindow : Window
    {
        string productNo;
        BackgroundWorker bwLoadData;
        List<SizeRunModel> sizeRunList;
        DataTable dt;
        List<OutsoleOutputModel> outsoleOutputToInsertList;
        BackgroundWorker bwInsert;
        List<OutsoleOutputModel> outsoleOutputList;
        public string resultString;
        public string outsoleActualStartDateAuto;
        public string outsoleActualFinishDateAuto;
        List<OffDayModel> offDayList;
        public OutsoleInputOutputWindow(string productNo, string outsoleActualStartDateAuto, string outsoleActualFinishDateAuto)
        {
            this.productNo = productNo;
            this.outsoleActualStartDateAuto = outsoleActualStartDateAuto;
            this.outsoleActualFinishDateAuto = outsoleActualFinishDateAuto;
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            sizeRunList = new List<SizeRunModel>();
            dt = new DataTable();
            outsoleOutputToInsertList = new List<OutsoleOutputModel>();
            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);
            outsoleOutputList = new List<OutsoleOutputModel>();
            offDayList = new List<OffDayModel>();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = String.Format("{0} for {1}", this.Title, productNo);
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleOutputList = OutsoleOutputController.Select(productNo);
            sizeRunList = SizeRunController.Select(productNo);
            offDayList = OffDayController.SelectDate();
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //dt.Columns.Clear();
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dt.Columns.Add(String.Format("Column{0}", i), typeof(Int32));
                DataGridTextColumn column = new DataGridTextColumn();
                column.SetValue(TagProperty, sizeRun.SizeNo);
                column.Header = String.Format("{0}\n({1})", sizeRun.SizeNo, sizeRun.Quantity);
                column.MinWidth = 40;
                column.Binding = new Binding(String.Format("Column{0}", i));
                dgOutsoleMaterial.Columns.Add(column);
            }
            colCompleted.DisplayIndex = dgOutsoleMaterial.Columns.Count - 1;

            DataRow dr = dt.NewRow();
            for (int j = 0; j <= sizeRunList.Count - 1; j++)
            {
                SizeRunModel sizeRun = sizeRunList[j];
                dr[String.Format("Column{0}", j)] = outsoleOutputList.Where(o => o.SizeNo == sizeRun.SizeNo).Sum(o => o.Quantity);
            }
            dt.Rows.Add(dr);

            dgOutsoleMaterial.ItemsSource = dt.AsDataView();

            lblQtyTotal.Text = outsoleOutputList.Sum(s => s.Quantity).ToString();
            lblQtyOrder.Text = String.Format("/{0}", sizeRunList.Sum(s => s.Quantity));

            btnSave.IsEnabled = true;
            this.Cursor = null;
        }

        DateTime nowDate;
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            // get date
            nowDate = DateTime.Now.Date.AddDays(-1);
            while (offDayList.Select(s => s.Date).ToList().Contains(nowDate))
            {
                nowDate = nowDate.AddDays(-1);
            }

            dt = ((DataView)dgOutsoleMaterial.ItemsSource).ToTable();
            foreach (DataRow dr in dt.Rows)
            {
                int qtyActual = 0;
                for (int i = 0; i <= sizeRunList.Count - 1; i++)
                {
                    string sizeNo = sizeRunList[i].SizeNo;
                    int quantity = (Int32)dr[String.Format("Column{0}", i)];
                    qtyActual += quantity;
                    if (quantity >= 0)
                    {
                        OutsoleOutputModel model = new OutsoleOutputModel
                        {
                            ProductNo = productNo,
                            SizeNo = sizeNo,
                            Quantity = quantity,
                        };
                        outsoleOutputToInsertList.Add(model);
                    }
                }

                if (qtyActual > 0 && outsoleActualStartDateAuto == "")
                {
                    outsoleActualStartDateAuto = String.Format("{0:M/d}", nowDate);
                }
                if (qtyActual >= sizeRunList.Sum(s => s.Quantity) && outsoleActualFinishDateAuto == "")
                {
                    outsoleActualFinishDateAuto = String.Format("{0:M/d}", nowDate);
                }
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
            foreach (OutsoleOutputModel model in outsoleOutputToInsertList)
            {
                OutsoleOutputController.Insert(model);
            }
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSave.IsEnabled = true;
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Saved!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            int qtyBalance = sizeRunList.Sum(s => s.Quantity) - outsoleOutputToInsertList.Sum(s => s.Quantity);
            if (qtyBalance > 0)
            {
                resultString = qtyBalance.ToString();
                if (qtyBalance == sizeRunList.Sum(s => s.Quantity))
                {
                    resultString = "";
                }
            }
            else
            {
                resultString = String.Format("{0:M/d}", nowDate);
            }
            this.DialogResult = true;
        }

        private void btnCompleted_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = ((DataRowView)dgOutsoleMaterial.CurrentItem).Row;
            for (int i = 0; i <= sizeRunList.Count - 1; i++)
            {
                SizeRunModel sizeRun = sizeRunList[i];
                dr[String.Format("Column{0}", i)] = sizeRun.Quantity;
            }
            dgOutsoleMaterial.ItemsSource = null;
            dgOutsoleMaterial.ItemsSource = dt.AsDataView();
        }

        private void dgOutsoleMaterial_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
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

            int qtyOld = outsoleOutputList.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity);
            int qtyOrder = sizeRunList.Where(s => s.SizeNo == sizeNo).Sum(s => s.Quantity);
            TextBox txtCurrent = (TextBox)e.EditingElement;
            int qtyNew = 0;
            if (int.TryParse(txtCurrent.Text, out qtyNew) == true)
            {
                if (qtyNew == qtyOld)
                {
                    return;
                }
                int qtyInput = 0;
                qtyInput = (qtyOld + qtyNew);
                if (qtyOld + qtyNew < 0 || qtyOld + qtyNew > qtyOrder)
                {
                    qtyInput = qtyOld;
                }
                txtCurrent.Text = qtyInput.ToString();
                int qtyTotal = qtyInput;
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i <= sizeRunList.Count - 1; i++)
                    {
                        if (sizeRunList[i].SizeNo != sizeNo)
                        {
                            int qty = (Int32)dr[String.Format("Column{0}", i)];
                            qtyTotal += qty;
                        }
                    }
                }
                lblQtyTotal.Text = qtyTotal.ToString();
            }

        }
    }
}
