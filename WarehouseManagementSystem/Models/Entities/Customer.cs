using System;
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

        public string Fullname { get {  return $"{Firstname} {Lastname}"; } }

        public DateTime? DateOfBirth { get; set; }

        public int AddressId { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address? Address { get; set; }

        public decimal? DiscountPercentage { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }

        public Customer(string firstname, string lastname, int addressId)
        {
            Firstname = firstname;
            Lastname = lastname;
            AddressId = addressId;

            Shipments = new List<Shipment>();
        }

        public override string ToString()
        {
            return Fullname;
        }
    }
}
