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
    public partial class SearchProductionMemoWindow : Window
    {
        ObservableCollection<ProductionNumberModel> productionNumbers;
        BackgroundWorker threadInsert;
        BackgroundWorker threadSearch;
        List<SectionModel> sectionList;
        BackgroundWorker threadDelete;
        BackgroundWorker threadSearchByProductionNumber;
        List<ProductionMemoModel> productionMemoList;
        AccountModel account;
        public SearchProductionMemoWindow(AccountModel account)
        {
            InitializeComponent();
            this.account = account;

            productionNumbers = new ObservableCollection<ProductionNumberModel>();

            threadInsert = new BackgroundWorker();
            threadInsert.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadInsert_RunWorkerCompleted);
            threadInsert.DoWork += new DoWorkEventHandler(threadInsert_DoWork);

            threadSearch = new BackgroundWorker();
            threadSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadSearch_RunWorkerCompleted);
            threadSearch.DoWork += new DoWorkEventHandler(threadSearch_DoWork);

            sectionList = new List<SectionModel>();

            threadDelete = new BackgroundWorker();
            threadDelete.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadDelete_RunWorkerCompleted);
            threadDelete.DoWork += new DoWorkEventHandler(threadDelete_DoWork);

            threadSearchByProductionNumber = new BackgroundWorker();
            threadSearchByProductionNumber.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadSearchByProductionNumber_RunWorkerCompleted);
            threadSearchByProductionNumber.DoWork += new DoWorkEventHandler(threadSearchByProductionNumber_DoWork);

            productionMemoList = new List<ProductionMemoModel>();
        }             

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (account.ProductionMemo == true)
            {
                btnSave.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            sectionList = SectionModel.CreateList();
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Save?", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            string memoId = lblMemoId.Text;
            if (string.IsNullOrEmpty(memoId) == true)
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
            ProductionMemoModel model = new ProductionMemoModel() {
            MemoId = memoId,
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

            if(threadInsert.IsBusy == true)
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
            e.Result = ProductionMemoController.Update(model);
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
            bool result = (e.Result as bool?).Value;
            if (result == true)
            {
                MessageBox.Show("Save Successfull!", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Cannot Update, Something Wrong. Try Again!", "Update", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string memoId = txtMemoId.Text;
            memoId = memoId.Trim();
            if (string.IsNullOrEmpty(memoId) == true)
            {
                return;
            }
            if (threadSearch.IsBusy == true)
            {
                return;
            }

            lblMemoId.Text = null;
            cboSection.SelectedItem = null;
            productionNumbers.Clear();
            imgPicture.Source = null;
            imgPicture1.Source = null;
            imgPicture2.Source = null;
            imgPicture3.Source = null;
            imgPicture4.Source = null;

            this.Cursor = Cursors.Wait;
            btnSearch.IsEnabled = false;
            threadSearch.RunWorkerAsync(memoId);
        }

        private void threadSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            string memoId = e.Argument as string;
            e.Result = ProductionMemoController.First(memoId);
        }

        private void threadSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnSearch.IsEnabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ProductionMemoModel productionMemo = e.Result as ProductionMemoModel;
            if (productionMemo != null)
            {
                lblMemoId.Text = productionMemo.MemoId;
                SectionModel section = sectionList.Where(s => s.SectionId == productionMemo.SectionId).FirstOrDefault();
                if (section != null)
                {
                    cboSection.SelectedItem = section;
                }
                string[] productNumberArray = productionMemo.ProductionNumbers.Split(';');
                foreach (string productNumber in productNumberArray)
                {
                    if (string.IsNullOrEmpty(productNumber) == false)
                    {
                        productionNumbers.Add(new ProductionNumberModel() { Value = productNumber });
                    }
                }
                if (productionMemo.Picture != null)
                {
                    imgPicture.Source = ConvertJPGHelper.GetBitmapImageFromJPG(productionMemo.Picture);
                }
                if (productionMemo.Picture1 != null)
                {
                    imgPicture1.Source = ConvertJPGHelper.GetBitmapImageFromJPG(productionMemo.Picture1);
                }
                if (productionMemo.Picture2 != null)
                {
                    imgPicture2.Source = ConvertJPGHelper.GetBitmapImageFromJPG(productionMemo.Picture2);
                }
                if (productionMemo.Picture3 != null)
                {
                    imgPicture3.Source = ConvertJPGHelper.GetBitmapImageFromJPG(productionMemo.Picture3);
                }
                if (productionMemo.Picture4 != null)
                {
                    imgPicture4.Source = ConvertJPGHelper.GetBitmapImageFromJPG(productionMemo.Picture4);
                }
            }
            else
            {
                MessageBox.Show("Not Found!", "Search", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSavePicture_Click(object sender, RoutedEventArgs e)
        {
            SavePicture(imgPicture);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Delete?", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            string memoId = lblMemoId.Text;
            if (string.IsNullOrEmpty(memoId) == true)
            {
                return;
            }

            this.Cursor = Cursors.Wait;
            btnDelete.IsEnabled = false;
            threadDelete.RunWorkerAsync(memoId);
        }

        private void threadDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            string memoId = e.Argument as string;
            e.Result = ProductionMemoController.Delete(memoId);
        }

        private void threadDelete_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnDelete.IsEnabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool result = (e.Result as bool?).Value;
            if (result == true)
            {
                MessageBox.Show("Delete Successfull!", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Cannot Delete, Something Wrong. Try Again!", "Delete", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearchbyProductionNumber_Click(object sender, RoutedEventArgs e)
        {
            string productionNumber = txtProductionNumber.Text;
            productionNumber = productionNumber.Trim();
            if (string.IsNullOrEmpty(productionNumber) == true)
            {
                return;
            }
            if (threadSearchByProductionNumber.IsBusy == true)
            {
                return;
            }

            dgProductionMemo.ItemsSource = null;

            this.Cursor = Cursors.Wait;
            btnSearchbyProductionNumber.IsEnabled = false;
            productionMemoList.Clear();
            threadSearchByProductionNumber.RunWorkerAsync(productionNumber);
        }

        private void threadSearchByProductionNumber_DoWork(object sender, DoWorkEventArgs e)
        {
            string productionNumber = e.Argument as string;
            e.Result = ProductionMemoController.Select(null, productionNumber);
        }

        private void threadSearchByProductionNumber_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnSearchbyProductionNumber.IsEnabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            productionMemoList = e.Result as List<ProductionMemoModel>;
            List<ProductionMemoModel> productionMemoViewList = new List<ProductionMemoModel>();
            foreach (ProductionMemoModel productionMemo in productionMemoList)
            {
                ProductionMemoModel productionMemoView = new ProductionMemoModel 
                {
                    MemoId = productionMemo.MemoId,                    
                };
                productionMemoView.SectionId = "";
                SectionModel section = sectionList.Where(s => s.SectionId == productionMemo.SectionId).FirstOrDefault();
                if (section != null)
                {
                    productionMemoView.SectionId = section.Name;
                }
                productionMemoViewList.Add(productionMemoView);                
            }
            dgProductionMemo.ItemsSource = productionMemoViewList;
        }

        private void dgProductionMemo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductionMemoModel productionMemoView = dgProductionMemo.CurrentItem as ProductionMemoModel;
            if (productionMemoView == null)
            {
                return;
            }

            lblMemoId.Text = null;
            cboSection.SelectedItem = null;
            productionNumbers.Clear();
            imgPicture.Source = null;
            imgPicture1.Source = null;
            imgPicture2.Source = null;
            imgPicture3.Source = null;
            imgPicture4.Source = null;

            ProductionMemoModel productionMemo = productionMemoList.Where(p => p.MemoId == productionMemoView.MemoId).FirstOrDefault();
            if (productionMemo != null)
            {
                lblMemoId.Text = productionMemo.MemoId;
                SectionModel section = sectionList.Where(s => s.SectionId == productionMemo.SectionId).FirstOrDefault();
                if (section != null)
                {
                    cboSection.SelectedItem = section;
                }
                string[] productNumberArray = productionMemo.ProductionNumbers.Split(';');
                foreach (string productNumber in productNumberArray)
                {
                    if (string.IsNullOrEmpty(productNumber) == false)
                    {
                        productionNumbers.Add(new ProductionNumberModel() { Value = productNumber });
                    }
                }
                if (productionMemo.Picture != null)
                {
                    imgPicture.Source = ConvertJPGHelper.GetBitmapImageFromJPG(productionMemo.Picture);
                }
                if (productionMemo.Picture1 != null)
                {
                    imgPicture1.Source = ConvertJPGHelper.GetBitmapImageFromJPG(productionMemo.Picture1);
                }
                if (productionMemo.Picture2 != null)
                {
                    imgPicture2.Source = ConvertJPGHelper.GetBitmapImageFromJPG(productionMemo.Picture2);
                }
                if (productionMemo.Picture3 != null)
                {
                    imgPicture3.Source = ConvertJPGHelper.GetBitmapImageFromJPG(productionMemo.Picture3);
                }
                if (productionMemo.Picture4 != null)
                {
                    imgPicture4.Source = ConvertJPGHelper.GetBitmapImageFromJPG(productionMemo.Picture4);
                }
            }
            else
            {
                MessageBox.Show("Not Found!", "Search", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void miClear_Click(object sender, RoutedEventArgs e)
        {
            MenuItem miClear = sender as MenuItem;
            Image imgSelect = ((ContextMenu)miClear.Parent).PlacementTarget as Image;
            imgSelect.Source = null;
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

        private void SavePicture(Image imgSelect)
        {
            BitmapImage bitmapImage = imgSelect.Source as BitmapImage;
            if (bitmapImage == null)
            {
                return;
            }
            BitmapEncoder bitmapEncoder = new JpegBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Picture";
            sfd.Filter = "Picture Files (*.jpeg)|*.jpeg";
            sfd.FileName = "MemoPicture";

            if (sfd.ShowDialog() == false)
            {
                return;
            }

            using (var fileStream = new System.IO.FileStream(sfd.FileName, System.IO.FileMode.Create))
            {
                bitmapEncoder.Save(fileStream);
            }
        }

        private void miSavePicture1_Click(object sender, RoutedEventArgs e)
        {
            SavePicture(imgPicture1);
        }

        private void miSavePicture2_Click(object sender, RoutedEventArgs e)
        {
            SavePicture(imgPicture2);
        }

        private void miSavePicture3_Click(object sender, RoutedEventArgs e)
        {
            SavePicture(imgPicture3);
        }

        private void miSavePicture4_Click(object sender, RoutedEventArgs e)
        {
            SavePicture(imgPicture4);
        }
    }    
}
