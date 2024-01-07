using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Support_data;
using WarehouseManagementSystem.Windows;
using Zone = WarehouseManagementSystem.Models.Entities.Zone;

namespace WarehouseManagementSystem.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

        public MainViewModel MainViewModel 
        { 
            get { return mainViewModel; }
        }

        private ObservableCollection<CategoryViewModel> categories;

        public ObservableCollection<CategoryViewModel> Categories
        {
            get { return categories; }
            set
            {
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

        private ProductsSelectorsFilterModel productSelectors;

        public ProductsSelectorsFilterModel ProductSelectors
        {
            get { return productSelectors; }
            set
            {
                if (productSelectors != value)
                {
                    productSelectors = value;
                    OnPropertyChanged(nameof(ProductSelectors));
                }
            }
        }

        private ProductsCategoriesSelectorsFilterModel categoriesSelector;

        public ProductsCategoriesSelectorsFilterModel CategoriesSelector
        {
            get { return categoriesSelector; }
            set
            {
                if (categoriesSelector != value)
                {
                    categoriesSelector = value;
                    OnPropertyChanged(nameof(CategoriesSelector));
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
                    UpdateFilteredProducts();
                }
            }
        }

        private ObservableCollection<Product>? filteredProducts;

        public ObservableCollection<Product>? FilteredProducts
        {
            get { return filteredProducts; }
            set
            {
                if (filteredProducts != value)
                {
                    filteredProducts = value;
                    OnPropertyChanged(nameof(FilteredProducts));
                }
            }
        }

        private Product? selectedProduct;

        public Product? SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                if (selectedProduct != value)
                {
                    selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                    UpdateUnallocatedDataAsync(value);
                    UpdateInputQuantity(string.Empty);
                }
            }
        }

        private UnallocatedProductInstancesModel? unallocatedProductInstances;

        public UnallocatedProductInstancesModel? UnallocatedProductInstances
        {
            get { return unallocatedProductInstances; }
            set
            {
                if (unallocatedProductInstances != value)
                {
                    unallocatedProductInstances = value;
                    OnPropertyChanged(nameof(UnallocatedProductInstances));
                }
            }
        }

        private ObservableCollection<Zone>? zones;

        public ObservableCollection<Zone>? Zones
        {
            get { return zones; }
            set
            {
                if (zones != value)
                {
                    zones = value;
                    OnPropertyChanged(nameof(Zones));
                }
            }
        }

        private Zone? selectedZone;

        public Zone? SelectedZone
        {
            get { return selectedZone; }
            set
            {
                if (selectedZone != value)
                {
                    selectedZone = value;
                    OnPropertyChanged(nameof(SelectedZone));
                    UpdateZonePositions(value);
                }
            }
        }

        private ObservableCollection<ZonePosition>? zonePositions;

        public ObservableCollection<ZonePosition>? ZonePositions
        {
            get { return zonePositions; }
            set
            {
                if (zonePositions != value)
                {
                    zonePositions = value;
                    OnPropertyChanged(nameof(ZonePositions));
                }
            }
        }

        private ZonePosition? selectedZonePosition;

        public ZonePosition? SelectedZonePosition
        {
            get { return selectedZonePosition; }
            set
            {
                if (selectedZonePosition != value)
                {
                    selectedZonePosition = value;
                    OnPropertyChanged(nameof(SelectedZonePosition));
                    UpdateSelectedZonePositionFreeCapacity(value);
                }
            }
        }

        private int selectedZonePositionFreeCapacity;

        public int SelectedZonePositionFreeCapacity
        {
            get { return selectedZonePositionFreeCapacity; }
            set
            {
                if (selectedZonePositionFreeCapacity != value)
                {
                    selectedZonePositionFreeCapacity = value;
                    OnPropertyChanged(nameof(SelectedZonePositionFreeCapacity));
                }
            }
        }

        private string? inputQuantity;

        public string? InputQuantity
        {
            get { return inputQuantity; }
            set
            {
                if (inputQuantity != value)
                {
                    inputQuantity = value;
                    OnPropertyChanged(nameof(InputQuantity));
                    UpdateCapacityToBeAllocated(value);
                }
            }
        }

        private int? capacityToBeAllocated;

        public int? CapacityToBeAllocated
        {
            get { return capacityToBeAllocated; }
            set
            {
                if (capacityToBeAllocated != value)
                {
                    capacityToBeAllocated = value;
                    OnPropertyChanged(nameof(CapacityToBeAllocated));
                }
            }
        }

        public ProductsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            categories = new ObservableCollection<CategoryViewModel>();
            productSelectors = new ProductsSelectorsFilterModel(this);
            categoriesSelector = new ProductsCategoriesSelectorsFilterModel(this);
            CategoriesSelector.CheckboxAllCategoriesChecked = false;

            InitializeAsync();
        }

        public async void InitializeAsync()
        {
            Categories.Clear();
            Zones?.Clear();
            await InitializeCategoriesFromDBAsync();
            await InitializeZonesFromDBAsync();
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

        private async Task InitializeZonesFromDBAsync()
        {
            try
            {
                using (var dbManager = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    Zones = await dbManager.GetZonesAsync();
                    if (Zones.Any())
                    {
                        SelectedZone = Zones.First();
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

        internal void UpdateProducts()
        {
            if (CategoriesSelector.CheckboxAllCategoriesChecked)
            {
                var allProducts = GetAllProducts(Categories);
                Products = new ObservableCollection<Product>(allProducts);
            }
            else if (CategoriesSelector.CheckboxAllCategoriesUnchecked && SelectedCategory != null)
            {
                Products = SelectedCategory.Products;
            }
            else
            {
                Products = new ObservableCollection<Product>();
            }
        }

        public IEnumerable<Product> GetAllProducts(IEnumerable<CategoryViewModel> categories)
        {
            var products = categories.SelectMany(category => category.Products);
            var childProducts = categories.SelectMany(category => GetAllProducts(category.Children));

            return products.Concat(childProducts);
        }

        private async void UpdateUnallocatedDataAsync(Product? product)
        {
            UnallocatedProductInstancesModel tempModel = new UnallocatedProductInstancesModel();
            if (product != null)
            {
                try
                {
                    using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                    {
                        tempModel.UnallocatedBalance = await db.GetUnallocatedProductInstancesSumAsync(product.Id);
                        tempModel.UnallocatedCapacity = tempModel.UnallocatedBalance * (product.Capacity ?? 0);
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
            UnallocatedProductInstances = tempModel;
        }

        private async void UpdateZonePositions(Zone? zone)
        {
            ObservableCollection<ZonePosition> zonePos = new ObservableCollection<ZonePosition>();
            if (zone != null)
            {
                try
                {
                    using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                    {
                        zonePos = await db.GetZonePozitionsAsync(zone.Id);
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
            ZonePositions = zonePos;
            if (ZonePositions.Any())
            {
                SelectedZonePosition = ZonePositions.First();
            }
        }

        private async void UpdateSelectedZonePositionFreeCapacity(ZonePosition? zonePosition)
        {
            if (zonePosition != null)
            {
                try
                {
                    using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                    {
                        SelectedZonePositionFreeCapacity = db.GetZonePositionFreeCapacity(zonePosition.Id);
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
        }

        private void UpdateInputQuantity(string value)
        {
            if (value == string.Empty || int.TryParse(value, out int quantity) && quantity >= 0)
            {
                InputQuantity = value;
            }
        }

        private void UpdateCapacityToBeAllocated(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                CapacityToBeAllocated = 0;
            }
            else
            {
                CapacityToBeAllocated = Convert.ToInt32(inputQuantity) * SelectedProduct?.Capacity;
            }
        }

        public void UpdateFilteredProducts()
        {
            if (Products != null)
            {
                if (ProductSelectors.SectionUnallocatedSelected)
                {
                    FilteredProducts = new ObservableCollection<Product>();
                    try
                    {
                        using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                        {
                            foreach (var product in Products)
                            {
                                if (db.GetUnallocatedProductInstancesSum(product.Id) > 0)
                                {
                                    FilteredProducts.Add(product);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                        {
                            logger.LogError(ex);
                        }
                    }                    
                }
                else if (ProductSelectors.SectionInStockSelected)
                {
                    var filProd = Products.Where(p => p.Quantity > 0).ToList();
                    FilteredProducts = new ObservableCollection<Product>(filProd);
                }
                else if (ProductSelectors.SectionNotInStockSelected)
                {
                    var filProd = Products.Where(p => p.Quantity <= 0).ToList();
                    FilteredProducts = new ObservableCollection<Product>(filProd);
                }
                else if (ProductSelectors.SectionAllProductsSelected)
                {
                    FilteredProducts = Products;
                }
            }
        }

        public ICommand BackCommand => new RelayCommand(Back);
        public ICommand SaveReportCommand => new RelayCommand(SaveReport);
        public ICommand AddCommand => new RelayCommand(AddProduct);
        public ICommand EditCommand => new RelayCommand(EditProduct);

        private void Back(object parameter)
        {
            mainViewModel.NavigateBack();
        }

        private void SaveReport(object parameter)
        {
            /////////////////////////////////////
        }

        private void AddProduct(object parameter)
        {
            SupportWindow supportWindow = new SupportWindow(new AddEditProductViewModel(this, Categories));
            supportWindow.ShowDialog();
            InitializeAsync();
        }

        private void EditProduct(object parameter)
        {
            if (SelectedProduct != null)
            {
                SupportWindow supportWindow = new SupportWindow(new AddEditProductViewModel(this, SelectedProduct, Categories));
                supportWindow.ShowDialog();
                InitializeAsync();
            }
        }
    }
}
