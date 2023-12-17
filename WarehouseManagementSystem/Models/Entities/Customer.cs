﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int AddressId { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address? Address { get; set; }

        public decimal? DiscountPercentage { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }

        public Customer(string firstName, string lastName, int addressId)
        {
            Firstname = firstName;
            Lastname = lastName;
            AddressId = addressId;

            Shipments = new List<Shipment>();
        }
    }
}