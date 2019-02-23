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
    /// Interaction logic for UpdateMachineRequirementWindow.xaml
    /// </summary>
    public partial class UpdateMachineRequirementWindow : Window
    {
        BackgroundWorker bwSearch;
        string articleNo;

        DataTable dt;
        MachineRequirementModel machineRequirementModel;
        MachineRequirementModel machineRequirementToUpdate;
        BackgroundWorker bwUpdate;
        BackgroundWorker bwDelete;

        public UpdateMachineRequirementWindow()
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
            if (String.IsNullOrEmpty(txtArticleNo.Text) == true)
            { return; }
            if (bwSearch.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                articleNo = txtArticleNo.Text;
                btnSearch.IsEnabled = false;
                btnDelete.IsEnabled = false;

                btnUpdateMachineRequirement.IsEnabled = false;
                dt = new DataTable();
                bwSearch.RunWorkerAsync();
            }
        }

        private void bwSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            machineRequirementModel = MachineRequirementController.SelectTop1(articleNo);
        }

        private void bwSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgMachineRequirement.ItemsSource = null;
            if (machineRequirementModel != null)
            {
                dgMachineRequirement.ItemsSource = new List<MachineRequirementModel>() { machineRequirementModel, };
                btnUpdateMachineRequirement.IsEnabled = true;
            }
            btnSearch.IsEnabled = true;
            btnDelete.IsEnabled = true;
            this.Cursor = null;
        }



        private void btnUpdateMachineRequirement_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm Save?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            if (bwUpdate.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                btnUpdateMachineRequirement.IsEnabled = false;
                machineRequirementToUpdate = dgMachineRequirement.Items.OfType<MachineRequirementModel>().FirstOrDefault();
                bwUpdate.RunWorkerAsync();
            }
        }
        private void bwUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            machineRequirementToUpdate.ArticleNo = articleNo;
            MachineRequirementController.Insert(machineRequirementToUpdate);
        }

        private void bwUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnUpdateMachineRequirement.IsEnabled = true;
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
            if (String.IsNullOrEmpty(txtArticleNo.Text) == true ||
                MessageBox.Show("Confirm Delete?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            if (bwDelete.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                articleNo = txtArticleNo.Text;
                btnSearch.IsEnabled = false;
                btnDelete.IsEnabled = false;
                bwDelete.RunWorkerAsync();
            }
        }
        private void bwDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            MachineRequirementController.Delete(articleNo);
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

            machineRequirementModel = null;
            dgMachineRequirement.ItemsSource = null;
            btnUpdateMachineRequirement.IsEnabled = false;
            MessageBox.Show("Deleted!", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void dgMachineRequirement_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0)
            {
                scrHeader.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }

        
    }
}
