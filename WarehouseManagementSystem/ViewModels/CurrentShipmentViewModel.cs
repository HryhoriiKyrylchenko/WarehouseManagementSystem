using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;

namespace WarehouseManagementSystem.ViewModels
{
    public class CurrentShipmentViewModel : ViewModelBase
    {
        private DateTime? shipmentDate;
        public DateTime? ShipmentDate
        {
            get { return shipmentDate; }
            set
            {
                if (shipmentDate != value)
                {
                    shipmentDate = value;
                    OnPropertyChanged(nameof(ShipmentDate));
                }
            }
        }

        private Customer? customer;
        public Customer? Customer
        {
            get { return customer; }
            set
            {
                if (customer != value)
                {
                    customer = value;
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }

        private string? batchNumber;
        public string? BatchNumber
        {
            get { return batchNumber; }
            set
            {
                if (batchNumber != value)
                {
                    batchNumber = value;
                    OnPropertyChanged(nameof(BatchNumber));
                }
            }
        }

        private string? shipmentNumber;
        public string? ShipmentNumber
        {
            get { return shipmentNumber; }
            set
            {
                if (shipmentNumber != value)
                {
                    shipmentNumber = value;
                    OnPropertyChanged(nameof(ShipmentNumber));
                }
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

        private ObservableCollection<ShipmentItemViewModel> shipmentItems;
        public ObservableCollection<ShipmentItemViewModel> ShipmentItems
        {
            get { return shipmentItems; }
            set
            {
                if (shipmentItems != value)
                {
                    shipmentItems = value;
                    OnPropertyChanged(nameof(ShipmentItems));
                }
            }
        }

        public CurrentShipmentViewModel()
        {
            shipmentItems = new ObservableCollection<ShipmentItemViewModel>();
        }

        public void RefreshShipmentItems()
        {
            OnPropertyChanged(nameof(ShipmentItems));
        }
    }
}
