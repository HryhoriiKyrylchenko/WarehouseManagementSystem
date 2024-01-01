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

        public async Task<decimal> GetUnallocatedProductInstancesSumAsync(int productId)
        {
            var unallocatedProductItems = await dbContext.Products
                .Where(p => p.Id == productId)
                .Select(p => p.Quantity - p.ProductsInZonePositions.Sum(z => z.Quantity))
                .FirstOrDefaultAsync();

            return unallocatedProductItems;
        }

        public async Task<ObservableCollection<Zone>> GetZonesAsync()
        {
            var zones = await dbContext.Zones.ToListAsync();
            return new ObservableCollection<Zone>(zones);
        }

        public async Task<ObservableCollection<ZonePosition>> GetZonePozitionsAsync(int zoneId)
        {
            var zonePositions = await dbContext.ZonePositions
                .Where(zp => zp.ZoneId == zoneId)
                .ToListAsync();
            return new ObservableCollection<ZonePosition>(zonePositions);
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
    }
}
