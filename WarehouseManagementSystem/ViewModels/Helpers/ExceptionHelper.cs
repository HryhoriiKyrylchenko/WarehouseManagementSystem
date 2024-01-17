using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.ViewModels.Helpers
{
    public static class ExceptionHelper
    {
        public static void HandleException(Exception ex)
        {
            using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
            {
                logger.LogError(ex);
            }
        }

        public static async Task HandleExceptionAsync(Exception ex)
        {
            using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
            {
                await logger.LogErrorAsync(ex);
            }
        }
    }
}
