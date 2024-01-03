using System;
using System.Collections.Generic;
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
    public class ProductCategoryBuilder : IBuilder<ProductCategory>
    {
        private ProductCategory category;

        public ProductCategoryBuilder(string categoryName)
        {
            try
            {
                this.category = InitializeAsync(new ProductCategory(categoryName)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public ProductCategoryBuilder(ProductCategory category) 
        {
            try
            {
                this.category = InitializeAsync(category).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        private ProductCategory Initialize(ProductCategory category)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddProductCategory(category);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return category;
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

        private async Task<ProductCategory> InitializeAsync(ProductCategory category)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddProductCategoryAsync(category);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return category;
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

        public ProductCategoryBuilder WithPreviousCategory(int previousCategoryId)
        {
            category.PreviousCategoryId = previousCategoryId;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = entityManager.UpdateProductCategory(category);
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

        public async Task<ProductCategoryBuilder> WithPreviousCategoryAsync(int previousCategoryId)
        {
            category.PreviousCategoryId = previousCategoryId;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = await entityManager.UpdateProductCategoryAsync(category);
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

        public ProductCategoryBuilder WithAdditionalInfo(string additionalInfo)
        {
            category.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = entityManager.UpdateProductCategory(category);
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

        public async Task<ProductCategoryBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            category.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = await entityManager.UpdateProductCategoryAsync(category);
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

        public ProductCategory Build()
        {
            return category;
        }
    }
}
