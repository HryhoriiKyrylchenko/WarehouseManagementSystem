using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.ViewModels
{
    public class SummaryViewModel : ViewModelBase
    {
        private Warehouse currentWarehouse;

        private string totalCapacity;
        public string TotalCapacity 
        {
            get { return totalCapacity; } 
            set
            {
                if(totalCapacity != value) 
                {
                    totalCapacity = value;
                    OnPropertyChanged(nameof(TotalCapacity));
                }
            }
        }
        private string freeCapacity;
        public string FreeCapacity
        {
            get { return freeCapacity; }
            set
            {
                if (freeCapacity != value)
                {
                    freeCapacity = value;
                    OnPropertyChanged(nameof(FreeCapacity));
                }
            }
        }
        private string occupancyPercentage;
        public string OccupancyPercentage
        {
            get { return occupancyPercentage; }
            set
            {
                if (occupancyPercentage != value)
                {
                    occupancyPercentage = value;
                    OnPropertyChanged(nameof(OccupancyPercentage));
                }
            }
        }
        private string totalZones;
        public string TotalZones
        {
            get { return totalZones; }
            set
            {
                if (totalZones != value)
                {
                    totalZones = value;
                    OnPropertyChanged(nameof(TotalZones));
                }
            }
        }
        private string unusedZones;
        public string UnusedZones
        {
            get { return unusedZones; }
            set
            {
                if (unusedZones != value)
                {
                    unusedZones = value;
                    OnPropertyChanged(nameof(UnusedZones));
                }
            }
        }
        private string totalProducts;
        public string TotalProducts
        {
            get { return totalProducts; }
            set
            {
                if (totalProducts != value)
                {
                    totalProducts = value;
                    OnPropertyChanged(nameof(TotalProducts));
                }
            }
        }
        private string unallocatedProducts;
        public string UnallocatedProducts
        {
            get { return unallocatedProducts; }
            set
            {
                if (unallocatedProducts != value)
                {
                    unallocatedProducts = value;
                    OnPropertyChanged(nameof(UnallocatedProducts));
                }
            }
        }

        public SummaryViewModel(Warehouse warehouse) 
        {
            currentWarehouse = warehouse;

            totalCapacity = string.Empty;
            freeCapacity = string.Empty;
            occupancyPercentage = string.Empty;
            totalZones = string.Empty;
            unusedZones = string.Empty;
            totalProducts = string.Empty;
            unallocatedProducts = string.Empty;
        }

        public async Task GetDataAsync()
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new Models.WarehouseDbContext()))
                {
                    int totalCapacity = await db.GetTotalWarehouseCapacityAsync(currentWarehouse);
                    TotalCapacity = totalCapacity.ToString();

                    int freeCapacity = await db.GetFreeWarehouseCapacityAsync(currentWarehouse);
                    FreeCapacity = freeCapacity.ToString();

                    double occupancyPercentage = await db.GetWarehouseOccupancyPercentageAsync(currentWarehouse);
                    OccupancyPercentage = occupancyPercentage.ToString();

                    int totalZones = await db.CountTotalZonesAsync(currentWarehouse);
                    TotalZones = totalZones.ToString();

                    int unusedZones = await db.CountUnusedZonesAsync(currentWarehouse);
                    UnusedZones = unusedZones.ToString();

                    int totalProducts = await db.CountTotalProductsInWarehouseAsync(currentWarehouse);
                    TotalProducts = totalProducts.ToString();

                    int unallocatedProducts = await db.CountUnallocatedProductsInWarehouseAsync(currentWarehouse);
                    UnallocatedProducts = unallocatedProducts.ToString();
                }
            }
            catch (Exception ex)
            {
                using(ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    await logger.LogErrorAsync(ex);
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Error while receiving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
    }
}
