using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.ViewModels.Helpers;

namespace WarehouseManagementSystem.Services
{
    public static class ConnectionTester
    {
        public static bool TestConnectionString(string? connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false; 
            }
        }
    }
}
