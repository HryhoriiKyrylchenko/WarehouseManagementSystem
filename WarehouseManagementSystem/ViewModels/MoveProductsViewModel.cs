using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
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
using WarehouseManagementSystem.ViewModels.Support_data;
using WarehouseManagementSystem.Windows;
using Zone = WarehouseManagementSystem.Models.Entities.Zone;

namespace WarehouseManagementSystem.ViewModels
{
    public class MoveProductsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

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
                    UpdateProductInZonePozitionsByCategory();
                }
            }
        }

        private ObservableCollection<ProductInZonePosition>? productsInZonePozitions;

        public ObservableCollection<ProductInZonePosition>? ProductInZonePozitions
        {
            get { return productsInZonePozitions; }
            set
            {
                if (productsInZonePozitions != value)
                {
                    productsInZonePozitions = value;
                    OnPropertyChanged(nameof(ProductInZonePozitions));
                }
            }
        }

        private ProductInZonePosition? selectedProductInZonePosition;

        public ProductInZonePosition? SelectedProductInZonePosition
        {
            get { return selectedProductInZonePosition; }
            set
            {
                if (selectedProductInZonePosition != value)
                {
                    selectedProductInZonePosition = value;
                    OnPropertyChanged(nameof(SelectedProductInZonePosition));
                    if (value != null)
                    {
                        UpdateAvailableDataAsync(value);
                        UpdateManufactureAndExpiryDates(value);
                    }
                    else
                    {
                        AvailableBalanceToMove = 0;
                        AvailableCapacityToMove = 0;
                        ProductManufactureDate = null;
                        ProductExpiryDate = null;
                    }
                    UpdateInputQuantity(string.Empty);
                }
            }
        }

        private bool sectionByZoneSelected;

        public bool SectionByZoneSelected
        {
            get { return sectionByZoneSelected; }
            set
            {
                if (sectionByZoneSelected != value)
                {
                    sectionByZoneSelected = value;
                    OnPropertyChanged(nameof(SectionByZoneSelected));

                    if (value)
                    {
                        SectionByCategorySelected = false;
                    }
                }
            }
        }

        private bool sectionByCategorySelected;

        public bool SectionByCategorySelected
        {
            get { return sectionByCategorySelected; }
            set
            {
                if (sectionByCategorySelected != value)
                {
                    sectionByCategorySelected = value;
                    OnPropertyChanged(nameof(SectionByCategorySelected));

                    if (value)
                    {
                        SectionByZoneSelected = false;
                    }
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
                    UpdateProductInZonePozitionsByZonePozition();
                }
            }
        }

        private Zone? selectedZoneMoveTo;

        public Zone? SelectedZoneMoveTo
        {
            get { return selectedZoneMoveTo; }
            set
            {
                if (selectedZoneMoveTo != value)
                {
                    selectedZoneMoveTo = value;
                    OnPropertyChanged(nameof(SelectedZoneMoveTo));
                    UpdateZonePositionsMoveTo(value);
                }
            }
        }

        private ObservableCollection<ZonePosition>? zonePositionsMoveTo;

        public ObservableCollection<ZonePosition>? ZonePositionsMoveTo
        {
            get { return zonePositionsMoveTo; }
            set
            {
                if (zonePositionsMoveTo != value)
                {
                    zonePositionsMoveTo = value;
                    OnPropertyChanged(nameof(ZonePositionsMoveTo));
                }
            }
        }

        private ZonePosition? selectedZonePositionMoveTo;

        public ZonePosition? SelectedZonePositionMoveTo
        {
            get { return selectedZonePositionMoveTo; }
            set
            {
                if (selectedZonePositionMoveTo != value)
                {
                    selectedZonePositionMoveTo = value;
                    OnPropertyChanged(nameof(SelectedZonePositionMoveTo));
                    UpdateSelectedZonePositionFreeCapacity(value);
                }
            }
        }

        private decimal? availableBalanceToMove;

        public decimal? AvailableBalanceToMove
        {
            get { return availableBalanceToMove; }
            set
            {
                if (availableBalanceToMove != value)
                {
                    availableBalanceToMove = value;
                    OnPropertyChanged(nameof(AvailableBalanceToMove));
                }
            }
        }

        private decimal? availableCapacityToMove;

        public decimal? AvailableCapacityToMove
        {
            get { return availableCapacityToMove; }
            set
            {
                if (availableCapacityToMove != value)
                {
                    availableCapacityToMove = value;
                    OnPropertyChanged(nameof(AvailableCapacityToMove));
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
                    UpdateCapacityToBeMoved(value);
                }
            }
        }

        private int? capacityToBeMoved;

        public int? CapacityToBeMoved
        {
            get { return capacityToBeMoved; }
            set
            {
                if (capacityToBeMoved != value)
                {
                    capacityToBeMoved = value;
                    OnPropertyChanged(nameof(CapacityToBeMoved));
                }
            }
        }

        private DateTime? productManufactureDate;

        public DateTime? ProductManufactureDate
        {
            get { return productManufactureDate; }
            set
            {
                if (productManufactureDate != value)
                {
                    productManufactureDate = value;
                    OnPropertyChanged(nameof(ProductManufactureDate));
                }
            }
        }

        private DateTime? productExpiryDate;

        public DateTime? ProductExpiryDate
        {
            get { return productExpiryDate; }
            set
            {
                if (productExpiryDate != value)
                {
                    productExpiryDate = value;
                    OnPropertyChanged(nameof(ProductExpiryDate));
                }
            }
        }

        private DateTime? reportDateFrom;

        public DateTime? ReportDateFrom
        {
            get { return reportDateFrom; }
            set
            {
                if (reportDateFrom != value)
                {
                    reportDateFrom = value;
                    OnPropertyChanged(nameof(ReportDateFrom));
                }
            }
        }

        private DateTime? reportDateTo;

        public DateTime? ReportDateTo
        {
            get { return reportDateTo; }
            set
            {
                if (reportDateTo != value)
                {
                    reportDateTo = value;
                    OnPropertyChanged(nameof(ReportDateTo));
                }
            }
        }

        public MoveProductsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            categories = new ObservableCollection<CategoryViewModel>();

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await InitializeCategoriesFromDBAsync();
            await InitializeZonesFromDBAsync();

            SectionByZoneSelected = true;
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

        private async void UpdateProductInZonePozitionsByCategory()
        {
            if (SelectedCategory != null)
            {
                if (ProductInZonePozitions != null)
                {
                    ProductInZonePozitions.Clear();
                }
                else
                {
                    ProductInZonePozitions = new ObservableCollection<ProductInZonePosition>();
                }

                foreach (var product in SelectedCategory.Products)
                {
                    try
                    {
                        using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                        {
                            var prodInZones = await db.GetProductInZonePositionsByProductAsync(product.Id);
                            if (prodInZones.Any())
                            {
                                foreach (var prod in prodInZones)
                                {
                                    ProductInZonePozitions.Add(prod);
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
            }
            else
            {
                ProductInZonePozitions = new ObservableCollection<ProductInZonePosition>();
            }
        }
        private void UpdateProductInZonePozitionsByZonePozition()
        {
            if (SelectedZonePosition != null)
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    ProductInZonePozitions = db.GetProductInZonePositions(SelectedZonePosition.Id);
                }
            }
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

        private void UpdateAvailableDataAsync(ProductInZonePosition product)
        {
            if (product.Product != null)
            {
                AvailableBalanceToMove = product.Quantity;
                AvailableCapacityToMove = product.Quantity * product.Product.Capacity;
            }
            else
            {
                AvailableBalanceToMove = 0;
                AvailableCapacityToMove = 0;
            }
        }

        private void UpdateManufactureAndExpiryDates(ProductInZonePosition product)
        {
            ProductManufactureDate = product.ManufactureDate;
            ProductExpiryDate = product.ExpiryDate;
        }

        private async void UpdateZonePositionsMoveTo(Zone? zone)
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
            ZonePositionsMoveTo = zonePos;
            if (ZonePositionsMoveTo.Any())
            {
                SelectedZonePositionMoveTo = ZonePositionsMoveTo.First();
            }
        }

        private void UpdateSelectedZonePositionFreeCapacity(ZonePosition? zonePosition)
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
                        logger.LogError(ex);
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

        private void UpdateCapacityToBeMoved(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                CapacityToBeMoved = 0;
            }
            else
            {
                CapacityToBeMoved = Convert.ToInt32(inputQuantity) * SelectedProductInZonePosition?.Product?.Capacity;
            }
        }

        public ICommand BackCommand => new RelayCommand(Back);
        public ICommand MoveCommand => new RelayCommand(MoveProduct);
        public ICommand SaveReportCommand => new RelayCommand(SaveReport);

        private void Back(object parameter)
        {
            mainViewModel.NavigateBack();
        }

        private void MoveProduct(object parameter)
        {
            if (SelectedProductInZonePosition != null)
            {
                if (GetConfirmation() == MessageBoxResult.OK)
                {
                    try
                    {
                        if (!IsDataToMoveCorrect()) return;

                        int productToMovedId = SelectedProductInZonePosition.ProductId;
                        int quantityToBeMoved = Convert.ToInt32(InputQuantity);
                        int zonePositionToMoveId;
                        if (SelectedZonePositionMoveTo != null)
                        {
                            zonePositionToMoveId = SelectedZonePositionMoveTo.Id;
                        }
                        else
                        {
                            MessageBox.Show("Select zone position to move product",
                                "Caution",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                            return;
                        }

                        using (var scope = new TransactionScope())
                        {
                            try
                            {
                                if (SelectedProductInZonePosition.ZonePositionId != zonePositionToMoveId
                                    || SelectedProductInZonePosition.ManufactureDate != ProductManufactureDate
                                    || SelectedProductInZonePosition.ExpiryDate != ProductExpiryDate)
                                {

                                    ProductInZonePosition? existingProduct;

                                    using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                                    {
                                        existingProduct = db.GetProductInZonePositionByProduct(productToMovedId,
                                                                                                zonePositionToMoveId,
                                                                                                ProductManufactureDate,
                                                                                                ProductExpiryDate);
                                    }


                                    if (existingProduct != null)
                                    {
                                        using (EntityManager db = new EntityManager(new WarehouseDbContext()))
                                        {
                                            existingProduct.Quantity += quantityToBeMoved;
                                            db.UpdateProductInZonePosition(existingProduct);
                                        }
                                    }
                                    else
                                    {
                                        ProductInZonePositionBuilder newAllocator = new ProductInZonePositionBuilder(productToMovedId,
                                                                                                            quantityToBeMoved,
                                                                                                            zonePositionToMoveId);
                                        if (ProductManufactureDate != null)
                                        {
                                            newAllocator.WithManufactureDate((DateTime)ProductManufactureDate);
                                        }
                                        if (ProductExpiryDate != null)
                                        {
                                            newAllocator.WithExpiryDate((DateTime)ProductExpiryDate);
                                        }
                                    }

                                    MovementHistoryBuilder newMHB = new MovementHistoryBuilder(DateTime.Now,
                                                                                               productToMovedId,
                                                                                               quantityToBeMoved,
                                                                                               SelectedProductInZonePosition.ZonePositionId,
                                                                                               zonePositionToMoveId);

                                    using (EntityManager db = new EntityManager(new WarehouseDbContext()))
                                    {
                                        if (quantityToBeMoved < SelectedProductInZonePosition.Quantity)
                                        {
                                            SelectedProductInZonePosition.Quantity -= quantityToBeMoved;
                                            db.UpdateProductInZonePosition(SelectedProductInZonePosition);
                                        }
                                        else if (quantityToBeMoved == SelectedProductInZonePosition.Quantity)
                                        {
                                            db.DeleteProductInZonePosition(SelectedProductInZonePosition);
                                        }
                                    }


                                    SelectedProductInZonePosition = null;
                                }

                                scope.Complete();
                            }
                            catch
                            {
                                scope.Dispose();
                                throw;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Some error occured",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                        using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                        {
                            logger.LogError(ex);
                        }
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select product to move",
                        "Caution",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                return;
            }
        }

        private bool IsDataToMoveCorrect()
        {
            if (SelectedZonePositionMoveTo == null)
            {
                MessageBox.Show("Select zone position to move product",
                    "Caution",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return false;
            }

            if (SelectedZonePositionFreeCapacity <= 0)
            {
                MessageBox.Show("No space in selected zone position",
                    "Caution",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return false;
            }

            if (CapacityToBeMoved <= 0)
            {
                MessageBox.Show("Enter quantity to move",
                    "Caution",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return false;
            }

            if (CapacityToBeMoved > SelectedZonePositionFreeCapacity)
            {
                MessageBox.Show("Not enough space in selected zone position",
                    "Caution",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return false;
            }

            if (AvailableBalanceToMove < CapacityToBeMoved)
            {
                MessageBox.Show("Not enough products to move",
                    "Caution",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return false;
            }

            if (ProductManufactureDate != null && ProductManufactureDate > DateTime.Now)
            {
                MessageBox.Show("Incorrect manufacture date",
                    "Caution",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return false;
            }

            if (ProductExpiryDate != null && ProductManufactureDate != null && ProductExpiryDate < ProductManufactureDate)
            {
                MessageBox.Show("Incorrect expiry date",
                    "Caution",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private void SaveReport(object parameter)
        {
            List<MovementHistory>? movements;

            using (WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
            {
                 movements = db.GetMovementHistoriesFiltered(ReportDateFrom, ReportDateTo);
            }

            if (movements.Any()
                && mainViewModel.LoginService.CurrentUser != null)
            {
                string title = GenereteTitle();
                string content = GenereteContentToJson(movements);

                SupportWindow supportWindow = new SupportWindow(new SaveReportViewModel(title,
                                                                                    Enums.ReportTypeEnum.MOVEMENTS,
                                                                                    content,
                                                                                    mainViewModel.LoginService.CurrentUser.Id));
                supportWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("No info to be saved",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private string GenereteTitle()
        {
            StringBuilder newTitle = new StringBuilder();
            newTitle.Append("Movements/");

            string formattedDateFrom = ReportDateFrom?.ToString("dd-MM-yyyy") ?? "N/A";
            newTitle.Append($"From {formattedDateFrom}/");

            string formattedDateTo = ReportDateTo?.ToString("dd-MM-yyyy") ?? "N/A";
            newTitle.Append($"To {formattedDateTo}/");

            return newTitle.ToString();
        }

        private string GenereteContentToJson(List<MovementHistory> content)
        {
            return JsonConvert.SerializeObject(content, Formatting.None);
        }

        private MessageBoxResult GetConfirmation()
        {
            return MessageBox.Show("Do you want to make this changes?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }
    }
}
