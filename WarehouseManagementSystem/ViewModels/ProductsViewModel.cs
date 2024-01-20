using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Attributes;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Builders;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Helpers;
using WarehouseManagementSystem.ViewModels.Support_data;
using WarehouseManagementSystem.Windows;
using Zone = WarehouseManagementSystem.Models.Entities.Zone;
using WarehouseManagementSystem.Enums;

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

        private DateTime? manufactureDate;
        public DateTime? ManufactureDate
        {
            get { return manufactureDate; }
            set
            {
                if (manufactureDate != value)
                {
                    manufactureDate = value;
                    OnPropertyChanged(nameof(ManufactureDate));
                }
            }
        }

        private DateTime? expiryDate;
        public DateTime? ExpiryDate
        {
            get { return expiryDate; }
            set
            {
                if (expiryDate != value)
                {
                    expiryDate = value;
                    OnPropertyChanged(nameof(ExpiryDate));
                }
            }
        }

        private PermissionManager permissionManager;
        public PermissionManager PermissionManager
        {
            get { return permissionManager; }
            private set
            {
                if (permissionManager != value)
                {
                    permissionManager = value;
                }
            }
        }

        public ICommand BackCommand => new RelayCommand(Back);
        public ICommand SaveReportCommand => new RelayCommand(SaveReport, parameter => permissionManager.CanExecute(parameter,
            typeof(ProductsViewModel).GetMethod(nameof(SaveReport)) ?? throw new ArgumentNullException()));
        public ICommand AddCommand => new RelayCommand(AddProduct, parameter => permissionManager.CanExecute(parameter, 
            typeof(ProductsViewModel).GetMethod(nameof(AddProduct)) ?? throw new ArgumentNullException()));
        public ICommand EditCommand => new RelayCommand(EditProduct, parameter => permissionManager.CanExecute(parameter,
            typeof(ProductsViewModel).GetMethod(nameof(EditProduct)) ?? throw new ArgumentNullException()));
        public ICommand AllocateCommand => new RelayCommand(AllocateProduct, parameter => permissionManager.CanExecute(parameter,
            typeof(ProductsViewModel).GetMethod(nameof(AllocateProduct)) ?? throw new ArgumentNullException()));

        public ProductsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            if (MainViewModel.LoginService.CurrentUser == null) throw new ArgumentNullException();
            permissionManager = new PermissionManager(MainViewModel.LoginService.CurrentUser.Role);
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
                            var rootViewModel = await dbManager.BuildCategoryViewModelTreeAsync(rootCategory,
                                                                               mainViewModel.LoginService.CurrentWarehouse);
                            Categories.Add(rootViewModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await ExceptionHelper.HandleExceptionAsync(ex);
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
                await ExceptionHelper.HandleExceptionAsync(ex);
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
                    await ExceptionHelper.HandleExceptionAsync(ex);
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
                    await ExceptionHelper.HandleExceptionAsync(ex);
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
                    await ExceptionHelper.HandleExceptionAsync(ex);
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
                        ExceptionHelper.HandleException(ex);
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

        private void Back(object parameter)
        {
            mainViewModel.NavigateBack();
        }

        [AccessPermission(UserPermissionEnum.ManageAllData, 
                          UserPermissionEnum.CreateAllReports,
                          UserPermissionEnum.CreateSelfReports)]
        public void SaveReport(object parameter)
        {
            try
            {
                if (FilteredProducts == null
                || !FilteredProducts.Any())
                {
                    MessageHelper.ShowErrorMessage("No info to be saved");
                    return;
                }

                if (mainViewModel.LoginService.CurrentUser == null)
                {
                    throw new ArgumentNullException("Current program user is null");
                }

                string title = GenereteTitle();
                string content = GenereteContentToJson(FilteredProducts);

                SupportWindow supportWindow = new SupportWindow(new SaveReportViewModel(title,
                                                                                    Enums.ReportTypeEnum.PRODUCTS,
                                                                                    content,
                                                                                    mainViewModel.LoginService.CurrentUser.Id));
                supportWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                HandleSaveReportException(ex);
            }
        }

        private void HandleSaveReportException(Exception ex)
        {
            MessageHelper.ShowErrorMessage("Failed to save a report");
            ExceptionHelper.HandleException(ex);
        }

        private string GenereteTitle()
        {
            StringBuilder newTitle = new StringBuilder();
            newTitle.Append("Products/");

            if (CategoriesSelector.CheckboxAllCategoriesChecked)
            {
                newTitle.Append("All categories/");
            }
            else
            {
                newTitle.Append($"Category: {SelectedCategory}/");
            }

            if (ProductSelectors.SectionUnallocatedSelected)
            {
                newTitle.Append("Unallocated/");
            }
            else if (ProductSelectors.SectionInStockSelected)
            {
                newTitle.Append("In stock/");
            }
            else if (ProductSelectors.SectionNotInStockSelected)
            {
                newTitle.Append("Not in stock/");
            }
            else if (ProductSelectors.SectionAllProductsSelected)
            {
                newTitle.Append("All products/");
            }

            return newTitle.ToString();
        }

        private string GenereteContentToJson(ICollection<Product> products)
        {
            return JsonConvert.SerializeObject(products, Formatting.None);
        }

        [AccessPermission(UserPermissionEnum.ManageAllData,
                          UserPermissionEnum.AddProducts,
                          UserPermissionEnum.EditProducts)]
        public void AllocateProduct(object parameter)
        {
            if (SelectedProduct == null || SelectedZonePosition == null)
            {
                MessageHelper.ShowCautionMessage("Select product to allocate");
                return;
            }

            if (ConfirmationHelper.GetConfirmation() != MessageBoxResult.OK)
            {
                return;
            }

            try
            {
                if (!IsDataToAllocateCorrect())
                {
                    return;
                }

                int productToAllocateId = SelectedProduct.Id;
                int quantityToAllocate = Convert.ToInt32(InputQuantity);
                int zonePositionToAllocateId = SelectedZonePosition.Id;

                CreateProductInZonePosition(productToAllocateId, 
                                            quantityToAllocate, 
                                            zonePositionToAllocateId,
                                            manufactureDate,
                                            ExpiryDate);

                SelectedProduct = null;

                UpdateFilteredProducts();
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
        }

        private void CreateProductInZonePosition(int productId, int quantity, int zonePositionId, 
                                                DateTime? manufactureDate, DateTime? expiryDate)
        {
            var productInZonePositionBuilder = new ProductInZonePositionBuilder(productId,
                                                                    quantity,
                                                                    zonePositionId);

            if (manufactureDate != null)
            {
                productInZonePositionBuilder.WithManufactureDate((DateTime)manufactureDate);
            }
            if (expiryDate != null)
            {
                productInZonePositionBuilder.WithExpiryDate((DateTime)expiryDate);
            }
        }

        private bool IsDataToAllocateCorrect()
        {
            if (selectedZonePosition == null)
            {
                MessageHelper.ShowCautionMessage("Select zone position to allocate product");
                return false;
            }

            if (SelectedZonePositionFreeCapacity <= 0)
            {
                MessageHelper.ShowCautionMessage("No space in selected zone position");
                return false;
            }

            if (CapacityToBeAllocated <= 0)
            {
                MessageHelper.ShowCautionMessage("Enter quantity to allocate");
                return false;
            }

            if (CapacityToBeAllocated > SelectedZonePositionFreeCapacity)
            {
                MessageHelper.ShowCautionMessage("Not enough space in selected zone position");
                return false;
            }

            if (UnallocatedProductInstances?.UnallocatedCapacity < CapacityToBeAllocated)
            {
                MessageHelper.ShowCautionMessage("Not enough products to allocate");
                return false;
            }

            if (ManufactureDate != null && ManufactureDate > DateTime.Now)
            {
                MessageHelper.ShowCautionMessage("Incorrect manufacture date");
                return false;
            }

            if (ExpiryDate != null && ManufactureDate != null && ExpiryDate < ManufactureDate)
            {
                MessageHelper.ShowCautionMessage("Incorrect expiry date");
                return false;
            }

            return true;
        }

        [AccessPermission(UserPermissionEnum.ManageAllData, 
                          UserPermissionEnum.AddProducts)]
        public void AddProduct(object parameter)
        {
            SupportWindow supportWindow = new SupportWindow(new AddEditProductViewModel(this, Categories));
            supportWindow.ShowDialog();
            InitializeAsync();
        }

        [AccessPermission(UserPermissionEnum.ManageAllData,
                          UserPermissionEnum.EditProducts)]
        public void EditProduct(object parameter)
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
