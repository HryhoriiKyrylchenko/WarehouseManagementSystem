using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using Zone = WarehouseManagementSystem.Models.Entities.Zone;

namespace WarehouseManagementSystem.Models.Builders
{
    public class ZoneBuilder : IBuilder<Zone>
    {
        private Zone zone;

        public ZoneBuilder(string name, int warehouseId, int categoryId, int capacity)
        {
            try
            {
                this.zone = Initialize(new Zone(name, warehouseId, categoryId, capacity));
            }
            catch
            {
                throw;
            }
        }

        public ZoneBuilder(Zone zone)
        {
            try
            {
                this.zone = Initialize(zone);
            }
            catch
            {
                throw;
            }
        }

        private Zone Initialize(Zone zone)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddZone(zone);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return zone;
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

        public ZoneBuilder WithAdditionalInfo(string additionalInfo)
        {
            zone.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    zone = entityManager.UpdateZone(zone);
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

        public Zone Build()
        {
            return zone;
        }
    }
}
