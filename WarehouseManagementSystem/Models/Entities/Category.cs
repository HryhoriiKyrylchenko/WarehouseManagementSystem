using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public int? PreviousCategoryId { get; set; }

        [ForeignKey("PreviousCategoryId")]
        public virtual Category? PreviousCategory { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Category(string categoryName)
        {
            CategoryName = categoryName;

            Products = new List<Product>();
        }
    }
}
