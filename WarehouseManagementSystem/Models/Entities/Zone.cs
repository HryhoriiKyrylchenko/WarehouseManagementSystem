using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int ZoneCategoryId {  get; set; }

        [ForeignKey("ZoneCategoryId")]
        public virtual ZoneCategory? ZoneCategory { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<ZonePosition> ZonePositions { get; set; }

        public Zone(string name, int warehouseId, int zoneCategoryId, int capacity)
        {
            Name = name;
            WarehouseId = warehouseId;
            ZoneCategoryId = zoneCategoryId;
            Capacity = capacity;

            ZonePositions = new List<ZonePosition>();
        }
    }
}
