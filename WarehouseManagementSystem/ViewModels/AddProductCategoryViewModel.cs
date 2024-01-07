using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WarehouseManagementSystem.Commands;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Models.Builders;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.ViewModels
{
    public class AddProductCategoryViewModel : ViewModelBaseRequestClose
    {
        private AddEditProductViewModel mainViewModel;

        private string? categoryName;
        public string? CategoryName
        {
            get { return categoryName; }
            set
            {
                if (categoryName != value)
                {
                    categoryName = value;
                    OnPropertyChanged(nameof(CategoryName));
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
                    this.additionalInfo = value;
                    OnPropertyChanged(nameof(AdditionalInfo));
                };
            }
        }

        private ObservableCollection<CategoryViewModel> categories;
        public ObservableCollection<CategoryViewModel> Categories
        {
            get { return categories; }
            set
            {
                if (categories != value)
                {
                    categories = value;
                    OnPropertyChanged(nameof(Categories));
                }
            }
        }

        private CategoryViewModel? parentCategory;
        public CategoryViewModel? ParentCategory
        {
            get { return parentCategory; }
            set
            {
                if (parentCategory != value)
                {
                    parentCategory = value;
                    OnPropertyChanged(nameof(ParentCategory));
                }
            }
        }

        private bool selectorWithParentChecked;
        public bool SelectorWithParentChecked
        {
            get { return selectorWithParentChecked; }
            set
            {
                selectorWithParentChecked = value;
                OnPropertyChanged(nameof(SelectorWithParentChecked));

                if (value)
                {
                    SelectorWithoutParentChecked = false;
                }
            }
        }

        private bool selectorWithoutParentChecked;
        public bool SelectorWithoutParentChecked
        {
            get { return selectorWithoutParentChecked; }
            set
            {
                selectorWithoutParentChecked = value;
                OnPropertyChanged(nameof(SelectorWithParentChecked));

                if (value)
                {
                    SelectorWithParentChecked = false;
                }
            }
        }

        public ICommand AddCommand => new RelayCommand(AddProductCategory);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        public AddProductCategoryViewModel(AddEditProductViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            SelectorWithoutParentChecked = true;
            categories = new ObservableCollection<CategoryViewModel>();

            InitializeAsync();
        }

        public async void InitializeAsync()
        {
            await InitializeCategoriesFromDBAsync();
        }

        private async Task InitializeCategoriesFromDBAsync()
        {
            try
            {
                using (var dbManager = new WarehouseDBManager(new WarehouseDbContext()))
                {
                    var rootCategories = await dbManager.GetRootCategoriesAsync(mainViewModel.MainViewModel.MainViewModel.LoginService.CurrentWarehouse);
                    if (rootCategories != null)
                    {
                        foreach (var rootCategory in rootCategories)
                        {
                            var rootViewModel = await dbManager.BuildCategoryViewModelTreeAsync(rootCategory, 
                                mainViewModel.MainViewModel.MainViewModel.LoginService.CurrentWarehouse);
                            Categories.Add(rootViewModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                {
                    await logger.LogErrorAsync(ex);
                }
            }
        }

        private void AddProductCategory(object obj)
        {
            if (GetConfirmation() == MessageBoxResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(CategoryName))
                {
                    try
                    {
                        var newPCB = new ProductCategoryBuilder(CategoryName);

                        if (SelectorWithParentChecked && ParentCategory != null)
                        {
                            newPCB = newPCB.WithPreviousCategory(ParentCategory.Category.Id);
                        }
                        else if (SelectorWithParentChecked && ParentCategory != null)
                        {
                            MessageBox.Show("Select parent cetegory",
                                "Caution",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }

                        if (!string.IsNullOrWhiteSpace(AdditionalInfo))
                        {
                            newPCB = newPCB.WithAdditionalInfo(AdditionalInfo);
                        }

                        CloseParentWindow();
                    }
                    catch (Exception ex)
                    {
                        using (ErrorLogger logger = new ErrorLogger(new Models.WarehouseDbContext()))
                        {
                            logger.LogError(ex);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect data entered", "Caution", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
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