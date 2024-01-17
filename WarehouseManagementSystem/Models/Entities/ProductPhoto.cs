using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class ProductPhoto
    {
        [Key]
        public int Id { get; set; }

        public byte[] PhotoData { get; set; }

        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        public ProductPhoto(byte[] photoData)
        {
            PhotoData = photoData;
        }
        public ProductPhoto(byte[] photoData, int productId) 
        {
            PhotoData = photoData;
            ProductId = productId;
        }
    }
}
