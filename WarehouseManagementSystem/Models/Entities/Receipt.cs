using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Receipt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ReceiptDate { get; set; }

        public int SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public string BatchNumber { get; set; }

        public string? ShipmentNumber { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<ReceiptItem> ReceiptItems { get; set; }

        public Receipt(DateTime receiptDate, int supplierId, int userId, string batchNumber)
        {
            ReceiptDate = receiptDate;
            SupplierId = supplierId;
            UserId = userId;
            BatchNumber = batchNumber;

            ReceiptItems = new List<ReceiptItem>();
        }
    }
}
