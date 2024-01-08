using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public int? PreviousCategoryId { get; set; }

        [ForeignKey("PreviousCategoryId")]
        public virtual ProductCategory? PreviousCategory { get; set; }

        public string? AdditionalInfo { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }

        public ProductCategory(string categoryName)
        {
            CategoryName = categoryName;

            Products = new List<Product>();
        }
    }
}
