using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

        private ObservableCollection<CategoryViewModel> categories;

        public ObservableCollection<CategoryViewModel> Categories 
        { 
            get { return categories; } 
            set {
                if (categories != value)
                {
                    categories = value;
                    OnPropertyChanged(nameof(Categories));
                }
            }
        }

        private CategoryViewModel? selectedCategory;

        public CategoryViewModel? SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    UpdateProducts();
                }
            }
        }

        private ObservableCollection<Product>? products;

        public ObservableCollection<Product>? Products
        {
            get { return products; }
            set
            {
                if (products != value)
                {
                    products = value;
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        public ProductsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            categories = new ObservableCollection<CategoryViewModel>();

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await InitializeCategoriesFromDBAsync();
        }

        private async Task InitializeCategoriesFromDBAsync()
        {
            try
            {
                using (var dbManager = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    var rootCategories = await dbManager.GetRootCategoriesAsync(mainViewModel.LoginService.CurrentWarehouse);
                    if (rootCategories != null) 
                    {
                        foreach (var rootCategory in rootCategories)
                        {
                            var rootViewModel = await dbManager.BuildCategoryViewModelTreeAsync(rootCategory, mainViewModel.LoginService.CurrentWarehouse);
                            Categories.Add(rootViewModel);
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    await logger.LogErrorAsync(ex);
                }
            }    
        }

        private void UpdateProducts()
        {
            if (SelectedCategory != null)
            {
                var products = GetProductsForCategory(SelectedCategory);
                Products = new ObservableCollection<Product>(products);
            }
            else
            {
                Products = new ObservableCollection<Product>();
            }
        }

        private List<Product> GetProductsForCategory(CategoryViewModel categoryViewModel)
        {
            var products = new List<Product>();

            void AddProductsFromCategory(CategoryViewModel category)
            {
                products.AddRange(category.Category.Products);

                foreach (var childCategory in category.Children)
                {
                    AddProductsFromCategory(childCategory);
                }
            }

            AddProductsFromCategory(categoryViewModel);

            return products;
        }

        public ICommand BackCommand => new RelayCommand(Back);

        private void Back(object parameter)
        {
            mainViewModel.NavigateBack();
        }
    }
}
