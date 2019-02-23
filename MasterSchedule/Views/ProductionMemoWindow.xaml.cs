using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MasterSchedule.Models;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using MasterSchedule.Helpers;
using System.ComponentModel;
using MasterSchedule.Controllers;
namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for ProductionMemoWindow.xaml
    /// </summary>
    public partial class ProductionMemoWindow : Window
    {
        ObservableCollection<ProductionNumberModel> productionNumbers;
        BackgroundWorker threadInsert;
        public ProductionMemoWindow()
        {
            InitializeComponent();
            productionNumbers = new ObservableCollection<ProductionNumberModel>();

            threadInsert = new BackgroundWorker();
            threadInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadInsert_RunWorkerCompleted);
            threadInsert.DoWork += new DoWorkEventHandler(threadInsert_DoWork);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<SectionModel> sectionList = SectionModel.CreateList();
            cboSection.ItemsSource = sectionList;
            if (sectionList.Count > 0)
            {
                cboSection.SelectedItem = sectionList.FirstOrDefault();
            }
            dgProductionNumber.ItemsSource = productionNumbers;
        }

        private void btnBrowserPicture_Click(object sender, RoutedEventArgs e)
        {
            BrowserPicture(imgPicture);
        }

        private void miBrowserPicture1_Click(object sender, RoutedEventArgs e)
        {
            BrowserPicture(imgPicture1);
        }

        private void miBrowserPicture2_Click(object sender, RoutedEventArgs e)
        {
            BrowserPicture(imgPicture2);
        }

        private void miBrowserPicture3_Click(object sender, RoutedEventArgs e)
        {
            BrowserPicture(imgPicture3);
        }

        private void miBrowserPicture4_Click(object sender, RoutedEventArgs e)
        {
            BrowserPicture(imgPicture4);
        }

        private void BrowserPicture(Image imgSelect)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "+ Picture";
            ofd.Filter = "Picture Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == false)
            {
                return;
            }
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri(ofd.FileName,
                 UriKind.RelativeOrAbsolute);
            bitmapImage.BeginInit();
            bitmapImage.UriSource = uri;
            bitmapImage.DecodePixelWidth = 768;
            bitmapImage.EndInit();

            imgSelect.Source = bitmapImage;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SectionModel section = cboSection.SelectedItem as SectionModel;
            if (section == null)
            {
                return;
            }
            List<ProductionNumberModel> productionNumberList = productionNumbers.Where(p => p != null && string.IsNullOrEmpty(p.Value.Trim()) == false).ToList();
            if (productionNumberList.Count <= 0)
            {
                return;
            }
            BitmapImage picture = imgPicture.Source as BitmapImage;
            BitmapImage picture1 = imgPicture1.Source as BitmapImage;
            BitmapImage picture2 = imgPicture2.Source as BitmapImage;
            BitmapImage picture3 = imgPicture3.Source as BitmapImage;
            BitmapImage picture4 = imgPicture4.Source as BitmapImage;
            if (picture == null && picture1 == null && picture2 == null && picture3 == null && picture4 == null)
            {
                return;
            }
            string productionNumberString = "";
            foreach (ProductionNumberModel productionNumber in productionNumberList)
            {
                productionNumberString += productionNumber.Value + ";";
            }
            ProductionMemoModel model = new ProductionMemoModel()
            {
                SectionId = section.SectionId,
                ProductionNumbers = productionNumberString,
                Picture = null,
                Picture1 = null,
                Picture2 = null,
                Picture3 = null,
                Picture4 = null,
            };
            if (picture != null)
            {
                model.Picture = ConvertJPGHelper.GetJPGFromBitmapImage(picture);
            }
            if (picture1 != null)
            {
                model.Picture1 = ConvertJPGHelper.GetJPGFromBitmapImage(picture1);
            }
            if (picture2 != null)
            {
                model.Picture2 = ConvertJPGHelper.GetJPGFromBitmapImage(picture2);
            }
            if (picture3 != null)
            {
                model.Picture3 = ConvertJPGHelper.GetJPGFromBitmapImage(picture3);
            }
            if (picture4 != null)
            {
                model.Picture4 = ConvertJPGHelper.GetJPGFromBitmapImage(picture4);
            }

            if (threadInsert.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            btnSave.IsEnabled = false;
            threadInsert.RunWorkerAsync(model);
        }

        private void threadInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            ProductionMemoModel model = e.Argument as ProductionMemoModel;
            string memoId = ProductionMemoController.Insert(model);
            e.Result = memoId;
        }

        private void threadInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnSave.IsEnabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string memoId = e.Result as string;
            if (string.IsNullOrEmpty(memoId) == false)
            {
                MessageBox.Show(string.Format("Save Successfull! [Memo No.] is ({0}).", memoId), "Insert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Cannot Save, Exist Already or Something Wrong. Try Again!", "Insert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void miClear_Click(object sender, RoutedEventArgs e)
        {
            MenuItem miClear = sender as MenuItem;
            Image imgSelect = ((ContextMenu)miClear.Parent).PlacementTarget as Image;
            imgSelect.Source = null;
        }
    }

    public class ProductionNumberModel
    {
        public string Value { get; set; }
    }
}
