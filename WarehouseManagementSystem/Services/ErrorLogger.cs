using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Builders;
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
                ErrorLog errorLog = new ErrorLogBuilder(ex.Message, ex.StackTrace ?? "", DateTime.Now).Build();

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
