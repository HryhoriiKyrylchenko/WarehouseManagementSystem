using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    public class SupplierBuilder : IBuilder<Supplier>
    {
        private Supplier supplier;

        public SupplierBuilder(string name, int addressId)
        {
            try
            {
                this.supplier = Initialize(new Supplier(name, addressId));
            }
            catch
            {
                throw;
            }
        }

        public SupplierBuilder(Supplier supplier)
        {
            try
            {
                this.supplier = Initialize(supplier);
            }
            catch
            {
                throw;
            }
        }

        private Supplier Initialize(Supplier supplier)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddSupplier(supplier);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return supplier;
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

        private async Task<Supplier> InitializeAsync(Supplier supplier)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddSupplierAsync(supplier);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return supplier;
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

        public SupplierBuilder WithAdditionalInfo(string additionalInfo)
        {
            supplier.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    supplier = entityManager.UpdateSupplier(supplier);
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

        public async Task<SupplierBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            supplier.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    supplier = await entityManager.UpdateSupplierAsync(supplier);
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

        public Supplier Build()
        {
            return supplier;
        }
    }
}
