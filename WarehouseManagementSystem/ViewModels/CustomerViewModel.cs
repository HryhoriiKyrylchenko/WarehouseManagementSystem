using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {
        private string? firstname;
        public string? Firstname
        {
            get { return firstname; }
            set
            {
                if (firstname != value)
                {
                    this.firstname = value;
                    OnPropertyChanged(nameof(Firstname));
                };
            }
        }

        private string? lastname;
        public string? Lastname
        {
            get { return lastname; }
            set
            {
                if (lastname != value)
                {
                    this.lastname = value;
                    OnPropertyChanged(nameof(Lastname));
                };
            }
        }

        private DateTime? dateOfBirth;
        public DateTime? DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                if (dateOfBirth != value)
                {
                    this.dateOfBirth = value;
                    OnPropertyChanged(nameof(DateOfBirth));
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

        private string? discountPercentage;
        public string? DiscountPercentage
        {
            get { return discountPercentage; }
            set
            {
                if (discountPercentage != value)
                {
                    this.discountPercentage = value;
                    OnPropertyChanged(nameof(DiscountPercentage));
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
