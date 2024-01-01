using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.ViewModels.Support_data
{
    public record UnallocatedProductInstancesModel
    {
        public decimal UnallocatedBalance { get; set; }
        public decimal UnallocatedCapacity { get; set; }
    }
}
