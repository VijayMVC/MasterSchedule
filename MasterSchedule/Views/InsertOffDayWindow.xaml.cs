using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using MasterSchedule.Models;
using System.Collections.ObjectModel;
using MasterSchedule.Controllers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for OffDayWindow.xaml
    /// </summary>
    public partial class InsertOffDayWindow : Window
    {
        AccountModel account;
        BackgroundWorker bwLoadData;
        List<OffDayModel> offDayList;
        ObservableCollection<OffDayModel> offDayViewList;
        BackgroundWorker bwInsert;
        OffDayModel offDayToInsert;
        List<OffDayModel> offDayToDeleteList;
        BackgroundWorker bwDelete;
        public InsertOffDayWindow(AccountModel account)
        {
            this.account = account;
            bwLoadData = new BackgroundWorker();
            bwLoadData.WorkerSupportsCancellation = true;
            bwLoadData.DoWork += new DoWorkEventHandler(bwLoadData_DoWork);
            bwLoadData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadData_RunWorkerCompleted);
            bwInsert = new BackgroundWorker();
            bwInsert.DoWork += new DoWorkEventHandler(bwInsert_DoWork);
            bwInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsert_RunWorkerCompleted);
            offDayList = new List<OffDayModel>();
            offDayViewList = new ObservableCollection<OffDayModel>();
            offDayToInsert = new OffDayModel();
            offDayToDeleteList = new List<OffDayModel>();
            bwDelete = new BackgroundWorker();
            bwDelete.DoWork += new DoWorkEventHandler(bwDelete_DoWork);
            bwDelete.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwDelete_RunWorkerCompleted);
            InitializeComponent();
        }              

        private void bwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            offDayList = OffDayController.Select();
            foreach (OffDayModel offDay in offDayList)
            {
                offDayViewList.Add(offDay);
            }
        }

        private void bwLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            dgOffDay.ItemsSource = null;
            dgOffDay.ItemsSource = offDayViewList;
            if (account.OffDay == true)
            {
                btnInsert.IsEnabled = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpDate.SelectedDate = DateTime.Now;
            if (bwLoadData.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                offDayViewList.Clear();
                bwLoadData.RunWorkerAsync();
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = dpDate.SelectedDate.Value.Date;
            string remarks = txtRemarks.Text;
            offDayToInsert = new OffDayModel
            {
                Date = date,
                Remarks = remarks,
            };

            if (bwInsert.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnInsert.IsEnabled = false;
                bwInsert.RunWorkerAsync();
            }            
        }

        private void bwInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            OffDayController.Insert(offDayToInsert);
        }

        private void bwInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnInsert.IsEnabled = true;
            this.Cursor = null;
            OffDayModel offDay = offDayViewList.Where(o => o.Date.Date == offDayToInsert.Date.Date).FirstOrDefault();
            if (offDay == null)
            {
                offDayViewList.Add(offDayToInsert);
            }
            else
            {
                offDay.Remarks = offDayToInsert.Remarks;
            }
            dgOffDay.ScrollIntoView(offDayToInsert);
        }

        private void dgOffDay_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dgOffDay.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Confirm Delete?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    offDayToDeleteList.AddRange(dgOffDay.SelectedItems.OfType<OffDayModel>());
                    if (bwDelete.IsBusy == false)
                    {
                        this.Cursor = Cursors.Wait;
                        bwDelete.RunWorkerAsync();
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void bwDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (OffDayModel offDay in offDayToDeleteList)
            {
                OffDayController.Delete(offDay.Date.Date);
            }
        }

        private void bwDelete_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            offDayToDeleteList.Clear();
            MessageBox.Show("Deleted!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }  
    }
}
