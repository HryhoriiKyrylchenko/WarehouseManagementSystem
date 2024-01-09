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
    public class ZonePositionBuilder : IBuilder<ZonePosition>
    {
        private ZonePosition zonePosition;

        public ZonePositionBuilder(string name, int zoneId, int capacity)
        {
            try
            {
                this.zonePosition = Initialize(new ZonePosition(name, zoneId, capacity));
            }
            catch
            {
                throw;
            }
        }

        public ZonePositionBuilder(ZonePosition zonePosition)
        {
            try
            {
                this.zonePosition = Initialize(zonePosition);
            }
            catch
            {
                throw;
            }
        }

        private ZonePosition Initialize(ZonePosition zonePosition)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddZonePosition(zonePosition);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return zonePosition;
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

        private async Task<ZonePosition> InitializeAsync(ZonePosition zonePosition)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddZonePositionAsync(zonePosition);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return zonePosition;
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

        public ZonePositionBuilder WithAdditionalInfo(string additionalInfo)
        {
            zonePosition.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    zonePosition = entityManager.UpdateZonePosition(zonePosition);
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

        public async Task<ZonePositionBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            zonePosition.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    zonePosition = await entityManager.UpdateZonePositionAsync(zonePosition);
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

        public ZonePosition Build()
        {
            return zonePosition;
        }
    }
}
