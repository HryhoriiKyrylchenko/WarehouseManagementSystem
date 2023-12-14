using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Label
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Barcode { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        public Label()
        {
            Barcode = "";
        }
        public Label(string barcode, int productId)
        {
            Barcode = barcode;
            ProductId = productId;
        }
    }
}
