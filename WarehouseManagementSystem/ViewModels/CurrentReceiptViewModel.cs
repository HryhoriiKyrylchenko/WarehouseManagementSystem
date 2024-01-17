using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Models.Entities.Support_classes;

namespace WarehouseManagementSystem.ViewModels
{
    public class CurrentReceiptViewModel : ViewModelBase
    {
        private DateTime? receiptDate;
        public DateTime? ReceiptDate 
        {
            get { return receiptDate; }
            set
            {
                if (receiptDate != value)
                {
                    receiptDate = value;
                    OnPropertyChanged(nameof(ReceiptDate));
                }
            }
        }

        private Supplier? supplier;
        public Supplier? Supplier
        {
            get { return supplier; }
            set
            {
                if (supplier != value)
                {
                    supplier = value;
                    OnPropertyChanged(nameof(Supplier));
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

        private ObservableCollection<ReceiptItemViewModel> receiptItems;
        public ObservableCollection<ReceiptItemViewModel> ReceiptItems
        {
            get { return receiptItems; }
            set
            {
                if (receiptItems != value)
                {
                    receiptItems = value;
                    OnPropertyChanged(nameof(ReceiptItems));
                }
            }
        }

        public CurrentReceiptViewModel()
        {
            receiptItems = new ObservableCollection<ReceiptItemViewModel>();
        }

        public void RefreshReceiptItems()
        {
            OnPropertyChanged(nameof(ReceiptItems));
        }
    }
}
