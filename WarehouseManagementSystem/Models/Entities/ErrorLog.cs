using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class ErrorLog
    {
        [Key]
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public DateTime Timestamp { get; set; }

        public ErrorLog(string errorMessage, string stackTrace, DateTime timestamp) 
        {
            ErrorMessage = errorMessage;
            StackTrace = stackTrace;
            Timestamp = timestamp;
        }
    }
}
