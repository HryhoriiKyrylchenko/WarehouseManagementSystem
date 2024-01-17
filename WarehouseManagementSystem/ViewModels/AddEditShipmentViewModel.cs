using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Builders;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Helpers;
using WarehouseManagementSystem.Windows;
using static System.Formats.Asn1.AsnWriter;

namespace WarehouseManagementSystem.ViewModels
{
    public class AddEditShipmentViewModel : ViewModelBaseRequestClose
    {
        private readonly ShipmentsViewModel mainViewModel;

        private Shipment? shipment;
        public Shipment? Shipment
        {
            get { return shipment; }
            set
            {
                if (shipment != value)
                {
                    this.shipment = value;
                    OnPropertyChanged(nameof(Shipment));
                };
            }
        }

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
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
                }
            }
        }

        private string? outputQuantity;
        public string? OutputQuantity
        {
            get { return outputQuantity; }
            set
            {
                if (outputQuantity != value)
                {
                    outputQuantity = value;
                    OnPropertyChanged(nameof(OutputQuantity));
                }
            }
        }

        private ObservableCollection<Customer> customers;
        public ObservableCollection<Customer> Customers
        {
            get { return customers; }
            set
            {
                if (customers != value)
                {
                    customers = value;
                    OnPropertyChanged(nameof(Customers));
                }
            }
        }

        private CurrentShipmentViewModel currentShipmentViewModel;
        public CurrentShipmentViewModel CurrentShipmentViewModel
        {
            get { return currentShipmentViewModel; }
            set
            {
                if (currentShipmentViewModel != value)
                {
                    currentShipmentViewModel = value;
                    OnPropertyChanged(nameof(CurrentShipmentViewModel));
                }
            }
        }

        private ShipmentItemViewModel? selectedShipmentItem;
        public ShipmentItemViewModel? SelectedShipmentItem
        {
            get { return selectedShipmentItem; }
            set
            {
                if (selectedShipmentItem != value)
                {
                    selectedShipmentItem = value;
                    OnPropertyChanged(nameof(SelectedShipmentItem));
                    if (value != null)
                    {
                        UpdatePropertiesData(value);
                    }
                }
            }
        }

        public ICommand AddCustomerCommand => new RelayCommand(AddCustomer);
        public ICommand AddShipmentItemCommand => new RelayCommand(AddShipmentItem);
        public ICommand EditShipmentItemCommand => new RelayCommand(EditShipmentItem);
        public ICommand DeleteShipmentItemCommand => new RelayCommand(DeleteShipmentItem);
        public ICommand OkCommand => new RelayCommand(AddEditShipment);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public AddEditShipmentViewModel(ShipmentsViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            currentShipmentViewModel = new CurrentShipmentViewModel();
            products = new ObservableCollection<Product>();
            customers = new ObservableCollection<Customer>();

            Initialize();
        }

        public AddEditShipmentViewModel(ShipmentsViewModel mainViewModel, Shipment shipment)
        {
            this.mainViewModel = mainViewModel;
            this.Shipment = shipment;
            currentShipmentViewModel = new CurrentShipmentViewModel();
            products = new ObservableCollection<Product>();
            customers = new ObservableCollection<Customer>();

            Initialize();
            SetSelectedCustomer(Shipment);
        }

        private void Initialize()
        {
            UpdateCustomers();
            InitializeProductsFromDB();
            CurrentShipmentViewModel = InitializeCurrentReceiptViewModel(this.Shipment);
        }

        private void SetSelectedCustomer(Shipment shipment)
        {
            if (shipment.Customer != null)
            {
                CurrentShipmentViewModel.Customer = Customers.Where(s => s.Id == shipment.Customer.Id).FirstOrDefault();
            }
        }

        public void UpdateCustomers()
        {
            Customers.Clear();
            InitializeCustomersFromDB();
        }

        private async void InitializeProductsFromDB()
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    Products = await db.GetProductsByWarehouseInStockAsync(mainViewModel.MainViewModel.LoginService.CurrentWarehouse.Id);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
        }

        private void InitializeCustomersFromDB()
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    Customers = db.GetCustomers();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
        }

        private CurrentShipmentViewModel InitializeCurrentReceiptViewModel(Shipment? shipment)
        {
            CurrentShipmentViewModel currentShipmentViewModel = new CurrentShipmentViewModel();
            if (shipment != null)
            {
                currentShipmentViewModel.ShipmentDate = shipment.ShipmentDate;
                currentShipmentViewModel.Customer = shipment.Customer;
                currentShipmentViewModel.BatchNumber = shipment.BatchNumber;
                currentShipmentViewModel.ShipmentNumber = shipment.ShipmentNumber;
                currentShipmentViewModel.AdditionalInfo = shipment.AdditionalInfo;
                if (shipment.ShipmentItems != null)
                {
                    foreach (var item in shipment.ShipmentItems)
                    {
                        currentShipmentViewModel.ShipmentItems.Add(new ShipmentItemViewModel(item.Id, item.Product, item.Quantity));
                    }
                }
            }

            return currentShipmentViewModel;
        }

        private void UpdatePropertiesData(ShipmentItemViewModel shipmentItem)
        {
            if (shipmentItem.Product != null)
            {
                SelectedProduct = Products.Where(p => p.Id == shipmentItem.Product.Id).FirstOrDefault();
            }
            OutputQuantity = shipmentItem.Quantity.ToString();
        }

        private void AddCustomer(object parameter)
        {
            SupportWindow supportWindow = new SupportWindow(new AddCustomerViewModel(this));
            supportWindow.ShowDialog();
            UpdateCustomers();
        }

        private bool TryParseQuantity(string output, out int quantity)
        {
            return int.TryParse(output, out quantity);
        }

        private void AddShipmentItem(object obj)
        {
            if (ConfirmationHelper.GetConfirmation() != MessageBoxResult.OK)
            {
                return;
            }

            if (SelectedProduct != null
                && !string.IsNullOrWhiteSpace(OutputQuantity)
                && TryParseQuantity(OutputQuantity, out int quantity)
                && quantity > 0)
            {
                AddShipmentItemToCollection(SelectedProduct, quantity);
            }
            else
            {
                MessageHelper.ShowCautionMessage("Product and quantity fields are required");
            }
        }

        private void AddShipmentItemToCollection(Product selectedProduct, int quantity)
        {
            InitializwShipmentItems();

            using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
            {
                var prod = db.GetProduct(selectedProduct.Id);
                if (prod != null
                    && prod.Quantity < quantity)
                {
                    MessageHelper.ShowCautionMessage("Not enough selected products in stock");
                    return;
                }
            }

            CurrentShipmentViewModel.ShipmentItems.Add(new ShipmentItemViewModel(selectedProduct, quantity));
        }

        private void InitializwShipmentItems()
        {
            if (CurrentShipmentViewModel.ShipmentItems == null)
            {
                CurrentShipmentViewModel.ShipmentItems = new ObservableCollection<ShipmentItemViewModel>();
            }
        }

        private void EditShipmentItem(object obj)
        {
            if (CurrentShipmentViewModel.ShipmentItems == null
                || SelectedShipmentItem == null)
            {
                MessageHelper.ShowCautionMessage("Choose shipment item to edit");
                return;
            }

            if (ConfirmationHelper.GetConfirmation() != MessageBoxResult.OK)
            {
                return;
            }

            if (SelectedProduct != null
            && !string.IsNullOrWhiteSpace(OutputQuantity)
            && TryParseQuantity(OutputQuantity, out int quantity)
            && quantity > 0)
            {
                try
                {
                    HandleEditShipmentItem(SelectedShipmentItem, SelectedProduct, quantity);
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowErrorMessage(ex.Message);
                    ExceptionHelper.HandleException(ex);
                }
            }
            else
            {
                MessageHelper.ShowCautionMessage("Product and quantity fields are required");
            }
        }

        private void HandleEditShipmentItem(ShipmentItemViewModel selectedShipmentItem, Product selectedProduct, int quantity)
        {
            if (selectedShipmentItem.Product != null)
            {
                try
                {
                    if (selectedShipmentItem.Product == selectedProduct)
                    {
                        if (selectedShipmentItem.Quantity < quantity)
                        {
                            HandleInsufficientStock(selectedProduct.Id, quantity);
                        }

                        selectedShipmentItem.Quantity = quantity;
                    }
                    else
                    {
                        HandleInsufficientStock(selectedProduct.Id, quantity);

                        selectedShipmentItem.Product = SelectedProduct;
                        selectedShipmentItem.Quantity = quantity;
                    }

                    CurrentShipmentViewModel.RefreshShipmentItems();
                }
                catch
                {
                    throw;
                }
            }
        }

        private void HandleInsufficientStock(int productId, int requiredQuantity)
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    var prod = db.GetProduct(productId);
                    if (prod != null && prod.Quantity < requiredQuantity)
                    {
                        throw new ArgumentException("Not enough selected products in stock");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void DeleteShipmentItem(object obj)
        {
            if (CurrentShipmentViewModel.ShipmentItems == null
                || SelectedShipmentItem == null)
            {
                MessageHelper.ShowCautionMessage("Choose shipment item to delete");
                return;
            }

            if (SelectedShipmentItem.Id != null
               && SelectedShipmentItem.Product != null)
            {
                MessageHelper.ShowCautionMessage("This data was already added to database, " +
                    "so quantity of a deleted product from the shipment will be " +
                    "added to unallocated product quantity in database.");
            }

            if (ConfirmationHelper.GetConfirmation() == MessageBoxResult.OK)
            {
                CurrentShipmentViewModel.ShipmentItems.Remove(SelectedShipmentItem);
            }
        }

        private void AddEditShipment(object obj)
        {
            if (!IsShipmentDataValid())
            {
                MessageHelper.ShowCautionMessage("Invalid shipment data, enter valid data");
                return;
            }

            if (ConfirmationHelper.GetConfirmation() != MessageBoxResult.OK)
            {
                return;
            }

            try
            {
                if (Shipment != null)
                {
                    EditExistingShipment();
                }
                else
                {
                    CreateNewShipment();
                }

                CloseParentWindow();
            }
            catch (Exception ex)
            {
                HandleShipmentException(ex);
            }
        }

        private bool ValidateShipmentData()
        {
            if (CurrentShipmentViewModel.ShipmentDate == null
                || CurrentShipmentViewModel.Customer == null
                || string.IsNullOrWhiteSpace(CurrentShipmentViewModel.BatchNumber)
                || mainViewModel.MainViewModel.LoginService.CurrentUser == null
                || CurrentShipmentViewModel.ShipmentItems.Count <= 0)
            {
                MessageHelper.ShowCautionMessage("Please fill in all required fields.");
                return false;
            }

            return true;
        }

        private void CreateNewShipment()
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (CurrentShipmentViewModel.ShipmentDate != null
                        && CurrentShipmentViewModel.Customer != null
                        && !string.IsNullOrWhiteSpace(CurrentShipmentViewModel.BatchNumber)
                        && mainViewModel.MainViewModel.LoginService.CurrentUser != null
                        && CurrentShipmentViewModel.ShipmentItems.Count > 0)
                    {
                        var tempShipment = new ShipmentBuilder((DateTime)CurrentShipmentViewModel.ShipmentDate,
                                                            CurrentShipmentViewModel.Customer.Id,
                                                            mainViewModel.MainViewModel.LoginService.CurrentUser.Id,
                                                            CurrentShipmentViewModel.BatchNumber);

                        if (!string.IsNullOrWhiteSpace(CurrentShipmentViewModel.ShipmentNumber))
                        {
                            tempShipment.WithShipmentNumber(CurrentShipmentViewModel.ShipmentNumber);
                        }

                        if (!string.IsNullOrWhiteSpace(CurrentShipmentViewModel.AdditionalInfo))
                        {
                            tempShipment.WithAdditionalInfo(CurrentShipmentViewModel.AdditionalInfo);
                        }

                        if (CurrentShipmentViewModel.ShipmentItems.Count > 0)
                        {
                            Shipment = tempShipment.Build();

                            using (WarehouseDBManager dbW = new WarehouseDBManager(new WarehouseDbContext()))
                            {
                                foreach (var item in CurrentShipmentViewModel.ShipmentItems)
                                {
                                    if (item.Product != null)
                                    {
                                        var prod = dbW.GetProduct(item.Product.Id);

                                        if (prod != null)
                                        {
                                            if (prod.Quantity < item.Quantity)
                                            {
                                                MessageBox.Show($"There is not enough quantity of product  {prod.Name}" +
                                                                $" in stock to ship. Change the quantity to the available " +
                                                                $"quantity ({prod.Quantity}).",
                                                                "Caution",
                                                                MessageBoxButton.OK,
                                                                MessageBoxImage.Error);

                                                return;
                                            }
                                            else
                                            {
                                                new ShipmentItemBuilder(Shipment.Id, item.Product.Id, item.Quantity);
                                                DeductProduct(prod.Id, item.Quantity);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    scope.Complete();
                    CloseParentWindow();
                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    MessageBox.Show("Some error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                    {
                        logger.LogError(ex);
                    }
                }
            }
        }

        private void EditExistingShipment()
        {
            if (!ValidateShipmentData())
            {
                return;
            }

            try
            {
                if (Shipment != null)
                {
                    UpdateShipment(Shipment);
                }
            }
            catch
            {
                throw;
            }
        }

        private void UpdateShipment(Shipment shipment)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    UpdateShipmentDate(shipment);
                    UpdateCustomer(shipment);
                    UpdateBatchNumber(shipment);
                    UpdateShipmentNumber(shipment);
                    UpdateAdditionalInfo(shipment);
                    UpdateShipmentItems(shipment);

                    SaveShipmentToDatabase(shipment);
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

        private void UpdateShipmentItems(Shipment shipment)
        {
            if (CurrentShipmentViewModel.ShipmentItems.Count <= 0)
            {
                throw new ArgumentNullException("At least one shipment item required");
            }

            using (EntityManager db = new EntityManager(new WarehouseDbContext()))
            using (WarehouseDBManager dbW = new WarehouseDBManager(new WarehouseDbContext()))
            {
                var newShipmentItems = CurrentShipmentViewModel.ShipmentItems.ToList();

                var itemsToDelete = shipment.ShipmentItems.Where(si => !newShipmentItems.Any(i => i.Id == si.Id)).ToList();

                ProcessDeletedShipmentItems(db, dbW, itemsToDelete);

                foreach (var item in newShipmentItems)
                {
                    ProcessNewOrUpdateShipmentItem(db, dbW, shipment, item);
                }
            }
        }

        private void ProcessNewOrUpdateShipmentItem(EntityManager db, WarehouseDBManager dbW, Shipment shipment, ShipmentItemViewModel item)
        {
            if (item.Product != null)
            {
                if (item.Id != null)
                {
                    var itemToUpdate = shipment.ShipmentItems.FirstOrDefault(x => x.Id == item.Id);

                    if (itemToUpdate != null)
                    {
                        UpdateExistingShipmentItem(db, dbW, itemToUpdate, item);
                    }
                }
                else
                {
                    AddNewShipmentItem(db, dbW, shipment, item);
                }
            }
        }

        private void AddNewShipmentItem(EntityManager db, WarehouseDBManager dbW, Shipment shipment, ShipmentItemViewModel item)
        {
            if (item.Product == null)
            {
                return;
            }

            var prod = dbW.GetProduct(item.Product.Id);

            if (prod != null)
            {
                HandleInsufficientStock(dbW, prod, item.Quantity);

                new ShipmentItemBuilder(shipment.Id, item.Product.Id, item.Quantity);
                DeductProduct(prod.Id, item.Quantity);
            }
        }

        private void HandleInsufficientStock(WarehouseDBManager dbW, Product product, int requiredQuantity)
        {
            if (product.Quantity < requiredQuantity)
            {
                throw new ArgumentException($"Not enough quantity of product {product.Name} in stock to ship. " +
                                            $"Change the quantity to the available quantity ({product.Quantity}).");
            }
        }

        private void UpdateExistingShipmentItem(EntityManager db, WarehouseDBManager dbW, ShipmentItem itemToUpdate, ShipmentItemViewModel newItem)
        {
            if (newItem.Product == null)
            {
                return;
            }

            var prod = dbW.GetProduct(newItem.Product.Id);

            if (prod != null)
            {
                if (itemToUpdate.ProductId == newItem.Product.Id)
                {
                    UpdateQuantityForSameProduct(db, dbW, itemToUpdate, newItem, prod);
                }
                else
                {
                    UpdateDifferentProduct(db, dbW, itemToUpdate, newItem);
                }
            }
        }

        private void UpdateDifferentProduct(EntityManager db, WarehouseDBManager dbW, ShipmentItem itemToUpdate, ShipmentItemViewModel newItem)
        {
            if (newItem.Product == null)
            {
                return;
            }

            var oldProd = dbW.GetProduct(itemToUpdate.ProductId);
            var newProd = dbW.GetProduct(newItem.Product.Id);

            if (oldProd != null
                && newProd != null)
            {
                if (newProd.Quantity < newItem.Quantity)
                {
                    HandleInsufficientStock(dbW, newProd, newItem.Quantity);
                }
                else
                {
                    AddQuantityToProduct(oldProd.Id, itemToUpdate.Quantity);
                    DeductProduct(newProd.Id, newItem.Quantity);
                    itemToUpdate.ProductId = newItem.Product.Id;
                    itemToUpdate.Quantity = newItem.Quantity;
                }

                db.UpdateShipmentItem(itemToUpdate);
            }
        }

        private void UpdateQuantityForSameProduct(EntityManager db, WarehouseDBManager dbW, ShipmentItem itemToUpdate, ShipmentItemViewModel newItem, Product prod)
        {
            if (itemToUpdate.Quantity < newItem.Quantity)
            {
                HandleInsufficientStock(dbW, prod, newItem.Quantity - itemToUpdate.Quantity);

                DeductProduct(prod.Id, (newItem.Quantity - itemToUpdate.Quantity));
            }
            else if (itemToUpdate.Quantity > newItem.Quantity)
            {
                AddQuantityToProduct(prod.Id, (itemToUpdate.Quantity - newItem.Quantity));
            }

            itemToUpdate.Quantity = newItem.Quantity;
            db.UpdateShipmentItem(itemToUpdate);
        }

        private void ProcessDeletedShipmentItems(EntityManager db, WarehouseDBManager dbW, List<ShipmentItem> itemsToDelete)
        {
            if (itemsToDelete.Count > 0)
            {
                foreach (var item in itemsToDelete)
                {
                    var productToUpdate = dbW.GetProduct(item.ProductId);
                    if (productToUpdate != null)
                    {
                        AddQuantityToProduct(productToUpdate.Id, item.Quantity);
                    }

                    db.DeleteShipmentItem(item);
                }

                MessageHelper.ShowCautionMessage($"Quantity of {itemsToDelete.Count} deleted products from the shipment " +
                    $"was added to the database as unallocated!");
            }
        }

        private void UpdateAdditionalInfo(Shipment shipment)
        {
            if (!string.IsNullOrWhiteSpace(CurrentShipmentViewModel.AdditionalInfo)
                && shipment.AdditionalInfo != CurrentShipmentViewModel.AdditionalInfo)
            {
                shipment.AdditionalInfo = CurrentShipmentViewModel.AdditionalInfo;
            }
        }

        private void UpdateShipmentNumber(Shipment shipment)
        {
            if (!string.IsNullOrWhiteSpace(CurrentShipmentViewModel.ShipmentNumber)
                && shipment.ShipmentNumber != CurrentShipmentViewModel.ShipmentNumber)
            {
                shipment.ShipmentNumber = CurrentShipmentViewModel.ShipmentNumber;
            }
        }

        private void UpdateBatchNumber(Shipment shipment)
        {
            if (string.IsNullOrWhiteSpace(CurrentShipmentViewModel.BatchNumber))
            {
                throw new ArgumentNullException("Batch number is required");
            }

            if (shipment.BatchNumber != CurrentShipmentViewModel.BatchNumber)
            {
                shipment.BatchNumber = CurrentShipmentViewModel.BatchNumber;
            }
        }

        private void UpdateCustomer(Shipment shipment)
        {
            if (CurrentShipmentViewModel.Customer == null)
            {
                throw new ArgumentNullException("Customer is required");
            }

            if (shipment.CustomerId != CurrentShipmentViewModel.Customer.Id)
            {
                shipment.CustomerId = CurrentShipmentViewModel.Customer.Id;
            }
        }

        private void UpdateShipmentDate(Shipment shipment)
        {
            if (CurrentShipmentViewModel.ShipmentDate == null)
            {
                throw new ArgumentNullException("Receipt date is required");
            }

            if (shipment.ShipmentDate != CurrentShipmentViewModel.ShipmentDate)
            {
                shipment.ShipmentDate = (DateTime)CurrentShipmentViewModel.ShipmentDate;
            }
        }

        private void SaveShipmentToDatabase(Shipment shipment)
        {
            try
            {
                using (EntityManager db = new EntityManager(new Models.WarehouseDbContext()))
                {
                    db.UpdateShipment(shipment);
                }
            }
            catch
            {
                throw;
            }
        }

        private void HandleShipmentException(Exception ex)
        {
            MessageHelper.ShowErrorMessage("Failed to create or edit a shipment");
            ExceptionHelper.HandleException(ex);
        }

        public void AddQuantityToProduct(int productId, int quantity)
        {
            using (WarehouseDBManager dbW = new WarehouseDBManager(new WarehouseDbContext()))
            using (EntityManager db = new EntityManager(new WarehouseDbContext()))
            {
                var product = dbW.GetProduct(productId);
                if (product != null)
                {
                    product.Quantity += quantity;
                    db.UpdateProduct(product);
                }
            }
        }

        public void DeductProduct(int productId, decimal quantity)
        {
            using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
            {
                decimal unallocatedQuantity = db.GetUnallocatedProductInstancesSum(productId);

                if (unallocatedQuantity >= quantity)
                {
                    DeductQuantityFromProduct(productId, quantity);
                }
                else
                {
                    DeductQuantityFromZonePositions(productId, quantity);
                    DeductQuantityFromProduct(productId, quantity);
                }
            }
        }

        private void DeductQuantityFromProduct(int productId, decimal quantity)
        {
            using (WarehouseDBManager dbW = new WarehouseDBManager(new WarehouseDbContext()))
            using (EntityManager db = new EntityManager(new WarehouseDbContext()))
            {
                var product = dbW.GetProduct(productId);
                if (product != null)
                {
                    product.Quantity -= quantity;
                    db.UpdateProduct(product);
                }
            }
        }

        private void DeductQuantityFromZonePositions(int productId, decimal quantity)
        {
            try
            {
                using (WarehouseDBManager dbW = new WarehouseDBManager(new WarehouseDbContext()))
                using (EntityManager db = new EntityManager(new WarehouseDbContext()))
                {
                    var positionsWithProduct = dbW.GetProductInZonePozitionsByProduct(productId);

                    foreach (var position in positionsWithProduct)
                    {
                        if (position.Quantity <= quantity)
                        {
                            quantity -= position.Quantity;

                            // Add logic to control shipment from position

                            db.DeleteProductInZonePosition(position);

                            if (quantity == 0) break;
                        }
                        else
                        {
                            position.Quantity -= (int)quantity;
                            db.UpdateProductInZonePosition(position);
                            break;
                        }
                    }
                }

            }
            catch
            {
                throw;
            }
        }

        private bool IsShipmentDataValid()
        {
            if (CurrentShipmentViewModel.ShipmentDate == null
                || CurrentShipmentViewModel.Customer == null
                || string.IsNullOrWhiteSpace(CurrentShipmentViewModel.BatchNumber)
                || CurrentShipmentViewModel.ShipmentItems.Count <= 0)
            {
                return false;
            }
            return true;
        }

        private void Cancel(object obj)
        {
            if (ConfirmationHelper.GetCancelConfirmation() == MessageBoxResult.OK)
            {
                CloseParentWindow();
            }
        }
    }
}
