using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;

namespace WarehouseManagementSystem.ViewModels
{
    public class ReceiptsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

        public ReceiptsViewModel(MainViewModel mainViewModel)
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
