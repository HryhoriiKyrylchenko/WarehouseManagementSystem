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
    public class ReceiptItemBuilder : IBuilder<ReceiptItem>
    {
        private ReceiptItem receiptItem;

        public ReceiptItemBuilder(int receiptId, int productId, int quantity)
        {
            try
            {
                this.receiptItem = Initialize(new ReceiptItem(receiptId, productId, quantity));
            }
            catch
            {
                throw;
            }
        }

        public ReceiptItemBuilder(ReceiptItem receiptItem)
        {
            try
            {
                this.receiptItem = Initialize(receiptItem);
            }
            catch
            {
                throw;
            }
        }

        private ReceiptItem Initialize(ReceiptItem receiptItem)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddReceiptItem(receiptItem);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return receiptItem;
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

        private async Task<ReceiptItem> InitializeAsync(ReceiptItem receiptItem)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddReceiptItemAsync(receiptItem);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return receiptItem;
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

        public ReceiptItem Build()
        {
            return receiptItem;
        }
    }
}
