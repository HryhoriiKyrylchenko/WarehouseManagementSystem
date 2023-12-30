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

        public async Task<List<Category>> GetRootCategoriesAsync(Warehouse warehouse)
        {
            return await dbContext.Categories.Where(c => c.PreviousCategoryId == null
                                                        && c.Products.Any(p => IsProductInWarehouse(p, warehouse)))
                                             .ToListAsync();
        }

        public async Task<CategoryViewModel> BuildCategoryViewModelTreeAsync(Category category, Warehouse warehouse)
        {
            var categoryViewModel = new CategoryViewModel(category);

            var childCategories = await dbContext.Categories.Where(c => c.PreviousCategoryId == category.Id
                                                                        && c.Products.Any(p => IsProductInWarehouse(p, warehouse)))
                                                            .ToListAsync();

            foreach (var childCategory in childCategories)
            {
                var childViewModel = await BuildCategoryViewModelTreeAsync(childCategory, warehouse);
                categoryViewModel.Children.Add(childViewModel);
            }

            var products = await GetProductsForCategoryAsync(category, warehouse);
            categoryViewModel.Products = new ObservableCollection<Product>(products);

            return categoryViewModel;
        }

        private async Task<List<Product>> GetProductsForCategoryAsync(Category category, Warehouse warehouse)
        {
            var products = new List<Product>();

            async Task AddProductsFromCategoryAsync(Category currentCategory, Warehouse warehouse)
            {
                var currentProducts = await dbContext.Products
                    .Where(p => p.CategoryId == currentCategory.Id)
                    .Where(p => IsProductInWarehouse(p, warehouse))
                    .ToListAsync();

                products.AddRange(currentProducts);

                var childCategories = await dbContext.Categories
                    .Where(c => c.PreviousCategoryId == currentCategory.Id
                                && c.Products.Any(p => IsProductInWarehouse(p, warehouse)))
                    .ToListAsync();

                foreach (var childCategory in childCategories)
                {
                    await AddProductsFromCategoryAsync(childCategory, warehouse);
                }
            }

            await AddProductsFromCategoryAsync(category, warehouse);

            return products;
        }

        public User? FindUserByUsernameAndPassword(string username, string password)
        {
            return dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public async Task<ObservableCollection<Warehouse>> GetAllWarehousesAsync()
        {
            var warehouses = await dbContext.Warehouses.ToListAsync();

            return new ObservableCollection<Warehouse>(warehouses);
        }

        public async Task<int> GetTotalWarehouseCapacityAsync(Warehouse warehouse)
        {
            return await dbContext.Zones.Where(z => z.WarehouseId == warehouse.Id).SumAsync(z => z.Capacity);
        }

        public async Task<int> GetFreeWarehouseCapacityAsync(Warehouse warehouse)
        {
            return await dbContext.Zones.Where(z => z.WarehouseId == warehouse.Id).SumAsync(z => z.FreeSpace);
        }

        public async Task<double> GetWarehouseOccupancyPercentageAsync(Warehouse warehouse)
        {
            int totalCapasity = await GetTotalWarehouseCapacityAsync(warehouse);
            int totalFreeSpace = await GetFreeWarehouseCapacityAsync(warehouse);

            return 100.0 - totalFreeSpace * 100.0 / totalCapasity;
        }

        public async Task<int> CountTotalZonesAsync(Warehouse warehouse)
        {
            return await dbContext.Zones.Where(z => z.WarehouseId == warehouse.Id).CountAsync();
        }

        public async Task<int> CountUnusedZonesAsync(Warehouse warehouse)
        {
            return await dbContext.Zones.Where(z => z.WarehouseId == warehouse.Id)
                                        .Where(z => z.Capacity == z.FreeSpace).CountAsync();
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
