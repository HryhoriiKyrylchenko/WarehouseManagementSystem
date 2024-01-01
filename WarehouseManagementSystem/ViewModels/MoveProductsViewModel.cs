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
                        UpdateUnallocatedDataAsync(value.Product);
                    }
                    else
                    {
                        UnallocatedProductInstances = new UnallocatedProductInstancesModel();
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
                    UpdateProductInZonePozitionsByZonePozition();
                    UpdateSelectedZonePositionFreeCapacity(value);
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
                        SelectedZoneMoveTo = Zones.First();
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

        public ICommand BackCommand => new RelayCommand(Back);

        private void Back(object parameter)
        {
            mainViewModel.NavigateBack();
        }

        private async void UpdateProductInZonePozitionsByCategory()
        {
            if (SelectedCategory != null)
            {
                if (ProductInZonePozitions != null )
                {
                    ProductInZonePozitions.Clear();
                }
                else
                {
                    ProductInZonePozitions = new ObservableCollection<ProductInZonePosition>();
                }

                foreach(var product in SelectedCategory.Products)
                {
                    try
                    {
                        using(WarehouseDBManager db = new WarehouseDBManager(new WarehouseDbContext()))
                        {
                            var prodInZones = await db.GetProductInZonePositionsByProductAsync(product.Id);
                            if(prodInZones.Any())
                            {
                                foreach(var prod in prodInZones)
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
                        tempModel.UnallocatedCapacity = tempModel.UnallocatedBalance * product.Capacity;
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
                CapacityToBeAllocated = Convert.ToInt32(inputQuantity) * SelectedProductInZonePosition?.Product?.Capacity;
            }
        }
    }
}
