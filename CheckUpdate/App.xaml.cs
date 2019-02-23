using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;

namespace CheckUpdate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private App()
        {
            InitializeComponent();
        }
        [STAThread]
        private static void Main()
        {           
            App app = new App();
            app.Run(new MainWindow());            
        }
    }
}
