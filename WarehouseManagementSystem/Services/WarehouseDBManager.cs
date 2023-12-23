using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

            return categoryViewModel;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
