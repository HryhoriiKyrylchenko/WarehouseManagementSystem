using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    public class ProductBuilder : IBuilder<Product>
    {
        private Product product;

        public ProductBuilder(string productCode, string name, UnitsOfMeasureEnum unitOfMeasure, decimal quantity, int capacity, decimal price, int warehouseId)
        {
            try
            {
                this.product = InitializeAsync(new Product(productCode, name, unitOfMeasure, quantity, capacity, price, warehouseId)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public ProductBuilder(Product product)
        {
            try
            {
                this.product = InitializeAsync(product).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        private Product Initialize(Product product)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddProduct(product);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return product;
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

        private async Task<Product> InitializeAsync(Product product)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddProductAsync(product);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return product;
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

        public ProductBuilder WithDescription(string description)
        {
            product.Description = description;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product = entityManager.UpdateProduct(product);
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

        public async Task<ProductBuilder> WithDescriptionAsync(string description)
        {
            product.Description = description;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product = await entityManager.UpdateProductAsync(product);
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

        public ProductBuilder WithManufacturer(Manufacturer manufacturer)
        {
            product.Manufacturer = manufacturer;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product = entityManager.UpdateProduct(product);
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

        public async Task<ProductBuilder> WithManufacturerAsync(Manufacturer manufacturer)
        {
            product.Manufacturer = manufacturer;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product = await entityManager.UpdateProductAsync(product);
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

        public ProductBuilder WithDiscountPercentage(decimal? discountPercentage)
        {
            product.DiscountPercentage = discountPercentage;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product = entityManager.UpdateProduct(product);
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

        public async Task<ProductBuilder> WithDiscountPercentageAsync(decimal? discountPercentage)
        {
            product.DiscountPercentage = discountPercentage;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product = await entityManager.UpdateProductAsync(product);
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

        public ProductBuilder WithCategory(int categoryId)
        {
            product.CategoryId = categoryId;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product = entityManager.UpdateProduct(product);
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

        public async Task<ProductBuilder> WithCategoryAsync(int categoryId)
        {
            product.CategoryId = categoryId;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product = await entityManager.UpdateProductAsync(product);
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

        public ProductBuilder WithProductDetail(string key, string value)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product.AddProductDetail(key, value);
                    product = entityManager.UpdateProduct(product);
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

        public async Task<ProductBuilder> WithProductDetailAsync(string key, string value)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product.AddProductDetail(key, value);
                    product = await entityManager.UpdateProductAsync(product);
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

        public async Task<ProductBuilder> WithProductDetailsAsync(string productDetails)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product.ProductDetails = productDetails;
                    product = await entityManager.UpdateProductAsync(product);
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

        public ProductBuilder WithAdditionalInfo(string additionalInfo)
        {
            product.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product = entityManager.UpdateProduct(product);
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

        public async Task<ProductBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            product.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    product = await entityManager.UpdateProductAsync(product);
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

        public Product Build()
        {
            return product;
        }
    }
}
