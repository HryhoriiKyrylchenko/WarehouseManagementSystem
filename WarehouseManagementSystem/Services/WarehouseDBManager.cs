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

        public async Task<List<Category>> GetRootCategoriesAsync()
        {
            return await dbContext.Categories.Where(c => c.PreviousCategoryId == null).ToListAsync();
        }

        public async Task<CategoryViewModel> BuildCategoryViewModelTreeAsync(Category category)
        {
            var categoryViewModel = new CategoryViewModel(category);

            var childCategories = await dbContext.Categories
                .Where(c => c.PreviousCategoryId == category.Id)
                .ToListAsync();

            foreach (var childCategory in childCategories)
            {
                var childViewModel = await BuildCategoryViewModelTreeAsync(childCategory);
                categoryViewModel.Children.Add(childViewModel);
            }

            var products = await GetProductsForCategoryAsync(category);
            categoryViewModel.Products = new ObservableCollection<Product>(products);

            return categoryViewModel;
        }

        private async Task<List<Product>> GetProductsForCategoryAsync(Category category)
        {
            var products = new List<Product>();

            async Task AddProductsFromCategoryAsync(Category currentCategory)
            {
                var currentProducts = await dbContext.Products
                    .Where(p => p.CategoryId == currentCategory.Id)
                    .ToListAsync();

                products.AddRange(currentProducts);

                var childCategories = await dbContext.Categories
                    .Where(c => c.PreviousCategoryId == currentCategory.Id)
                    .ToListAsync();

                foreach (var childCategory in childCategories)
                {
                    await AddProductsFromCategoryAsync(childCategory);
                }
            }

            await AddProductsFromCategoryAsync(category);

            return products;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
