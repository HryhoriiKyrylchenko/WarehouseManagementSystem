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
using WarehouseManagementSystem.ViewModels.Helpers;
using WarehouseManagementSystem.ViewModels.Interfaces;
using WarehouseManagementSystem.Windows;

namespace WarehouseManagementSystem.ViewModels
{
    class AddManufacturerViewModel : ViewModelBaseRequestClose, IHasAddress
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

        public Address? Address
        {
            set
            {
                if (ManufacturerViewModel.Address != value)
                {
                    ManufacturerViewModel.Address = value;
                };
            }
        }

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
            if (string.IsNullOrWhiteSpace(ManufacturerViewModel.Name))
            {
                MessageHelper.ShowCautionMessage("Name is required");
                return;
            }

            if (ConfirmationHelper.GetConfirmation() != MessageBoxResult.OK)
            {
                return;
            }

            try
            {
                var newManufacturerBuilder = new ManufacturerBuilder(ManufacturerViewModel.Name);

                if (!string.IsNullOrWhiteSpace(ManufacturerViewModel.Description))
                {
                    newManufacturerBuilder.WithDescription(ManufacturerViewModel.Description);
                }
                if (ManufacturerViewModel.Address != null)
                {
                    newManufacturerBuilder.WithAddress(ManufacturerViewModel.Address.Id);
                }
                if (!string.IsNullOrWhiteSpace(ManufacturerViewModel.AdditionalInfo))
                {
                    newManufacturerBuilder.WithAdditionalInfo(ManufacturerViewModel.AdditionalInfo);
                }

                Manufacturer newManufacturer = newManufacturerBuilder.Build();
                mainViewModel.CurrentProductViewModel.Manufacturer = newManufacturer;
                CloseParentWindow();
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
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
                ExceptionHelper.HandleException(ex);
            }

            CloseParentWindow();
        }
    }
}
