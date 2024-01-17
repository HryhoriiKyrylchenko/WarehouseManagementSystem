using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.ViewModels
{
    public class ManufacturerViewModel : ViewModelBase 
    {
        private string? name;
        public string? Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    this.name = value;
                    OnPropertyChanged(nameof(Name));
                };
            }
        }

        private string? description;
        public string? Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    this.description = value;
                    OnPropertyChanged(nameof(Description));
                };
            }
        }

        private Address? address;
        public Address? Address
        {
            get { return address; }
            set
            {
                if (address != value)
                {
                    this.address = value;
                    OnPropertyChanged(nameof(Address));
                };
            }
        }

        private string? additionalInfo;
        public string? AdditionalInfo
        {
            get { return additionalInfo; }
            set
            {
                if (additionalInfo != value)
                {
                    this.additionalInfo = value;
                    OnPropertyChanged(nameof(AdditionalInfo));
                };
            }
        }
    }
}
