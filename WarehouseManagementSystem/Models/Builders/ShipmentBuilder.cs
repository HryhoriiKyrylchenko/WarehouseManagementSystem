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
    public class ShipmentBuilder : IBuilder<Shipment>
    {
        private Shipment shipment;

        public ShipmentBuilder(DateTime shipmentDate, int customerId, int userId, string batchNumber)
        {
            try
            {
                this.shipment = InitializeAsync(new Shipment(shipmentDate, customerId, userId, batchNumber)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }
        public ShipmentBuilder(Shipment shipment)
        {
            try
            {
                this.shipment = InitializeAsync(shipment).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        private Shipment Initialize(Shipment shipment)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddShipment(shipment);
                    return shipment;
                }
                catch (DuplicateObjectException)
                {
                    return shipment;
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

        private async Task<Shipment> InitializeAsync(Shipment shipment)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddShipmentAsync(shipment);
                    return shipment;
                }
                catch (DuplicateObjectException)
                {
                    return shipment;
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

        public ShipmentBuilder WithShipmentNumber(string shipmentNumber)
        {
            shipment.ShipmentNumber = shipmentNumber;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    shipment = entityManager.UpdateShipment(shipment);
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

        public async Task<ShipmentBuilder> WithShipmentNumberAsync(string shipmentNumber)
        {
            shipment.ShipmentNumber = shipmentNumber;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    shipment = await entityManager.UpdateShipmentAsync(shipment);
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

        public ShipmentBuilder WithAdditionalInfo(string additionalInfo)
        {
            shipment.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    shipment = entityManager.UpdateShipment(shipment);
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

        public async Task<ShipmentBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            shipment.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    shipment = await entityManager.UpdateShipmentAsync(shipment);
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

        public Shipment Build()
        {
            return shipment;
        }
    }
}
