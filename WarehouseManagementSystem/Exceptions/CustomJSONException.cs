using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Exceptions
{
    public class CustomJsonException : JsonException
    {
        public CustomJsonException(JsonException parentException, string message) :  base("Error in string:" + message, parentException)
        { 
        }
    }
}
