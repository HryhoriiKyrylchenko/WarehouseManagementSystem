using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Migrations;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Support_data;

namespace WarehouseManagementSystem.ViewModels
{
    public class ReceiptsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

        private ReceiptsSelectorsFilterModel filterSelectors;

        public ReceiptsSelectorsFilterModel FilterSelectors
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

        private ObservableCollection<Receipt>? receipts;

        public ObservableCollection<Receipt>? Receipts
        {
            get { return receipts; }
            set
            {
                if (receipts != value)
                {
                    receipts = value;
                    OnPropertyChanged(nameof(Receipts));
                }
            }
        }

        private Receipt? selectedReceipt;

        public Receipt? SelectedReceipt
        {
            get { return selectedReceipt; }
            set
            {
                if (selectedReceipt != value)
                {
                    selectedReceipt = value;
                    OnPropertyChanged(nameof(SelectedReceipt));
                }
            }
        }

        private ObservableCollection<Supplier>? suppliers;

        public ObservableCollection<Supplier>? Suppliers
        {
            get { return suppliers; }
            set
            {
                if (suppliers != value)
                {
                    suppliers = value;
                    OnPropertyChanged(nameof(Suppliers));
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

        public ReceiptsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            filterSelectors = new ReceiptsSelectorsFilterModel(this);
            FilterSelectors.SectionAllSuppliersSelected = true;
            FilterSelectors.SectionAllUsersSelected = true;

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await InitializeSuppliersFromDBAsync();
            await InitializeUsersFromDBAsync();
        }

        private async Task InitializeSuppliersFromDBAsync()
        {
            try
            {
                using (var dbManager = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    Suppliers = await dbManager.GetSuppliersAsync();
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
        public ICommand SaveReportCommand => new RelayCommand(SaveReport);
        public ICommand AddCommand => new RelayCommand(AddReceipt);
        public ICommand EditCommand => new RelayCommand(EditReceipt);

        private async void Show(object obj)
        {
            try
            {
                using (var dbManager = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    
                    Receipts = await dbManager.GetReceiptsByFilterAsync(FilterSelectors);
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

        private void SaveReport(object parameter)
        {
            /////////////////////////////////////
        }

        private void AddReceipt(object parameter)
        {
            /////////////////////////////////////
        }

        private void EditReceipt(object parameter)
        {
            /////////////////////////////////////
        }
    }
}
