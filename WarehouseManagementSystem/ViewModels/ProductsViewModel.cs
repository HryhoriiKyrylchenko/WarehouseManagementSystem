using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

        public ObservableCollection<CategoryViewModel> Categories { get; set; }

        public ProductsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            Categories = new ObservableCollection<CategoryViewModel>();

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await InitializwCategoriesFromDBAsync();
        }

        private async Task InitializwCategoriesFromDBAsync()
        {
            using (var dbManager = new WarehouseDBManager(new WarehouseDbContext()))
            {
                var rootCategories = await dbManager.GetRootCategoriesAsync();

                foreach (var rootCategory in rootCategories)
                {
                    var rootViewModel = await dbManager.BuildCategoryViewModelTreeAsync(rootCategory);
                    Categories.Add(rootViewModel);
                }
            }           
        }

        public ICommand BackCommand => new RelayCommand(Back);

        private void Back(object parameter)
        {
            mainViewModel.NavigateBack();
        }
    }
}
