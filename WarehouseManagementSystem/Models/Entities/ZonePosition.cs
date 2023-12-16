using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class ZonePosition
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Capacity { get; set; }

        public int FreeSpace => Capacity - ProductsInZonePosition.Sum(p => p.Quantity * ((p.Product == null) ? 0 : p.Product.Capacity));

        public int ZoneId { get; set; }

        [ForeignKey("ZoneId")]
        public virtual Zone? Zone { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<ProductInZonePosition> ProductsInZonePosition { get; set; }

        public virtual ICollection<MovementHistory> MovementHistories { get; set; }

        public ZonePosition(string name, int zoneId, int capacity)
        {
            Name = name;
            ZoneId = zoneId;
            Capacity = capacity;

            ProductsInZonePosition = new List<ProductInZonePosition>();
            MovementHistories = new List<MovementHistory>();
        }
    }
}
