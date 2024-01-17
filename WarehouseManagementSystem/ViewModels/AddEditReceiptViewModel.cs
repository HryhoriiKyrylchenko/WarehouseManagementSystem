using Newtonsoft.Json;
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
using WarehouseManagementSystem.Models.Entities.Support_classes;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.ViewModels.Helpers;
using WarehouseManagementSystem.Windows;
using static System.Formats.Asn1.AsnWriter;

namespace WarehouseManagementSystem.ViewModels
{
    public class AddEditReceiptViewModel : ViewModelBaseRequestClose
    {
        private readonly ReceiptsViewModel mainViewModel;

        private Receipt? receipt;

        public Receipt? Receipt
        {
            get { return receipt; }
            set
            {
                if (receipt != value)
                {
                    this.receipt = value;
                    OnPropertyChanged(nameof(Receipt));
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
                }
            }
        }

        private ObservableCollection<Supplier> suppliers;
        public ObservableCollection<Supplier> Suppliers
        {
            get { return suppliers; }
            set
            {
                if (suppliers != value)
                {
                    suppliers = value;
                    OnPropertyChanged(nameof(Suppliers));
                }
            }
        }

        private CurrentReceiptViewModel currentReceiptViewModel;
        public CurrentReceiptViewModel CurrentReceiptViewModel
        {
            get { return currentReceiptViewModel; }
            set
            {
                if (currentReceiptViewModel != value)
                {
                    currentReceiptViewModel = value;
                    OnPropertyChanged(nameof(CurrentReceiptViewModel));
                }
            }
        }

        private ReceiptItemViewModel? selectedReceiptItem;
        public ReceiptItemViewModel? SelectedReceiptItem
        {
            get { return selectedReceiptItem; }
            set
            {
                if (selectedReceiptItem != value)
                {
                    selectedReceiptItem = value;
                    OnPropertyChanged(nameof(SelectedReceiptItem));
                    if (value != null)
                    {
                        UpdatePropertiesData(value);
                    }
                }
            }
        }

        public ICommand AddSupplierCommand => new RelayCommand(AddSupplier);
        public ICommand AddReceiptItemCommand => new RelayCommand(AddReceiptItem);
        public ICommand EditReceiptItemCommand => new RelayCommand(EditReceiptItem);
        public ICommand DeleteReceiptItemCommand => new RelayCommand(DeleteReceiptItem);
        public ICommand OkCommand => new RelayCommand(AddEditReceipt);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public AddEditReceiptViewModel(ReceiptsViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            currentReceiptViewModel = new CurrentReceiptViewModel();
            products = new ObservableCollection<Product>();
            suppliers = new ObservableCollection<Supplier>();

            Initialize();
        }

        public AddEditReceiptViewModel(ReceiptsViewModel mainViewModel, Receipt receipt)
        {
            this.mainViewModel = mainViewModel;
            this.Receipt = receipt;
            currentReceiptViewModel = new CurrentReceiptViewModel();
            products = new ObservableCollection<Product>();
            suppliers = new ObservableCollection<Supplier>();

            Initialize();
            SetSelectedSupplier(Receipt);
        }

        private void Initialize()
        {
            UpdateSuppliers();
            InitializeProductsFromDB();
            CurrentReceiptViewModel = InitializeCurrentReceiptViewModel(this.Receipt);
        }

        private void SetSelectedSupplier(Receipt receipt)
        {
            if (receipt.Supplier != null)
            {
                CurrentReceiptViewModel.Supplier = Suppliers.Where(s => s.Id == receipt.Supplier.Id).FirstOrDefault();
            }
        }

        public void UpdateSuppliers()
        {
            Suppliers.Clear();
            InitializeSuppliersFromDB();
        }

        private async void InitializeProductsFromDB()
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    Products = await db.GetProductsAsync();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
        }

        private void InitializeSuppliersFromDB()
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    Suppliers = db.GetSuppliers();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
        }

        private CurrentReceiptViewModel InitializeCurrentReceiptViewModel(Receipt? receipt)
        {
            CurrentReceiptViewModel currentReceiptViewModel = new CurrentReceiptViewModel();
            if (receipt != null)
            {
                currentReceiptViewModel.ReceiptDate = receipt.ReceiptDate;
                currentReceiptViewModel.Supplier = receipt.Supplier;
                currentReceiptViewModel.BatchNumber = receipt.BatchNumber;
                currentReceiptViewModel.ShipmentNumber = receipt.ShipmentNumber;
                currentReceiptViewModel.AdditionalInfo = receipt.AdditionalInfo;
                if (receipt.ReceiptItems != null)
                {
                    foreach (var item in receipt.ReceiptItems)
                    {
                        currentReceiptViewModel.ReceiptItems.Add(new ReceiptItemViewModel(item.Id, item.Product, item.Quantity));
                    }
                }
            }

            return currentReceiptViewModel;
        }

        private void UpdatePropertiesData(ReceiptItemViewModel receiptItem)
        {
            if (receiptItem.Product != null)
            {
                SelectedProduct = Products.Where(p => p.Id == receiptItem.Product.Id).FirstOrDefault();
            }
            InputQuantity = receiptItem.Quantity.ToString();
        }

        private void AddSupplier(object parameter)
        {
            SupportWindow supportWindow = new SupportWindow(new AddSupplierViewModel(this));
            supportWindow.ShowDialog();
            UpdateSuppliers();
        }

        private void AddReceiptItem(object obj)
        {
            if (ConfirmationHelper.GetConfirmation() != MessageBoxResult.OK)
            {
                return;
            }

            if (SelectedProduct != null
                && !string.IsNullOrWhiteSpace(InputQuantity)
                && TryParseQuantity(InputQuantity, out int quantity)
                && quantity > 0)
            {
                AddReceiptItemToCollection(SelectedProduct, quantity);
            }
            else
            {
                MessageHelper.ShowCautionMessage("Product and valid quantity are required");
            }
        }

        private void AddReceiptItemToCollection(Product selectedProduct, int quantity)
        {
            InitializeReceiptItems();

            CurrentReceiptViewModel.ReceiptItems.Add(new ReceiptItemViewModel(selectedProduct, quantity));
        }

        private void InitializeReceiptItems()
        {
            if (CurrentReceiptViewModel.ReceiptItems == null)
            {
                CurrentReceiptViewModel.ReceiptItems = new ObservableCollection<ReceiptItemViewModel>();
            }
        }

        private bool TryParseQuantity(string input, out int quantity)
        {
            return int.TryParse(input, out quantity);
        }

        private void EditReceiptItem(object obj)
        {
            if (CurrentReceiptViewModel.ReceiptItems == null
                || SelectedReceiptItem == null)
            {
                MessageHelper.ShowCautionMessage("Choose receipt item to edit");
                return;
            }

            if (ConfirmationHelper.GetConfirmation() != MessageBoxResult.OK)
            {
                return;
            }

            if (SelectedProduct != null
                && !string.IsNullOrWhiteSpace(InputQuantity)
                && TryParseQuantity(InputQuantity, out int newQuantity)
                && newQuantity > 0)
            {
                if (SelectedReceiptItem.Id != null)
                {
                    if (SelectedReceiptItem.Product != SelectedProduct
                        || SelectedReceiptItem.Quantity > newQuantity)
                    {
                        MessageHelper.ShowCautionMessage("You cannot independently change a product or reduce its quantity if " +
                                                         "it was previously entered into the database. To make the " +
                                                         "appropriate changes, contact your program administrator.");
                        return;
                    }
                }

                try
                {
                    UpdateReceiptItemQuantity(SelectedReceiptItem, newQuantity);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.HandleException(ex);
                }
            }
            else
            {
                MessageHelper.ShowCautionMessage("Product and valid quantity are required");
            }
        }

        private void UpdateReceiptItemQuantity(ReceiptItemViewModel receiptItem, int newQuantity)
        {
            receiptItem.Quantity = newQuantity;
            CurrentReceiptViewModel.RefreshReceiptItems();
        }


        // Change logic of the method "AddEditReceipt" when added functionality to delete existing in DB ReceiptItems
        private void DeleteReceiptItem(object obj)
        {
            if (CurrentReceiptViewModel.ReceiptItems == null
                || SelectedReceiptItem == null)
            {
                MessageHelper.ShowCautionMessage("Choose receipt item to delete");
                return;
            }

            if (SelectedReceiptItem.Id != null)
            {
                MessageHelper.ShowCautionMessage("You cannot independently delete " +
                                                "a product previously entered into the database. To make the " +
                                                "appropriate changes, contact your program administrator.");
                return;
            }

            if (ConfirmationHelper.GetConfirmation() == MessageBoxResult.OK)
            {
                CurrentReceiptViewModel.ReceiptItems.Remove(SelectedReceiptItem);
            }
        }

        private void AddEditReceipt(object obj)
        {
            if (!IsReceiptDataValid())
            {
                MessageHelper.ShowErrorMessage("Invalid receipt data, enter valid data");
                return;
            }

            if (ConfirmationHelper.GetConfirmation() != MessageBoxResult.OK)
            {
                return;
            }

            try
            {
                if (Receipt != null)
                {
                    EditExistingReceipt();
                }
                else
                {
                    CreateNewReceipt();
                }

                CloseParentWindow();
            }
            catch (Exception ex)
            {
                HandleReceiptException(ex);
            }
        }

        private void HandleReceiptException(Exception ex)
        {
            MessageHelper.ShowErrorMessage("Failed to create or edit a receipt");
            ExceptionHelper.HandleException(ex);
        }

        private void CreateNewReceipt()
        {
            if (!ValidateReceiptData())
            {
                return;
            }

            try
            {
                BuildNewReceipt();
            }
            catch
            {
                throw;
            }
        }

        private void BuildNewReceipt()
        {
            if (CurrentReceiptViewModel.ReceiptDate == null
                    || CurrentReceiptViewModel.Supplier == null
                    || string.IsNullOrWhiteSpace(CurrentReceiptViewModel.BatchNumber)
                    || mainViewModel.MainViewModel.LoginService.CurrentUser == null)
            {
                throw new ArgumentNullException("Required data to create new receipt is null");
            }

            using (var scope = new TransactionScope())
            {
                try
                {
                    var tempReceipt = new ReceiptBuilder((DateTime)CurrentReceiptViewModel.ReceiptDate,
                                                        CurrentReceiptViewModel.Supplier.Id,
                                                        mainViewModel.MainViewModel.LoginService.CurrentUser.Id,
                                                        CurrentReceiptViewModel.BatchNumber);

                    if (!string.IsNullOrWhiteSpace(CurrentReceiptViewModel.ShipmentNumber))
                    {
                        tempReceipt.WithShipmentNumber(CurrentReceiptViewModel.ShipmentNumber);
                    }

                    if (!string.IsNullOrWhiteSpace(CurrentReceiptViewModel.AdditionalInfo))
                    {
                        tempReceipt.WithAdditionalInfo(CurrentReceiptViewModel.AdditionalInfo);
                    }

                    Receipt updatedReceipt = tempReceipt.Build();

                    HandleReceiptItems(updatedReceipt);

                    scope.Complete();
                }
                catch
                {
                    scope.Dispose();
                    throw;
                }
            }
        }

        private void HandleReceiptItems(Receipt updatedReceipt)
        {
            if (CurrentReceiptViewModel.ReceiptItems.Count > 0)
            {
                using (EntityManager dbE = new EntityManager(new WarehouseDbContext()))
                using (WarehouseDBManager dbW = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    foreach (var item in CurrentReceiptViewModel.ReceiptItems)
                    {
                        if (item.Product != null)
                        {
                            var prod = dbW.GetProduct(item.Product.Id);
                            if (prod != null)
                            {
                                prod.Quantity += item.Quantity;

                                new ReceiptItemBuilder(updatedReceipt.Id, item.Product.Id, item.Quantity);
                                dbE.UpdateProduct(prod);
                            }
                        }
                    }
                }
            }
        }

        private bool ValidateReceiptData()
        {
            if (CurrentReceiptViewModel.ReceiptDate == null
                    || CurrentReceiptViewModel.Supplier == null
                    || string.IsNullOrWhiteSpace(CurrentReceiptViewModel.BatchNumber)
                    || mainViewModel.MainViewModel.LoginService.CurrentUser == null)
            {
                MessageHelper.ShowCautionMessage("Please fill in all required fields.");
                return false;
            }

            return true;
        }

        private void EditExistingReceipt()
        {
            if (!ValidateReceiptData())
            {
                return;
            }

            try
            {
                if (Receipt != null)
                {
                    UpdateReceipt(Receipt);
                }
            }
            catch
            {
                throw;
            }
        }

        private void UpdateReceipt(Receipt receipt)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    UpdateReceiptDate(receipt);
                    UpdateSupplier(receipt);
                    UpdateBatchNumber(receipt);
                    UpdateShipmentNumber(receipt);
                    UpdateAdditionalInfo(receipt);
                    UpdateReceiptItems(receipt);

                    SaveReceiptToDatabase(receipt);
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

        private void SaveReceiptToDatabase(Receipt receipt)
        {
            try
            {
                using (EntityManager db = new EntityManager(new WarehouseDbContext()))
                {
                    db.UpdateReceipt(receipt);
                }
            }
            catch
            {
                throw;
            }
        }

        private void UpdateReceiptItems(Receipt receipt)
        {
            if (CurrentReceiptViewModel.ReceiptItems.Count <= 0)
            {
                throw new ArgumentNullException("At least one receipt item required");
            }

            using (EntityManager db = new EntityManager(new WarehouseDbContext()))
            using (WarehouseDBManager dbW = new WarehouseDBManager(new WarehouseDbContext()))
            {
                var newReceiptItems = CurrentReceiptViewModel.ReceiptItems.ToList();

                var itemsToDelete = receipt.ReceiptItems.Where(ri => !newReceiptItems.Any(i => i.Id == ri.Id)).ToList();

                if (itemsToDelete.Count > 0)
                {
                    foreach (var item in itemsToDelete)
                    {
                        /// Logic to change product quantity if deletion available

                        //db.DeleteReceiptItem(item);
                    }
                }

                foreach (var item in newReceiptItems)
                {
                    if (item.Product != null)
                    {
                        if (item.Id != null)
                        {
                            var itemToUpdate = receipt.ReceiptItems.FirstOrDefault(x => x.Id == item.Id);

                            if (itemToUpdate != null)
                            {
                                if (itemToUpdate.ProductId == item.Product.Id)
                                {
                                    if (itemToUpdate.Quantity > item.Quantity)
                                    {
                                        throw new ArgumentException("Impossible reduce a product quantity if " +
                                                    "it was previously entered into the database. To make the " +
                                                    "appropriate changes, contact your program administrator.");
                                    }
                                    else
                                    {
                                        var prod = dbW.GetProduct(itemToUpdate.ProductId);
                                        if (prod != null)
                                        {
                                            prod.Quantity += (item.Quantity - itemToUpdate.Quantity);
                                            db.UpdateProduct(prod);
                                        }
                                    }

                                    itemToUpdate.Quantity = item.Quantity;
                                    db.UpdateReceiptItem(itemToUpdate);
                                }
                                else
                                {
                                    throw new ArgumentException("You cannot independently change a product if " +
                                                    "it was previously entered into the database. To make the " +
                                                    "appropriate changes, contact your program administrator.");
                                }
                            }
                        }
                        else
                        {
                            var prod = dbW.GetProduct(item.Product.Id);
                            if (prod != null)
                            {
                                prod.Quantity += item.Quantity;

                                new ReceiptItemBuilder(receipt.Id, item.Product.Id, item.Quantity);
                                db.UpdateProduct(prod);
                            }
                        }
                    }
                }
            }
        }

        private void UpdateAdditionalInfo(Receipt receipt)
        {
            if (!string.IsNullOrWhiteSpace(CurrentReceiptViewModel.AdditionalInfo)
                            && receipt.AdditionalInfo != CurrentReceiptViewModel.AdditionalInfo)
            {
                receipt.AdditionalInfo = CurrentReceiptViewModel.AdditionalInfo;
            }
        }

        private void UpdateShipmentNumber(Receipt receipt)
        {
            if (!string.IsNullOrWhiteSpace(CurrentReceiptViewModel.ShipmentNumber)
                            && receipt.ShipmentNumber != CurrentReceiptViewModel.ShipmentNumber)
            {
                receipt.ShipmentNumber = CurrentReceiptViewModel.ShipmentNumber;
            }
        }

        private void UpdateBatchNumber(Receipt receipt)
        {
            if (string.IsNullOrWhiteSpace(CurrentReceiptViewModel.BatchNumber))
            {
                throw new ArgumentNullException("Batch number is required");
            }

            if (receipt.BatchNumber != CurrentReceiptViewModel.BatchNumber)
            {
                receipt.BatchNumber = CurrentReceiptViewModel.BatchNumber;
            }
        }

        private void UpdateSupplier(Receipt receipt)
        {
            if (CurrentReceiptViewModel.Supplier == null)
            {
                throw new ArgumentNullException("Supplier is required");
            }

            if (receipt.SupplierId != CurrentReceiptViewModel.Supplier.Id)
            {
                receipt.SupplierId = CurrentReceiptViewModel.Supplier.Id;
            }
        }

        private void UpdateReceiptDate(Receipt receipt)
        {
            if (CurrentReceiptViewModel.ReceiptDate == null)
            {
                throw new ArgumentNullException("Receipt date is required");
            }

            if (receipt.ReceiptDate != CurrentReceiptViewModel.ReceiptDate)
            {
                receipt.ReceiptDate = (DateTime)CurrentReceiptViewModel.ReceiptDate;
            }
        }

        private bool IsReceiptDataValid()
        {
            if (CurrentReceiptViewModel.ReceiptDate == null
                || CurrentReceiptViewModel.Supplier == null
                || string.IsNullOrWhiteSpace(CurrentReceiptViewModel.BatchNumber)
                || CurrentReceiptViewModel.ReceiptItems.Count <= 0)
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
