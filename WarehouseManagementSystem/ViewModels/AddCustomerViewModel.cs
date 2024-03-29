﻿using System;
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
using WarehouseManagementSystem.ViewModels.Interfaces;
using WarehouseManagementSystem.Windows;
using System.Transactions;
using WarehouseManagementSystem.ViewModels.Helpers;

namespace WarehouseManagementSystem.ViewModels
{
    public class AddCustomerViewModel : ViewModelBaseRequestClose, IHasAddress
    {
        private readonly AddEditShipmentViewModel mainViewModel;

        private CustomerViewModel customerViewModel;
        public CustomerViewModel CustomerViewModel
        {
            get { return customerViewModel; }
            set
            {
                if (customerViewModel != value)
                {
                    this.customerViewModel = value;
                    OnPropertyChanged(nameof(CustomerViewModel));
                };
            }
        }
        public Address? Address
        {
            set
            {
                if (CustomerViewModel.Address != value)
                {
                    CustomerViewModel.Address = value;
                };
            }
        }

        public ICommand AddAddressCommand => new RelayCommand(AddAddress);
        public ICommand EditAddressCommand => new RelayCommand(EditAddress);
        public ICommand AddCommand => new RelayCommand(Add);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public AddCustomerViewModel(AddEditShipmentViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            customerViewModel = new CustomerViewModel();
        }

        private void AddAddress(object obj)
        {
            SupportWindow supportWindow = new SupportWindow(new AddEditAddressViewModel(this));
            supportWindow.ShowDialog();
        }

        private void EditAddress(object obj)
        {
            if (CustomerViewModel.Address != null)
            {
                SupportWindow supportWindow = new SupportWindow(new AddEditAddressViewModel(this, CustomerViewModel.Address));
                supportWindow.ShowDialog();
            }
        }

        private void Add(object obj)
        {
            if (ConfirmationHelper.GetConfirmation() == MessageBoxResult.OK)
            {
                if (IsValidCustomer())
                {
                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            Customer newCustomer = CreateCustomer();
                            mainViewModel.UpdateCustomers();
                            mainViewModel.CurrentShipmentViewModel.Customer = mainViewModel
                                                                                .Customers
                                                                                .FirstOrDefault(c => c.Id == newCustomer.Id);

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
                else
                {
                    MessageHelper.ShowCautionMessage("Name and address is required");
                }
            }
        }

        private bool IsValidCustomer()
        {
            return !string.IsNullOrWhiteSpace(CustomerViewModel.Firstname)
                && !string.IsNullOrWhiteSpace(CustomerViewModel.Lastname)
                && CustomerViewModel.Address != null;
        }

        private Customer CreateCustomer()
        {
            if (CustomerViewModel.Firstname == null 
                || CustomerViewModel.Lastname == null 
                || CustomerViewModel.Address == null)
            {
                throw new NullReferenceException("Required parametr is null");
            }

            var newCB = new CustomerBuilder(CustomerViewModel.Firstname,
                                            CustomerViewModel.Lastname,
                                            CustomerViewModel.Address.Id);

            if (CustomerViewModel.DateOfBirth != null)
            {
                newCB = newCB.WithDateOfBirth((DateTime)CustomerViewModel.DateOfBirth);
            }

            if (!string.IsNullOrWhiteSpace(CustomerViewModel.DiscountPercentage))
            {
                newCB = newCB.WithDiscountPercentage(Convert.ToDecimal(CustomerViewModel.DiscountPercentage));
            }

            if (!string.IsNullOrWhiteSpace(CustomerViewModel.AdditionalInfo))
            {
                newCB = newCB.WithAdditionalInfo(CustomerViewModel.AdditionalInfo);
            }

            return newCB.Build();
        }

        private void Cancel(object obj)
        {
            if (ConfirmationHelper.GetCancelConfirmation() == MessageBoxResult.OK)
            {
                try
                {
                    if (CustomerViewModel.Address != null)
                    {
                        using (EntityManager db = new EntityManager(new Models.WarehouseDbContext()))
                        {
                            db.DeleteAddress(CustomerViewModel.Address);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHelper.HandleException(ex);
                }
                finally
                {
                    CloseParentWindow();
                }
            }
        }
    }
}
