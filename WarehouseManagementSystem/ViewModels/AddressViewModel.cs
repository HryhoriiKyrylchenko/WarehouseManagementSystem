using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.ViewModels
{
    public class AddressViewModel : ViewModelBase
    {
        private string? country;
        public string? Country
        {
            get { return country; }
            set
            {
                if (country != value)
                {
                    this.country = value;
                    OnPropertyChanged(nameof(Country));
                };
            }
        }

        private string? index;
        public string? Index
        {
            get { return index; }
            set
            {
                if (index != value)
                {
                    this.index = value;
                    OnPropertyChanged(nameof(Index));
                };
            }
        }

        private string? city;
        public string? City
        {
            get { return city; }
            set
            {
                if (city != value)
                {
                    this.city = value;
                    OnPropertyChanged(nameof(City));
                };
            }
        }

        private string? street;
        public string? Street
        {
            get { return street; }
            set
            {
                if (street != value)
                {
                    this.street = value;
                    OnPropertyChanged(nameof(Street));
                };
            }
        }

        private string? buildingNumber;
        public string? BuildingNumber
        {
            get { return buildingNumber; }
            set
            {
                if (buildingNumber != value)
                {
                    this.buildingNumber = value;
                    OnPropertyChanged(nameof(BuildingNumber));
                };
            }
        }

        private string? room;
        public string? Room
        {
            get { return room; }
            set
            {
                if (room != value)
                {
                    this.room = value;
                    OnPropertyChanged(nameof(Room));
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
