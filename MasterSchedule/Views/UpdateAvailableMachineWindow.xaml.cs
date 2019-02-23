using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using System.Data;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpdateAvailableMachineWindow.xaml
    /// </summary>
    public partial class UpdateAvailableMachineWindow : Window
    {
        DataTable dt;
        string Id;
        BackgroundWorker bwSearch;
        BackgroundWorker bwUpdate;
        BackgroundWorker bwDelete;

        AvailableMachineModel availableMachineModel;
        AvailableMachineModel availableMachineToUpdate;
        public UpdateAvailableMachineWindow()
        {
            bwSearch = new BackgroundWorker();
            bwSearch.DoWork += new DoWorkEventHandler(bwSearch_DoWork);
            bwSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSearch_RunWorkerCompleted);

            bwUpdate = new BackgroundWorker();
            bwUpdate.DoWork += new DoWorkEventHandler(bwUpdate_DoWork);
            bwUpdate.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwUpdate_RunWorkerCompleted);

            bwDelete = new BackgroundWorker();
            bwDelete.DoWork += new DoWorkEventHandler(bwDelete_DoWork);
            bwDelete.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwDelete_RunWorkerCompleted);

            InitializeComponent();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtId.Text) == true)
            { return; }
            if (bwSearch.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                Id = txtId.Text;
                btnSearch.IsEnabled = false;
                btnDelete.IsEnabled = false;

                btnUpdateAvailableMachine.IsEnabled = false;
                dt = new DataTable();
                bwSearch.RunWorkerAsync();
            }
        }
        private void bwSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            availableMachineModel = AvailableMachineController.SelectTop1(Id);
        }
        private void bwSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgAvailableMachine.ItemsSource = null;
            if (availableMachineModel != null)
            {
                dgAvailableMachine.ItemsSource = new List<AvailableMachineModel>() { availableMachineModel, };
                btnUpdateAvailableMachine.IsEnabled = true;
            }
            btnSearch.IsEnabled = true;
            btnDelete.IsEnabled = true;
            this.Cursor = null;
        }
        private void btnUpdateAvailableMachine_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            if (bwUpdate.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnUpdateAvailableMachine.IsEnabled = false;
                availableMachineToUpdate = dgAvailableMachine.Items.OfType<AvailableMachineModel>().FirstOrDefault();
                bwUpdate.RunWorkerAsync();
            }
        }
        private void bwUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            availableMachineToUpdate.Id = Id;
            AvailableMachineController.Insert(availableMachineToUpdate);
        }
        private void bwUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnUpdateAvailableMachine.IsEnabled = true;
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
            if (String.IsNullOrEmpty(txtId.Text) == true ||
                MessageBox.Show("Confirm Delete?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            if (bwDelete.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                Id = txtId.Text;
                btnSearch.IsEnabled = false;
                btnDelete.IsEnabled = false;
                bwDelete.RunWorkerAsync();
            }
        }
        private void bwDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            AvailableMachineController.Delete(Id);
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

            availableMachineModel = null;
            dgAvailableMachine.ItemsSource = null;
            btnUpdateAvailableMachine.IsEnabled = false;
            MessageBox.Show("Deleted!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void dgAvailableMachine_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0)
            {
                scrHeader.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }

    }
}
