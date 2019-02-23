using System.Windows;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for RawMaterialSearchBoxWindow.xaml
    /// </summary>
    public partial class RawMaterialSearchBoxWindow : Window
    {
        public delegate void GetString(string findWhat, bool isMatch, bool isShow);
        public GetString GetFindWhat;
        public RawMaterialSearchBoxWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnFindAll_Click(object sender, RoutedEventArgs e)
        {
            if (GetFindWhat != null)
            {
                string findWhat = txtFindWhat.Text;
                bool isMatch = cboIsMatch.IsChecked.Value;
                bool isShow = rbShow.IsChecked.Value;
                GetFindWhat(findWhat, isMatch, isShow);
                txtFindWhat.SelectAll();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtFindWhat.Focus();
        }

        private void cboIsMatch_Checked(object sender, RoutedEventArgs e)
        {
            if (spShowHide != null)
            {
                spShowHide.Visibility = Visibility.Collapsed;
            }
        }

        private void cboIsMatch_Unchecked(object sender, RoutedEventArgs e)
        {
            if (spShowHide != null)
            {
                spShowHide.Visibility = Visibility.Visible;
            }
        }
    }
}
