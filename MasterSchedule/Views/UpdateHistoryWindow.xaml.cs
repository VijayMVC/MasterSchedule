using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for UpdateHistoryWindow.xaml
    /// </summary>
    public partial class UpdateHistoryWindow : Window
    {
        string updateInformation;
        public UpdateHistoryWindow(string updateInformation)
        {
            this.updateInformation = updateInformation;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUpdateInformation.Text = updateInformation;
        }

    }
}
