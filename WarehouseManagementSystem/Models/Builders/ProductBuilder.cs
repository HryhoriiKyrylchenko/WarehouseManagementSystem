using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            product = new Product(productCode, name, unitOfMeasure, quantity, capacity, price);

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    product = warehousManager.AddProduct(product);
                }
                catch (Exception ex)
                {
                    using(var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                    {
                        errorLogger.LogError(ex);
                    }

                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); ///
                }
            }
        }

        public ProductBuilder(Product product)
        {
            this.product = product;
        }

        public ProductBuilder WithDescription(string description)
        {
            product.Description = description;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    product = warehousManager.UpdateProduct(product);
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

            return this;
        }

        public ProductBuilder WithManufacturer(Manufacturer manufacturer)
        {
            product.Manufacturer = manufacturer;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    product = warehousManager.UpdateProduct(product);
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

        public ProductBuilder WithDiscountPercentage(decimal? discountPercentage)
        {
            product.DiscountPercentage = discountPercentage;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    product = warehousManager.UpdateProduct(product);
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

        public ProductBuilder WithSubcategory(int subcategoryId)
        {
            product.SubcategoryId = subcategoryId;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    product = warehousManager.UpdateProduct(product);
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

        public ProductBuilder WithDetails(string key, string value)
        {
            ProductDetail newProductDetail = new ProductDetail(product.Id, key, value);

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    warehousManager.AddProductDetail(newProductDetail);
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

        public Product Build()
        {
            return product;
        }
    }
}
