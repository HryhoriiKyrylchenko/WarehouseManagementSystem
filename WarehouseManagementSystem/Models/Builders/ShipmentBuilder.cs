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
    public class ShipmentBuilder : IBuilder<Shipment>
    {
        private Shipment shipment;

        public ShipmentBuilder(DateTime shipmentDate, int customerId, int userId, string batchNumber)
        {
            shipment = new Shipment(shipmentDate, customerId, userId, batchNumber);

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    shipment = warehousManager.AddShipment(shipment);
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
        public ShipmentBuilder(Shipment shipment)
        {
            this.shipment = shipment;
        }

        public ShipmentBuilder WithShipmentNumber(string shipmentNumber)
        {
            shipment.ShipmentNumber = shipmentNumber;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    shipment = warehousManager.UpdateShipment(shipment);
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

        public ShipmentBuilder WithAdditionalInfo(string additionalInfo)
        {
            shipment.AdditionalInfo = additionalInfo;

            using (var warehousManager = new WarehouseManager(new WarehouseDbContext()))
            {
                try
                {
                    shipment = warehousManager.UpdateShipment(shipment);
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

        public Shipment Build()
        {
            return shipment;
        }
    }
}
