using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities.Enums;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Enum CategoryName { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Subcategory> Subcategories { get; set; }

        public Category(Enum categoryName)
        {
            CategoryName = categoryName;

            Subcategories = new List<Subcategory>();
        }
    }
}
