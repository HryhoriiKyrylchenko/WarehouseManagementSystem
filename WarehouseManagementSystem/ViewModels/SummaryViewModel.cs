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
using WarehouseManagementSystem.ViewModels.Helpers;

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

        public void GetData()
        {
            try
            {
                using (WarehouseDBManager db = new WarehouseDBManager(new Models.WarehouseDbContext()))
                {
                    TotalCapacity = db.GetTotalWarehouseCapacity(currentWarehouse).ToString();
                    FreeCapacity = db.GetFreeWarehouseCapacity(currentWarehouse).ToString();
                    OccupancyPercentage = db.GetWarehouseOccupancyPercentage(currentWarehouse).ToString("F2");
                    TotalZones = db.CountTotalZones(currentWarehouse).ToString();
                    UnusedZones = db.CountUnusedZones(currentWarehouse).ToString();
                    TotalProducts = db.CountTotalProductsInWarehouse(currentWarehouse).ToString();
                    UnallocatedProducts = db.CountUnallocatedProductsInWarehouse(currentWarehouse).ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageHelper.ShowErrorMessage($"Error while receiving data: {ex.Message}");
                });
            }
        }
    }
}
