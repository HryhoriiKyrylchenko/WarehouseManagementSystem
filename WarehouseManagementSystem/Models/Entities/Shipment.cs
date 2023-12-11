using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Shipment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ShipmentDate { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public string BatchNumber { get; set; }

        public string? ShipmentNumber { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<ShipmentItem> ShipmentItems { get; set; }

        public Shipment(DateTime shipmentDate, int customerId, int userId, string batchNumber)
        {
            ShipmentDate = shipmentDate;
            CustomerId = customerId;
            UserId = userId;
            BatchNumber = batchNumber;

            ShipmentItems = new List<ShipmentItem>();
        }
    }
}
