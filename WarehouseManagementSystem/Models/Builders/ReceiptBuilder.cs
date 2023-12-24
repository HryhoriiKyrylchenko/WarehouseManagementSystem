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
    public class ReceiptBuilder : IBuilder<Receipt>
    {
        private Receipt receipt;

        public ReceiptBuilder(DateTime receiptDate, int supplierId, int userId, string batchNumber)
        {
            try
            {
                this.receipt = InitializeAsync(new Receipt(receiptDate, supplierId, userId, batchNumber)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public ReceiptBuilder(Receipt receipt) 
        {
            try
            {
                this.receipt = InitializeAsync(receipt).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        private Receipt Initialize(Receipt receipt)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddReceipt(receipt);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return receipt;
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

        private async Task<Receipt> InitializeAsync(Receipt receipt)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddReceiptAsync(receipt);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return receipt;
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

        public ReceiptBuilder WithShipmentNumber(string shipmentNumber)
        {
            receipt.ShipmentNumber = shipmentNumber;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    receipt = entityManager.UpdateReceipt(receipt);
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

        public async Task<ReceiptBuilder> WithShipmentNumberAsync(string shipmentNumber)
        {
            receipt.ShipmentNumber = shipmentNumber;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    receipt = await entityManager.UpdateReceiptAsync(receipt);
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

        public ReceiptBuilder WithAdditionalInfo(string additionalInfo)
        {
            receipt.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    receipt = entityManager.UpdateReceipt(receipt);
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

        public async Task<ReceiptBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            receipt.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    receipt = await entityManager.UpdateReceiptAsync(receipt);
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

        public Receipt Build()
        {
            return receipt;
        }
    }
}
