using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Subcategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SubcategoryName { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Subcategory(string name, int categoryId)
        {
            SubcategoryName = name; 
            CategoryId = categoryId;

            Products = new List<Product>();
        }
    }
}
