using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Helpers;
using WarehouseManagementSystem.ViewModels.Support_data;
using WarehouseManagementSystem.Windows;

namespace WarehouseManagementSystem.ViewModels
{
    public class ShipmentsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;
        public MainViewModel MainViewModel
        {
            get { return mainViewModel; }
        }


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
                await ExceptionHelper.HandleExceptionAsync(ex);
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
                await ExceptionHelper.HandleExceptionAsync(ex);
            }
        }

        public ICommand BackCommand => new RelayCommand(Back);
        public ICommand ShowCommand => new RelayCommand(Show);
        public ICommand SaveReportCommand => new RelayCommand(SaveReport);
        public ICommand AddCommand => new RelayCommand(AddShipment);
        public ICommand EditCommand => new RelayCommand(EditShipment);

        private async void Show(object parameter)
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
                await ExceptionHelper.HandleExceptionAsync(ex);
            }
        }

        private void Back(object parameter)
        {
            mainViewModel.NavigateBack();
        }

        private void SaveReport(object parameter)
        {
            try
            {
                if (Shipments == null
                || !Shipments.Any())
                {
                    MessageHelper.ShowErrorMessage("No info to be saved");
                    return;
                }

                if (mainViewModel.LoginService.CurrentUser == null)
                {
                    throw new ArgumentNullException("Current program user is null");
                }

                string title = GenereteTitle();
                string content = GenereteContentToJson(Shipments);

                SupportWindow supportWindow = new SupportWindow(new SaveReportViewModel(title,
                                                                                    Enums.ReportTypeEnum.SHIPMENTS,
                                                                                    content,
                                                                                    mainViewModel.LoginService.CurrentUser.Id));
                supportWindow.ShowDialog();

            }
            catch (Exception ex)
            {
                HandleSaveReportException(ex);
            }
        }

        private void HandleSaveReportException(Exception ex)
        {
            MessageHelper.ShowErrorMessage("Failed to save a report");
            ExceptionHelper.HandleException(ex);
        }

        private string GenereteTitle()
        {
            StringBuilder newTitle = new StringBuilder();
            newTitle.Append("Shipment/");

            string formattedDateFrom = FilterSelectors.SectionDateFrom?.ToString("dd-MM-yyyy") ?? "N/A";
            newTitle.Append($"From {formattedDateFrom}/");

            string formattedDateTo = FilterSelectors.SectionDateTo?.ToString("dd-MM-yyyy") ?? "N/A";
            newTitle.Append($"To {formattedDateTo}/");

            if (FilterSelectors.SectionAllCustomersSelected)
            {
                newTitle.Append("All customers/");
            }
            else if (FilterSelectors.SectionByCustomerSelected)
            {
                newTitle.Append($"Customer: {FilterSelectors.SelectedCustomer}/");
            }

            if (FilterSelectors.SectionAllUsersSelected)
            {
                newTitle.Append("All users/");
            }
            else if (FilterSelectors.SectionByUserSelected)
            {
                newTitle.Append($"User: {FilterSelectors.SelectedUser}/");
            }

            return newTitle.ToString();
        }

        private string GenereteContentToJson(ICollection<Shipment> content)
        {
            return JsonConvert.SerializeObject(content, Formatting.None);
        }

        private void AddShipment(object parameter)
        {
            SupportWindow supportWindow = new SupportWindow(new AddEditShipmentViewModel(this));
            supportWindow.ShowDialog();
            InitializeAsync();
            Show(this);
        }

        private void EditShipment(object parameter)
        {
            if (SelectedShipment != null)
            {
                SupportWindow supportWindow = new SupportWindow(new AddEditShipmentViewModel(this, SelectedShipment));
                supportWindow.ShowDialog();
                InitializeAsync();
                Show(this);
            }
        }
    }
}
