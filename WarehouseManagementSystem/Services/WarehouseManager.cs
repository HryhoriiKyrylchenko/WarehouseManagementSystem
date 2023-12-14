using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.Services
{
    public class WarehouseManager : IDisposable
    {
        private readonly WarehouseDbContext dbContext;

        public WarehouseManager(WarehouseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Product AddProduct(Product newProduct)
        {
            dbContext.Products.Add(newProduct);
            dbContext.SaveChanges();
            return newProduct;
        }

        public Product UpdateProduct(Product updatedProduct)
        {
            var existingProduct = dbContext.Products.Find(updatedProduct.Id);

            if (existingProduct != null)
            {
                dbContext.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);
                dbContext.SaveChanges();
                return existingProduct;
            }
            else
            {
                throw new ArgumentException("Current product does not exist in the database");
            }
        }

        public bool DeleteProduct(Product product)
        {
            var productToDelete = dbContext.Products.Find(product.Id);

            if (productToDelete != null)
            {
                dbContext.Products.Remove(productToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public ProductDetail AddProductDetail(ProductDetail newProductDetail)
        {
            dbContext.ProductDetails.Add(newProductDetail);
            dbContext.SaveChanges();
            return newProductDetail;
        }

        public ProductDetail UpdateProductDetail(ProductDetail updatedProductDetail)
        {
            var existingProductDetail = dbContext.ProductDetails.Find(updatedProductDetail.Id);

            if (existingProductDetail != null)
            {
                dbContext.Entry(existingProductDetail).CurrentValues.SetValues(updatedProductDetail);
                dbContext.SaveChanges();
                return existingProductDetail;
            }
            else
            {
                throw new ArgumentException("Current product detail does not exist in the database");
            }
        }

        public bool DeleteProductDetail(ProductDetail productDetail)
        {
            var productDetailToDelete = dbContext.ProductDetails.Find(productDetail.Id);

            if (productDetailToDelete != null)
            {
                dbContext.ProductDetails.Remove(productDetailToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Subcategory AddSubcategory(Subcategory newSubcategory)
        {
            dbContext.Subcategories.Add(newSubcategory);
            dbContext.SaveChanges();
            return newSubcategory;
        }

        public Subcategory UpdateSubcategory(Subcategory updatedSubcategory)
        {
            var existingSubcategory = dbContext.Subcategories.Find(updatedSubcategory.Id);

            if (existingSubcategory != null)
            {
                dbContext.Entry(existingSubcategory).CurrentValues.SetValues(updatedSubcategory);
                dbContext.SaveChanges();
                return existingSubcategory;
            }
            else
            {
                throw new ArgumentException("Current subcategory does not exist in the database");
            }
        }

        public bool DeleteSubcategory(Subcategory subcategory)
        {
            var subcategoryToDelete = dbContext.Subcategories.Find(subcategory.Id);

            if (subcategoryToDelete != null)
            {
                dbContext.Subcategories.Remove(subcategoryToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Category AddCategory(Category newCategory)
        {
            dbContext.Categories.Add(newCategory);
            dbContext.SaveChanges();
            return newCategory;
        }

        public Category UpdateCategory(Category updatedCategory)
        {
            var existingCategory = dbContext.Categories.Find(updatedCategory.Id);

            if (existingCategory != null)
            {
                dbContext.Entry(existingCategory).CurrentValues.SetValues(updatedCategory);
                dbContext.SaveChanges();
                return existingCategory;
            }
            else
            {
                throw new ArgumentException("Current category does not exist in the database");
            }
        }

        public bool DeleteCategory(Category category)
        {
            var categoryToDelete = dbContext.Categories.Find(category.Id);

            if (categoryToDelete != null)
            {
                dbContext.Categories.Remove(categoryToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public MovementHistory AddMovementHistory(MovementHistory newMovementHistory)
        {
            dbContext.MovementHistories.Add(newMovementHistory);
            dbContext.SaveChanges();
            return newMovementHistory;
        }

        public MovementHistory UpdateMovementHistory(MovementHistory updatedMovementHistory)
        {
            var existingMovementHistory = dbContext.MovementHistories.Find(updatedMovementHistory.Id);

            if (existingMovementHistory != null)
            {
                dbContext.Entry(existingMovementHistory).CurrentValues.SetValues(updatedMovementHistory);
                dbContext.SaveChanges();
                return existingMovementHistory;
            }
            else
            {
                throw new ArgumentException("Current movement history does not exist in the database");
            }
        }

        public bool DeleteMovementHistory(MovementHistory movementHistory)
        {
            var movementHistoryToDelete = dbContext.MovementHistories.Find(movementHistory.Id);

            if (movementHistoryToDelete != null)
            {
                dbContext.MovementHistories.Remove(movementHistoryToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public ZonePosition AddZonePosition(ZonePosition newZonePosition)
        {
            dbContext.ZonePositions.Add(newZonePosition);
            dbContext.SaveChanges();
            return newZonePosition;
        }

        public ZonePosition UpdateZonePosition(ZonePosition updatedZonePosition)
        {
            var existingZonePosition = dbContext.ZonePositions.Find(updatedZonePosition.Id);

            if (existingZonePosition != null)
            {
                dbContext.Entry(existingZonePosition).CurrentValues.SetValues(updatedZonePosition);
                dbContext.SaveChanges();
                return existingZonePosition;
            }
            else
            {
                throw new ArgumentException("Current zone position does not exist in the database");
            }
        }

        public bool DeleteZonePosition(ZonePosition zonePosition)
        {
            var zonePositionToDelete = dbContext.ZonePositions.Find(zonePosition.Id);

            if (zonePositionToDelete != null)
            {
                dbContext.ZonePositions.Remove(zonePositionToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Zone AddZone(Zone newZone)
        {
            dbContext.Zones.Add(newZone);
            dbContext.SaveChanges();
            return newZone;
        }

        public Zone UpdateZone(Zone updatedZone)
        {
            var existingZone = dbContext.Zones.Find(updatedZone.Id);

            if (existingZone != null)
            {
                dbContext.Entry(existingZone).CurrentValues.SetValues(updatedZone);
                dbContext.SaveChanges();
                return existingZone;
            }
            else
            {
                throw new ArgumentException("Current zone does not exist in the database");
            }
        }

        public bool DeleteZone(Zone zone)
        {
            var zoneToDelete = dbContext.Zones.Find(zone.Id);

            if (zoneToDelete != null)
            {
                dbContext.Zones.Remove(zoneToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Report AddReport(Report newReport)
        {
            dbContext.Reports.Add(newReport);
            dbContext.SaveChanges();
            return newReport;
        }

        public Report UpdateReport(Report updatedReport)
        {
            var existingReport = dbContext.Reports.Find(updatedReport.Id);

            if (existingReport != null)
            {
                dbContext.Entry(existingReport).CurrentValues.SetValues(updatedReport);
                dbContext.SaveChanges();
                return existingReport;
            }
            else
            {
                throw new ArgumentException("Current report does not exist in the database");
            }
        }

        public bool DeleteReport(Report report)
        {
            var reportToDelete = dbContext.Reports.Find(report.Id);

            if (reportToDelete != null)
            {
                dbContext.Reports.Remove(reportToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public User AddUser(User newUser)
        {
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
            return newUser;
        }

        public User UpdateUser(User updatedUser)
        {
            var existingUser = dbContext.Users.Find(updatedUser.Id);

            if (existingUser != null)
            {
                dbContext.Entry(existingUser).CurrentValues.SetValues(updatedUser);
                dbContext.SaveChanges();
                return existingUser;
            }
            else
            {
                throw new ArgumentException("Current user does not exist in the database");
            }
        }

        public bool DeleteUser(User user)
        {
            var userToDelete = dbContext.Users.Find(user.Id);

            if (userToDelete != null)
            {
                dbContext.Users.Remove(userToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Shipment AddShipment(Shipment newShipment)
        {
            dbContext.Shipments.Add(newShipment);
            dbContext.SaveChanges();
            return newShipment;
        }

        public Shipment UpdateShipment(Shipment updatedShipment)
        {
            var existingShipment = dbContext.Shipments.Find(updatedShipment.Id);

            if (existingShipment != null)
            {
                dbContext.Entry(existingShipment).CurrentValues.SetValues(updatedShipment);
                dbContext.SaveChanges();
                return existingShipment;
            }
            else
            {
                throw new ArgumentException("Current shipment detail does not exist in the database");
            }
        }

        public bool DeleteShipment(Shipment shipment)
        {
            var shipmentToDelete = dbContext.Shipments.Find(shipment.Id);

            if (shipmentToDelete != null)
            {
                dbContext.Shipments.Remove(shipmentToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public ShipmentItem AddShipmentItem(ShipmentItem newShipmentItem)
        {
            dbContext.ShipmentItems.Add(newShipmentItem);
            dbContext.SaveChanges();
            return newShipmentItem;
        }

        public ShipmentItem UpdateShipmentItem(ShipmentItem updatedShipmentItem)
        {
            var existingShipmentItem = dbContext.ShipmentItems.Find(updatedShipmentItem.Id);

            if (existingShipmentItem != null)
            {
                dbContext.Entry(existingShipmentItem).CurrentValues.SetValues(updatedShipmentItem);
                dbContext.SaveChanges();
                return existingShipmentItem;
            }
            else
            {
                throw new ArgumentException("Current shipment item detail does not exist in the database");
            }
        }

        public bool DeleteShipmentItem(ShipmentItem shipmentItem)
        {
            var shipmentItemToDelete = dbContext.ShipmentItems.Find(shipmentItem.Id);

            if (shipmentItemToDelete != null)
            {
                dbContext.ShipmentItems.Remove(shipmentItemToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Receipt AddReceipt(Receipt newReceipt)
        {
            dbContext.Receipts.Add(newReceipt);
            dbContext.SaveChanges();
            return newReceipt;
        }

        public Receipt UpdateReceipt(Receipt updatedReceipt)
        {
            var existingReceipt = dbContext.Receipts.Find(updatedReceipt.Id);

            if (existingReceipt != null)
            {
                dbContext.Entry(existingReceipt).CurrentValues.SetValues(updatedReceipt);
                dbContext.SaveChanges();
                return existingReceipt;
            }
            else
            {
                throw new ArgumentException("Current receipt does not exist in the database");
            }
        }

        public bool DeleteReceipt(Receipt receipt)
        {
            var receiptToDelete = dbContext.Receipts.Find(receipt.Id);

            if (receiptToDelete != null)
            {
                dbContext.Receipts.Remove(receiptToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public ReceiptItem AddReceiptItem(ReceiptItem newReceiptItem)
        {
            dbContext.ReceiptItems.Add(newReceiptItem);
            dbContext.SaveChanges();
            return newReceiptItem;
        }

        public ReceiptItem UpdateReceiptItem(ReceiptItem updatedReceiptItem)
        {
            var existingReceiptItem = dbContext.ReceiptItems.Find(updatedReceiptItem.Id);

            if (existingReceiptItem != null)
            {
                dbContext.Entry(existingReceiptItem).CurrentValues.SetValues(updatedReceiptItem);
                dbContext.SaveChanges();
                return existingReceiptItem;
            }
            else
            {
                throw new ArgumentException("Current receipt item does not exist in the database");
            }
        }

        public bool DeleteReceiptItem(ReceiptItem receiptItem)
        {
            var receiptItemToDelete = dbContext.ReceiptItems.Find(receiptItem.Id);

            if (receiptItemToDelete != null)
            {
                dbContext.ReceiptItems.Remove(receiptItemToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Warehouse AddWarehouse(Warehouse newWarehouse)
        {
            dbContext.Warehouses.Add(newWarehouse);
            dbContext.SaveChanges();
            return newWarehouse;
        }

        public Warehouse UpdateWarehouse(Warehouse updatedWarehouse)
        {
            var existingWarehouse = dbContext.Warehouses.Find(updatedWarehouse.Id);

            if (existingWarehouse != null)
            {
                dbContext.Entry(existingWarehouse).CurrentValues.SetValues(updatedWarehouse);
                dbContext.SaveChanges();
                return existingWarehouse;
            }
            else
            {
                throw new ArgumentException("Current warehouse item does not exist in the database");
            }
        }

        public bool DeleteWarehouse(Warehouse warehouse)
        {
            var warehouseToDelete = dbContext.Warehouses.Find(warehouse.Id);

            if (warehouseToDelete != null)
            {
                dbContext.Warehouses.Remove(warehouseToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Customer AddCustomer(Customer newCustomer)
        {
            dbContext.Customers.Add(newCustomer);
            dbContext.SaveChanges();
            return newCustomer;
        }

        public Customer UpdateCustomer(Customer updatedCustomer)
        {
            var existingCustomer = dbContext.Customers.Find(updatedCustomer.Id);

            if (existingCustomer != null)
            {
                dbContext.Entry(existingCustomer).CurrentValues.SetValues(updatedCustomer);
                dbContext.SaveChanges();
                return existingCustomer;
            }
            else
            {
                throw new ArgumentException("Current customer does not exist in the database");
            }
        }

        public bool DeleteCustomer(Customer customer)
        {
            var customerToDelete = dbContext.Customers.Find(customer.Id);

            if (customerToDelete != null)
            {
                dbContext.Customers.Remove(customerToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Address AddAddress(Address newAddress)
        {
            dbContext.Addresses.Add(newAddress);
            dbContext.SaveChanges();
            return newAddress;
        }

        public Address UpdateAddress(Address updatedAddress)
        {
            var existingAddress = dbContext.Addresses.Find(updatedAddress.Id);

            if (existingAddress != null)
            {
                dbContext.Entry(existingAddress).CurrentValues.SetValues(updatedAddress);
                dbContext.SaveChanges();
                return existingAddress;
            }
            else
            {
                throw new ArgumentException("Current address does not exist in the database");
            }
        }

        public bool DeleteAddress(Address address)
        {
            var addressToDelete = dbContext.Addresses.Find(address.Id);

            if (addressToDelete != null)
            {
                dbContext.Addresses.Remove(addressToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public ProductInZonePosition AddProductInZonePosition(ProductInZonePosition newProductInZonePosition)
        {
            dbContext.ProductInZonePositions.Add(newProductInZonePosition);
            dbContext.SaveChanges();
            return newProductInZonePosition;
        }

        public ProductInZonePosition UpdateProductInZonePosition(ProductInZonePosition updatedProductInZonePosition)
        {
            var existingProductInZonePosition = dbContext.ProductInZonePositions.Find(updatedProductInZonePosition.Id);

            if (existingProductInZonePosition != null)
            {
                dbContext.Entry(existingProductInZonePosition).CurrentValues.SetValues(updatedProductInZonePosition);
                dbContext.SaveChanges();
                return existingProductInZonePosition;
            }
            else
            {
                throw new ArgumentException("Current product in zone position does not exist in the database");
            }
        }

        public bool DeleteProductInZonePosition(ProductInZonePosition productInZonePosition)
        {
            var productInZonePositionToDelete = dbContext.ProductInZonePositions.Find(productInZonePosition.Id);

            if (productInZonePositionToDelete != null)
            {
                dbContext.ProductInZonePositions.Remove(productInZonePositionToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Manufacturer AddManufacturer(Manufacturer newManufacturer)
        {
            dbContext.Manufacturers.Add(newManufacturer);
            dbContext.SaveChanges();
            return newManufacturer;
        }

        public Manufacturer UpdateManufacturer(Manufacturer updatedManufacturer)
        {
            var existingManufacturer = dbContext.Manufacturers.Find(updatedManufacturer.Id);

            if (existingManufacturer != null)
            {
                dbContext.Entry(existingManufacturer).CurrentValues.SetValues(updatedManufacturer);
                dbContext.SaveChanges();
                return existingManufacturer;
            }
            else
            {
                throw new ArgumentException("Current manufacturer does not exist in the database");
            }
        }

        public bool DeleteManufacturer(Manufacturer manufacturer)
        {
            var manufacturerToDelete = dbContext.Manufacturers.Find(manufacturer.Id);

            if (manufacturerToDelete != null)
            {
                dbContext.Manufacturers.Remove(manufacturerToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
