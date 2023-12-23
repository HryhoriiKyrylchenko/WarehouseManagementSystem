using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;

namespace WarehouseManagementSystem.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Stack<ViewModelBase> viewModelStack;
        private ViewModelBase? currentViewModel;

        public ViewModelBase CurrentViewModel 
        {
            get { return currentViewModel ?? this; }
            set
            {
                if (currentViewModel != value)
                {
                    currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ShowProductsCommand => new RelayCommand(_ => NavigateToViewModel(new ProductsViewModel(this)));
        public ICommand ShowReceiptsCommand => new RelayCommand(_ => NavigateToViewModel(new ReceiptsViewModel(this)));
        public ICommand ShowShipmentsCommand => new RelayCommand(_ => NavigateToViewModel(new ShipmentsViewModel(this)));
        public ICommand ShowReportsCommand => new RelayCommand(_ => NavigateToViewModel(new ReportsViewModel(this)));
        public ICommand ShowSettingsCommand => new RelayCommand(_ => NavigateToViewModel(new SettingsViewModel(this)));

        public MainViewModel()
        {
            viewModelStack = new Stack<ViewModelBase>();
            CurrentViewModel = this;
        }

        public void NavigateToViewModel(ViewModelBase viewModel)
        {
            viewModelStack.Push(CurrentViewModel); 
            CurrentViewModel = viewModel;
        }

        public void NavigateBack()
        {
            if (viewModelStack.Count > 0)
            {
                CurrentViewModel = viewModelStack.Pop();
            }
        }
    }
}
