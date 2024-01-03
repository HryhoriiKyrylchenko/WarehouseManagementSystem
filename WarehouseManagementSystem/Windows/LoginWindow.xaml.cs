using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels;

namespace WarehouseManagementSystem.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged
    {
        LoginService loginService;

        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<Warehouse> warehouses;
        public ObservableCollection<Warehouse> Warehouses
        {
            get { return warehouses; }
            set
            {
                if (warehouses != value)
                {
                    warehouses = value;
                    OnPropertyChanged(nameof(Warehouses));
                }
            }
        }

        private Warehouse? selectedWarehouse;

        public Warehouse? SelectedWarehouse
        {
            get { return selectedWarehouse; }
            set
            {
                if (selectedWarehouse != value)
                {
                    selectedWarehouse = value;
                    OnPropertyChanged(nameof(SelectedWarehouse));
                }
            }
        }

        public LoginWindow(LoginService loginService)
        {
            InitializeComponent();
            this.loginService = loginService;
            warehouses = new ObservableCollection<Warehouse>();

            Warehouses = GetWarehouses();
            DataContext = this;
            if (Warehouses.Count > 0)
            {
                ComboBoxWarehouses.SelectedIndex = 0;
            }
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBoxUsername.Text;
            string password = TextBoxPassword.Text;
            if (IsValidInput(username, password, SelectedWarehouse))
            {
                if(loginService.Login(username, password, SelectedWarehouse))
                {
                    Close();
                }
                else
                {
                    MessageBox.Show("Wrong data entered");
                    TextBoxUsername.Text = string.Empty;
                    TextBoxPassword.Text = string.Empty;
                }
            }
        }

        private ObservableCollection<Warehouse> GetWarehouses()
        {
            using(WarehouseDBManager db = new WarehouseDBManager(new Models.WarehouseDbContext()))
            {
                return db.GetAllWarehouses();
            }
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public bool IsValidInput(string username, string password, Warehouse? warehouse)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || warehouse == null)
            {
                MessageBox.Show("All fields are required");
                return false;
            }

            return true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                if (comboBox.SelectedItem is Warehouse warehouse)
                {
                    SelectedWarehouse = warehouse;
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
