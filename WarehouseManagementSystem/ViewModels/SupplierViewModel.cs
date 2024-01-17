using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.ViewModels
{
    public class SupplierViewModel : ViewModelBase 
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
