using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.ViewModels.Support_data
{
    public class ProductsSelectorsFilterModel : ViewModelBase
    {
        private ProductsViewModel currentViewModel;

        private bool sectionUnallocatedSelected;

        public bool SectionUnallocatedSelected
        {
            get { return sectionUnallocatedSelected; }
            set
            {
                if (sectionUnallocatedSelected != value)
                {
                    sectionUnallocatedSelected = value;
                    OnPropertyChanged(nameof(SectionUnallocatedSelected));

                    if (value)
                    {
                        SectionInStockSelected = false;
                        SectionNotInStockSelected = false;
                        SectionAllProductsSelected = false;
                        currentViewModel.UpdateFilteredProducts();
                    }
                }
            }
        }

        private bool sectionInStockSelected;

        public bool SectionInStockSelected
        {
            get { return sectionInStockSelected; }
            set
            {
                if (sectionInStockSelected != value)
                {
                    sectionInStockSelected = value;
                    OnPropertyChanged(nameof(SectionInStockSelected));

                    if (value)
                    {
                        SectionUnallocatedSelected = false;
                        SectionNotInStockSelected = false;
                        SectionAllProductsSelected = false;
                        currentViewModel.UpdateFilteredProducts();
                    }
                }
            }
        }

        private bool sectionNotInStockSelected;

        public bool SectionNotInStockSelected
        {
            get { return sectionNotInStockSelected; }
            set
            {
                if (sectionNotInStockSelected != value)
                {
                    sectionNotInStockSelected = value;
                    OnPropertyChanged(nameof(SectionNotInStockSelected));

                    if (value)
                    {
                        SectionUnallocatedSelected = false;
                        SectionInStockSelected = false;
                        SectionAllProductsSelected = false;
                        currentViewModel.UpdateFilteredProducts();
                    }
                }
            }
        }

        private bool sectionAllProductsSelected;

        public bool SectionAllProductsSelected
        {
            get { return sectionAllProductsSelected; }
            set
            {
                if (sectionAllProductsSelected != value)
                {
                    sectionAllProductsSelected = value;
                    OnPropertyChanged(nameof(SectionAllProductsSelected));

                    if (value)
                    {
                        SectionUnallocatedSelected = false;
                        SectionInStockSelected = false;
                        SectionNotInStockSelected = false;
                        currentViewModel.UpdateFilteredProducts(); 
                    }
                }
            }
        }

        public ProductsSelectorsFilterModel(ProductsViewModel currentViewModel)
        {
            this.currentViewModel = currentViewModel;
            SectionInStockSelected = true;
        }
    }
}
