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
    public class CategoryBuilder : IBuilder<Category>
    {
        private Category category;

        public CategoryBuilder(string categoryName)
        {
            try
            {
                this.category = InitializeAsync(new Category(categoryName)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public CategoryBuilder(Category category) 
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

        private Category Initialize(Category category)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddCategory(category);
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

        private async Task<Category> InitializeAsync(Category category)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddCategoryAsync(category);
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

        public CategoryBuilder WithPreviousCategory(int previousCategoryId)
        {
            category.PreviousCategoryId = previousCategoryId;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = entityManager.UpdateCategory(category);
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

        public async Task<CategoryBuilder> WithPreviousCategoryAsync(int previousCategoryId)
        {
            category.PreviousCategoryId = previousCategoryId;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = await entityManager.UpdateCategoryAsync(category);
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

        public CategoryBuilder WithAdditionalInfo(string additionalInfo)
        {
            category.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = entityManager.UpdateCategory(category);
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

        public async Task<CategoryBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            category.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = await entityManager.UpdateCategoryAsync(category);
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

        public Category Build()
        {
            return category;
        }
    }
}
