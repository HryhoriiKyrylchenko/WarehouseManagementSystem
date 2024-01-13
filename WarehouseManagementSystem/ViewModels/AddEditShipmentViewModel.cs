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
            if (Shipment.Customer != null)
            {
                CurrentShipmentViewModel.Customer = Customers.Where(s => s.Id == Shipment.Customer.Id).FirstOrDefault();
            }
        }

        private void Initialize()
        {
            UpdateCustomers();
            InitializeProductsFromDB();
            CurrentShipmentViewModel = InitializeCurrentReceiptViewModel(this.Shipment);
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
                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    logger.LogError(ex);
                }
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
                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    logger.LogError(ex);
                }
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

        private void AddShipmentItem(object obj)
        {
            if (SelectedProduct != null
                && !string.IsNullOrWhiteSpace(OutputQuantity)
                && Convert.ToInt32(OutputQuantity) > 0
                && GetConfirmation() == MessageBoxResult.OK)
            {
                if (CurrentShipmentViewModel.ShipmentItems == null)
                {
                    CurrentShipmentViewModel.ShipmentItems = new ObservableCollection<ShipmentItemViewModel>();
                }

                using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    var prod = db.GetProduct(SelectedProduct.Id);
                    if (prod != null
                        && prod.Quantity < (Convert.ToInt32(OutputQuantity)))
                    {
                        MessageBox.Show("Not enough selected products in stock",
                                    "Caution",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation);

                        return;
                    }
                }

                CurrentShipmentViewModel.ShipmentItems.Add(new ShipmentItemViewModel(SelectedProduct, Convert.ToInt32(OutputQuantity)));
            }
            else
            {
                MessageBox.Show("Product and quantity fields are required",
                        "Caution",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
            }
        }

        private void EditShipmentItem(object obj)
        {
            if (CurrentShipmentViewModel.ShipmentItems != null
                && SelectedShipmentItem != null)
            {
                if (SelectedProduct != null
                && !string.IsNullOrWhiteSpace(OutputQuantity)
                && Convert.ToInt32(OutputQuantity) > 0)
                {
                    if (GetConfirmation() == MessageBoxResult.OK)
                    {
                        try
                        {
                            if (SelectedShipmentItem.Product != null)
                            {
                                if (SelectedShipmentItem.Product == SelectedProduct)
                                {
                                    if (SelectedShipmentItem.Quantity < Convert.ToInt32(OutputQuantity))
                                    {
                                        using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                                        {
                                            var prod = db.GetProduct(SelectedShipmentItem.Product.Id);
                                            if (prod != null
                                                && prod.Quantity < (Convert.ToInt32(OutputQuantity) - SelectedShipmentItem.Quantity))
                                            {
                                                MessageBox.Show("Not enough selected products in stock",
                                                            "Caution",
                                                            MessageBoxButton.OK,
                                                            MessageBoxImage.Exclamation);

                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        SelectedShipmentItem.Quantity = Convert.ToInt32(OutputQuantity);
                                        CurrentShipmentViewModel.RefreshShipmentItems();
                                    }
                                }
                                else
                                {
                                    using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                                    {
                                        var newProd = db.GetProduct(SelectedProduct.Id);
                                        if (newProd != null
                                            && newProd.Quantity < (Convert.ToInt32(OutputQuantity)))
                                        {
                                            MessageBox.Show("Not enough selected products in stock",
                                                        "Caution",
                                                        MessageBoxButton.OK,
                                                        MessageBoxImage.Exclamation);

                                            return;
                                        }
                                    }

                                    SelectedShipmentItem.Product = SelectedProduct;
                                    SelectedShipmentItem.Quantity = Convert.ToInt32(OutputQuantity);
                                    CurrentShipmentViewModel.RefreshShipmentItems();
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
                }
                else
                {
                    MessageBox.Show("Product and quantity fields are required",
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

        private void DeleteShipmentItem(object obj)
        {
            if (CurrentShipmentViewModel.ShipmentItems != null
                && SelectedShipmentItem != null)
            {
                if (GetConfirmation() == MessageBoxResult.OK)
                {
                    if (SelectedShipmentItem.Id != null
                        && SelectedShipmentItem.Product != null)
                    {
                        MessageBox.Show("This data was already added to database, " +
                            "so quantity of deleted product from shipment will be " +
                            "added to unallocated product quantity in database.",
                            "Caution",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                    }

                    CurrentShipmentViewModel.ShipmentItems.Remove(SelectedShipmentItem);
                }
            }
        }

        private void AddEditShipment(object obj)
        {
            if (IsShipmentDataValid())
            {
                if (GetConfirmation() == MessageBoxResult.OK)
                {
                    if (Shipment != null)
                    {
                        using (var scope = new TransactionScope())
                        {
                            try
                            {
                                if (CurrentShipmentViewModel.ShipmentDate != null
                                && Shipment.ShipmentDate != CurrentShipmentViewModel.ShipmentDate)
                                {
                                    Shipment.ShipmentDate = (DateTime)CurrentShipmentViewModel.ShipmentDate;
                                }
                                else if (CurrentShipmentViewModel.ShipmentDate == null)
                                {
                                    MessageBox.Show("Receipt date is required",
                                            "Caution",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                                    return;
                                }

                                if (CurrentShipmentViewModel.Customer != null
                                    && Shipment.CustomerId != CurrentShipmentViewModel.Customer.Id)
                                {
                                    Shipment.CustomerId = CurrentShipmentViewModel.Customer.Id;
                                }
                                else if (CurrentShipmentViewModel.Customer == null)
                                {
                                    MessageBox.Show("Customer is required",
                                            "Caution",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                                    return;
                                }

                                if (CurrentShipmentViewModel.BatchNumber != null
                                        && Shipment.BatchNumber != CurrentShipmentViewModel.BatchNumber)
                                {
                                    Shipment.BatchNumber = CurrentShipmentViewModel.BatchNumber;
                                }
                                else if (string.IsNullOrWhiteSpace(CurrentShipmentViewModel.BatchNumber))
                                {
                                    MessageBox.Show("Batch number is required",
                                        "Caution",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                                    return;
                                }

                                if (!string.IsNullOrWhiteSpace(CurrentShipmentViewModel.ShipmentNumber)
                                        && Shipment.ShipmentNumber != CurrentShipmentViewModel.ShipmentNumber)
                                {
                                    Shipment.ShipmentNumber = CurrentShipmentViewModel.ShipmentNumber;
                                }

                                if (!string.IsNullOrWhiteSpace(CurrentShipmentViewModel.AdditionalInfo)
                                        && Shipment.AdditionalInfo != CurrentShipmentViewModel.AdditionalInfo)
                                {
                                    Shipment.AdditionalInfo = CurrentShipmentViewModel.AdditionalInfo;
                                }

                                if (CurrentShipmentViewModel.ShipmentItems.Count > 0)
                                {
                                    using (EntityManager db = new EntityManager(new WarehouseDbContext()))
                                    using (WarehouseDBManager dbW = new WarehouseDBManager(new WarehouseDbContext()))
                                    {
                                        var newShipmentItems = CurrentShipmentViewModel.ShipmentItems.ToList();

                                        var itemsToDelete = Shipment.ShipmentItems
                                                                    .Where(si => !newShipmentItems.Any(i => i.Id == si.Id)).ToList();

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

                                            MessageBox.Show($"Quantity of {itemsToDelete.Count} deleted products from shipment" +
                                                $"was added to database as unallocated!",
                                                "Caution",
                                                MessageBoxButton.OK,
                                                MessageBoxImage.Error);
                                        }

                                        foreach (var item in newShipmentItems)
                                        {
                                            if (item.Product != null)
                                            {
                                                if (item.Id != null)
                                                {
                                                    var itemToUpdate = Shipment.ShipmentItems.FirstOrDefault(x => x.Id == item.Id);

                                                    if (itemToUpdate != null)
                                                    {
                                                        if (itemToUpdate.ProductId == item.Product.Id)
                                                        {
                                                            var prod = dbW.GetProduct(item.Product.Id);

                                                            if (prod != null)
                                                            {
                                                                if (itemToUpdate.Quantity < item.Quantity)
                                                                {
                                                                    if (prod.Quantity < (item.Quantity - itemToUpdate.Quantity))
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
                                                                        DeductProduct(prod.Id, (item.Quantity - itemToUpdate.Quantity));
                                                                    }
                                                                }
                                                                else if (itemToUpdate.Quantity > item.Quantity)
                                                                {
                                                                    AddQuantityToProduct(prod.Id, (itemToUpdate.Quantity - item.Quantity));
                                                                }

                                                                itemToUpdate.Quantity = item.Quantity;
                                                                db.UpdateShipmentItem(itemToUpdate);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            var oldProd = dbW.GetProduct(itemToUpdate.ProductId);
                                                            var newProd = dbW.GetProduct(item.Product.Id);

                                                            if (oldProd != null
                                                                && newProd != null)
                                                            {
                                                                if (newProd.Quantity < item.Quantity)
                                                                {
                                                                    MessageBox.Show($"There is not enough quantity of product  " +
                                                                        $"{newProd.Name} in stock to ship. Change the quantity to the " +
                                                                        $"available quantity ({newProd.Quantity}).",
                                                                        "Caution",
                                                                        MessageBoxButton.OK,
                                                                        MessageBoxImage.Error);

                                                                    return;
                                                                }
                                                                else
                                                                {
                                                                    AddQuantityToProduct(oldProd.Id, itemToUpdate.Quantity);
                                                                    DeductProduct(newProd.Id, item.Quantity);
                                                                    itemToUpdate.ProductId = item.Product.Id;
                                                                    itemToUpdate.Quantity = item.Quantity;
                                                                }

                                                                db.UpdateShipmentItem(itemToUpdate);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
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

                                using (EntityManager db = new EntityManager(new Models.WarehouseDbContext()))
                                {
                                    db.UpdateShipment(Shipment);
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
                    else
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
                }
            }
            else
            {
                MessageBox.Show("Invalid shipment data, enter valid data",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
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
            if (GetCancelConfirmation() == MessageBoxResult.OK)
            {
                CloseParentWindow();
            }
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
    }
}
