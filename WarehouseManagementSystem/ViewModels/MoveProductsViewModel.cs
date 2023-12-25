using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.ViewModels
{
    public class MoveProductsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

        public MoveProductsViewModel(MainViewModel mainViewModel) 
        {
            this.mainViewModel = mainViewModel;
        }

        public ICommand BackCommand => new RelayCommand(Back);

        private void Back(object parameter)
        {
            mainViewModel.NavigateBack();
        }
    }
}
