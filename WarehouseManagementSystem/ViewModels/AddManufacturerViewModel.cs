using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models.Builders;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.Windows;

namespace WarehouseManagementSystem.ViewModels
{
    class AddManufacturerViewModel : ViewModelBaseRequestClose
    {
        private readonly AddEditProductViewModel mainViewModel;

        private ManufacturerViewModel manufacturerViewModel;
        public ManufacturerViewModel ManufacturerViewModel
        {
            get { return manufacturerViewModel; }
            set
            {
                if (manufacturerViewModel != value)
                {
                    this.manufacturerViewModel = value;
                    OnPropertyChanged(nameof(ManufacturerViewModel));
                };
            }
        }

        public ICommand AddAddressCommand => new RelayCommand(AddAddress);
        public ICommand EditAddressCommand => new RelayCommand(EditAddress);
        public ICommand AddCommand => new RelayCommand(Add);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public AddManufacturerViewModel(AddEditProductViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            manufacturerViewModel = new ManufacturerViewModel();
        }

        private void AddAddress(object obj)
        {
            SupportWindow supportWindow = new SupportWindow(new AddEditAddressViewModel(this));
            supportWindow.ShowDialog();
        }

        private void EditAddress(object obj)
        {
            if (ManufacturerViewModel.Address != null)
            {
                SupportWindow supportWindow = new SupportWindow(new AddEditAddressViewModel(this, ManufacturerViewModel.Address));
                supportWindow.ShowDialog();
            }
        }

        private void Add(object obj)
        {
            if (GetConfirmation() == MessageBoxResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(ManufacturerViewModel.Name))
                {
                    try
                    {
                        var newMB = new ManufacturerBuilder(ManufacturerViewModel.Name);

                        if (!string.IsNullOrWhiteSpace(ManufacturerViewModel.Description))
                        {
                            newMB = newMB.WithDescription(ManufacturerViewModel.Description);
                        }
                        if (ManufacturerViewModel.Address != null)
                        {
                            newMB = newMB.WithAddress(ManufacturerViewModel.Address.Id);
                        }
                        if (!string.IsNullOrWhiteSpace(ManufacturerViewModel.AdditionalInfo))
                        {
                            newMB = newMB.WithAdditionalInfo(ManufacturerViewModel.AdditionalInfo);
                        }

                        Manufacturer newManufacturer = newMB.Build();
                        mainViewModel.Initialize();
                        mainViewModel.CurrentProductViewModel.Manufacturer = newManufacturer;
                        CloseParentWindow();
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                        {
                            logger.LogError(ex);
                        }
                    }
                }
            }
        }

        private MessageBoxResult GetConfirmation()
        {
            return MessageBox.Show("Do you want to make this changes?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        private void Cancel(object obj)
        {
            try
            {
                if (ManufacturerViewModel.Address != null)
                {
                    using (EntityManager db = new EntityManager(new Models.WarehouseDbContext()))
                    {
                        db.DeleteAddress(ManufacturerViewModel.Address);
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    logger.LogError(ex);
                }
            }

            CloseParentWindow();
        }
    }
}
