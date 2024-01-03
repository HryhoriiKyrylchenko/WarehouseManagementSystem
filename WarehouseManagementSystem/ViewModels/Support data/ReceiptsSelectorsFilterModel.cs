using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.ViewModels.Support_data
{
    public class ReceiptsSelectorsFilterModel : ViewModelBase
    {
        private ReceiptsViewModel currentViewModel;

        private bool sectionAllSuppliersSelected;

        public bool SectionAllSuppliersSelected
        {
            get { return sectionAllSuppliersSelected; }
            set
            {
                if (sectionAllSuppliersSelected != value)
                {
                    sectionAllSuppliersSelected = value;
                    OnPropertyChanged(nameof(SectionAllSuppliersSelected));

                    if (value)
                    {
                        SectionBySupplierSelected = false;
                    }
                }
            }
        }

        private bool sectionBySupplierSelected;

        public bool SectionBySupplierSelected
        {
            get { return sectionBySupplierSelected; }
            set
            {
                if (sectionBySupplierSelected != value)
                {
                    sectionBySupplierSelected = value;
                    OnPropertyChanged(nameof(SectionBySupplierSelected));

                    if (value)
                    {
                        SectionAllSuppliersSelected = false;
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

        private Supplier? selectedSupplier;

        public Supplier? SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                if (selectedSupplier != value)
                {
                    selectedSupplier = value;
                    OnPropertyChanged(nameof(SelectedSupplier));
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

        public ReceiptsSelectorsFilterModel(ReceiptsViewModel currentViewModel)
        {
            this.currentViewModel = currentViewModel;
        }
    }
}
