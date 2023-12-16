﻿using System;
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
                this.address = Initialize(new Address(country, index, city, street, buildingNumber));
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
                this.address = Initialize(address);
            }
            catch
            {
                throw;
            }
        }

        private Address Initialize(Address address)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddAddress(address);
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
                        errorLogger.LogError(ex);
                    }
                    throw;
                }
            }
        }

        public AddressBuilder WithRoom(string room)
        {
            address.Room = room;

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

        public Address Build()
        {
            return address;
        }
    }
}
