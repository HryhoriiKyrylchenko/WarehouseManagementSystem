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
    public class ErrorLogBuilder : IBuilder<ErrorLog>
    {
        private ErrorLog errorLog;

        public ErrorLogBuilder(string errorMessage, string stackTrace, DateTime timestamp)
        {
            try
            {
                this.errorLog = InitializeAsync(new ErrorLog(errorMessage, stackTrace, timestamp)).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        public ErrorLogBuilder(ErrorLog errorLog)
        {
            try
            {
                this.errorLog = InitializeAsync(errorLog).GetAwaiter().GetResult();
            }
            catch
            {
                throw;
            }
        }

        private ErrorLog Initialize(ErrorLog errorLog)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = entityManager.AddErrorLog(errorLog);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return errorLog;
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

        private async Task<ErrorLog> InitializeAsync(ErrorLog errorLog)
        {
            using (var entityManager = new EntityManager(new WarehouseDbContext()))
            {
                try
                {
                    var initializer = await entityManager.AddErrorLogAsync(errorLog);
                    return initializer;
                }
                catch (DuplicateObjectException)
                {
                    return errorLog;
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

        public ErrorLog Build()
        {
            return errorLog;
        }
    }
}
