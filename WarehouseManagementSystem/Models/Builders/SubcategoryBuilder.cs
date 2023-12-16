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
    public class SubcategoryBuilder : IBuilder<Subcategory>
    {
        private Subcategory subcategory;

        public SubcategoryBuilder(string name, int categoryId)
        {
            try
            {
                this.subcategory = Initialize(new Subcategory(name, categoryId));
            }
            catch
            {
                throw;
            }
        }

        public SubcategoryBuilder(Subcategory subcategory) 
        {
            try
            {
                this.subcategory = Initialize(subcategory);
            }
            catch
            {
                throw;
            }
        }

        private Subcategory Initialize(Subcategory subcategory)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddSubcategory(subcategory);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return subcategory;
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

        public SubcategoryBuilder WithAdditionalInfo(string additionalInfo)
        {
            subcategory.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    subcategory = entityManager.UpdateSubcategory(subcategory);
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

        public Subcategory Build()
        {
            return subcategory;
        }
    }
}
