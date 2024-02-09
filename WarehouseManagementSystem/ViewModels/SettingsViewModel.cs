using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Helpers;
using WarehouseManagementSystem.ViewModels.Support_data;

namespace WarehouseManagementSystem.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

        private string currentConnectionString;
        public string CurrentConnectionString
        {
            get { return currentConnectionString; }
            set
            {
                if (currentConnectionString != value)
                {
                    currentConnectionString = value;
                    OnPropertyChanged(nameof(CurrentConnectionString));
                }
            }
        }

        public SettingsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            currentConnectionString = GetCurrentConnectionString();
        }

        private static string GetCurrentConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

            string? connectionString = configuration.GetConnectionString("CurrentConnection");

            return connectionString ?? "";
        }

        public ICommand BackCommand => new RelayCommand(Back);
        public ICommand SaveCommand => new RelayCommand(Save);

        private void Save(object parameter)
        {
            if (ConfirmationHelper.GetConfirmation() == MessageBoxResult.OK)
            {
                SaveNewConnectionString();
            }
        }

        private void Back(object parameter)
        {
            mainViewModel.NavigateBack();
        }

        private void SaveNewConnectionString()
        {
            if (string.IsNullOrWhiteSpace(CurrentConnectionString))
            {
                MessageHelper.ShowCautionMessage("Enter valid connection string");
                return;
            }

            AppSettings.UpdateConnectionString(CurrentConnectionString);
        }
    }
}
