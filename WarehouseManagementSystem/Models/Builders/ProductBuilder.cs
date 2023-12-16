using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Models.Entities.Enums;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    public class ProductBuilder : IBuilder<Product>
    {
        private Product product;

        public ProductBuilder(string productCode, string name, UnitsOfMeasureEnum unitOfMeasure, decimal quantity, int capacity, decimal price)
        {
            try
            {
                this.product = Initialize(new Product(productCode, name, unitOfMeasure, quantity, capacity, price));
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
                this.product = Initialize(product);
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

        public ProductBuilder WithSubcategory(int subcategoryId)
        {
            product.SubcategoryId = subcategoryId;

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

        public ProductBuilder WithDetails(string key, string value)
        {
            ProductDetail newProductDetail = new ProductDetailBuilder(product.Id, key, value).Build();

            return this;
        }

        public Product Build()
        {
            return product;
        }
    }
}
