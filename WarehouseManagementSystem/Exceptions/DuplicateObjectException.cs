using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Exceptions
{
    public class DuplicateObjectException : ApplicationException
    {
        public DuplicateObjectException(string message = "Duplicate object found in the database") : base(message)
        {
        }
    }
}
