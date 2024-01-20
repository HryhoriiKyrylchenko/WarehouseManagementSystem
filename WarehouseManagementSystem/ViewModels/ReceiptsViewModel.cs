using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Attributes;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Migrations;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Helpers;
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

        private PermissionManager permissionManager;
        public PermissionManager PermissionManager
        {
            get { return permissionManager; }
            private set
            {
                if (permissionManager != value)
                {
                    permissionManager = value;
                }
            }
        }

        public ICommand BackCommand => new RelayCommand(Back);
        public ICommand ShowCommand => new RelayCommand(Show, parameter => permissionManager.CanExecute(parameter,
            typeof(ReceiptsViewModel).GetMethod(nameof(Show)) ?? throw new ArgumentNullException()));
        public ICommand SaveReportCommand => new RelayCommand(SaveReport, parameter => permissionManager.CanExecute(parameter,
            typeof(ReceiptsViewModel).GetMethod(nameof(SaveReport)) ?? throw new ArgumentNullException()));
        public ICommand AddCommand => new RelayCommand(AddReceipt, parameter => permissionManager.CanExecute(parameter,
            typeof(ReceiptsViewModel).GetMethod(nameof(AddReceipt)) ?? throw new ArgumentNullException()));
        public ICommand EditCommand => new RelayCommand(EditReceipt, parameter => permissionManager.CanExecute(parameter,
            typeof(ReceiptsViewModel).GetMethod(nameof(EditReceipt)) ?? throw new ArgumentNullException()));

        public ReceiptsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            if (MainViewModel.LoginService.CurrentUser == null) throw new ArgumentNullException();
            permissionManager = new PermissionManager(MainViewModel.LoginService.CurrentUser.Role);
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

        [AccessPermission(UserPermissionEnum.ViewAllData,
                          UserPermissionEnum.ViewAllReceipts,
                          UserPermissionEnum.ViewSelfReceipts)]
        public async void Show(object obj)
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
                await ExceptionHelper.HandleExceptionAsync(ex);
            }
        }

        private void Back(object parameter)
        {
            mainViewModel.NavigateBack();
        }

        [AccessPermission(UserPermissionEnum.ManageAllData,
                          UserPermissionEnum.CreateAllReports,
                          UserPermissionEnum.CreateSelfReports)]
        public void SaveReport(object parameter)
        {
            try
            {
                if (Receipts == null
                || !Receipts.Any())
                {
                    MessageHelper.ShowErrorMessage("No info to be saved");
                    return;
                }

                if (mainViewModel.LoginService.CurrentUser == null)
                {
                    throw new ArgumentNullException("Current program user is null");
                }

                string title = GenereteTitle();
                string content = GenereteContentToJson(Receipts);

                SupportWindow supportWindow = new SupportWindow(new SaveReportViewModel(title,
                                                                                    Enums.ReportTypeEnum.RECEIPTS,
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

        [AccessPermission(UserPermissionEnum.ManageAllData,
                          UserPermissionEnum.ManageAllReceipts,
                          UserPermissionEnum.ManageSelfReceipts)]
        public void AddReceipt(object parameter)
        {
            SupportWindow supportWindow = new SupportWindow(new AddEditReceiptViewModel(this));
            supportWindow.ShowDialog();
            InitializeAsync();
            Show(this);
        }

        [AccessPermission(UserPermissionEnum.ManageAllData,
                          UserPermissionEnum.ManageAllReceipts,
                          UserPermissionEnum.ManageSelfReceipts)]
        public void EditReceipt(object parameter)
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
