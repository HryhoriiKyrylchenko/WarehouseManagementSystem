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

        public ICommand ShowProductsCommand { get; }
        

        public MainViewModel()
        {
            ShowProductsCommand = new RelayCommand(ShowProducts);

            viewModelStack = new Stack<ViewModelBase>();
            CurrentViewModel = this;
        }

        private void ShowProducts(object parameter)
        {
            NavigateToViewModel(new ProductsViewModel(this));
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
