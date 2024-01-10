using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.ViewModels.Interfaces
{
    public interface IHasAddress
    {
        public Address? Address { set; }
    }
}
