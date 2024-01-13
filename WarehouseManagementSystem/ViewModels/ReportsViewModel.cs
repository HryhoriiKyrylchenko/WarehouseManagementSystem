using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Models.Entities.Support_classes;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Support_data;
using WarehouseManagementSystem.Windows;

namespace WarehouseManagementSystem.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

        private ReportsSelectorsFilterModel filterSelectors;

        public ReportsSelectorsFilterModel FilterSelectors
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

        private ObservableCollection<Report>? reports;

        public ObservableCollection<Report>? Reports
        {
            get { return reports; }
            set
            {
                if (reports != value)
                {
                    reports = value;
                    OnPropertyChanged(nameof(Reports));
                }
            }
        }

        private Report? selectedReport;

        public Report? SelectedReport
        {
            get { return selectedReport; }
            set
            {
                if (selectedReport != value)
                {
                    selectedReport = value;
                    OnPropertyChanged(nameof(SelectedReport));
                    UpdateSelectedReportContent();
                }
            }
        }

        private ObservableCollection<object>? selectedReportContent;

        public ObservableCollection<object>? SelectedReportContent
        {
            get { return selectedReportContent; }
            set
            {
                if (selectedReportContent != value)
                {
                    selectedReportContent = value;
                    OnPropertyChanged(nameof(SelectedReportContent));
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

        private ObservableCollection<DateTime>? reportsDates;

        public ObservableCollection<DateTime>? ReportsDates
        {
            get { return reportsDates; }
            set
            {
                if (reportsDates != value)
                {
                    reportsDates = value;
                    OnPropertyChanged(nameof(ReportsDates));
                }
            }
        }

        public ICommand BackCommand => new RelayCommand(Back);
        public ICommand ShowCommand => new RelayCommand(Show);
        public ICommand AddCommand => new RelayCommand(AddReport);
        public ICommand DeleteCommand => new RelayCommand(DeleteReport);

        public ReportsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            filterSelectors = new ReportsSelectorsFilterModel();

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await InitializeReportsAndReportsDatesFromDBAsync();
            await InitializeUsersFromDBAsync();
        }

        private async Task InitializeReportsAndReportsDatesFromDBAsync()
        {
            try
            {
                using (var dbManager = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    Reports = await dbManager.GetReportsAsync();
                    if (Reports != null && Reports.Any())
                    {
                        ReportsDates = new ObservableCollection<DateTime>(Reports.Select(r => r.ReportDate).ToList());
                    }
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

        private async void Show(object obj)
        {
            try
            {
                using (var dbManager = new WarehouseDBManager(new WarehouseDbContext()))
                {

                    Reports = await dbManager.GetReportsByFilterAsync(FilterSelectors);
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

        private void AddReport(object parameter)
        {
            SupportWindow supportWindow = new SupportWindow(new ReportSelectionViewModel(mainViewModel));
            supportWindow.ShowDialog();
        }

        private void DeleteReport(object parameter)
        {
            if (SelectedReport != null
                && mainViewModel.LoginService.CurrentUser != null
                && SelectedReport.User != null)
            {
                if(mainViewModel.LoginService.CurrentUser.Role == UserRolesEnum.ADMIN
                || mainViewModel.LoginService.CurrentUser.Role == UserRolesEnum.DIRECTOR
                || mainViewModel.LoginService.CurrentUser.Role == UserRolesEnum.MANAGER)
                {
                    if (GetConfirmation() == MessageBoxResult.OK)
                    {
                        DeleteSelectedReport(SelectedReport);
                        Reports?.Remove(SelectedReport);
                        SelectedReport = null;
                    }
                }
                else if (mainViewModel.LoginService.CurrentUser.Role == UserRolesEnum.GUEST
                    || mainViewModel.LoginService.CurrentUser.Username != SelectedReport.User.Username)
                {
                    MessageBox.Show("You do not have rigts to do this changes",
                        "Caution",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Exclamation);
                }
                else if (mainViewModel.LoginService.CurrentUser.Username == SelectedReport.User.Username)
                {
                    if (GetConfirmation() == MessageBoxResult.OK)
                    {
                        DeleteSelectedReport(SelectedReport);
                    }
                }
            } 
        }

        private void DeleteSelectedReport(Report report)
        {
            try
            {
                using (EntityManager db = new EntityManager(new WarehouseDbContext()))
                {
                    db.DeleteReport(report);
                }
            }
            catch (Exception ex)
            {
                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    logger.LogError(ex);
                }

                MessageBox.Show("Something went wrong, try again or contact your system administrator",
                        "Caution",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Exclamation);
            }
        }

        private MessageBoxResult GetConfirmation()
        {
            return MessageBox.Show("Do you want to make this changes?",
                "Confirmation",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
        }

        private void UpdateSelectedReportContent()
        {
            if (SelectedReport != null)
            {
                switch (SelectedReport.ReportType)
                {
                    case ReportTypeEnum.PRODUCTS:
                        var products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(SelectedReport.Content);
                        if (products != null)
                        {
                            SelectedReportContent = new ObservableCollection<object>(products);
                        }
                        break;
                    case ReportTypeEnum.MOVEMENTS:
                        var movements = JsonConvert.DeserializeObject<ObservableCollection<MovementHistory>>(SelectedReport.Content);
                        if (movements != null)
                        {
                            SelectedReportContent = new ObservableCollection<object>(movements);
                        }
                        break;
                    case ReportTypeEnum.RECEIPTS:
                        var receipts = JsonConvert.DeserializeObject<ObservableCollection<Receipt>>(SelectedReport.Content);
                        if (receipts != null)
                        {
                            SelectedReportContent = new ObservableCollection<object>(receipts);
                        }
                        break;
                    case ReportTypeEnum.SHIPMENTS:
                        var shipments = JsonConvert.DeserializeObject<ObservableCollection<Shipment>>(SelectedReport.Content);
                        if (shipments != null)
                        {
                            SelectedReportContent = new ObservableCollection<object>(shipments);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
