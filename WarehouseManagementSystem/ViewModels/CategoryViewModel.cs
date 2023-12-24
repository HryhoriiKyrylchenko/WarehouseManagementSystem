using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.ViewModels
{
    public class CategoryViewModel : ViewModelBase
    {
        public Category Category { get; set; }
        public ObservableCollection<CategoryViewModel> Children { get; set; }
        public ObservableCollection<Product> Products { get; set; }

        public CategoryViewModel(Category category)
        {
            Category = category;
            Children = new ObservableCollection<CategoryViewModel>();
            Products = new ObservableCollection<Product>();
        }
    }
}
