using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.ViewModels.Support_data
{
    public class ShipmentsSelectorsFilterModel : ViewModelBase
    {
        private ShipmentsViewModel currentViewModel;

        private bool sectionAllCustomersSelected;

        public bool SectionAllCustomersSelected
        {
            get { return sectionAllCustomersSelected; }
            set
            {
                if (sectionAllCustomersSelected != value)
                {
                    sectionAllCustomersSelected = value;
                    OnPropertyChanged(nameof(SectionAllCustomersSelected));

                    if (value)
                    {
                        SectionByCustomerSelected = false;
                    }
                }
            }
        }

        private bool sectionByCustomerSelected;

        public bool SectionByCustomerSelected
        {
            get { return sectionByCustomerSelected; }
            set
            {
                if (sectionByCustomerSelected != value)
                {
                    sectionByCustomerSelected = value;
                    OnPropertyChanged(nameof(SectionByCustomerSelected));

                    if (value)
                    {
                        SectionAllCustomersSelected = false;
                    }
                }
            }
        }

        private bool sectionAllUsersSelected;

        public bool SectionAllUsersSelected
        {
            get { return sectionAllUsersSelected; }
            set
            {
                if (sectionAllUsersSelected != value)
                {
                    sectionAllUsersSelected = value;
                    OnPropertyChanged(nameof(SectionAllUsersSelected));

                    if (value)
                    {
                        SectionByUserSelected = false;
                    }
                }
            }
        }

        private bool sectionByUserSelected;

        public bool SectionByUserSelected
        {
            get { return sectionByUserSelected; }
            set
            {
                if (sectionByUserSelected != value)
                {
                    sectionByUserSelected = value;
                    OnPropertyChanged(nameof(SectionByUserSelected));

                    if (value)
                    {
                        SectionAllUsersSelected = false;
                    }
                }
            }
        }

        private DateTime? sectionDateFrom;

        public DateTime? SectionDateFrom
        {
            get { return sectionDateFrom; }
            set
            {
                if (sectionDateFrom != value)
                {
                    sectionDateFrom = value;
                    OnPropertyChanged(nameof(SectionDateFrom));
                }
            }
        }

        private DateTime? sectionDateTo;

        public DateTime? SectionDateTo
        {
            get { return sectionDateTo; }
            set
            {
                if (sectionDateTo != value)
                {
                    sectionDateTo = value;
                    OnPropertyChanged(nameof(SectionDateTo));
                }
            }
        }

        private Customer? selectedCustomer;

        public Customer? SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                if (selectedCustomer != value)
                {
                    selectedCustomer = value;
                    OnPropertyChanged(nameof(SelectedCustomer));
                }
            }
        }

        private User? selectedUser;

        public User? SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }

        public ShipmentsSelectorsFilterModel(ShipmentsViewModel currentViewModel)
        {
            this.currentViewModel = currentViewModel;
        }
    }
}
