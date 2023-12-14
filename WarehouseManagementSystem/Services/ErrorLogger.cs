using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.Services
{
    public class ErrorLogger : IDisposable
    {
        private readonly WarehouseDbContext dbContext;

        public ErrorLogger(WarehouseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void LogError(Exception ex)
        {
            try
            {
                var errorLog = new ErrorLog(ex.Message, ex.StackTrace ?? "", DateTime.Now);

                dbContext.ErrorLogs.Add(errorLog);
                dbContext.SaveChanges();
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
