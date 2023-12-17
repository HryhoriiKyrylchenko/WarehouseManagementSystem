using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Exceptions;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.Services
{
    public class EntityManager : IDisposable
    {
        private readonly WarehouseDbContext dbContext;

        public EntityManager(WarehouseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Product AddProduct(Product newProduct)
        {
            var existingProduct = dbContext.Products.Find(newProduct.Id);

            if (existingProduct == null)
            {
                dbContext.Products.Add(newProduct);
                dbContext.SaveChanges();
                return newProduct;
            }
            else
            {
                throw new DuplicateObjectException("Current product already exists in the database");
            }
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

        public Category AddCategory(Category newCategory)
        {
            var existingCategory = dbContext.Categories.Find(newCategory.Id);

            if (existingCategory == null)
            {
                dbContext.Categories.Add(newCategory);
                dbContext.SaveChanges();
                return newCategory;
            }
            else
            {
                throw new DuplicateObjectException("Current category already exists in the database");
            }
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
            var existingMovementHistory = dbContext.MovementHistories.Find(newMovementHistory.Id);

            if (existingMovementHistory == null)
            {
                dbContext.MovementHistories.Add(newMovementHistory);
                dbContext.SaveChanges();
                return newMovementHistory;
            }
            else
            {
                throw new DuplicateObjectException("Current movement history already exists in the database");
            }
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
            var existingZonePosition = dbContext.ZonePositions.Find(newZonePosition.Id);

            if (existingZonePosition == null)
            {
                dbContext.ZonePositions.Add(newZonePosition);
                dbContext.SaveChanges();
                return newZonePosition;
            }
            else
            {
                throw new DuplicateObjectException("Current zone position already exists in the database");
            }
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
            var existingZone = dbContext.Zones.Find(newZone.Id);

            if (existingZone == null)
            {
                dbContext.Zones.Add(newZone);
                dbContext.SaveChanges();
                return newZone;
            }
            else
            {
                throw new DuplicateObjectException("Current zone already exists in the database");
            }
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
            var existingReport = dbContext.Reports.Find(newReport.Id);

            if (existingReport == null)
            {
                dbContext.Reports.Add(newReport);
                dbContext.SaveChanges();
                return newReport;
            }
            else
            {
                throw new DuplicateObjectException("Current report already exists in the database");
            }
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
            var existingUser = dbContext.Users.Find(newUser.Id);

            if (existingUser == null)
            {
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                return newUser;
            }
            else
            {
                throw new DuplicateObjectException("Current user already exists in the database");
            }
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
            var existingShipment = dbContext.Shipments.Find(newShipment.Id);

            if (existingShipment == null)
            {
                dbContext.Shipments.Add(newShipment);
                dbContext.SaveChanges();
                return newShipment;
            }
            else
            {
                throw new DuplicateObjectException("Current shipment already exists in the database");
            }
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
            var existingShipmentItem = dbContext.ShipmentItems.Find(newShipmentItem.Id);

            if (existingShipmentItem == null)
            {
                dbContext.ShipmentItems.Add(newShipmentItem);
                dbContext.SaveChanges();
                return newShipmentItem;
            }
            else
            {
                throw new DuplicateObjectException("Current shipment item already exists in the database");
            }
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
            var existingReceipt = dbContext.Receipts.Find(newReceipt.Id);

            if (existingReceipt == null)
            {
                dbContext.Receipts.Add(newReceipt);
                dbContext.SaveChanges();
                return newReceipt;
            }
            else
            {
                throw new DuplicateObjectException("Current receipt already exists in the database");
            }
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
            var existingReceiptItem = dbContext.ReceiptItems.Find(newReceiptItem.Id);

            if (existingReceiptItem == null)
            {
                dbContext.ReceiptItems.Add(newReceiptItem);
                dbContext.SaveChanges();
                return newReceiptItem;
            }
            else
            {
                throw new DuplicateObjectException("Current receipt item already exists in the database");
            }
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
            var existingWarehouse = dbContext.Warehouses.Find(newWarehouse.Id);

            if (existingWarehouse == null)
            {
                dbContext.Warehouses.Add(newWarehouse);
                dbContext.SaveChanges();
                return newWarehouse;
            }
            else
            {
                throw new DuplicateObjectException("Current warehouse already exists in the database");
            }
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

        public Supplier AddSupplier(Supplier newSupplier)
        {
            var existingSupplier = dbContext.Suppliers.Find(newSupplier.Id);

            if (existingSupplier == null)
            {
                dbContext.Suppliers.Add(newSupplier);
                dbContext.SaveChanges();
                return newSupplier;
            }
            else
            {
                throw new DuplicateObjectException("Current supplier already exists in the database");
            }
        }

        public Supplier UpdateSupplier(Supplier updatedSupplier)
        {
            var existingSupplier = dbContext.Suppliers.Find(updatedSupplier.Id);

            if (existingSupplier != null)
            {
                dbContext.Entry(existingSupplier).CurrentValues.SetValues(updatedSupplier);
                dbContext.SaveChanges();
                return existingSupplier;
            }
            else
            {
                throw new ArgumentException("Current supplier item does not exist in the database");
            }
        }

        public bool DeleteSupplier(Supplier supplier)
        {
            var supplierToDelete = dbContext.Suppliers.Find(supplier.Id);

            if (supplierToDelete != null)
            {
                dbContext.Suppliers.Remove(supplierToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public Customer AddCustomer(Customer newCustomer)
        {
            var existingCustomer = dbContext.Customers.Find(newCustomer.Id);

            if (existingCustomer == null)
            {
                dbContext.Customers.Add(newCustomer);
                dbContext.SaveChanges();
                return newCustomer;
            }
            else
            {
                throw new DuplicateObjectException("Current customer already exists in the database");
            }
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
            var existingAddress = dbContext.Addresses.Find(newAddress.Id);

            if (existingAddress == null)
            {
                dbContext.Addresses.Add(newAddress);
                dbContext.SaveChanges();
                return newAddress;
            }
            else
            {
                throw new DuplicateObjectException("Current address already exists in the database");
            }
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

        public ProductPhoto AddProductPhoto(ProductPhoto newProductPhoto)
        {
            var existingProductPhoto = dbContext.ProductPhotos.Find(newProductPhoto.Id);

            if (existingProductPhoto == null)
            {
                dbContext.ProductPhotos.Add(newProductPhoto);
                dbContext.SaveChanges();
                return newProductPhoto;
            }
            else
            {
                throw new DuplicateObjectException("Current product photo already exists in the database");
            }
        }

        public ProductPhoto UpdateProductPhoto(ProductPhoto updatedProductPhoto)
        {
            var existingProductPhoto = dbContext.ProductPhotos.Find(updatedProductPhoto.Id);

            if (existingProductPhoto != null)
            {
                dbContext.Entry(existingProductPhoto).CurrentValues.SetValues(updatedProductPhoto);
                dbContext.SaveChanges();
                return existingProductPhoto;
            }
            else
            {
                throw new ArgumentException("Current product photo item does not exist in the database");
            }
        }

        public bool DeleteProductPhoto(ProductPhoto productPhoto)
        {
            var productPhotoToDelete = dbContext.ProductPhotos.Find(productPhoto.Id);

            if (productPhotoToDelete != null)
            {
                dbContext.ProductPhotos.Remove(productPhotoToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public ProductInZonePosition AddProductInZonePosition(ProductInZonePosition newProductInZonePosition)
        {
            var existingProductInZonePosition = dbContext.ProductInZonePositions.Find(newProductInZonePosition.Id);

            if (existingProductInZonePosition == null)
            {
                dbContext.ProductInZonePositions.Add(newProductInZonePosition);
                dbContext.SaveChanges();
                return newProductInZonePosition;
            }
            else
            {
                throw new DuplicateObjectException("Current product in zone position already exists in the database");
            }
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
            var existingManufacturer = dbContext.Manufacturers.Find(newManufacturer.Id);

            if (existingManufacturer == null)
            {
                dbContext.Manufacturers.Add(newManufacturer);
                dbContext.SaveChanges();
                return newManufacturer;
            }
            else
            {
                throw new DuplicateObjectException("Current manufacturer already exists in the database");
            }
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

        public Label AddLabel(Label newLabel)
        {
            var existingLabel = dbContext.Labels.Find(newLabel.Id);

            if (existingLabel == null)
            {
                dbContext.Labels.Add(newLabel);
                dbContext.SaveChanges();
                return newLabel;
            }
            else
            {
                throw new DuplicateObjectException("Current label already exists in the database");
            }
        }

        public Label UpdateLabel(Label updatedLabel)
        {
            var existingLabel = dbContext.Labels.Find(updatedLabel.Id);

            if (existingLabel != null)
            {
                dbContext.Entry(existingLabel).CurrentValues.SetValues(updatedLabel);
                dbContext.SaveChanges();
                return existingLabel;
            }
            else
            {
                throw new ArgumentException("Current label does not exist in the database");
            }
        }

        public bool DeleteLabel(Label label)
        {
            var labelToDelete = dbContext.Labels.Find(label.Id);

            if (labelToDelete != null)
            {
                dbContext.Labels.Remove(labelToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public ErrorLog AddErrorLog(ErrorLog newErrorLog)
        {
            var existingErrorLog = dbContext.ErrorLogs.Find(newErrorLog.Id);

            if (existingErrorLog == null)
            {
                dbContext.ErrorLogs.Add(newErrorLog);
                dbContext.SaveChanges();
                return newErrorLog;
            }
            else
            {
                throw new DuplicateObjectException("Current error log already exists in the database");
            }
        }

        public ErrorLog UpdateErrorLog(ErrorLog updatedErrorLog)
        {
            var existingErrorLog = dbContext.ErrorLogs.Find(updatedErrorLog.Id);

            if (existingErrorLog != null)
            {
                dbContext.Entry(existingErrorLog).CurrentValues.SetValues(updatedErrorLog);
                dbContext.SaveChanges();
                return existingErrorLog;
            }
            else
            {
                throw new ArgumentException("Current error log does not exist in the database");
            }
        }

        public bool DeleteErrorLog(ErrorLog errorLog)
        {
            var errorLogToDelete = dbContext.ErrorLogs.Find(errorLog.Id);

            if (errorLogToDelete != null)
            {
                dbContext.ErrorLogs.Remove(errorLogToDelete);
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
