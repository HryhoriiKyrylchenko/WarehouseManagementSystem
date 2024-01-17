using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.ViewModels.Interfaces;

namespace WarehouseManagementSystem.ViewModels
{
    public class ReportSelectionViewModel : ViewModelBaseRequestClose
    {
        private readonly MainViewModel mainViewModel;

        private ReportTypeEnum? selectedReportType;

        public ReportTypeEnum? SelectedReportType
        {
            get { return selectedReportType; }
            set
            {
                if (selectedReportType != value)
                {
                    selectedReportType = value;
                    OnPropertyChanged(nameof(SelectedReportType));
                }
            }
        }

        public ICommand SelectCommand => new RelayCommand(Select);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public ReportSelectionViewModel(MainViewModel mainViewModel) 
        {
            this.mainViewModel = mainViewModel;
        }

        private void Select(object parameter)
        {
            ViewModelBase? selectedViewModel = null;

            switch (selectedReportType)
            {
                case ReportTypeEnum.PRODUCTS:
                    selectedViewModel = new ProductsViewModel(mainViewModel);
                    break;

                case ReportTypeEnum.MOVEMENTS:
                    selectedViewModel = new MoveProductsViewModel(mainViewModel);
                    break;

                case ReportTypeEnum.RECEIPTS:
                    selectedViewModel = new ReceiptsViewModel(mainViewModel);
                    break;

                case ReportTypeEnum.SHIPMENTS:
                    selectedViewModel = new ShipmentsViewModel(mainViewModel);
                    break;
            }

            if (selectedViewModel != null)
            {
                mainViewModel.NavigateToViewModel(selectedViewModel);
            }
            
            CloseParentWindow();
        }

        private void Cancel(object parameter)
        {
            CloseParentWindow();
        }
    }
}
