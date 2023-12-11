using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities.Enums;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Zone
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual Warehouse? Warehouse { get; set; }

        public int Capacity { get; set; }

        public int FreeSpace => ZonePositions.Sum(zp => zp.FreeSpace) + (Capacity - ZonePositions.Sum(zp => zp.Capacity));

        public int CategoryId {  get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<ZonePosition> ZonePositions { get; set; }

        public Zone(string name, int warehouseId, int categoryId)
        {
            Name = name;
            WarehouseId = warehouseId;
            CategoryId = categoryId;

            ZonePositions = new List<ZonePosition>();
        }
    }
}
