using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;

namespace WarehouseManagementSystem.ViewModels
{
    public class SupportViewModel : ViewModelBase
    {
        private readonly Stack<ViewModelBase> viewModelStack;

        private ViewModelBase currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                if (currentViewModel != value)
                {
                    currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        //public ICommand ShowProductsCommand => new RelayCommand(_ => NavigateToViewModel(new ProductsViewModel(this)));

        public SupportViewModel(ViewModelBase viewModel)
        {
            viewModelStack = new Stack<ViewModelBase>();
            currentViewModel = viewModel;
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
