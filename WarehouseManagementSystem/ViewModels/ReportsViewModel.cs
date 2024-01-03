using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
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

        private string? selectedReportContent;

        public string? SelectedReportContent
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
            ///////////////////////////////////////////
        }

        private void UpdateSelectedReportContent()
        {
            if (selectedReport != null) 
            {
                //selectedReportContent = selectedReport.Content;
            }
        }
    }
}
