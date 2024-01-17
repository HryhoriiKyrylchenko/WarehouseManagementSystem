using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Builders;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Models.Entities.Support_classes;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Helpers;
using WarehouseManagementSystem.Windows;
using static System.Formats.Asn1.AsnWriter;

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
                    ExceptionHelper.HandleException(ex);
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

        private CategoryViewModel? SetCategoryById(IEnumerable<CategoryViewModel> categories, int? categoryId)
        {
            if (categories.Any())
            {
                var category = categories.FirstOrDefault(c => c.Category.Id == categoryId);

                if (category != null)
                {
                    return category;
                }

                foreach (var child in categories)
                {
                    if (child.Children != null && child.Children.Any())
                    {
                        var result = SetCategoryById(child.Children, categoryId);
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
                ExceptionHelper.HandleException(ex);
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
                await ExceptionHelper.HandleExceptionAsync(ex);
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
            if (ConfirmationHelper.GetConfirmation() != MessageBoxResult.OK)
            {
                return;
            }

            var categoryToDelete = CurrentProductViewModel.SelectedCategory?.Category;
            if (!IsCategoryDeletionAvailable(categoryToDelete))
            {
                MessageHelper.ShowCautionMessage("This category cannot be deleted!");
                return;
            }

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
                ExceptionHelper.HandleException(ex);
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
                    ExceptionHelper.HandleException(ex);
                }
            }

            return false;
        }

        private void AddProductDetail(object obj)
        {
            if (!IsProductDetailDataValid())
            {
                MessageHelper.ShowCautionMessage("Detail Firstname and detail value fields are required");
                return;
            }

            if (ConfirmationHelper.GetConfirmation() == MessageBoxResult.OK)
            {
                InitializeProductDetails();
                AddNewProductDetail();
            }
        }

        private void AddNewProductDetail()
        {
            if (CurrentProductViewModel.NewProductDetailKey != null
                && CurrentProductViewModel.NewProductDetailValue != null
                && CurrentProductViewModel.ProductDetails != null)
            {
                string newKey = CurrentProductViewModel.NewProductDetailKey;
                string newValue = CurrentProductViewModel.NewProductDetailValue;
                CurrentProductViewModel.ProductDetails.Add(new ProductDetail(newKey, newValue));
            }
        }

        private void InitializeProductDetails()
        {
            if (CurrentProductViewModel.ProductDetails == null)
            {
                CurrentProductViewModel.ProductDetails = new ObservableCollection<ProductDetail>();
            }
        }

        private bool IsProductDetailDataValid()
        {
            if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.NewProductDetailKey)
                && !string.IsNullOrWhiteSpace(CurrentProductViewModel.NewProductDetailValue))
            {
                return true;
            }
            return false;
        }

        private void EditProductDetail(object obj)
        {
            if (!IsValidateSelectionAndProductDetails())
            {
                MessageHelper.ShowCautionMessage("Choose product detail to edit");
                return;
            }

            if (!IsProductDetailDataValid())
            {
                MessageHelper.ShowCautionMessage("Detail Firstname and detail value fields are required");
                return;
            }

            if (ConfirmationHelper.GetConfirmation() == MessageBoxResult.OK)
            {
                UpdateProductDetail();
            }
        }

        private bool IsValidateSelectionAndProductDetails()
        {
            if (CurrentProductViewModel.ProductDetails == null
                || CurrentProductViewModel.SelectedProductDetail == null)
            {
                return false;
            }
            return true;
        }

        private void UpdateProductDetail()
        {
            if (CurrentProductViewModel.NewProductDetailKey != null
                && CurrentProductViewModel.NewProductDetailValue != null
                && CurrentProductViewModel.SelectedProductDetail != null)
            {
                string newKey = CurrentProductViewModel.NewProductDetailKey;
                string newValue = CurrentProductViewModel.NewProductDetailValue;
                CurrentProductViewModel.SelectedProductDetail.Key = newKey;
                CurrentProductViewModel.SelectedProductDetail.Value = newValue;
                CurrentProductViewModel.RefreshSelectedProductDetail();
            }
        }

        private void DeleteProductDetail(object obj)
        {
            if (!IsValidateSelectionAndProductDetails())
            {
                MessageHelper.ShowCautionMessage("Choose product detail to delete");
                return;
            }
            if (ConfirmationHelper.GetConfirmation() == MessageBoxResult.OK)
            {
                RemoveSelectedProductDetail();
            }
        }

        private void RemoveSelectedProductDetail()
        {
            if (CurrentProductViewModel.ProductDetails != null
                && CurrentProductViewModel.SelectedProductDetail != null)
            {
                CurrentProductViewModel.ProductDetails.Remove(CurrentProductViewModel.SelectedProductDetail);
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
                InitializeProductPhotos();

                foreach (var filePath in openFileDialog.FileNames)
                {
                    try
                    {
                        byte[] photoData = File.ReadAllBytes(filePath);
                        AddPhotoToCollection(photoData);
                    }
                    catch (Exception ex)
                    {
                        HandlePhotoProcessingError(filePath, ex);
                    }
                }
            }
        }

        private void AddPhotoToCollection(byte[] photoData)
        {
            if (CurrentProductViewModel.ProductPhotos != null)
            {
                ProductPhoto productPhoto = new ProductPhoto(photoData);
                CurrentProductViewModel.ProductPhotos.Add(productPhoto);
            }
        }

        private void HandlePhotoProcessingError(string filePath, Exception ex)
        {
            MessageHelper.ShowErrorMessage($"Error processing file '{filePath}': {ex.Message}");
            ExceptionHelper.HandleException(ex);
        }

        private void InitializeProductPhotos()
        {
            if (CurrentProductViewModel.ProductPhotos == null)
            {
                CurrentProductViewModel.ProductPhotos = new ObservableCollection<ProductPhoto>();
            }
        }

        private void DeletePhoto(object obj)
        {
            if (!IsValidateSelectionAndProductPhotos())
            {
                MessageHelper.ShowCautionMessage("Choose product image to delete");
                return;
            }

            if (ConfirmationHelper.GetConfirmation() == MessageBoxResult.OK)
            {
                RemoveSelectedProductPhoto();
            }
        }

        private void RemoveSelectedProductPhoto()
        {
            if (CurrentProductViewModel.ProductPhotos != null
                && CurrentProductViewModel.SelectedProductPhoto != null)
            {
                CurrentProductViewModel.ProductPhotos.Remove(CurrentProductViewModel.SelectedProductPhoto);
            }
        }

        private bool IsValidateSelectionAndProductPhotos()
        {
            if (CurrentProductViewModel.ProductPhotos == null
                || CurrentProductViewModel.SelectedProductPhoto == null)
            {
                return false;
            }
            return true;
        }

        private void AddEditProduct(object obj)
        {
            if (!IsProductDataValid())
            {
                MessageHelper.ShowErrorMessage("Invalid product data, enter valid data");
                return;
            }

            if (ConfirmationHelper.GetConfirmation() != MessageBoxResult.OK)
            {
                return;
            }

            try
            {
                if (Product != null)
                {
                    EditExistingProduct();
                }
                else
                {
                    CreateNewProduct();
                }

                CloseParentWindow();
            }
            catch (Exception ex)
            {
                HandleProductException(ex);
            }
        }

        private bool ValidateProductData()
        {
            if (string.IsNullOrWhiteSpace(CurrentProductViewModel.ProductCode)
                || string.IsNullOrWhiteSpace(CurrentProductViewModel.Name)
                || CurrentProductViewModel.UnitOfMeasure == null
                || CurrentProductViewModel.Quantity == null
                || CurrentProductViewModel.Capacity == null
                || CurrentProductViewModel.Price == null
                || CurrentProductViewModel.SelectedCategory == null)
            {
                MessageHelper.ShowCautionMessage("Please fill in all required fields.");
                return false;
            }

            if ((Product != null 
                && Product.ProductCode != CurrentProductViewModel.ProductCode
                && !IsProductCodeAvailable(CurrentProductViewModel.ProductCode))
                || (Product == null
                && !IsProductCodeAvailable(CurrentProductViewModel.ProductCode)))
            {
                MessageHelper.ShowCautionMessage("Entered product code is already in use.");
                return false;
            }

            return true;
        }

        private void BuildNewProduct()
        {
            if (CurrentProductViewModel.ProductCode == null
                || CurrentProductViewModel.Name == null
                || CurrentProductViewModel.UnitOfMeasure == null
                || CurrentProductViewModel.Quantity == null
                || CurrentProductViewModel.Capacity == null
                || CurrentProductViewModel.Price == null
                || CurrentProductViewModel.SelectedCategory == null)
            {
                throw new ArgumentNullException("Required data to create new product is null");
            }

            using (var scope = new TransactionScope())
            {
                try
                {
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

                    if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.AdditionalInfo))
                    {
                        tempProduct = tempProduct.WithAdditionalInfo(CurrentProductViewModel.AdditionalInfo);
                    }

                    if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.AdditionalInfo))
                    {
                        tempProduct = tempProduct.WithAdditionalInfo(CurrentProductViewModel.AdditionalInfo);
                    }

                    if (CurrentProductViewModel.ProductDetails != null
                        && CurrentProductViewModel.ProductDetails.Any())
                    {
                        string productDetails = HandleProductDetails(CurrentProductViewModel.ProductDetails);
                        tempProduct = tempProduct.WithProductDetails(productDetails);
                    }

                    Product updatedProduct = tempProduct.Build();

                    HandleProductPhotos(updatedProduct);

                    scope.Complete();
                }
                catch
                {
                    scope.Dispose();
                    throw;
                }
            }
        }

        private string HandleProductDetails(IEnumerable<ProductDetail> productDetails)
        {
            return JsonConvert.SerializeObject(productDetails, Formatting.None);
        }

        private void HandleProductPhotos(Product tempProduct)
        {
            if (CurrentProductViewModel.ProductPhotos != null 
                && CurrentProductViewModel.ProductPhotos.Any())
            {
                tempProduct.ProductPhotos = CurrentProductViewModel.ProductPhotos;
                foreach (var photo in tempProduct.ProductPhotos)
                {
                    photo.ProductId = tempProduct.Id;
                }

                using (EntityManager db = new EntityManager(new Models.WarehouseDbContext()))
                {
                    db.UpdateProduct(tempProduct);
                }
            }
        }

        private void CreateNewProduct()
        {
            if (!ValidateProductData())
            {
                return;
            }

            try
            {
                BuildNewProduct();
            }
            catch
            {
                throw;
            } 
        }

        private void EditExistingProduct()
        {
            if (!ValidateProductData())
            {
                return;
            }

            try
            {
                if (Product != null)
                {
                    UpdateProduct(Product);
                }
            }
            catch
            {
                throw;
            }
        }

        private void UpdateProductCode(Product Product)
        {
            if (string.IsNullOrWhiteSpace(CurrentProductViewModel.ProductCode))
            {
                throw new ArgumentNullException("ProductCode is required");
            }

            if (Product.ProductCode != CurrentProductViewModel.ProductCode
                && !IsProductCodeAvailable(CurrentProductViewModel.ProductCode))
            {
                throw new ArgumentException("Entered product code is already in use.");
            }

            if (Product.ProductCode != CurrentProductViewModel.ProductCode)
            {
                Product.ProductCode = CurrentProductViewModel.ProductCode;
            }
        }

        private void UpdateProductName(Product Product)
        {
            if (string.IsNullOrWhiteSpace(CurrentProductViewModel.Name))
            {
                throw new ArgumentNullException("Name is required");
            }
            
            if (Product.Name != CurrentProductViewModel.Name)
            {
                Product.Name = CurrentProductViewModel.Name;
            }
        }

        private void UpdateUnitOfMeasure(Product Product)
        {
            if (CurrentProductViewModel.UnitOfMeasure == null)
            {
                throw new ArgumentNullException("Units of measure is required");
            }
            
            if (Product.UnitOfMeasure != CurrentProductViewModel.UnitOfMeasure)
            {
                Product.UnitOfMeasure = CurrentProductViewModel.UnitOfMeasure;
            }

        }

        private void UpdateQuantity(Product Product)
        {
            if (string.IsNullOrWhiteSpace(CurrentProductViewModel.QuantityString)
                || CurrentProductViewModel.Quantity == null)
            {
                throw new ArgumentNullException("Quantity is required");
            }
            if (Product.Quantity > CurrentProductViewModel.Quantity)
            {
                throw new ArgumentException("Impossible reduce a product quantity if " +
                                                 "it was previously entered into the database. To make the " +
                                                 "appropriate changes, contact your program administrator.");
            }
            if (Product.Quantity != CurrentProductViewModel.Quantity)
            {
                Product.Quantity = CurrentProductViewModel.Quantity;
            }
        }

        private void UpdateCapacity(Product Product)
        {
            if (string.IsNullOrWhiteSpace(CurrentProductViewModel.CapacityString)
                || CurrentProductViewModel.Capacity == null)
            {
                throw new ArgumentNullException("Capacity is required");
            }
            if (Product.Capacity != CurrentProductViewModel.Capacity)
            {
                Product.Capacity = CurrentProductViewModel.Capacity;
            }
        }

        private void UpdatePrice(Product Product)
        {
            if (string.IsNullOrWhiteSpace(CurrentProductViewModel.PriceString)
                ||CurrentProductViewModel.Price == null)
            {
                throw new ArgumentNullException("Price is required");
            }
            if (Product.Price != CurrentProductViewModel.Price)
            {
                Product.Price = (decimal)CurrentProductViewModel.Price;
            }
        }

        private void UpdateCategory(Product Product)
        {
            if (CurrentProductViewModel.SelectedCategory == null)
            {
                throw new ArgumentNullException("Category is required");
            }
            if (Product.CategoryId != CurrentProductViewModel.SelectedCategory.Category.Id)
            {
                Product.CategoryId = CurrentProductViewModel.SelectedCategory.Category.Id;
            }
            
        }

        private void UpdateDescription(Product Product)
        {
            if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.Description)
                && Product.Description != CurrentProductViewModel.Description)
            {
                Product.Description = CurrentProductViewModel.Description;
            }
        }

        private void UpdateManufacturer(Product Product)
        {
            if (CurrentProductViewModel.Manufacturer != null
                && Product.ManufacturerId != CurrentProductViewModel.Manufacturer.Id)
            {
                Product.ManufacturerId = CurrentProductViewModel.Manufacturer.Id;
            }
        }

        private void UpdateDiscountPercentage(Product Product)
        {
            if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.DiscountPercentageString)
                && CurrentProductViewModel.DiscountPercentage != null
                && Product.DiscountPercentage != CurrentProductViewModel.DiscountPercentage)
            {
                Product.DiscountPercentage = CurrentProductViewModel.DiscountPercentage;
            }
        }

        private void UpdateProductDetails(Product Product)
        {
            if (CurrentProductViewModel.ProductDetails != null
                && CurrentProductViewModel.ProductDetails.Any())
            {
                try
                {
                    Product.ProductDetails = JsonConvert.SerializeObject(CurrentProductViewModel.ProductDetails, Formatting.None);
                }
                catch
                {
                    throw;
                }
            }
        }

        private void UpdateProductPhotos(Product Product)
        {
            if (CurrentProductViewModel.ProductPhotos != null
                && CurrentProductViewModel.ProductPhotos.Any()
                && Product.ProductPhotos != CurrentProductViewModel.ProductPhotos)
            {
                Product.ProductPhotos = CurrentProductViewModel.ProductPhotos;
                foreach (var photo in Product.ProductPhotos)
                {
                    photo.ProductId = Product.Id;
                }
            }
        }

        private void UpdateAdditionalInfo(Product Product)
        {
            if (!string.IsNullOrWhiteSpace(CurrentProductViewModel.AdditionalInfo)
                && Product.AdditionalInfo != CurrentProductViewModel.AdditionalInfo)
            {
                Product.AdditionalInfo = CurrentProductViewModel.AdditionalInfo;
            }
        }

        private void SaveProductToDatabase(Product Product)
        {
            try
            {
                using (EntityManager db = new EntityManager(new WarehouseDbContext()))
                {
                    db.UpdateProduct(Product);
                }
            }
            catch 
            {
                throw;
            }
        }

        private void UpdateProduct(Product Product)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    UpdateProductCode(Product);
                    UpdateProductName(Product);
                    UpdateUnitOfMeasure(Product);
                    UpdateQuantity(Product);
                    UpdateCapacity(Product);
                    UpdatePrice(Product);
                    UpdateCategory(Product);
                    UpdateDescription(Product);
                    UpdateManufacturer(Product);
                    UpdateDiscountPercentage(Product);
                    UpdateProductDetails(Product);
                    UpdateProductPhotos(Product);
                    UpdateAdditionalInfo(Product);

                    SaveProductToDatabase(Product);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    MessageHelper.ShowErrorMessage(ex.Message);
                    throw;
                }
            }
        }

        private void HandleProductException(Exception ex)
        {
            MessageHelper.ShowErrorMessage("Failed to create or edit a product");
            ExceptionHelper.HandleException(ex);
        }

        private void Cancel(object obj)
        {
            if (ConfirmationHelper.GetCancelConfirmation() == MessageBoxResult.OK)
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
                ExceptionHelper.HandleException(ex);
                return false;
            }
        }
    }
}
