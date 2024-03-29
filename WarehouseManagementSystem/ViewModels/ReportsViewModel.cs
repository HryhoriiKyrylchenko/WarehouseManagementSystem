﻿using Newtonsoft.Json;
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
using WarehouseManagementSystem.Attributes;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Models.Entities.Support_classes;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Helpers;
using WarehouseManagementSystem.ViewModels.Support_data;
using WarehouseManagementSystem.Windows;

namespace WarehouseManagementSystem.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;
        public MainViewModel MainViewModel
        {
            get { return mainViewModel; }
        }

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
            typeof(ReportsViewModel).GetMethod(nameof(Show)) ?? throw new ArgumentNullException()));
        public ICommand AddCommand => new RelayCommand(AddReport, parameter => permissionManager.CanExecute(parameter,
            typeof(ReportsViewModel).GetMethod(nameof(AddReport)) ?? throw new ArgumentNullException()));
        public ICommand DeleteCommand => new RelayCommand(DeleteReport, parameter => permissionManager.CanExecute(parameter,
            typeof(ReportsViewModel).GetMethod(nameof(DeleteReport)) ?? throw new ArgumentNullException()));

        public ReportsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            if (MainViewModel.LoginService.CurrentUser == null) throw new ArgumentNullException();
            permissionManager = new PermissionManager(MainViewModel.LoginService.CurrentUser.Role);
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

        [AccessPermission(UserPermissionEnum.ViewAllData,
                          UserPermissionEnum.ViewAllReports,
                          UserPermissionEnum.ViewSelfReports)]
        public async void Show(object obj)
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
        public void AddReport(object parameter)
        {
            SupportWindow supportWindow = new SupportWindow(new ReportSelectionViewModel(mainViewModel));
            supportWindow.ShowDialog();
        }

        [AccessPermission(UserPermissionEnum.ManageAllData)]
        public void DeleteReport(object parameter)
        {
            if (SelectedReport == null
                || mainViewModel.LoginService.CurrentUser == null
                || SelectedReport.User == null)
            {
                MessageHelper.ShowCautionMessage("Select report to delete");
                return;
            }

            if (mainViewModel.LoginService.CurrentUser.Role == UserRolesEnum.ADMIN
            || mainViewModel.LoginService.CurrentUser.Role == UserRolesEnum.DIRECTOR
            || mainViewModel.LoginService.CurrentUser.Role == UserRolesEnum.MANAGER)
            {
                if (ConfirmationHelper.GetConfirmation() == MessageBoxResult.OK)
                {
                    DeleteSelectedReport(SelectedReport);
                    Reports?.Remove(SelectedReport);
                    SelectedReport = null;
                }
            }
            else if (mainViewModel.LoginService.CurrentUser.Role == UserRolesEnum.GUEST
                || mainViewModel.LoginService.CurrentUser.Username != SelectedReport.User.Username)
            {
                MessageHelper.ShowCautionMessage("You do not have rigts to do this changes");
            }
            else if (mainViewModel.LoginService.CurrentUser.Username == SelectedReport.User.Username)
            {
                if (ConfirmationHelper.GetConfirmation() == MessageBoxResult.OK)
                {
                    DeleteSelectedReport(SelectedReport);
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
                ExceptionHelper.HandleException(ex);
                MessageHelper.ShowCautionMessage("Something went wrong, try again or contact your system administrator");
            }
        }

        private void UpdateSelectedReportContent()
        {
            try
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
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                MessageHelper.ShowCautionMessage("Something went wrong");
            }
        }
    }
}
