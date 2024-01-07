using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.Windows;

namespace WarehouseManagementSystem.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Stack<ViewModelBase> viewModelStack;

        private ViewModelBase? currentViewModel;

        public ViewModelBase CurrentViewModel 
        {
            get { return currentViewModel ?? this; }
            set
            {
                if (currentViewModel != value)
                {
                    currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        private LoginService loginService;

        public LoginService LoginService
        {
            get { return loginService; }
            set
            {
                if (loginService != value)
                {
                    loginService = value;
                    OnPropertyChanged();
                }
            }
        }

        private SummaryViewModel summaryViewModel;

        public SummaryViewModel SummaryViewModel
        {
            get { return summaryViewModel; }
            set
            {
                if (summaryViewModel != value)
                {
                    summaryViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ShowProductsCommand => new RelayCommand(_ => NavigateToViewModel(new ProductsViewModel(this)));
        public ICommand ShowMoveProductsCommand => new RelayCommand(_ => NavigateToViewModel(new MoveProductsViewModel(this)));
        public ICommand ShowReceiptsCommand => new RelayCommand(_ => NavigateToViewModel(new ReceiptsViewModel(this)));
        public ICommand ShowShipmentsCommand => new RelayCommand(_ => NavigateToViewModel(new ShipmentsViewModel(this)));
        public ICommand ShowReportsCommand => new RelayCommand(_ => NavigateToViewModel(new ReportsViewModel(this)));
        public ICommand ShowSettingsCommand => new RelayCommand(_ => NavigateToViewModel(new SettingsViewModel(this)));
        public ICommand LogoutCommand => new RelayCommand(_ => Logout());
        public ICommand RefreshSummaryCommand => new RelayCommand(_ => RefreshSummary());

        public MainViewModel()
        {
            loginService = new LoginService();
            CheckUserLogin();

            viewModelStack = new Stack<ViewModelBase>();
            CurrentViewModel = this;

            summaryViewModel = new SummaryViewModel(loginService.CurrentWarehouse);
            OnPropertyChanged(nameof(SummaryViewModel));

            ThreadPool.QueueUserWorkItem(InitializeAsync);
        }

        private  void InitializeAsync(object? state)
        {
            summaryViewModel.GetData();

            UpdateUI();
        }

        private void UpdateUI()
        {
            OnPropertyChanged(nameof(SummaryViewModel));
        }

        private void CheckUserLogin()
        {
            while (!loginService.IsUserLoggedIn())
            {
                ShowLoginWindow();
            }
        }

        private void ShowLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow(loginService);
            loginWindow.ShowDialog();
        }

        public void NavigateToViewModel(ViewModelBase viewModel)
        {
            viewModelStack.Push(CurrentViewModel); 
            CurrentViewModel = viewModel;
        }

        public void NavigateBack()
        {
            if (viewModelStack.Count > 0)
            {
                CurrentViewModel = viewModelStack.Pop();
            }
        }

        public void Logout()
        {
            loginService.Logout();
            CheckUserLogin();
        }

        private void RefreshSummary()
        {
            summaryViewModel.TotalCapacity = string.Empty;
            summaryViewModel.FreeCapacity = string.Empty;
            summaryViewModel.OccupancyPercentage = string.Empty;
            summaryViewModel.TotalZones = string.Empty;
            summaryViewModel.UnusedZones = string.Empty;
            summaryViewModel.TotalProducts = string.Empty;
            summaryViewModel.UnallocatedProducts = string.Empty;
            ThreadPool.QueueUserWorkItem(InitializeAsync);
        }
    }
}
