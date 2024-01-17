using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarehouseManagementSystem.ViewModels.Helpers
{
    public static class ConfirmationHelper
    {
        public static MessageBoxResult GetConfirmation()
        {
            return MessageBox.Show("Do you want to make this changes?",
                "Confirmation",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
        }

        public static MessageBoxResult GetCancelConfirmation()
        {
            return MessageBox.Show("All unsaved data will be lost! Continue?",
                "Confirmation",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
        }
    }
}
