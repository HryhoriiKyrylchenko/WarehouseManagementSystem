﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Builders;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Models.Entities.Support_classes;
using WarehouseManagementSystem.Services;
using WarehouseManagementSystem.Windows;

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
            if (Receipt.Supplier != null)
            {
                CurrentReceiptViewModel.Supplier = Suppliers.Where(s => s.Id == Receipt.Supplier.Id).FirstOrDefault();
            }
        }

        private void Initialize()
        {
            UpdateSuppliers();
            InitializeProductsFromDB();
            CurrentReceiptViewModel = InitializeCurrentReceiptViewModel(this.Receipt);
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
                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    logger.LogError(ex);
                }
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
                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    logger.LogError(ex);
                }
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
                    foreach(var item in receipt.ReceiptItems)
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
            if (SelectedProduct != null
                && !string.IsNullOrWhiteSpace(InputQuantity)
                && Convert.ToInt32(InputQuantity) > 0
                && GetConfirmation() == MessageBoxResult.OK)
            {
                if (CurrentReceiptViewModel.ReceiptItems == null)
                {
                    CurrentReceiptViewModel.ReceiptItems = new ObservableCollection<ReceiptItemViewModel>();
                }

                CurrentReceiptViewModel.ReceiptItems.Add(new ReceiptItemViewModel(SelectedProduct, Convert.ToInt32(InputQuantity)));
            }
            else
            {
                MessageBox.Show("Product and quantity fields are required",
                        "Caution",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
            }
        }

        private void EditReceiptItem(object obj)
        {
            if (CurrentReceiptViewModel.ReceiptItems != null
                && SelectedReceiptItem != null)
            {
                if (SelectedProduct != null
                && !string.IsNullOrWhiteSpace(InputQuantity)
                && Convert.ToInt32(InputQuantity) > 0
                && GetConfirmation() == MessageBoxResult.OK)
                {
                    try
                    {
                        SelectedReceiptItem.Product = SelectedProduct;
                        SelectedReceiptItem.Quantity = Convert.ToInt32(InputQuantity);
                        CurrentReceiptViewModel.RefreshReceiptItems();
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

        private void DeleteReceiptItem(object obj)
        {
            if (CurrentReceiptViewModel.ReceiptItems != null
                && SelectedReceiptItem != null)
            {
                if (GetConfirmation() == MessageBoxResult.OK)
                {
                    CurrentReceiptViewModel.ReceiptItems.Remove(SelectedReceiptItem);
                }
            }
        }

        private void AddEditReceipt(object obj)
        {
            if (IsReceiptDataValid())
            {
                if (GetConfirmation() == MessageBoxResult.OK)
                {
                    if (Receipt != null)
                    {
                        try
                        {
                            if (CurrentReceiptViewModel.ReceiptDate != null
                            && Receipt.ReceiptDate != CurrentReceiptViewModel.ReceiptDate)
                            {
                                Receipt.ReceiptDate = (DateTime)CurrentReceiptViewModel.ReceiptDate;
                            }
                            else if (CurrentReceiptViewModel.ReceiptDate == null)
                            {
                                MessageBox.Show("Receipt date is required",
                                        "Caution",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                                return;
                            }

                            if (CurrentReceiptViewModel.Supplier != null
                                && Receipt.SupplierId != CurrentReceiptViewModel.Supplier.Id)
                            {
                                Receipt.SupplierId = CurrentReceiptViewModel.Supplier.Id;
                            }
                            else if (CurrentReceiptViewModel.Supplier == null)
                            {
                                MessageBox.Show("Supplier is required",
                                        "Caution",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                                return;
                            }

                            if (CurrentReceiptViewModel.BatchNumber != null
                                    && Receipt.BatchNumber != CurrentReceiptViewModel.BatchNumber)
                            {
                                Receipt.BatchNumber = CurrentReceiptViewModel.BatchNumber;
                            }
                            else if (string.IsNullOrWhiteSpace(CurrentReceiptViewModel.BatchNumber))
                            {
                                MessageBox.Show("Batch number is required",
                                    "Caution",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                            }

                            if (!string.IsNullOrWhiteSpace(CurrentReceiptViewModel.ShipmentNumber)
                                    && Receipt.ShipmentNumber != CurrentReceiptViewModel.ShipmentNumber)
                            {
                                Receipt.ShipmentNumber = CurrentReceiptViewModel.ShipmentNumber;
                            }

                            if (!string.IsNullOrWhiteSpace(CurrentReceiptViewModel.AdditionalInfo)
                                    && Receipt.AdditionalInfo != CurrentReceiptViewModel.AdditionalInfo)
                            {
                                Receipt.AdditionalInfo = CurrentReceiptViewModel.AdditionalInfo;
                            }

                            if (CurrentReceiptViewModel.ReceiptItems.Any())
                            {
                                using (EntityManager db = new EntityManager(new WarehouseDbContext()))
                                {
                                    var newReceiptItems = CurrentReceiptViewModel.ReceiptItems.ToList();

                                    var itemsToDelete = Receipt.ReceiptItems.Where(ri => !newReceiptItems.Any(i => i.Id == ri.Id)).ToList();
                                    if (itemsToDelete.Count > 0)
                                    {
                                        foreach (var item in itemsToDelete)
                                        {
                                            db.DeleteReceiptItem(item);
                                        }
                                    }

                                    foreach (var item in newReceiptItems)
                                    {
                                        if (item.Product != null)
                                        {
                                            if (item.Id != null)
                                            {
                                                var itemToUpdate = Receipt.ReceiptItems.FirstOrDefault(x => x.Id == item.Id);
                                                if (itemToUpdate != null)
                                                {
                                                    itemToUpdate.ProductId = item.Product.Id;
                                                    itemToUpdate.Quantity = item.Quantity;
                                                    db.UpdateReceiptItem(itemToUpdate);
                                                }
                                            }
                                            else
                                            {
                                                var newItem = new ReceiptItemBuilder(Receipt.Id, item.Product.Id, item.Quantity).Build();
                                                Receipt.ReceiptItems.Add(newItem);
                                            }
                                        }
                                    }
                                }
                            }

                            using (EntityManager db = new EntityManager(new Models.WarehouseDbContext()))
                            {
                                db.UpdateReceipt(Receipt);
                            }

                            CloseParentWindow();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Some error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                            {
                                logger.LogError(ex);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (CurrentReceiptViewModel.ReceiptDate != null
                                && CurrentReceiptViewModel.Supplier != null
                                && !string.IsNullOrWhiteSpace(CurrentReceiptViewModel.BatchNumber)
                                && mainViewModel.MainViewModel.LoginService.CurrentUser != null)
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

                                if (CurrentReceiptViewModel.ReceiptItems.Count > 0)
                                {
                                    Receipt = tempReceipt.Build();
                                    foreach (var item in CurrentReceiptViewModel.ReceiptItems)
                                    {
                                        if (item.Product != null)
                                        {
                                            var newItem = new ReceiptItemBuilder(Receipt.Id, item.Product.Id, item.Quantity).Build();
                                        }
                                    }
                                }
                            }

                            CloseParentWindow();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Some error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                            {
                                logger.LogError(ex);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid receipt data, enter valid data", 
                    "Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        private bool IsReceiptDataValid()
        {
            if (CurrentReceiptViewModel.ReceiptDate == null 
                || CurrentReceiptViewModel.Supplier == null
                || string.IsNullOrWhiteSpace(CurrentReceiptViewModel.BatchNumber))
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