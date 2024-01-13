using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.ViewModels;
using WarehouseManagementSystem.ViewModels.Support_data;

namespace WarehouseManagementSystem.Services
{
    public class WarehouseDBManager : IDisposable
    {
        private readonly WarehouseDbContext dbContext;

        public WarehouseDBManager(WarehouseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public async Task<List<ProductCategory>> GetRootCategoriesAsync(Warehouse warehouse)
        {
            return await dbContext.ProductCategories.Where(c => c.PreviousCategoryId == null).ToListAsync();
        }

        public async Task<CategoryViewModel> BuildCategoryViewModelTreeAsync(ProductCategory category, Warehouse warehouse)
        {
            var categoryViewModel = new CategoryViewModel(category);
            try
            {
                var products = await GetProductsForCategoryAsync(category, warehouse);
                if (products.Any())
                {
                    categoryViewModel.Products = new ObservableCollection<Product>(products);
                }

                var childCategories = await dbContext.ProductCategories.Where(c => c.PreviousCategoryId == category.Id).ToListAsync();

                foreach (var childCategory in childCategories)
                {
                    var childViewModel = await BuildCategoryViewModelTreeAsync(childCategory, warehouse);
                    categoryViewModel.Children.Add(childViewModel);
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show(ex.Message));
                throw;
            }

            return categoryViewModel;
        }

        public bool DoesCategoryHaveProducts(ProductCategory category)
        {
            return (dbContext.Products.Where(p => p.CategoryId == category.Id).Count() > 0);
        }

        public bool DoesCategoryHaveChildrenCategories(ProductCategory category)
        {
            return (dbContext.ProductCategories.Where(pc => pc.PreviousCategoryId == category.Id).Count() > 0);
        }

        private async Task<List<Product>> GetProductsForCategoryAsync(ProductCategory category, Warehouse warehouse)
        {
            return await dbContext.Products
                    .Where(p => p.CategoryId == category.Id && p.WarehouseId == warehouse.Id)
                    .ToListAsync();
        }

        public User? FindUserByUsernameAndPassword(string username, string password)
        {
            return dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public ObservableCollection<Warehouse> GetAllWarehouses()
        {
            var warehouses = dbContext.Warehouses.ToList();

            return new ObservableCollection<Warehouse>(warehouses);
        }

        public async Task<ObservableCollection<Warehouse>?> GetAllWarehousesAsync()
        {
            return new ObservableCollection<Warehouse>(await dbContext.Warehouses.ToListAsync());
        }

        public int GetTotalWarehouseCapacity(Warehouse warehouse)
        {
            return dbContext.Zones.Where(z => z.WarehouseId == warehouse.Id).Sum(z => z.Capacity);
        }

        public double GetWarehouseOccupancyPercentage(Warehouse warehouse)
        {
            int totalCapasity = GetTotalWarehouseCapacity(warehouse);
            int totalFreeSpace = GetFreeWarehouseCapacity(warehouse);

            return (totalCapasity == 0) ? 0 : (100.0 - totalFreeSpace * 100.0 / totalCapasity);
        }

        public int CountTotalZones(Warehouse warehouse)
        {
            return dbContext.Zones.Where(z => z.WarehouseId == warehouse.Id).Count();
        }

        public int GetFreeWarehouseCapacity(Warehouse warehouse)
        {
            int freeCapacity = 0;

            var zoneIds = dbContext.Zones.Select(z => z.Id).ToList();

            foreach (var zoneId in zoneIds)
            {
                freeCapacity += GetZoneFreeCapacity(zoneId);
            }

            return freeCapacity;
        }

        public int CountUnusedZones(Warehouse warehouse)
        {
            var zones = dbContext.Zones.ToList();
            return zones.Where(z => z.Capacity == GetZoneFreeCapacity(z.Id)).Count();
        }


        public int CountTotalProductsInWarehouse(Warehouse warehouse)
        {
            return dbContext.Products.Where(p => p.WarehouseId == warehouse.Id).Count();
        }

        public decimal CountUnallocatedProductsInWarehouse(Warehouse warehouse)
        {
            return dbContext.Products
                .Where(p => p.WarehouseId == warehouse.Id)
                .Where(p => p.Quantity > p.ProductsInZonePositions.Sum(z => z.Quantity))
                .Count();
        }

        public decimal GetUnallocatedProductInstancesSum(int productId)
        {
            var unallocatedProductItems = dbContext.Products
                .Where(p => p.Id == productId)
                .Select(p => p.Quantity - p.ProductsInZonePositions.Sum(z => z.Quantity))
                .FirstOrDefault();

            return unallocatedProductItems ?? 0;
        }

        public async Task<decimal> GetUnallocatedProductInstancesSumAsync(int productId)
        {
            var unallocatedProductItems = await dbContext.Products
                .Where(p => p.Id == productId)
                .Select(p => p.Quantity - p.ProductsInZonePositions.Sum(z => z.Quantity))
                .FirstOrDefaultAsync();

            return unallocatedProductItems ?? 0;
        }

        public async Task<ObservableCollection<Zone>> GetZonesAsync()
        {
            var zones = await dbContext.Zones.Include(z => z.ZonePositions).ToListAsync();
            return new ObservableCollection<Zone>(zones);
        }

        public ObservableCollection<Supplier> GetSuppliers()
        {
            var suppliers = dbContext.Suppliers.ToList();
            return new ObservableCollection<Supplier>(suppliers);
        }

        public ObservableCollection<Customer> GetCustomers()
        {
            var customers = dbContext.Customers.ToList();
            return new ObservableCollection<Customer>(customers);
        }

        public async Task<ObservableCollection<Supplier>> GetSuppliersAsync()
        {
            var suppliers = await dbContext.Suppliers.ToListAsync();
            return new ObservableCollection<Supplier>(suppliers);
        }

        public async Task<ObservableCollection<Customer>> GetCustomersAsync()
        {
            var customers = await dbContext.Customers.ToListAsync();
            return new ObservableCollection<Customer>(customers);
        }

        public async Task<ObservableCollection<Report>?> GetReportsAsync()
        {
            var reports = await dbContext.Reports.Include(r => r.User).ToListAsync();
            return new ObservableCollection<Report>(reports);
        }

        public ObservableCollection<Product> GetProducts()
        {
            var products = dbContext.Products.ToList();
            return new ObservableCollection<Product>(products);
        }

        public async Task<ObservableCollection<Product>> GetProductsAsync()
        {
            var products = await dbContext.Products.ToListAsync();
            return new ObservableCollection<Product>(products);
        }

        public async Task<ObservableCollection<Product>> GetProductsByWarehouseInStockAsync(int warehouseId)
        {
            var products = await dbContext.Products.Where(p => p.WarehouseId == warehouseId)
                                                   .Where(p => p.Quantity > 0)
                                                   .ToListAsync();
            return new ObservableCollection<Product>(products);
        }

        public ObservableCollection<Manufacturer> GetManufacturers()
        {
            var manufacturers = dbContext.Manufacturers.ToList();
            return new ObservableCollection<Manufacturer>(manufacturers);
        }

        public async Task<ObservableCollection<Manufacturer>> GetManufacturersAsync()
        {
            var manufacturers = await dbContext.Manufacturers.ToListAsync();
            return new ObservableCollection<Manufacturer>(manufacturers);
        }

        public async Task<ObservableCollection<User>> GetUsersWithoutAdminAsync()
        {
            var users = await dbContext.Users.Where(u => u.Role != Enums.UserRolesEnum.ADMIN).ToListAsync();
            return new ObservableCollection<User>(users);
        }

        public async Task<ObservableCollection<ZonePosition>> GetZonePozitionsAsync(int zoneId)
        {
            var zonePositions = await dbContext.ZonePositions
                .Where(zp => zp.ZoneId == zoneId)
                .ToListAsync();
            return new ObservableCollection<ZonePosition>(zonePositions);
        }

        public List<ProductInZonePosition> GetProductInZonePozitionsByProduct(int productId)
        {
            var zonePositions = dbContext.ProductInZonePositions
                .Where(zp => zp.ProductId == productId)
                .ToList();
            return zonePositions;
        }

        public async Task<ObservableCollection<ProductInZonePosition>> GetProductInZonePositionsByProductAsync(int productId)
        {
            var resultProductInZonePositions = await dbContext.ProductInZonePositions
                .Where(pzp => pzp.ProductId == productId)
                .Include(pzp => pzp.Product)
                .Include(pzp => pzp.ZonePosition)
                .ToListAsync();
            return new ObservableCollection<ProductInZonePosition>(resultProductInZonePositions);
        }

        public ObservableCollection<ProductInZonePosition> GetProductInZonePositions(int zonePozitionId)
        {
            var productInZonePositions = dbContext.ProductInZonePositions
                .Where(pzp => pzp.ZonePositionId == zonePozitionId)
                .Include(pzp => pzp.Product)
                .Include(pzp => pzp.ZonePosition)
                .ToList();
            return new ObservableCollection<ProductInZonePosition>(productInZonePositions);
        }

        public int GetZonePositionFreeCapacity(int zonePositionId)
        {
            var zonePositionWithProducts = dbContext.ZonePositions
                .Include(zp => zp.ProductsInZonePosition)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(zp => zp.Id == zonePositionId);

            return zonePositionWithProducts?.Capacity - zonePositionWithProducts?.ProductsInZonePosition.Sum(p => p.Quantity * (p.Product?.Capacity ?? 0)) ?? 0;
        }

        public int GetZoneFreeCapacity(int zoneId)
        {
            var zoneWithZonePositions = dbContext.Zones
                .Include(z => z.ZonePositions)
                .FirstOrDefault(z => z.Id == zoneId);

            if (zoneWithZonePositions == null)
                return 0;

            var zonePositionsFreeCapacities = zoneWithZonePositions.ZonePositions.Select(zp => GetZonePositionFreeCapacity(zp.Id));

            int zonePositionsFreeCapacity = zonePositionsFreeCapacities.Sum();
            int capacityDifference = zoneWithZonePositions.Capacity - zoneWithZonePositions.ZonePositions.Sum(zp => zp.Capacity);

            return zonePositionsFreeCapacity + capacityDifference;
        }

        public async Task<ObservableCollection<Receipt>?> GetReceiptsByFilterAsync(ReceiptsSelectorsFilterModel filterSelectors)
        {
            var query = dbContext.Receipts.AsQueryable();

            if (filterSelectors.SectionDateFrom.HasValue)
            {
                query = query.Where(r => r.ReceiptDate >= filterSelectors.SectionDateFrom.Value);
            }

            if (filterSelectors.SectionDateTo.HasValue)
            {
                query = query.Where(r => r.ReceiptDate <= filterSelectors.SectionDateTo.Value);
            }

            if (!filterSelectors.SectionAllSuppliersSelected && filterSelectors.SelectedSupplier != null)
            {
                query = query.Where(r => r.SupplierId == filterSelectors.SelectedSupplier.Id);
            }

            if (!filterSelectors.SectionAllUsersSelected && filterSelectors.SelectedUser != null)
            {
                query = query.Where(r => r.UserId == filterSelectors.SelectedUser.Id);
            }

            var result = await query
                .Include(r => r.Supplier)
                .Include(r => r.User)
                .Include(r => r.ReceiptItems)
                    .ThenInclude(ri => ri.Product)
                .ToListAsync();
            return new ObservableCollection<Receipt>(result);
        }

        public async Task<ObservableCollection<Shipment>?> GetShipmentsByFilterAsync(ShipmentsSelectorsFilterModel filterSelectors)
        {
            var query = dbContext.Shipments.AsQueryable();

            if (filterSelectors.SectionDateFrom.HasValue)
            {
                query = query.Where(r => r.ShipmentDate >= filterSelectors.SectionDateFrom.Value);
            }

            if (filterSelectors.SectionDateTo.HasValue)
            {
                query = query.Where(r => r.ShipmentDate <= filterSelectors.SectionDateTo.Value);
            }

            if (!filterSelectors.SectionAllCustomersSelected && filterSelectors.SelectedCustomer != null)
            {
                query = query.Where(r => r.CustomerId == filterSelectors.SelectedCustomer.Id);
            }

            if (!filterSelectors.SectionAllUsersSelected && filterSelectors.SelectedUser != null)
            {
                query = query.Where(r => r.UserId == filterSelectors.SelectedUser.Id);
            }

            var result = await query
                .Include(r => r.Customer)
                .Include(r => r.User)
                .Include(r => r.ShipmentItems)
                    .ThenInclude(ri => ri.Product)
                .ToListAsync();
            return new ObservableCollection<Shipment>(result);
        }

        public async Task<ObservableCollection<Report>?> GetReportsByFilterAsync(ReportsSelectorsFilterModel filterSelectors)
        {
            var query = dbContext.Reports.AsQueryable();

            if (filterSelectors.SectionByDateSelected && filterSelectors.SelectedDate.HasValue)
            {
                query = query.Where(r => r.ReportDate == filterSelectors.SelectedDate.Value);
            }

            if (filterSelectors.SectionByUserSelected && filterSelectors.SelectedUser != null)
            {
                query = query.Where(r => r.UserId == filterSelectors.SelectedUser.Id);
            }

            if (filterSelectors.SectionByTypeSelected && filterSelectors.SelectedType.HasValue)
            {
                query = query.Where(r => r.ReportType == filterSelectors.SelectedType);
            }

            var result = await query
                .Include(r => r.User)
                .ToListAsync();
            return new ObservableCollection<Report>(result);
        }

        public bool IsProductCodeInDB(string productCode)
        {
            return dbContext.Products.Any(p => p.ProductCode == productCode);
        }

        public ProductInZonePosition? GetProductInZonePositionByProduct(int productId, 
                                                                        int productInZonePositionId, 
                                                                        DateTime? manufactureDate, 
                                                                        DateTime? expiryDate)
        {
            return dbContext.ProductInZonePositions.Where(pzp => pzp.ProductId == productId
                                                && pzp.ZonePositionId == productInZonePositionId
                                                && pzp.ManufactureDate == manufactureDate
                                                && pzp.ExpiryDate == expiryDate).FirstOrDefault();
        }

        public List<MovementHistory> GetMovementHistoriesFiltered(DateTime? reportDateFrom, DateTime? reportDateTo)
        {
            IQueryable<MovementHistory> query = dbContext.MovementHistories;

            if (reportDateFrom.HasValue)
            {
                query = query.Where(mh => mh.MovementDate >= reportDateFrom.Value);
            }

            if (reportDateTo.HasValue)
            {
                query = query.Where(mh => mh.MovementDate <= reportDateTo.Value);
            }

            return query.ToList();
        }

        public Product? GetProduct(int productId)
        {
            return dbContext.Products.Where(p => p.Id == productId).FirstOrDefault();
        }
    }
}
