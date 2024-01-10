using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.ViewModels
{
    public  class ShipmentItemViewModel
    {
        public int? Id { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        public ShipmentItemViewModel(Product? product, int quantity)
        {
            Id = null;
            Product = product;
            Quantity = quantity;
        }

        public ShipmentItemViewModel(int? id, Product? product, int quantity) : this(product, quantity)
        {
            Id = Id;
        }
    }
}
