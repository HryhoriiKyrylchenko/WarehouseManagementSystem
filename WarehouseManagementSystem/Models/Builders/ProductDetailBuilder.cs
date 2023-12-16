using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    public class ProductDetailBuilder : IBuilder<ProductDetail>
    {
        private ProductDetail productDetail;

        public ProductDetailBuilder(int productId, string key, string value) 
        {
            try
            {
                this.productDetail = Initialize(new ProductDetail(productId, key, value));
            }
            catch
            {
                throw;
            }
        }

        public ProductDetailBuilder(ProductDetail productDetail) 
        {
            try
            {
                this.productDetail = Initialize(productDetail);
            }
            catch
            {
                throw;
            }
        }

        private ProductDetail Initialize(ProductDetail productDetail)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddProductDetail(productDetail);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return productDetail;
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

        public ProductDetail Build()
        {
            return productDetail;
        }
    }
}
