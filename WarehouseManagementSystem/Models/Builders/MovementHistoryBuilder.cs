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
    public class MovementHistoryBuilder : IBuilder<MovementHistory>
    {
        private MovementHistory movementHistory;

        public MovementHistoryBuilder(DateTime movementDate, int productId, int sourceZonePositionId, int destinationZonePositionId)
        {
            try
            {
                this.movementHistory = InitializeAsync(new MovementHistory(movementDate, productId, sourceZonePositionId, destinationZonePositionId)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public MovementHistoryBuilder(MovementHistory movementHistory)
        {
            try
            {
                this.movementHistory = InitializeAsync(movementHistory).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        private MovementHistory Initialize(MovementHistory movementHistory)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddMovementHistory(movementHistory);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return movementHistory;
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

        private async Task<MovementHistory> InitializeAsync(MovementHistory movementHistory)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddMovementHistoryAsync(movementHistory);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return movementHistory;
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

        public MovementHistoryBuilder WithAdditionalInfo(string additionalInfo)
        {
            movementHistory.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    movementHistory = entityManager.UpdateMovementHistory(movementHistory);
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

        public async Task<MovementHistoryBuilder> WithAdditionalInfoAsync(string additionalInfo)
        {
            movementHistory.AdditionalInfo = additionalInfo;

            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    movementHistory = await entityManager.UpdateMovementHistoryAsync(movementHistory);
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

        public MovementHistory Build()
        {
            return movementHistory;
        }
    }
}
