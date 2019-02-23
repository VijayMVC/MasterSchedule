using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;
using MasterSchedule.Helpers;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for LoginWindows.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        BackgroundWorker threadLogin;
        public LoginWindow()
        {
            threadLogin = new BackgroundWorker();
            threadLogin.DoWork += new DoWorkEventHandler(bwLogin_DoWork);
            threadLogin.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLogin_RunWorkerCompleted);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserName.Focus();
            if (DatabaseHelper.Exist() == true)
            {
                lblConnectionStatus.Text = "Connection Successful";
                lblConnectionStatus.Foreground = Brushes.Green;
                btnOk.IsEnabled = true;
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUserName.Text;
            if (string.IsNullOrEmpty(username) == true)
            {
                return;
            }
            string password = txtPassword.Password;
            if (string.IsNullOrEmpty(password) == true)
            {
                return;
            }
            if (threadLogin.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            btnOk.IsEnabled = false;
            threadLogin.RunWorkerAsync(new object[] { username, password });
        }

        private void bwLogin_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arguments = e.Argument as object[];
            string username = arguments[0] as string;
            string password = arguments[1] as string;  
            e.Result = AccountController.Select(username, password);
        }

        private void bwLogin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnOk.IsEnabled = true;
            if (e.Cancelled == true || e.Error != null)
            {
                MessageBox.Show("Login Failed.", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            AccountModel account = e.Result as AccountModel;
            if (account != null)
            {
                txtPassword.Password = "";
                MessageBox.Show(String.Format("Welcome, {0}!", account.FullName), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow window = new MainWindow(account);
                this.Hide();
                window.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Login Failed.", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
