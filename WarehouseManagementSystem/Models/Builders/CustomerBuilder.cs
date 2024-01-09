using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    public class CustomerBuilder : IBuilder<Customer>
    {
        private Customer customer;

        public CustomerBuilder(string firstname, string lastname, int addressId)
        {
            try
            {
                this.customer = Initialize(new Customer(firstname, lastname, addressId));
            }
            catch
            {
                throw;
            }
        }

        public CustomerBuilder(Customer customer) 
        {
            try
            {
                this.customer = Initialize(customer);
            }
            catch
            {
                throw;
            }
        }

        private Customer Initialize(Customer customer)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddCustomer(customer);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return customer;
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        errorLogger.LogError(ex);
                    }
                    throw;
                }
            }
        }

        private async Task<Customer> InitializeAsync(Customer customer)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddCustomerAsync(customer);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return customer;
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        await errorLogger.LogErrorAsync(ex);
                    }
                    throw;
                }
            }
        }

        public CustomerBuilder WithDateOfBirth(DateTime dateOfBirth)
        {
            customer.DateOfBirth = dateOfBirth;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    customer = entityManager.UpdateCustomer(customer);
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        errorLogger.LogError(ex);
                    }
                    throw;
                }
            }

            return this;
        }

        public async Task<CustomerBuilder> WithDateOfBirthAsync(DateTime dateOfBirth)
        {
            customer.DateOfBirth = dateOfBirth;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    customer = await entityManager.UpdateCustomerAsync(customer);
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        await errorLogger.LogErrorAsync(ex);
                    }
                    throw;
                }
            }

            return this;
        }

        public CustomerBuilder WithDiscountPercentage(decimal discountPercentage)
        {
            customer.DiscountPercentage = discountPercentage;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    customer = entityManager.UpdateCustomer(customer);
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        errorLogger.LogError(ex);
                    }
                    throw;
                }
            }

            return this;
        }

        public async Task<CustomerBuilder> WithDiscountPercentageAsync(decimal discountPercentage)
        {
            customer.DiscountPercentage = discountPercentage;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    customer = await entityManager.UpdateCustomerAsync(customer);
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        await errorLogger.LogErrorAsync(ex);
                    }
                    throw;
                }
            }

            return this;
        }

        public CustomerBuilder WithAdditionalInfo(string additionalInfo)
        {
            customer.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    customer = entityManager.UpdateCustomer(customer);
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        errorLogger.LogError(ex);
                    }
                    throw;
                }
            }

            return this;
        }

        public async Task<CustomerBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            customer.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    customer = await entityManager.UpdateCustomerAsync(customer);
                }
                catch (Exception ex)
                {
                    using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        await errorLogger.LogErrorAsync(ex);
                    }
                    throw;
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
