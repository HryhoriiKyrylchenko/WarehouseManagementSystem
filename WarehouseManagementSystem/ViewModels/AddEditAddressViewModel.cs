using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models.Builders;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.ViewModels
{
    internal class AddEditAddressViewModel : ViewModelBaseRequestClose
    {
        private AddManufacturerViewModel mainViewModel;

        private Address? address;
        public Address? Address
        {
            get { return address; }
            set
            {
                if (address != value)
                {
                    this.address = value;
                    OnPropertyChanged(nameof(Address));
                };
            }
        }

        private AddressViewModel addressViewModel;
        public AddressViewModel AddressViewModel
        {
            get { return addressViewModel; }
            set
            {
                if (addressViewModel != value)
                {
                    this.addressViewModel = value;
                    OnPropertyChanged(nameof(AddressViewModel));
                };
            }
        }

        public ICommand AddCommand => new RelayCommand(SetAddress);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public AddEditAddressViewModel(AddManufacturerViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            addressViewModel = new AddressViewModel();
        }

        public AddEditAddressViewModel(AddManufacturerViewModel mainViewModel, Address address)
        {
            this.mainViewModel = mainViewModel;
            addressViewModel = new AddressViewModel();
            Address = address;
            InitializeAddressViewModel(address);
        }

        private void InitializeAddressViewModel(Address address)
        {
            AddressViewModel.Country = address.Country;
            AddressViewModel.Index = address.Index;
            AddressViewModel.City = address.City;
            AddressViewModel.Street = address.Street;
            AddressViewModel.BuildingNumber = address.BuildingNumber;
            AddressViewModel.Room = address.Room;
            AddressViewModel.AdditionalInfo = address.AdditionalInfo;
        }

        private void SetAddress(object obj)
        {
            if (GetConfirmation() == MessageBoxResult.OK)
            {
                if (IsAddressDataValid())
                {
                    if (Address != null)
                    {
                        if (AddressViewModel.Country != null
                            && Address.Country != AddressViewModel.Country)
                        {
                            Address.Country = AddressViewModel.Country;
                        }
                        if (AddressViewModel.Index != null
                            && Address.Index != AddressViewModel.Index)
                        {
                            Address.Index = AddressViewModel.Index;
                        }
                        if (AddressViewModel.City != null
                            && Address.City != AddressViewModel.City)
                        {
                            Address.City = AddressViewModel.City;
                        }
                        if (AddressViewModel.Street != null
                            && Address.Street != AddressViewModel.Street)
                        {
                            Address.Street = AddressViewModel.Street;
                        }
                        if (AddressViewModel.BuildingNumber != null
                            && Address.BuildingNumber != AddressViewModel.BuildingNumber)
                        {
                            Address.BuildingNumber = AddressViewModel.BuildingNumber;
                        }
                        if (!string.IsNullOrWhiteSpace(AddressViewModel.Room)
                            && Address.Room != AddressViewModel.Room)
                        {
                            Address.Room = AddressViewModel.Room;
                        }
                        if (!string.IsNullOrWhiteSpace(AddressViewModel.AdditionalInfo)
                            && Address.AdditionalInfo != AddressViewModel.AdditionalInfo)
                        {
                            Address.AdditionalInfo = AddressViewModel.AdditionalInfo;
                        }

                        try
                        {
                            using (EntityManager db = new EntityManager(new Models.WarehouseDbContext()))
                            {
                                db.UpdateAddress(Address);
                            }

                            mainViewModel.ManufacturerViewModel.Address = Address;
                            CloseParentWindow();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Some error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                            {
                                logger.LogError(ex);
                            }

                        }
                    }
                    else
                    {
                        try
                        {
                            if (AddressViewModel.Country != null
                            && AddressViewModel.Index != null
                            && AddressViewModel.City != null
                            && AddressViewModel.Street != null
                            && AddressViewModel.BuildingNumber != null)
                            {
                                var tempAddress = new AddressBuilder(AddressViewModel.Country,
                                AddressViewModel.Index,
                                AddressViewModel.City,
                                AddressViewModel.Street,
                                AddressViewModel.BuildingNumber);


                                if (!string.IsNullOrWhiteSpace(AddressViewModel.Room))
                                {
                                    tempAddress = tempAddress.WithRoom(AddressViewModel.Room);
                                }
                                if (!string.IsNullOrWhiteSpace(AddressViewModel.AdditionalInfo))
                                {
                                    tempAddress = tempAddress.WithAdditionalInfo(AddressViewModel.AdditionalInfo);
                                }

                                mainViewModel.ManufacturerViewModel.Address = Address;
                                CloseParentWindow();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Some error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            Task.Run(async () =>
                            {
                                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                                {
                                    await logger.LogErrorAsync(ex);
                                }
                            }).Wait();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid address data, enter valid data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                mainViewModel.ManufacturerViewModel.Address = Address;
                CloseParentWindow();
            }
        }

        private bool IsAddressDataValid()
        {
            if (string.IsNullOrWhiteSpace(AddressViewModel.Country)
                || string.IsNullOrWhiteSpace(AddressViewModel.Index)
                || string.IsNullOrWhiteSpace(AddressViewModel.City)
                || string.IsNullOrWhiteSpace(AddressViewModel.Street)
                || string.IsNullOrWhiteSpace(AddressViewModel.BuildingNumber))
            {
                return false;
            }
            return true;
        }

        private MessageBoxResult GetConfirmation()
        {
            return MessageBox.Show("Do you want to make this changes?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }

        private void Cancel(object obj)
        {
            CloseParentWindow();
        }
    }
}