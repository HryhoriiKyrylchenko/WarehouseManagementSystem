using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private bool IsProductInWarehouse(Product product, Warehouse warehouse)
        {
            return product.ProductsInZonePositions.Any(pz => pz.ZonePosition != null
                                                            && pz.ZonePosition.Zone != null
                                                            && pz.ZonePosition.Zone.WarehouseId == warehouse.Id);
        }

        public async Task<List<ProductCategory>> GetRootCategoriesAsync(Warehouse warehouse)
        {
            return await dbContext.ProductCategories.Where(c => c.PreviousCategoryId == null).ToListAsync();
        }

        public async Task<CategoryViewModel> BuildCategoryViewModelTreeAsync(ProductCategory category, Warehouse warehouse)
        {
            var categoryViewModel = new CategoryViewModel(category);

            var products = await GetProductsForCategoryAsync(category, warehouse);
            if (products.Any())
            {
                categoryViewModel.Products = new ObservableCollection<Product>(products);
            }

            var childCategories = await dbContext.ProductCategories.Where(c => c.PreviousCategoryId == category.Id).ToListAsync();

            var childViewModels = await Task.WhenAll(childCategories.Select(childCategory =>
                                                                        BuildCategoryViewModelTreeAsync(childCategory, warehouse)));

            foreach (var childViewModel in childViewModels)
            {
                categoryViewModel.Children.Add(childViewModel);
            }

            return categoryViewModel;
        }

        private async Task<List<Product>> GetProductsForCategoryAsync(ProductCategory category, Warehouse warehouse)
        {
            var products = new List<Product>();

            async Task AddProductsFromCategoryAsync(ProductCategory currentCategory, Warehouse warehouse)
            {
                var currentProducts = await dbContext.Products
                    .Where(p => p.CategoryId == currentCategory.Id)
                    .Where(p => IsProductInWarehouse(p, warehouse))
                    .ToListAsync();

                products.AddRange(currentProducts);
            }

            await AddProductsFromCategoryAsync(category, warehouse);

            return products;
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

        public async Task<int> GetTotalWarehouseCapacityAsync(Warehouse warehouse)
        {
            return await dbContext.Zones.Where(z => z.WarehouseId == warehouse.Id).SumAsync(z => z.Capacity);
        }

        public async Task<int> GetFreeWarehouseCapacityAsync(Warehouse warehouse)
        {
            int freeCapacity = 0;
            await Task.Run(() =>
            {
                freeCapacity = dbContext.Zones
                    .Where(z => z.WarehouseId == warehouse.Id)
                    .AsEnumerable()
                    .Sum(z => z.FreeSpace);
            });
            return freeCapacity;
        }

        public async Task<double> GetWarehouseOccupancyPercentageAsync(Warehouse warehouse)
        {
            int totalCapasity = await GetTotalWarehouseCapacityAsync(warehouse);
            int totalFreeSpace = await GetFreeWarehouseCapacityAsync(warehouse);

            return (totalCapasity == 0) ? 0 : (100.0 - totalFreeSpace * 100.0 / totalCapasity);
        }

        public async Task<int> CountTotalZonesAsync(Warehouse warehouse)
        {
            return await dbContext.Zones.Where(z => z.WarehouseId == warehouse.Id).CountAsync();
        }

        public async Task<int> CountUnusedZonesAsync(Warehouse warehouse)
        {
            return await Task.Run(() =>
            {
                var zones = dbContext.Zones
                    .Where(z => z.WarehouseId == warehouse.Id)
                    .AsEnumerable()
                    .Where(z => z.Capacity == z.FreeSpace);

                return zones.Count();
            });
        }

        public async Task<int> CountTotalProductsInWarehouseAsync(Warehouse warehouse)
        {
            return await dbContext.Zones.Where(z => z.WarehouseId == warehouse.Id)
                                                        .SelectMany(z => z.ZonePositions)
                                                        .SelectMany(zp => zp.ProductsInZonePosition)
                                                        .Select(p => p.Product)
                                                        .Distinct()
                                                        .CountAsync();
        }

        public async Task<int> CountUnallocatedProductsInWarehouseAsync(Warehouse warehouse)
        {
            return await dbContext.Products
                    .Where(p => p.Quantity > 0)
                    .Where(p => p.ProductsInZonePositions.Count > 0)
                    .Where(p => p.ProductsInZonePositions
                        .Where(pz => pz.ZonePosition != null 
                                    && pz.ZonePosition.Zone != null 
                                    && pz.ZonePosition.Zone.WarehouseId == warehouse.Id)
                        .Sum(pz => pz.Quantity) < p.Quantity)
                    .CountAsync();
        }
    }
}
