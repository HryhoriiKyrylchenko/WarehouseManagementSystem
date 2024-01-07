using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Models.Entities.Support_classes;

namespace WarehouseManagementSystem.ViewModels
{
    public class CurrentProductViewModel : ViewModelBase
    {
        private string? productCode;
        public string? ProductCode
        {
            get { return productCode; }
            set
            {
                if (productCode != value)
                {
                    productCode = value;
                    OnPropertyChanged(nameof(ProductCode));
                }
            }
        }

        private string? name;
        public string? Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string? description;
        public string? Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private UnitsOfMeasureEnum? unitOfMeasure;
        public UnitsOfMeasureEnum? UnitOfMeasure
        {
            get { return unitOfMeasure; }
            set
            {
                if (unitOfMeasure != value)
                {
                    unitOfMeasure = value;
                    OnPropertyChanged(nameof(UnitOfMeasure));
                }
            }
        }

        private decimal? quantity;
        public decimal? Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        private string? quantityString;
        public string? QuantityString
        {
            get { return quantityString; }
            set
            {
                if (quantityString != value)
                {
                    quantityString = value;
                    OnPropertyChanged(nameof(QuantityString));
                    UpdateQuantity();
                }
            }
        }

        private int? capacity;
        public int? Capacity
        {
            get { return capacity; }
            set
            {
                if (capacity != value)
                {
                    capacity = value;
                    OnPropertyChanged(nameof(Capacity));
                }
            }
        }

        private string? capacityString;
        public string? CapacityString
        {
            get { return capacityString; }
            set
            {
                if (capacityString != value)
                {
                    capacityString = value;
                    OnPropertyChanged(nameof(CapacityString));
                    UpdateCapacity();
                }
            }
        }

        private Manufacturer? manufacturer;
        public Manufacturer? Manufacturer
        {
            get { return manufacturer; }
            set
            {
                if (manufacturer != value)
                {
                    manufacturer = value;
                    OnPropertyChanged(nameof(Manufacturer));
                }
            }
        }

        private decimal? price;
        public decimal? Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        private string? priceString;
        public string? PriceString
        {
            get { return priceString; }
            set
            {
                if (priceString != value)
                {
                    priceString = value;
                    OnPropertyChanged(nameof(PriceString));
                    UpdatePrice();
                }
            }
        }

        private decimal? discountPercentage;
        public decimal? DiscountPercentage
        {
            get { return discountPercentage; }
            set
            {
                if (discountPercentage != value)
                {
                    discountPercentage = value;
                    OnPropertyChanged(nameof(DiscountPercentage));
                }
            }
        }

        private string? discountPercentageString;
        public string? DiscountPercentageString
        {
            get { return discountPercentageString; }
            set
            {
                if (discountPercentageString != value)
                {
                    discountPercentageString = value;
                    OnPropertyChanged(nameof(DiscountPercentageString));
                    UpdateDiscountPercentage();
                }
            }
        }

        private CategoryViewModel? selectedCategory;

        public CategoryViewModel? SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        private ObservableCollection<ProductDetail>? productDetails;
        public ObservableCollection<ProductDetail>? ProductDetails
        {
            get { return productDetails; }
            set
            {
                if (productDetails != value)
                {
                    this.productDetails = value;
                    OnPropertyChanged(nameof(ProductDetails));
                };
            }
        }

        private ProductDetail? selectedProductDetail;
        public ProductDetail? SelectedProductDetail
        {
            get { return selectedProductDetail; }
            set
            {
                if (selectedProductDetail != value)
                {
                    this.selectedProductDetail = value;
                    OnPropertyChanged(nameof(SelectedProductDetail));
                    if (value != null)
                    {
                        UpdatePropertiesData(value);
                    }
                };
            }
        }

        private string? newProductDetailKey;
        public string? NewProductDetailKey
        {
            get { return newProductDetailKey; }
            set
            {
                if (newProductDetailKey != value)
                {
                    this.newProductDetailKey = value;
                    OnPropertyChanged(nameof(NewProductDetailKey));
                };
            }
        }

        private string? newProductDetailValue;
        public string? NewProductDetailValue
        {
            get { return newProductDetailValue; }
            set
            {
                if (newProductDetailValue != value)
                {
                    this.newProductDetailValue = value;
                    OnPropertyChanged(nameof(NewProductDetailValue));
                };
            }
        }

        private string? additionalInfo;
        public string? AdditionalInfo
        {
            get { return additionalInfo; }
            set
            {
                if (additionalInfo != value)
                {
                    additionalInfo = value;
                    OnPropertyChanged(nameof(AdditionalInfo));
                }
            }
        }

        private ObservableCollection<ProductPhoto>? productPhotos;
        public ObservableCollection<ProductPhoto>? ProductPhotos
        {
            get { return productPhotos; }
            set
            {
                if (productPhotos != value)
                {
                    productPhotos = value;
                    OnPropertyChanged(nameof(ProductPhotos));
                }
            }
        }

        private ProductPhoto? selectedProductPhoto;
        public ProductPhoto? SelectedProductPhoto
        {
            get { return selectedProductPhoto; }
            set
            {
                if (selectedProductPhoto != value)
                {
                    selectedProductPhoto = value;
                    OnPropertyChanged(nameof(SelectedProductPhoto));
                }
            }
        }

        private void UpdatePropertiesData(ProductDetail productDelail)
        {
            NewProductDetailKey = productDelail.Key;
            NewProductDetailValue = productDelail.Value;
        }

        private void UpdateQuantity()
        {
            if (!string.IsNullOrWhiteSpace(QuantityString))
            {
                decimal.TryParse(QuantityString, out decimal res);
                Quantity = res;
            }
        }

        private void UpdateCapacity()
        {
            if (!string.IsNullOrWhiteSpace(CapacityString))
            {
                int.TryParse(CapacityString, out int res);
                Capacity = res;
            }
        }
        
        private void UpdatePrice()
        {
            if (!string.IsNullOrWhiteSpace(PriceString))
            {
                decimal.TryParse(PriceString, out decimal res);
                Price = res;
            }
        }

        private void UpdateDiscountPercentage()
        {
            if (!string.IsNullOrWhiteSpace(DiscountPercentageString))
            {
                decimal.TryParse(DiscountPercentageString, out decimal res);
                DiscountPercentage = res;
            }
        }

        public void RefreshSelectedProductDetail()
        {
            OnPropertyChanged(nameof(ProductDetails));
        }
    }
}
