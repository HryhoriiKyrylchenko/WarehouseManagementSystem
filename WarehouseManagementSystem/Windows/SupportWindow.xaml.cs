using System;
using System.Collections.Generic;
using System.Linq;
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
using WarehouseManagementSystem.ViewModels;

namespace WarehouseManagementSystem.Windows
{
    /// <summary>
    /// Interaction logic for SupportWindow.xaml
    /// </summary>
    public partial class SupportWindow : Window
    {
        public SupportWindow(ViewModelBase viewModel)
        {
            InitializeComponent();
            DataContext = new SupportViewModel(viewModel);
        }
    }
}
