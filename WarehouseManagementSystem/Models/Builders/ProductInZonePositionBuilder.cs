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
    public class ProductInZonePositionBuilder : IBuilder<ProductInZonePosition>
    {
        private ProductInZonePosition productInZonePosition;

        public ProductInZonePositionBuilder(int productId, int quantity, int zonePositionId)
        {
            try
            {
                this.productInZonePosition = Initialize(new ProductInZonePosition(productId, quantity, zonePositionId));
            }
            catch
            {
                throw;
            }
        }

        public ProductInZonePositionBuilder(ProductInZonePosition productInZonePosition)
        {
            try
            {
                this.productInZonePosition = Initialize(productInZonePosition);
            }
            catch
            {
                throw;
            }
        }

        private ProductInZonePosition Initialize(ProductInZonePosition productInZonePosition)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddProductInZonePosition(productInZonePosition);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return productInZonePosition;
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

        public ProductInZonePositionBuilder WithManufactureDate(DateTime manufactureDate)
        {
            productInZonePosition.ManufactureDate = manufactureDate;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    productInZonePosition = entityManager.UpdateProductInZonePosition(productInZonePosition);
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

        public ProductInZonePositionBuilder WithExpiryDate(DateTime expiryDate)
        {
            productInZonePosition.ExpiryDate = expiryDate;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    productInZonePosition = entityManager.UpdateProductInZonePosition(productInZonePosition);
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

        public ProductInZonePosition Build()
        {
            return productInZonePosition;
        }
    }
}
