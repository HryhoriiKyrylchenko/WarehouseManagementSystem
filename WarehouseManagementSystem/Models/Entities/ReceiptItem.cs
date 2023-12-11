using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class ReceiptItem
    {
        [Key]
        public int Id { get; set; }

        public int ReceiptId { get; set; }

        [ForeignKey("ReceiptId")]
        public virtual Receipt? Receipt { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        public int Quantity { get; set; }

        public ReceiptItem(int receiptId, int productId, int quantity)
        {
            ReceiptId = receiptId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
