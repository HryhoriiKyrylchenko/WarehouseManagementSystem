using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class ShipmentItem
    {
        [Key]
        public int Id { get; set; }

        public int ShipmentId { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual Shipment? Shipment { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        public int Quantity { get; set; }

        public ShipmentItem(int shipmentId, int productId, int quantity)
        {
            ShipmentId = shipmentId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
