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
    public class AddressBuilder : IBuilder<Address>
    {
        private Address address;

        public AddressBuilder(string country, string index, string city, string street, int buildingNumber)
        {
            try
            {
                this.address = InitializeAsync(new Address(country, index, city, street, buildingNumber)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public AddressBuilder(Address address)
        {
            try
            {
                this.address = InitializeAsync(address).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        private async Task<Address> InitializeAsync(Address address)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddAddressAsync(address);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return address;
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

        public async Task<AddressBuilder> WithRoomAsync(string room)
        {
            address.Room = room;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    address = await entityManager.UpdateAddressAsync(address);
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

        public AddressBuilder WithAdditionalInfo(string additionalInfo)
        {
            address.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    address = entityManager.UpdateAddress(address);
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

        public async Task<AddressBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            address.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    address = await entityManager.UpdateAddressAsync(address);
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

        public Address Build()
        {
            return address;
        }
    }
}
