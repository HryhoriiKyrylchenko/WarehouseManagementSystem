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
using WarehouseManagementSystem.Migrations;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Support_data;
using WarehouseManagementSystem.Windows;

namespace WarehouseManagementSystem.ViewModels
{
    public class ReceiptsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;
        public MainViewModel MainViewModel
        {
            get { return mainViewModel; }
        }

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
            if (Receipts != null
                && Receipts.Any()
                && mainViewModel.LoginService.CurrentUser != null)
            {
                string title = GenereteTitle();
                string content = GenereteContentToJson(Receipts);

                SupportWindow supportWindow = new SupportWindow(new SaveReportViewModel(title,
                                                                                    Enums.ReportTypeEnum.RECEIPTS,
                                                                                    content,
                                                                                    mainViewModel.LoginService.CurrentUser.Id));
                supportWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("No info to be saved",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private string GenereteTitle()
        {
            StringBuilder newTitle = new StringBuilder();
            newTitle.Append("Receipts/");

            string formattedDateFrom = FilterSelectors.SectionDateFrom?.ToString("dd-MM-yyyy") ?? "N/A";
            newTitle.Append($"From {formattedDateFrom}/");

            string formattedDateTo = FilterSelectors.SectionDateTo?.ToString("dd-MM-yyyy") ?? "N/A";
            newTitle.Append($"To {formattedDateTo}/");

            if (FilterSelectors.SectionAllSuppliersSelected)
            {
                newTitle.Append("All customers/");
            }
            else if (FilterSelectors.SectionBySupplierSelected)
            {
                newTitle.Append($"Supplier: {FilterSelectors.SelectedSupplier}/");
            }

            if (FilterSelectors.SectionAllUsersSelected)
            {
                newTitle.Append("All users/");
            }
            else if (FilterSelectors.SectionByUserSelected)
            {
                newTitle.Append($"User: {FilterSelectors.SelectedSupplier}/");
            }

            return newTitle.ToString();
        }

        private string GenereteContentToJson(ICollection<Receipt> content)
        {
            return JsonConvert.SerializeObject(content, Formatting.None);
        }

        private void AddReceipt(object parameter)
        {
            SupportWindow supportWindow = new SupportWindow(new AddEditReceiptViewModel(this));
            supportWindow.ShowDialog();
            InitializeAsync();
            Show(this);
        }

        private void EditReceipt(object parameter)
        {
            if (SelectedReceipt != null)
            {
                SupportWindow supportWindow = new SupportWindow(new AddEditReceiptViewModel(this, SelectedReceipt));
                supportWindow.ShowDialog();
                InitializeAsync();
                Show(this);
            }
        }
    }
}
