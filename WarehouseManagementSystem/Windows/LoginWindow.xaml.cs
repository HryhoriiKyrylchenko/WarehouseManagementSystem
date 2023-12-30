using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class LoginWindow : Window
    {
        LoginService loginService;

        private ObservableCollection<Warehouse> Warehouses { get; set; }

        private Warehouse? selectedWarehouse;

        public Warehouse? SelectedWarehouse
        {
            get { return selectedWarehouse; }
            set
            {
                if (selectedWarehouse != value)
                {
                    selectedWarehouse = value;
                }
            }
        }

        public LoginWindow(LoginService loginService)
        {
            InitializeComponent();
            this.loginService = loginService;
            Warehouses = new ObservableCollection<Warehouse>();

            Warehouses = GetWarehousesAsync().GetAwaiter().GetResult();
            DataContext = this;
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
                    MessageBox.Show("Wrong username or password");
                    TextBoxUsername.Text = string.Empty;
                    TextBoxPassword.Text = string.Empty;
                }
            }
        }

        private async Task<ObservableCollection<Warehouse>> GetWarehousesAsync()
        {
            using(WarehouseDBManager db = new WarehouseDBManager(new Models.WarehouseDbContext()))
            {
                return await db.GetAllWarehousesAsync();
            }
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public bool IsValidInput(string username, string password, Warehouse? warehouse)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || warehouse != null)
            {
                MessageBox.Show("All fields are required");
                return false;
            }

            return true;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                if (listBox.SelectedItem is Warehouse warehouse)
                {
                    SelectedWarehouse = warehouse;
                }
            }
        }
    }
}
