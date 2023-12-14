using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    public class CustomerBuilder : IBuilder<Customer>
    {
        private Customer customer;

        public CustomerBuilder(string firstName, string lastName, int addressId)
        {
            customer = new Customer(firstName, lastName, addressId);

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    customer = warehousManager.AddCustomer(customer);
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        errorLogger.LogError(ex);
                    }

                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); ///
                }
            }
        }

        public CustomerBuilder(Customer customer) 
        {
            this.customer = customer;
        }

        public CustomerBuilder WithDateOfBirth(DateTime dateOfBirth)
        {
            customer.DateOfBirth = dateOfBirth;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    customer = warehousManager.UpdateCustomer(customer);
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        errorLogger.LogError(ex);
                    }

                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return this;
        }

        public CustomerBuilder WithDiscountPercentage(decimal discountPercentage)
        {
            customer.DiscountPercentage = discountPercentage;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    customer = warehousManager.UpdateCustomer(customer);
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        errorLogger.LogError(ex);
                    }

                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return this;
        }

        public CustomerBuilder WithAdditionalInfo(string additionalInfo)
        {
            customer.AdditionalInfo = additionalInfo;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    customer = warehousManager.UpdateCustomer(customer);
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        errorLogger.LogError(ex);
                    }

                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return this;
        }

        public Customer Build()
        {
            return customer;
        }
    }
}
