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
    public class ShipmentItemBuilder : IBuilder<ShipmentItem>
    {
        private ShipmentItem shipmentItem;

        public ShipmentItemBuilder(int shipmentId, int productId, int quantity)
        {
            try
            {
                this.shipmentItem = InitializeAsync(new ShipmentItem(shipmentId, productId, quantity)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public ShipmentItemBuilder(ShipmentItem shipmentItem)
        {
            try
            {
                this.shipmentItem = InitializeAsync(shipmentItem).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        private ShipmentItem Initialize(ShipmentItem shipmentItem)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddShipmentItem(shipmentItem);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return shipmentItem;
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

        private async Task<ShipmentItem> InitializeAsync(ShipmentItem shipmentItem)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddShipmentItemAsync(shipmentItem);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return shipmentItem;
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

        public ShipmentItem Build()
        {
            return shipmentItem;
        }
    }
}
