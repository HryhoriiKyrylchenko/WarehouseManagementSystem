using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarehouseManagementSystem.ViewModels.Helpers
{
    public static class MessageHelper
    {
        public static void ShowCautionMessage(string message)
        {
            MessageBox.Show(message, "Caution", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowInfoMessage(string message)
        {
            MessageBox.Show(message, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
