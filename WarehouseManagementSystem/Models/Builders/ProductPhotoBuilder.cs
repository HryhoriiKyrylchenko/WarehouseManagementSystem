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
    public class ProductPhotoBuilder : IBuilder<ProductPhoto>
    {
        private ProductPhoto productPhoto;

        public ProductPhotoBuilder(byte[] photoData, int productId)
        {
            try
            {
                this.productPhoto = InitializeAsync(new ProductPhoto(photoData, productId)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public ProductPhotoBuilder(ProductPhoto productPhoto)
        {
            try
            {
                this.productPhoto = InitializeAsync(productPhoto).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        private ProductPhoto Initialize(ProductPhoto productPhoto)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddProductPhoto(productPhoto);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return productPhoto;
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

        private async Task<ProductPhoto> InitializeAsync(ProductPhoto productPhoto)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddProductPhotoAsync(productPhoto);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return productPhoto;
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

        public ProductPhoto Build()
        {
            return productPhoto;
        }
    }
}
