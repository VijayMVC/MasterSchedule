using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
using MasterSchedule.Views;
using System.Reflection;

using MasterSchedule.Models;
using MasterSchedule.Helpers;
namespace MasterSchedule
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static Mutex _m;
        private App()
        {
            InitializeComponent();
        }
        [STAThread]
        private static void Main()
        {
            try
            {
                Mutex.OpenExisting("MasterSchedule");
                MessageBox.Show("Application Running...", "Master Schedule", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch
            {
                App._m = new Mutex(true, "MasterSchedule");
                App app = new App();
                app.Run(new LoginWindow());
                //app.Run(new ChartScheduleWindow());
                _m.ReleaseMutex();
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
            // Get the connectionStrings section. 
            ConfigurationSection configSection = config.GetSection("connectionStrings");
            //Ensures that the section is not already protected.
            if (configSection.SectionInformation.IsProtected == false)
            {
                //Uses the Windows Data Protection API (DPAPI) to encrypt the configuration section using a machine-specific secret key.
                configSection.SectionInformation.ProtectSection(
                    "DataProtectionConfigurationProvider");
                config.Save();
            }
        }
    }
}
