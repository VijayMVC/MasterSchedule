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

using System.ComponentModel;
using MasterSchedule.Models;
using MasterSchedule.Controllers;

namespace MasterSchedule.Views
{
    /// <summary>
    /// Interaction logic for AddOutsoleMaterialRackPositionWindow.xaml
    /// </summary>
    public partial class AddOutsoleMaterialRackPositionWindow : Window
    {
        public OutsoleMaterialRackPositionModel rackModel;
        OutsoleSuppliersModel supplier;
        string productNo;
        public bool IsRemove = false;
        public AddOutsoleMaterialRackPositionWindow(string productNo, OutsoleSuppliersModel supplier)
        {
            this.supplier = supplier;
            this.productNo = productNo;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtRackNumber.Focus();
            txtSupplier.Text = supplier.Name;
            btnAdd.IsDefault = true;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            IsRemove = false;

            string rackNumber = "";
            int cartonNumber = 0;
            rackNumber = txtRackNumber.Text.ToUpper().ToString();
            Int32.TryParse(txtCartonNumber.Text.ToString(), out cartonNumber);

            if (String.IsNullOrEmpty(rackNumber))
            {
                txtRackNumber.SelectAll();
                txtRackNumber.Focus();
                return;
            }
            if (cartonNumber == 0)
            {
                txtCartonNumber.SelectAll();
                txtCartonNumber.Focus();
                return;
            }

            rackModel = new OutsoleMaterialRackPositionModel()
            {
                ProductNo = productNo,
                OutsoleSupplierId = supplier.OutsoleSupplierId,
                RackNumber = rackNumber,
                CartonNumber = cartonNumber
            };

            this.Close();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            IsRemove = true;

            string rackNumber = "";
            int cartonNumber = 0;
            rackNumber = txtRackNumber.Text.ToUpper().ToString();
            Int32.TryParse(txtCartonNumber.Text.ToString(), out cartonNumber);

            if (String.IsNullOrEmpty(rackNumber))
            {
                txtRackNumber.SelectAll();
                txtRackNumber.Focus();
                return;
            }
            if (cartonNumber == 0)
            {
                txtCartonNumber.SelectAll();
                txtCartonNumber.Focus();
                return;
            }

            rackModel = new OutsoleMaterialRackPositionModel()
            {
                ProductNo = productNo,
                OutsoleSupplierId = supplier.OutsoleSupplierId,
                RackNumber = rackNumber,
                CartonNumber = cartonNumber
            };

            this.Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }
    }
}
