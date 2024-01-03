using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.ViewModels;

namespace WarehouseManagementSystem.Windows
{
    /// <summary>
    /// Interaction logic for SupportWindow.xaml
    /// </summary>
    public partial class SupportWindow : Window, INotifyPropertyChanged
    {
        private ViewModelBaseRequestClose currentViewModel;

        public ViewModelBaseRequestClose CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                if (currentViewModel != value)
                {
                    currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }
        public SupportWindow(ViewModelBaseRequestClose viewModel)
        {
            InitializeComponent();
            currentViewModel = viewModel;
            currentViewModel.RequestClose += UserControl_RequestClose;
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void UserControl_RequestClose(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
