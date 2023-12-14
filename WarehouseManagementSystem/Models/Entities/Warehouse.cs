using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int AddressId { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address? Address { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Zone> Zones { get; set; }

        public Warehouse(string name, int addressId, string additionalInfo = "")
        {
            Name = name;
            AddressId = addressId;
            AdditionalInfo = additionalInfo;

            Zones = new List<Zone>();
        }
    }
}
