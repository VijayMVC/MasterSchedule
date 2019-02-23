using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CheckUpdate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string filePath = @"MasterSchedule.exe";
            string checkPath = @"\\10.2.1.228\QC\SV-Master\Master Schedule\MasterSchedule.exe";
            if (File.Exists(filePath) == false)
            {
                return;
            }

            if (File.Exists(checkPath) == true)
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(filePath);
                FileVersionInfo fviUpdate = FileVersionInfo.GetVersionInfo(checkPath);

                if (fvi.ProductVersion.CompareTo(fviUpdate.ProductVersion) < 0)
                {
                    try
                    {
                        Mutex.OpenExisting("MasterSchedule");
                        MessageBox.Show("MasterSchedule Running... Exit & Try Again!", "Check Update for Master Schedule", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                    catch
                    {
                        File.Copy(checkPath, filePath, true);
                    }
                }
            }
            
            Process.Start(filePath);
            this.Close();
        }
    }
}
