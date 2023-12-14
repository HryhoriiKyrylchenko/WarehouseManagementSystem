using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models.Builders
{
    public class ReceiptBuilder : IBuilder<Receipt>
    {
        private Receipt receipt;

        public ReceiptBuilder(DateTime receiptDate, int supplierId, int userId, string batchNumber)
        {
            receipt = new Receipt(receiptDate, supplierId, userId, batchNumber);

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    receipt = warehousManager.AddReceipt(receipt);
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
        }

        public ReceiptBuilder(Receipt receipt) 
        {  
            this.receipt = receipt; 
        }

        public ReceiptBuilder WithShipmentNumber(string shipmentNumber)
        {
            receipt.ShipmentNumber = shipmentNumber;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    receipt = warehousManager.UpdateReceipt(receipt);
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

        public ReceiptBuilder WithAdditionalInfo(string additionalInfo)
        {
            receipt.AdditionalInfo = additionalInfo;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    receipt = warehousManager.UpdateReceipt(receipt);
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

        public Receipt Build()
        {
            return receipt;
        }
    }
}
