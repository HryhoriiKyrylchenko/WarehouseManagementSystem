using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models.Builders;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.Windows;
using WarehouseManagementSystem.ViewModels.Interfaces;
using System.Transactions;
using WarehouseManagementSystem.ViewModels.Helpers;

namespace WarehouseManagementSystem.ViewModels
{
    public class AddSupplierViewModel : ViewModelBaseRequestClose, IHasAddress
    {
        private readonly AddEditReceiptViewModel mainViewModel;

        private SupplierViewModel supplierViewModel;
        public SupplierViewModel SupplierViewModel
        {
            get { return supplierViewModel; }
            set
            {
                if (supplierViewModel != value)
                {
                    this.supplierViewModel = value;
                    OnPropertyChanged(nameof(SupplierViewModel));
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
                if (SupplierViewModel.Address != value)
                {
                    SupplierViewModel.Address = value;
                };
            }
        }

        public AddSupplierViewModel(AddEditReceiptViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            supplierViewModel = new SupplierViewModel();
        }

        private void AddAddress(object obj)
        {
            SupportWindow supportWindow = new SupportWindow(new AddEditAddressViewModel(this));
            supportWindow.ShowDialog();
        }

        private void EditAddress(object obj)
        {
            if (SupplierViewModel.Address != null)
            {
                SupportWindow supportWindow = new SupportWindow(new AddEditAddressViewModel(this, SupplierViewModel.Address));
                supportWindow.ShowDialog();
            }
        }

        private void Add(object obj)
        {
            if (ConfirmationHelper.GetConfirmation() != MessageBoxResult.OK)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(SupplierViewModel.Name)
                || SupplierViewModel.Address == null)
            {
                MessageHelper.ShowCautionMessage("Name and address is required");
                return;
            }

            using (var scope = new TransactionScope())
            {
                try
                {
                    var supplierBuilder = new SupplierBuilder(SupplierViewModel.Name, SupplierViewModel.Address.Id);

                    if (!string.IsNullOrWhiteSpace(SupplierViewModel.AdditionalInfo))
                    {
                        supplierBuilder.WithAdditionalInfo(SupplierViewModel.AdditionalInfo);
                    }

                    Supplier newSupplier = supplierBuilder.Build();
                    mainViewModel.UpdateSuppliers();
                    mainViewModel.CurrentReceiptViewModel.Supplier = mainViewModel.Suppliers.FirstOrDefault(s => s.Id == newSupplier.Id);

                    scope.Complete();
                    CloseParentWindow();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    ExceptionHelper.HandleException(ex);
                }
            }
        }

        private void Cancel(object obj)
        {
            if (ConfirmationHelper.GetCancelConfirmation() == MessageBoxResult.OK)
            {
                try
                {
                    if (SupplierViewModel.Address != null)
                    {
                        using (EntityManager db = new EntityManager(new Models.WarehouseDbContext()))
                        {
                            db.DeleteAddress(SupplierViewModel.Address);
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
}

