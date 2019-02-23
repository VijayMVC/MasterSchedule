using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for LoginWindows.xaml
    /// </summary>
    public partial class SelectOutsoleReleaseMaterialWindow : Window
    {
        AccountModel account;
        BackgroundWorker bwLoadData;
        List<OutsoleReleaseMaterialModel> outsoleReleaseMaterialList;
        public SelectOutsoleReleaseMaterialWindow(AccountModel account)
        {
            this.account = account;
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            outsoleReleaseMaterialList = new List<OutsoleReleaseMaterialModel>();
            InitializeComponent();
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            outsoleReleaseMaterialList = OutsoleReleaseMaterialController.SelectReportId();            
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            txtReportId.ItemsSource = outsoleReleaseMaterialList.Select(o => o.ReportId).Distinct().ToList();
            txtProductNo.ItemsSource = outsoleReleaseMaterialList.Select(o => o.ProductNo).Distinct().ToList();
            btnOk.IsEnabled = true;
            btnSearchExpand.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoadData.RunWorkerAsync();
            }           
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string reportId = txtReportId.Text;
            if (String.IsNullOrEmpty(reportId) == true)
            {
                return;
            }
            UpdateOutsoleReleaseMaterialWindow window = new UpdateOutsoleReleaseMaterialWindow(account, reportId);
            window.Show();
        }

        private void btnSearchExpand_Click(object sender, RoutedEventArgs e)
        {
            if (gridSearch.Visibility == Visibility.Collapsed)
            {
                gridSearch.Visibility = Visibility.Visible;
                txtReportId.IsEnabled = false;
                btnOk.IsEnabled = false;
                btnSearchExpand.Content =  "<< " + btnSearchExpand.Tag.ToString();
                return;
            }
            else if (gridSearch.Visibility == Visibility.Visible)
            {
                gridSearch.Visibility = Visibility.Collapsed;
                txtReportId.IsEnabled = true;
                btnOk.IsEnabled = true;
                btnSearchExpand.Content = btnSearchExpand.Tag.ToString() + " >>";
                return;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string productNo = txtProductNo.Text;
            if (string.IsNullOrEmpty(productNo) == true)
            {
                return;
            }
            List<String> reportIdList = outsoleReleaseMaterialList.Where(o => o.ProductNo.ToLower() == productNo.ToLower()).Select(o => o.ReportId).Distinct().ToList();
            lvReportId.ItemsSource = null;
            lvReportId.ItemsSource = reportIdList;
        }

        private void lvReportId_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                object reportId = lvReportId.SelectedItem;
                if (reportId == null)
                {
                    return;
                }
                UpdateOutsoleReleaseMaterialWindow window = new UpdateOutsoleReleaseMaterialWindow(account, reportId.ToString());
                window.Show();
            }
        }       
             
    }
}
