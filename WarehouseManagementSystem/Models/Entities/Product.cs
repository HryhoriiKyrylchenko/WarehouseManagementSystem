using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities.Support_classes;
using Newtonsoft.Json;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Enums;

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
        public UnitsOfMeasureEnum? UnitOfMeasure { get; set; }

        public decimal? Quantity { get; set; }

        public int? Capacity { get; set; }

        public int? ManufacturerId { get; set; }

        [ForeignKey("ManufacturerId")]
        public Manufacturer? Manufacturer { get; set; } 

        [Required]
        public decimal Price { get; set; }

        public decimal? DiscountPercentage { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ProductCategory? Category { get; set; }

        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual Warehouse? Warehouse { get; set; }

        public string? ProductDetails { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<MovementHistory> MovementHistories { get; set; }

        public virtual ICollection<ShipmentItem> ShipmentItems { get; set; }

        public virtual ICollection<ReceiptItem> ReceiptItems { get; set; }

        public virtual ICollection<ProductPhoto> ProductPhotos { get; set; }

        public virtual ICollection<ProductInZonePosition> ProductsInZonePositions { get; set; }

        public virtual ICollection<Label> Labels { get; set; }

        public Product (string productCode, string name, UnitsOfMeasureEnum? unitOfMeasure, decimal? quantity, int? capacity, decimal price, int warehouseId)
        {
            ProductCode = productCode;
            Name = name;
            UnitOfMeasure = unitOfMeasure;
            Quantity = quantity;
            Capacity = capacity;
            Price = price;
            WarehouseId = warehouseId;

            MovementHistories = new List<MovementHistory>();
            ShipmentItems = new List<ShipmentItem>();
            ReceiptItems = new List<ReceiptItem>();
            ProductPhotos = new List<ProductPhoto>();
            ProductsInZonePositions = new List<ProductInZonePosition>();
            Labels = new List<Label>();
        }

        public void AddProductDetail(string key, string value)
        {
            try
            {
                ProductDetail newDetail = new ProductDetail(key, value);
                List<ProductDetail> currentDetails = GetProductDetailsList();
                currentDetails.Add(newDetail);
                ProductDetails = JsonConvert.SerializeObject(currentDetails);
            }
            catch 
            {
                throw;
            }
        }

        private List<ProductDetail> GetProductDetailsList()
        {
            try
            {
                if (string.IsNullOrEmpty(ProductDetails))
                {
                    return new List<ProductDetail>();
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<ProductDetail>>(ProductDetails) ?? new List<ProductDetail>();
                }
            }
            catch (JsonException ex)
            {
                using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                {
                    CustomJsonException cex = new CustomJsonException(ex, ProductDetails ?? "");
                    errorLogger.LogError(cex);
                }
                return new List<ProductDetail>();
            }
            catch (Exception ex)
            {
                using (var errorLogger = new ErrorLogger(new WarehouseDbContext()))
                {
                    errorLogger.LogError(ex);
                }
                return new List<ProductDetail>();
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
