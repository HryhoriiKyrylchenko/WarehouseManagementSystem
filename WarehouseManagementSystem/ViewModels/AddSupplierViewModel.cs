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
            if (GetConfirmation() == MessageBoxResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(SupplierViewModel.Name) 
                    && SupplierViewModel.Address != null)
                {
                    try
                    {
                        var newSB = new SupplierBuilder(SupplierViewModel.Name, SupplierViewModel.Address.Id);

                        if (!string.IsNullOrWhiteSpace(SupplierViewModel.AdditionalInfo))
                        {
                            newSB = newSB.WithAdditionalInfo(SupplierViewModel.AdditionalInfo);
                        }

                        Supplier newSupplier = newSB.Build();
                        mainViewModel.UpdateSuppliers();
                        mainViewModel.CurrentReceiptViewModel.Supplier = newSupplier;
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
                else
                {
                    MessageBox.Show("Name and address is required",
                        "Caution",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }
        }

        private MessageBoxResult GetConfirmation()
        {
            return MessageBox.Show("Do you want to make this changes?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        private MessageBoxResult GetCancelConfirmation()
        {
            return MessageBox.Show("All unsaved data will be lost! Continue?",
                "Confirmation",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
        }

        private void Cancel(object obj)
        {
            if (GetCancelConfirmation() == MessageBoxResult.OK)
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
                    using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                    {
                        logger.LogError(ex);
                    }
                }

                CloseParentWindow();
            }
        }
    }
}
