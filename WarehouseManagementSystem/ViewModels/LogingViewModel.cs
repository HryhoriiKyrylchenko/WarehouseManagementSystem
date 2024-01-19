using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.ViewModels.Helpers;

namespace WarehouseManagementSystem.ViewModels
{
    public class LogingViewModel : ViewModelBaseRequestClose
    {
        LoginService loginService;

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

        private string? username;

        public string? Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        private string? password;
        public string? Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public ICommand LoginCommand => new RelayCommand(Login);
        public ICommand ExitCommand => new RelayCommand(Exit);

        public LogingViewModel(LoginService loginService)
        {
            this.loginService = loginService;
            warehouses = new ObservableCollection<Warehouse>();

            Warehouses = GetWarehouses();
            if (Warehouses.Count > 0)
            {
                SelectedWarehouse = Warehouses.First();
            }
        }

        private void Login(object parameter)
        {
            if (IsValidInput(Username, Password, SelectedWarehouse))
            {
                LoginProgram(Username, Password, SelectedWarehouse);
            }
        }

        private void LoginProgram(string? username, string? password, Warehouse? warehouse)
        {
            if (username == null || password == null || warehouse == null)
            {
                return;
            }

            if (loginService.Login(username, password, warehouse))
            {
                CloseParentWindow();
            }
            else
            {
                MessageHelper.ShowCautionMessage("Wrong data entered");
                Username = string.Empty;
                Password = string.Empty;
            }
        }

        private ObservableCollection<Warehouse> GetWarehouses()
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new Models.WarehouseDbContext()))
                {
                    return db.GetAllWarehouses();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                return new ObservableCollection<Warehouse>();
            }
        }

        private void Exit(object parameter)
        {
            Application.Current.Shutdown();
        }

        public bool IsValidInput(string? username, string? password, Warehouse? warehouse)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || warehouse == null)
            {
                MessageHelper.ShowCautionMessage("All fields are required");
                return false;
            }

            return true;
        }
    }
}
