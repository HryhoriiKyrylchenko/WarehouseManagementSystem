using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class MovementHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime MovementDate { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        public int SourceZonePositionId { get; set; }

        [ForeignKey("SourceZonePositionId")]
        [InverseProperty("SourceMovementHistories")]
        public virtual ZonePosition? SourceZonePosition { get; set; }

        public int DestinationZonePositionId { get; set; }

        [ForeignKey("DestinationZonePositionId")]
        [InverseProperty("DestinationMovementHistories")]
        public virtual ZonePosition? DestinationZonePosition { get; set; }

        public string? AdditionalInfo { get; set; }

        public MovementHistory(DateTime movementDate, int productId, int sourceZonePositionId, int destinationZonePositionId)
        {
            MovementDate = movementDate;
            ProductId = productId;
            SourceZonePositionId = sourceZonePositionId;
            DestinationZonePositionId = destinationZonePositionId;
        }
    }
}
