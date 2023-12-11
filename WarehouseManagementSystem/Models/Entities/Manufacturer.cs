using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public string? AdditionalInfo {  get; set; } 
        public Manufacturer(string name) 
        {
            Name = name;
        }
    }
}
