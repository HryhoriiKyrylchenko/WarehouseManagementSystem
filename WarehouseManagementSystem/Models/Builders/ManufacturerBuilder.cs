using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    internal class ManufacturerBuilder : IBuilder<Manufacturer>
    {
        private Manufacturer manufacturer;

        public ManufacturerBuilder(string name)
        {
            manufacturer = new Manufacturer(name);

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    manufacturer = warehousManager.AddManufacturer(manufacturer);
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

        public ManufacturerBuilder(Manufacturer manufacturer)
        {  
            this.manufacturer = manufacturer; 
        }

        public ManufacturerBuilder WithDescription(string description)
        {
            manufacturer.Description = description;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    manufacturer = warehousManager.UpdateManufacturer(manufacturer);
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

        public ManufacturerBuilder WithAddress(int addressId)
        {
            manufacturer.AddressId = addressId;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    manufacturer = warehousManager.UpdateManufacturer(manufacturer);
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

        public ManufacturerBuilder WithAdditionalInfo(string additionalInfo)
        {
            manufacturer.AdditionalInfo = additionalInfo;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    manufacturer = warehousManager.UpdateManufacturer(manufacturer);
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

        public Manufacturer Build()
        {
            return manufacturer;
        }
    }
}
