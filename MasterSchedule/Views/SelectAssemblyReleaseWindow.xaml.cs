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
    public partial class SelectAssemblyReleaseWindow : Window
    {
        AccountModel account;
        BackgroundWorker bwLoadData;
        List<AssemblyReleaseModel> assemblyReleaseList;
        public SelectAssemblyReleaseWindow(AccountModel account)
        {
            this.account = account;
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            assemblyReleaseList = new List<AssemblyReleaseModel>();
            InitializeComponent();
        }

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            assemblyReleaseList = AssemblyReleaseController.SelectReportId();
            
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            txtReportId.ItemsSource = assemblyReleaseList.Select(o => o.ReportId).Distinct().ToList();
            txtProductNo.ItemsSource = assemblyReleaseList.Select(o => o.ProductNo).Distinct().ToList();
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
            UpdateAssemblyReleaseWindow window = new UpdateAssemblyReleaseWindow(account, reportId);
            window.Show();
        }

        private void btnSearchExpand_Click(object sender, RoutedEventArgs e)
        {
            if (gridSearch.Visibility == Visibility.Collapsed)
            {
                gridSearch.Visibility = Visibility.Visible;
                txtReportId.IsEnabled = false;
                btnOk.IsEnabled = false;
                btnSearchExpand.Content = "<< " + btnSearchExpand.Tag.ToString();
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

        private void lvReportId_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                object reportId = lvReportId.SelectedItem;
                if (reportId == null)
                {
                    return;
                }
                UpdateAssemblyReleaseWindow window = new UpdateAssemblyReleaseWindow(account, reportId.ToString());
                window.Show();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string productNo = txtProductNo.Text;
            if (string.IsNullOrEmpty(productNo) == true)
            {
                return;
            }
            List<String> reportIdList = assemblyReleaseList.Where(o => o.ProductNo.ToLower() == productNo.ToLower()).Select(o => o.ReportId).Distinct().ToList();
            lvReportId.ItemsSource = null;
            lvReportId.ItemsSource = reportIdList;
        }
             
    }
}
