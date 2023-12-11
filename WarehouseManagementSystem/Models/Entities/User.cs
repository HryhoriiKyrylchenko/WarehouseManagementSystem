using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities.Enums;

namespace WarehouseManagementSystem.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserRolesEnum Role { get; set; }

        public string? AdditionalInfo { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }

        public virtual ICollection<Report> Reports { get; set; }

        public User(string username, string password, UserRolesEnum role)
        {
            Username = username;
            Password = password;
            Role = role;

            Shipments = new List<Shipment>();
            Receipts = new List<Receipt>();
            Reports = new List<Report>();
        }
    }
}
