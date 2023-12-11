using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities.Enums;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProductCode { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public UnitsOfMeasureEnum UnitOfMeasure { get; set; }

        public decimal Quantity { get; set; }

        public int Capacity { get; set; }

        public Manufacturer? Manufacturer { get; set; } 

        [Required]
        public decimal Price { get; set; }

        public decimal? DiscountPercentage { get; set; }

        public int? SubcategoryId { get; set; }

        [ForeignKey("SubcategoryId")]
        public virtual Subcategory? Subcategory { get; set; }

        public virtual ICollection<MovementHistory> MovementHistories { get; set; }

        public virtual ICollection<ShipmentItem> ShipmentItems { get; set; }

        public virtual ICollection<ReceiptItem> ReceiptItems { get; set; }

        public virtual ICollection<ProductPhoto> ProductPhotos { get; set; }

        public virtual ICollection<ProductInZonePosition> ProductsInZonePositions { get; set; }

        public Product (string productCode, string name, UnitsOfMeasureEnum unitOfMeasure, decimal quantity, int capacity, decimal price)
        {
            ProductCode = productCode;
            Name = name;
            UnitOfMeasure = unitOfMeasure;
            Quantity = quantity;
            Capacity = capacity;
            Price = price;

            MovementHistories = new List<MovementHistory>();
            ShipmentItems = new List<ShipmentItem>();
            ReceiptItems = new List<ReceiptItem>();
            ProductPhotos = new List<ProductPhoto>();
            ProductsInZonePositions = new List<ProductInZonePosition>();
        }
    }
}
