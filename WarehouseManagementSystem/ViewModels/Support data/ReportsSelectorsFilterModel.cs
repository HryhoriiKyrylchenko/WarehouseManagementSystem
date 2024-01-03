using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.ViewModels.Support_data
{
    public class ReportsSelectorsFilterModel : ViewModelBase
    {
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
                }
            }
        }

        private bool sectionByTypeSelected;

        public bool SectionByTypeSelected
        {
            get { return sectionByTypeSelected; }
            set
            {
                if (sectionByTypeSelected != value)
                {
                    sectionByTypeSelected = value;
                    OnPropertyChanged(nameof(SectionByTypeSelected));
                }
            }
        }

        private bool sectionByDateSelected;

        public bool SectionByDateSelected
        {
            get { return sectionByDateSelected; }
            set
            {
                if (sectionByDateSelected != value)
                {
                    sectionByDateSelected = value;
                    OnPropertyChanged(nameof(SectionByDateSelected));
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

        private ReportTypeEnum? selectedType;

        public ReportTypeEnum? SelectedType
        {
            get { return selectedType; }
            set
            {
                if (selectedType != value)
                {
                    selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));
                }
            }
        }

        private DateTime? selectedDate;

        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if (selectedDate != value)
                {
                    selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                }
            }
        }
    }
}
