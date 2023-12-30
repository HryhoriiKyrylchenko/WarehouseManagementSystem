using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.ViewModels
{
    public class SummaryViewModel : ViewModelBase
    {
        private Warehouse currentWarehouse;

        public string TotalCapacity { get; set; }
        public string FreeCapacity { get; set; }
        public string OccupancyPercentage { get; set; }
        public string TotalZones { get; set; }
        public string UnusedZones { get; set; }
        public string TotalProducts { get; set; }
        public string UnallocatedProducts { get; set; }

        public SummaryViewModel(Warehouse warehouse) 
        {
            currentWarehouse = warehouse;

            TotalCapacity = string.Empty;
            FreeCapacity = string.Empty;
            OccupancyPercentage = string.Empty;
            TotalZones = string.Empty;
            UnusedZones = string.Empty;
            TotalProducts = string.Empty;
            UnallocatedProducts = string.Empty;

            GetData().GetAwaiter().GetResult();
        }

        public async Task GetData()
        {
            using(WarehouseDBManager db = new WarehouseDBManager(new Models.WarehouseDbContext()))
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
    }
}
