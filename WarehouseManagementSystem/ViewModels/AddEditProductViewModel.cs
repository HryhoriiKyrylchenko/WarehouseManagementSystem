using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Migrations;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Builders;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Models.Entities.Support_classes;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.Windows;

namespace WarehouseManagementSystem.ViewModels
{
    public class AddEditProductViewModel : ViewModelBaseRequestClose
    {
        private readonly ProductsViewModel mainViewModel;

        public ProductsViewModel MainViewModel
        {
            get { return mainViewModel; }
        }

        private Product? product;

        public Product? Product
        {
            get { return product; }
            set
            {
                if (product != value)
                {
                    this.product = value;
                    OnPropertyChanged();
                };
            }
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

        private ObservableCollection<Manufacturer> manufacturers;

        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return manufacturers; }
            set
            {
                if (manufacturers != value)
                {
                    this.manufacturers = value;
                    OnPropertyChanged(nameof(Manufacturers));
                };
            }
        }

        private CurrentProductViewModel currentProductViewModel;

        public CurrentProductViewModel CurrentProductViewModel
        {
            get { return currentProductViewModel; }
            set
            {
                if (currentProductViewModel != value)
                {
                    this.currentProductViewModel = value;
                    OnPropertyChanged(nameof(CurrentProductViewModel));
                };
            }
        }

        public ICommand SetProductCodeCommand => new RelayCommand(SetProductCode);
        public ICommand AddManufacturerCommand => new RelayCommand(AddManufacturer);
        public ICommand AddCategoryCommand => new RelayCommand(AddProductCategory);
        public ICommand DeleteCategoryCommand => new RelayCommand(DeleteProductCategory);
        public ICommand AddProductDetailCommand => new RelayCommand(AddProductDetail);
        public ICommand EditProductDetailCommand => new RelayCommand(EditProductDetail);
        public ICommand DeleteProductDetailCommand => new RelayCommand(DeleteProductDetail);
        public ICommand AddPhotoCommand => new RelayCommand(AddPhoto);
        public ICommand DeletePhotoCommand => new RelayCommand(DeletePhoto);
        public ICommand OkCommand => new RelayCommand(AddEditProduct);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public AddEditProductViewModel(ProductsViewModel mainViewModel, ObservableCollection<CategoryViewModel> categories)
        {
            this.mainViewModel = mainViewModel;
            this.categories = categories;
            currentProductViewModel = new CurrentProductViewModel();
            manufacturers = new ObservableCollection<Manufacturer>();

            Initialize();
        }

        public AddEditProductViewModel(ProductsViewModel mainViewModel, Product product, ObservableCollection<CategoryViewModel> categories)
        {
            this.mainViewModel = mainViewModel;
            this.product = product;
            this.categories = categories;
            manufacturers = new ObservableCollection<Manufacturer>();
            currentProductViewModel = new CurrentProductViewModel();

            Initialize();
        }

        public void Initialize()
        {
            Manufacturers?.Clear();
            InitializeManufacturersFromDB();
            currentProductViewModel = InitializeCurrentProductViewModel(this.product);
        }

        private CurrentProductViewModel InitializeCurrentProductViewModel(Product? product)
        {
            CurrentProductViewModel currentProductViewModel = new CurrentProductViewModel();
            if (product != null)
            {
                currentProductViewModel.ProductCode = product.ProductCode;
                currentProductViewModel.Name = product.Name;
                currentProductViewModel.Description = product.Description;
                currentProductViewModel.UnitOfMeasure = product.UnitOfMeasure;
                currentProductViewModel.QuantityString = product.Quantity.ToString();
                currentProductViewModel.Quantity = product.Quantity;
                currentProductViewModel.CapacityString = product.Capacity.ToString();
                currentProductViewModel.Capacity = product.Capacity;
                currentProductViewModel.PriceString = product.Price.ToString();
                currentProductViewModel.Price = product.Price;
                currentProductViewModel.DiscountPercentageString = product.DiscountPercentage.ToString();
                currentProductViewModel.DiscountPercentage = product.DiscountPercentage;
                currentProductViewModel.ProductDetails = (product.ProductDetails == null || string.IsNullOrEmpty(product.ProductDetails))
                                                        ? new ObservableCollection<ProductDetail>()
                                                        : JsonConvert.DeserializeObject<ObservableCollection<ProductDetail>>(product.ProductDetails);
                currentProductViewModel.AdditionalInfo = product.AdditionalInfo;
                currentProductViewModel.ProductPhotos = (product.ProductPhotos == null)
                                                        ? new ObservableCollection<ProductPhoto>()
                                                        : new ObservableCollection<ProductPhoto>(product.ProductPhotos);
                try
                {
                    currentProductViewModel.SelectedCategory = SetCategoryById(Categories, product.CategoryId);
                    currentProductViewModel.Manufacturer = SetManufacturerById(Manufacturers, product.ManufacturerId);
                }
                catch (Exception ex)
                {
                    using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                    {
                        logger.LogError(ex);
                    }
                }
            }

            return currentProductViewModel;
        }

        private Manufacturer? SetManufacturerById(ObservableCollection<Manufacturer>? manufacturers, int? manufacturerId)
        {
            if (manufacturers != null && manufacturerId != null)
            {
                foreach (var manufacturer in manufacturers)
                {
                    if (manufacturer != null && manufacturer.Id == manufacturerId)
                    {
                        return manufacturer;
                    }
                }
            }

            return null;
        }

        private CategoryViewModel? SetCategoryById(ObservableCollection<CategoryViewModel> categories, int? categoryId)
        {
            if (categories != null && categories.Any())
            {
                foreach (var category in categories)
                {
                    if (category.Category != null && category.Category.Id == categoryId)
                    {
                        return category;
                    }

                    if(category.Children != null && category.Children.Any())
                    {
                        var result = SetCategoryById(category.Children, categoryId);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }

            return null;
        }

        private void InitializeManufacturersFromDB()
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    Manufacturers = db.GetManufacturers();
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

        private async Task InitializeManufacturersFromDBAsync()
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    Manufacturers = await db.GetManufacturersAsync();
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

        private void SetProductCode(object parameter)
        {
            CurrentProductViewModel.ProductCode = GenerateRandomProductCode();
        }

        private string GenerateRandomProductCode()
        {
            Random random = new Random();
            const int codeLength = 12;

            string productCode = string.Concat(Enumerable.Range(0, codeLength)
                .Select(_ => random.Next(10).ToString()));

            return productCode;
        }

        private void AddManufacturer(object parameter)
        {
            SupportWindow supportWindow = new SupportWindow(new AddManufacturerViewModel(this));
            supportWindow.ShowDialog();
            Initialize();
        }

        private void AddProductCategory(object parameter)
        {
            SupportWindow supportWindow = new SupportWindow(new AddProductCategoryViewModel(this));
            supportWindow.ShowDialog();

            mainViewModel.InitializeAsync();
            Categories = mainViewModel.Categories;
        }

        private void DeleteProductCategory(object parameter)
        {
            if (GetConfirmation() == MessageBoxResult.OK)
            {
                var categoryToDelete = CurrentProductViewModel.SelectedCategory?.Category;
                if (IsCategoryDeletionAvailable(categoryToDelete))
                {
                    try
                    {
                        if (categoryToDelete != null)
                        {
                            using (EntityManager db = new EntityManager(new WarehouseDbContext()))
                            {
                                db.DeleteProductCategory(categoryToDelete);
                            }
                            mainViewModel.InitializeAsync();
                            Categories = mainViewModel.Categories;
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
                else
                {
                    MessageBox.Show("This category cannot be deleted?",
                        "Caution",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }
        }

        private bool IsCategoryDeletionAvailable(ProductCategory? category)
        {
            if (category != null)
            {
                try
                {
                    using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                    {
                        if (!db.DoesCategoryHaveChildrenCategories(category)
                            && !db.DoesCategoryHaveProducts(category))
                        {
                            return true;
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

            return false;
        }

        private MessageBoxResult GetConfirmation()
        {
            return MessageBox.Show("Do you want to make this changes?",
                "Confirmation",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
        }

        private MessageBoxResult GetCancelConfirmation()
        {
            return MessageBox.Show("All unsaved data will be lost! Continue?",
                "Confirmation",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
        }

        private void AddProductDetail(object obj)
        {
            if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.NewProductDetailKey)
                && !string.IsNullOrWhiteSpace(CurrentProductViewModel.NewProductDetailValue)
                && GetConfirmation() == MessageBoxResult.OK)
            {
                if (CurrentProductViewModel.ProductDetails == null)
                {
                    CurrentProductViewModel.ProductDetails = new ObservableCollection<ProductDetail>();
                }
                string newKey = CurrentProductViewModel.NewProductDetailKey;
                string newValue = CurrentProductViewModel.NewProductDetailValue;
                CurrentProductViewModel.ProductDetails.Add(new ProductDetail(newKey, newValue));
            }
            else
            {
                MessageBox.Show("Detail name and detail value fields are required",
                        "Caution",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
            }
        }
        private void EditProductDetail(object obj)
        {
            if (CurrentProductViewModel.ProductDetails != null
                && CurrentProductViewModel.SelectedProductDetail != null)
            {
                if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.NewProductDetailKey)
                && !string.IsNullOrWhiteSpace(CurrentProductViewModel.NewProductDetailValue)
                && GetConfirmation() == MessageBoxResult.OK)
                {
                    string newKey = CurrentProductViewModel.NewProductDetailKey;
                    string newValue = CurrentProductViewModel.NewProductDetailValue;
                    CurrentProductViewModel.SelectedProductDetail.Key = newKey;
                    CurrentProductViewModel.SelectedProductDetail.Value = newValue;
                    CurrentProductViewModel.RefreshSelectedProductDetail();
                }
                else
                {
                    MessageBox.Show("Detail name and detail value fields are required",
                            "Caution",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Choose product detail to edit",
                        "Caution",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
            }
        }
        private void DeleteProductDetail(object obj)
        {
            if (CurrentProductViewModel.ProductDetails != null
                && CurrentProductViewModel.SelectedProductDetail != null)
            {
                if (GetConfirmation() == MessageBoxResult.OK)
                {
                    CurrentProductViewModel.ProductDetails.Remove(CurrentProductViewModel.SelectedProductDetail);
                }
            }
        }

        private void AddPhoto(object obj)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (CurrentProductViewModel.ProductPhotos == null)
                {
                    CurrentProductViewModel.ProductPhotos = new ObservableCollection<ProductPhoto>();
                }
                foreach (var filePath in openFileDialog.FileNames)
                {
                    try
                    {
                        byte[] photoData = File.ReadAllBytes(filePath);

                        ProductPhoto productPhoto = new ProductPhoto(photoData);

                        CurrentProductViewModel.ProductPhotos.Add(productPhoto);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error processing file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        using (ErrorLogger db = new ErrorLogger(new WarehouseDbContext()))
                        {
                            db.LogError(ex);
                        }
                    }
                }
            }
        }

        private void DeletePhoto(object obj)
        {
            if (CurrentProductViewModel.ProductPhotos != null
                && CurrentProductViewModel.SelectedProductPhoto != null)
            {
                if (GetConfirmation() == MessageBoxResult.OK)
                {
                    CurrentProductViewModel.ProductPhotos.Remove(CurrentProductViewModel.SelectedProductPhoto);
                }
            }
        }
        private void AddEditProduct(object obj)
        {
            if (IsProductDataValid())
            {
                if (Product != null)
                {
                    try
                    {
                        if (CurrentProductViewModel.ProductCode != null
                        && Product.ProductCode != CurrentProductViewModel.ProductCode)
                        {
                            if (!IsProductCodeAvailable(CurrentProductViewModel.ProductCode))
                            {
                                MessageBox.Show("Entered product code is already in use.",
                                "Caution",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                                return;
                            }

                            Product.ProductCode = CurrentProductViewModel.ProductCode;
                        }
                        else if (string.IsNullOrWhiteSpace(CurrentProductViewModel.ProductCode))
                        {
                            MessageBox.Show("ProductCode is required", 
                                "Caution", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
                            return;
                        }

                        if (CurrentProductViewModel.Name != null
                            && Product.Name != CurrentProductViewModel.Name)
                        {
                            Product.Name = CurrentProductViewModel.Name;
                        }
                        else if (string.IsNullOrWhiteSpace(CurrentProductViewModel.Name))
                        {
                            MessageBox.Show("Name is required",
                                "Caution",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }

                        if (CurrentProductViewModel.UnitOfMeasure != null
                            && Product.UnitOfMeasure != CurrentProductViewModel.UnitOfMeasure)
                        {
                            Product.UnitOfMeasure = CurrentProductViewModel.UnitOfMeasure;
                        }
                        else if (CurrentProductViewModel.UnitOfMeasure == null)
                        {
                            MessageBox.Show("Units of measure is required",
                                "Caution",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }

                        if (CurrentProductViewModel.Quantity != null
                            && Product.Quantity != CurrentProductViewModel.Quantity)
                        {
                            Product.Quantity = CurrentProductViewModel.Quantity;
                        }
                        else if (string.IsNullOrWhiteSpace(CurrentProductViewModel.QuantityString))
                        {
                            MessageBox.Show("Quantity is required",
                                "Caution",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }

                        if (CurrentProductViewModel.Capacity != null
                            && Product.Capacity != CurrentProductViewModel.Capacity)
                        {
                            Product.Capacity = CurrentProductViewModel.Capacity;
                        }
                        else if (string.IsNullOrWhiteSpace(CurrentProductViewModel.CapacityString))
                        {
                            MessageBox.Show("Capacity is required",
                                "Caution",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }

                        if (CurrentProductViewModel.Price != null
                            && Product.Price != CurrentProductViewModel.Price)
                        {
                            Product.Price = (decimal)CurrentProductViewModel.Price;
                        }
                        else if (string.IsNullOrWhiteSpace(CurrentProductViewModel.PriceString))
                        {
                            MessageBox.Show("Price is required",
                                "Caution",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }

                        if (CurrentProductViewModel.SelectedCategory != null
                            && Product.CategoryId != CurrentProductViewModel.SelectedCategory.Category.Id)
                        {
                            Product.CategoryId = CurrentProductViewModel.SelectedCategory.Category.Id;
                        }
                        else if (CurrentProductViewModel.SelectedCategory == null)
                        {
                            MessageBox.Show("Category is required",
                                "Caution",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }

                        if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.Description)
                            && Product.Description != CurrentProductViewModel.Description)
                        {
                            Product.Description = CurrentProductViewModel.Description;
                        }

                        if (CurrentProductViewModel.Manufacturer != null
                            && Product.ManufacturerId != CurrentProductViewModel.Manufacturer.Id)
                        {
                            Product.ManufacturerId = CurrentProductViewModel.Manufacturer.Id;
                        }

                        if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.DiscountPercentageString)
                            && CurrentProductViewModel.DiscountPercentage != null
                            && Product.DiscountPercentage != CurrentProductViewModel.DiscountPercentage)
                        {
                            Product.DiscountPercentage = CurrentProductViewModel.DiscountPercentage;
                        }

                        if (CurrentProductViewModel.ProductDetails != null
                            && CurrentProductViewModel.ProductDetails.Any())
                        {
                            Product.ProductDetails = JsonConvert.SerializeObject(CurrentProductViewModel.ProductDetails, Formatting.Indented);
                        }

                        if (CurrentProductViewModel.ProductPhotos != null
                            && CurrentProductViewModel.ProductPhotos.Any()
                            && Product.ProductPhotos != CurrentProductViewModel.ProductPhotos)
                        {
                            Product.ProductPhotos = CurrentProductViewModel.ProductPhotos;
                            foreach(var photo in Product.ProductPhotos)
                            {
                                photo.ProductId = Product.Id;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.AdditionalInfo)
                            && Product.AdditionalInfo != CurrentProductViewModel.AdditionalInfo)
                        {
                            Product.AdditionalInfo = CurrentProductViewModel.AdditionalInfo;
                        }

                        using (EntityManager db = new EntityManager(new Models.WarehouseDbContext()))
                        {
                            db.UpdateProduct(Product);
                        }

                        CloseParentWindow();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Some error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        Task.Run(async () =>
                        {
                            using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                            {
                                await logger.LogErrorAsync(ex);
                            }
                        }).Wait();
                    }
                }
                else
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.ProductCode)
                            && !string.IsNullOrWhiteSpace(CurrentProductViewModel.Name)
                            && CurrentProductViewModel.UnitOfMeasure != null
                            && CurrentProductViewModel.Quantity != null
                            && CurrentProductViewModel.Capacity != null
                            && CurrentProductViewModel.Price != null
                            && CurrentProductViewModel.SelectedCategory != null)
                        {
                            if (!IsProductCodeAvailable(CurrentProductViewModel.ProductCode))
                            {
                                MessageBox.Show("Entered product code is already in use.",
                                "Caution",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                                return;
                            }

                            var tempProduct = new ProductBuilder(CurrentProductViewModel.ProductCode,
                                CurrentProductViewModel.Name,
                                (UnitsOfMeasureEnum)CurrentProductViewModel.UnitOfMeasure,
                                (decimal)CurrentProductViewModel.Quantity,
                                (int)CurrentProductViewModel.Capacity,
                                (decimal)CurrentProductViewModel.Price,
                                mainViewModel.MainViewModel.LoginService.CurrentWarehouse.Id)
                                .WithCategory(CurrentProductViewModel.SelectedCategory.Category.Id);


                            if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.Description))
                            {
                                tempProduct = tempProduct.WithDescription(CurrentProductViewModel.Description);
                            }

                            if (CurrentProductViewModel.Manufacturer != null)
                            {
                                tempProduct = tempProduct.WithManufacturer(CurrentProductViewModel.Manufacturer);
                            }

                            if (CurrentProductViewModel.DiscountPercentage != null)
                            {
                                tempProduct = tempProduct.WithDiscountPercentage(CurrentProductViewModel.DiscountPercentage);
                            }

                            if (CurrentProductViewModel.ProductDetails != null
                                && CurrentProductViewModel.ProductDetails.Any())
                            {
                                tempProduct = tempProduct.WithProductDetails(JsonConvert.SerializeObject(CurrentProductViewModel.ProductDetails, Formatting.Indented));
                            }

                            if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.AdditionalInfo))
                            {
                                tempProduct = tempProduct.WithAdditionalInfo(CurrentProductViewModel.AdditionalInfo);
                            }

                            if (CurrentProductViewModel.ProductPhotos != null
                                && CurrentProductViewModel.ProductPhotos.Any())
                            {
                                var newProduct = tempProduct.Build();
                                newProduct.ProductPhotos = CurrentProductViewModel.ProductPhotos;
                                foreach (var photo in newProduct.ProductPhotos)
                                {
                                    photo.ProductId = newProduct.Id;
                                }

                                using (EntityManager db = new EntityManager(new Models.WarehouseDbContext()))
                                {
                                    db.UpdateProduct(newProduct);
                                }
                            }

                            mainViewModel.InitializeAsync();
                            CloseParentWindow();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Some error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        Task.Run(async () =>
                        {
                            using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                            {
                                await logger.LogErrorAsync(ex);
                            }
                        }).Wait();
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid product data, enter valid data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object obj)
        {
            if (GetCancelConfirmation() == MessageBoxResult.OK)
            {
                CloseParentWindow();
            }
        }

        private bool IsProductDataValid()
        {
            if (string.IsNullOrWhiteSpace(CurrentProductViewModel.ProductCode)
                || string.IsNullOrWhiteSpace(CurrentProductViewModel.Name)
                || CurrentProductViewModel.UnitOfMeasure == null
                || string.IsNullOrWhiteSpace(CurrentProductViewModel.QuantityString)
                || string.IsNullOrWhiteSpace(CurrentProductViewModel.CapacityString)
                || string.IsNullOrWhiteSpace(CurrentProductViewModel.PriceString)
                || CurrentProductViewModel.SelectedCategory == null)
            {
                return false;
            }
            return true;
        }

        private bool IsProductCodeAvailable(string productCode)
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    return !db.IsProductCodeInDB(productCode);
                }
            }
            catch (Exception ex) 
            {
                Task.Run(async () =>
                {
                    using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                    {
                        await logger.LogErrorAsync(ex);
                    }
                }).Wait();
                return false;
            }
        }
    }
}
