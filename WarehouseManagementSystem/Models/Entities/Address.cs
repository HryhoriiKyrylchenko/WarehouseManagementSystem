﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        public string Country { get; set; }

        public string Index { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public string? Room { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Warehouse> Warehouses { get; set; }

        public virtual ICollection<Supplier> Suppliers { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Manufacturer> Manufacturers { get; set; }

        public Address(string country, string index, string city, string street, string buildingNumber)
        {

            Country = country;
            Index = index;
            City = city;
            Street = street;
            BuildingNumber = buildingNumber;

            Warehouses = new List<Warehouse>();
            Suppliers = new List<Supplier>();
            Customers = new List<Customer>();
            Manufacturers = new List<Manufacturer>();
        }

        public override string ToString()
        { 
            string number = (Room == null) ? $"{BuildingNumber}" : $"{BuildingNumber}/{Room}";

            return $"{Street} {number}, {Index} {City}";
        }
    }
}
