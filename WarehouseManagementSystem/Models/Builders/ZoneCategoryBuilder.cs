using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    public class ZoneCategoryBuilder : IBuilder<ZoneCategory>
    {
        private ZoneCategory category;

        public ZoneCategoryBuilder(string categoryName)
        {
            try
            {
                this.category = InitializeAsync(new ZoneCategory(categoryName)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public ZoneCategoryBuilder(ZoneCategory category)
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

        private ZoneCategory Initialize(ZoneCategory category)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddZoneCategory(category);
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

        private async Task<ZoneCategory> InitializeAsync(ZoneCategory category)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddZoneCategoryAsync(category);
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

        public ZoneCategoryBuilder WithPreviousCategory(int previousCategoryId)
        {
            category.PreviousCategoryId = previousCategoryId;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = entityManager.UpdateZoneCategory(category);
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

        public async Task<ZoneCategoryBuilder> WithPreviousCategoryAsync(int previousCategoryId)
        {
            category.PreviousCategoryId = previousCategoryId;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = await entityManager.UpdateZoneCategoryAsync(category);
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

        public ZoneCategoryBuilder WithAdditionalInfo(string additionalInfo)
        {
            category.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = entityManager.UpdateZoneCategory(category);
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

        public async Task<ZoneCategoryBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            category.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    category = await entityManager.UpdateZoneCategoryAsync(category);
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

        public ZoneCategory Build()
        {
            return category;
        }
    }
}
