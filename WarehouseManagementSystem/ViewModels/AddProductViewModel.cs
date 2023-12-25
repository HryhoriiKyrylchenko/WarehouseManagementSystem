using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Models.Entities.Support_classes;

namespace WarehouseManagementSystem.ViewModels
{
    public class AddProductViewModel : ViewModelBase
    {
        private Product? product;

        public Product? Product
        {
            get { return product; }
            set
            {
                if (product != value)
                {
                    this.product = value;
                    OnPropertyChanged();
                };
            }
        }

        private ObservableCollection<ProductDetail> productDetails;

        public ObservableCollection<ProductDetail> ProductDetails
        {
            get { return productDetails; }
            set
            {
                if (productDetails != value)
                {
                    this.productDetails = value;
                    OnPropertyChanged();
                };
            }
        }

        private ObservableCollection<ProductPhoto> productPhotoes;

        public ObservableCollection<ProductPhoto> ProductPhotoes
        {
            get { return productPhotoes; }
            set
            {
                if (productPhotoes != value)
                {
                    this.productPhotoes = value;
                    OnPropertyChanged();
                };
            }
        }

        public AddProductViewModel()
        {
            productDetails = new ObservableCollection<ProductDetail>();
            productPhotoes = new ObservableCollection<ProductPhoto>();
        }

        public AddProductViewModel(Product product) 
        {
            this.product = product;
            productDetails = new ObservableCollection<ProductDetail>(); ////////////////
            productPhotoes = new ObservableCollection<ProductPhoto>(); ////////////////
        }
    }
}
