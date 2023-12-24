using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    public class ManufacturerBuilder : IBuilder<Manufacturer>
    {
        private Manufacturer manufacturer;

        public ManufacturerBuilder(string name)
        {
            try
            {
                this.manufacturer = InitializeAsync(new Manufacturer(name)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public ManufacturerBuilder(Manufacturer manufacturer)
        {
            try
            {
                this.manufacturer = InitializeAsync(manufacturer).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        private Manufacturer Initialize(Manufacturer manufacturer)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddManufacturer(manufacturer);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return manufacturer;
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

        private async Task<Manufacturer> InitializeAsync(Manufacturer manufacturer)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddManufacturerAsync(manufacturer);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return manufacturer;
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

        public ManufacturerBuilder WithDescription(string description)
        {
            manufacturer.Description = description;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    manufacturer = entityManager.UpdateManufacturer(manufacturer);
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

        public async Task<ManufacturerBuilder> WithDescriptionAsync(string description)
        {
            manufacturer.Description = description;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    manufacturer = await entityManager.UpdateManufacturerAsync(manufacturer);
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

        public ManufacturerBuilder WithAddress(int addressId)
        {
            manufacturer.AddressId = addressId;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    manufacturer = entityManager.UpdateManufacturer(manufacturer);
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

        public async Task<ManufacturerBuilder> WithAddressAsync(int addressId)
        {
            manufacturer.AddressId = addressId;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    manufacturer = await entityManager.UpdateManufacturerAsync(manufacturer);
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

        public ManufacturerBuilder WithAdditionalInfo(string additionalInfo)
        {
            manufacturer.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    manufacturer = entityManager.UpdateManufacturer(manufacturer);
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

        public async Task<ManufacturerBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            manufacturer.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    manufacturer = await entityManager.UpdateManufacturerAsync(manufacturer);
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

        public Manufacturer Build()
        {
            return manufacturer;
        }
    }
}
