using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class ZoneCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public int? PreviousCategoryId { get; set; }

        [ForeignKey("PreviousCategoryId")]
        public virtual ProductCategory? PreviousCategory { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Zone> Zones { get; set; }

        public ZoneCategory(string categoryName)
        {
            CategoryName = categoryName;

            Zones = new List<Zone>();
        }
    }
}
