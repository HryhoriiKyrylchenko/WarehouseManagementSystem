using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.ViewModels.Support_data
{
    public class ProductsCategoriesSelectorsFilterModel : ViewModelBase
    {
        private ProductsViewModel currentViewModel;

        private bool checkboxAllCategoriesChecked;

        public bool CheckboxAllCategoriesChecked
        {
            get { return checkboxAllCategoriesChecked; }
            set
            {
                checkboxAllCategoriesChecked = value;
                OnPropertyChanged(nameof(CheckboxAllCategoriesChecked));

                if (value)
                {
                    CheckboxAllCategoriesUnchecked = false;
                    currentViewModel.UpdateProducts();
                }
                else
                {
                    CheckboxAllCategoriesUnchecked = true;
                }
            }
        }

        private bool checkboxAllCategoriesUnchecked;

        public bool CheckboxAllCategoriesUnchecked
        {
            get { return checkboxAllCategoriesUnchecked; }
            set
            {
                if (checkboxAllCategoriesUnchecked != value)
                {
                    checkboxAllCategoriesUnchecked = value;
                    OnPropertyChanged(nameof(CheckboxAllCategoriesUnchecked));
                }
            }
        }

        public ProductsCategoriesSelectorsFilterModel(ProductsViewModel currentViewModel)
        {
            this.currentViewModel = currentViewModel;
        }
    }
}
