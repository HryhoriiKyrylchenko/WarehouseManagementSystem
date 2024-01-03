using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Support_data;

namespace WarehouseManagementSystem.ViewModels
{
    public class ShipmentsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

        private ShipmentsSelectorsFilterModel filterSelectors;

        public ShipmentsSelectorsFilterModel FilterSelectors
        {
            get { return filterSelectors; }
            set
            {
                if (filterSelectors != value)
                {
                    filterSelectors = value;
                    OnPropertyChanged(nameof(FilterSelectors));
                }
            }
        }

        private ObservableCollection<Shipment>? shipments;

        public ObservableCollection<Shipment>? Shipments
        {
            get { return shipments; }
            set
            {
                if (shipments != value)
                {
                    shipments = value;
                    OnPropertyChanged(nameof(Shipments));
                }
            }
        }

        private Shipment? selectedShipment;

        public Shipment? SelectedShipment
        {
            get { return selectedShipment; }
            set
            {
                if (selectedShipment != value)
                {
                    selectedShipment = value;
                    OnPropertyChanged(nameof(SelectedShipment));
                }
            }
        }

        private ObservableCollection<Customer>? customers;

        public ObservableCollection<Customer>? Customers
        {
            get { return customers; }
            set
            {
                if (customers != value)
                {
                    customers = value;
                    OnPropertyChanged(nameof(Customers));
                }
            }
        }

        private ObservableCollection<User>? users;

        public ObservableCollection<User>? Users
        {
            get { return users; }
            set
            {
                if (users != value)
                {
                    users = value;
                    OnPropertyChanged(nameof(Users));
                }
            }
        }

        public ShipmentsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            filterSelectors = new ShipmentsSelectorsFilterModel(this);
            FilterSelectors.SectionAllCustomersSelected = true;
            FilterSelectors.SectionAllUsersSelected = true;

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await InitializeCustomersFromDBAsync();
            await InitializeUsersFromDBAsync();
        }

        private async Task InitializeCustomersFromDBAsync()
        {
            try
            {
                using (var dbManager = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    Customers = await dbManager.GetCustomersAsync();
                }
            }
            catch (Exception ex)
            {
                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    await logger.LogErrorAsync(ex);
                }
            }
        }

        private async Task InitializeUsersFromDBAsync()
        {
            try
            {
                using (var dbManager = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    Users = await dbManager.GetUsersWithoutAdminAsync();
                }
            }
            catch (Exception ex)
            {
                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    await logger.LogErrorAsync(ex);
                }
            }
        }

        public ICommand BackCommand => new RelayCommand(Back);

        public ICommand ShowCommand => new RelayCommand(Show);

        private async void Show(object obj)
        {
            try
            {
                using (var dbManager = new WarehouseDBManager(new WarehouseDbContext()))
                {

                    Shipments = await dbManager.GetShipmentsByFilterAsync(FilterSelectors);
                }
            }
            catch (Exception ex)
            {
                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    await logger.LogErrorAsync(ex);
                }
            }
        }

        private void Back(object parameter)
        {
            mainViewModel.NavigateBack();
        }
    }
}
