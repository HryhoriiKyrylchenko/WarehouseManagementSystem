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
    public class ProductInZonePositionBuilder : IBuilder<ProductInZonePosition>
    {
        private ProductInZonePosition productInZonePosition;

        public ProductInZonePositionBuilder(int productId, int quantity, int zonePositionId)
        {
            productInZonePosition = new ProductInZonePosition(productId, quantity, zonePositionId);

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    productInZonePosition = warehousManager.AddProductInZonePosition(productInZonePosition);
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

        public ProductInZonePositionBuilder(ProductInZonePosition productInZonePosition)
        {
            this.productInZonePosition = productInZonePosition;
        }

        public ProductInZonePositionBuilder WithManufactureDate(DateTime manufactureDate)
        {
            productInZonePosition.ManufactureDate = manufactureDate;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    productInZonePosition = warehousManager.UpdateProductInZonePosition(productInZonePosition);
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

        public ProductInZonePositionBuilder WithExpiryDate(DateTime expiryDate)
        {
            productInZonePosition.ExpiryDate = expiryDate;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    productInZonePosition = warehousManager.UpdateProductInZonePosition(productInZonePosition);
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

        public ProductInZonePosition Build()
        {
            return productInZonePosition;
        }
    }
}
