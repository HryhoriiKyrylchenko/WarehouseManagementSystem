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
    public class WarehouseBuilder : IBuilder<Warehouse>
    {
        private Warehouse warehouse;

        public WarehouseBuilder(string name, int addressId)
        {
            try
            {
                this.warehouse = Initialize(new Warehouse(name, addressId));
            }
            catch
            {
                throw;
            }
        }

        public WarehouseBuilder(Warehouse warehouse)
        {
            try
            {
                this.warehouse = Initialize(warehouse);
            }
            catch
            {
                throw;
            }
        }

        private Warehouse Initialize(Warehouse warehouse)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddWarehouse(warehouse);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return warehouse;
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

        private async Task<Warehouse> InitializeAsync(Warehouse warehouse)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddWarehouseAsync(warehouse);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return warehouse;
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

        public WarehouseBuilder WithAdditionalInfo(string additionalInfo)
        {
            warehouse.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    warehouse = entityManager.UpdateWarehouse(warehouse);
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

        public async Task<WarehouseBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            warehouse.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    warehouse = await entityManager.UpdateWarehouseAsync(warehouse);
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

        public Warehouse Build()
        {
            return warehouse;
        }
    }
}
