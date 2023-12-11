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
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int AddressId { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address? Address { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }

        public Supplier(string name, int addressId)
        {
            Name = name;
            AddressId = addressId;

            Receipts = new List<Receipt>();
        }
    }
}
