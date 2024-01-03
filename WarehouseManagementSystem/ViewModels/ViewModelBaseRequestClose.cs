using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.ViewModels.Interfaces;

namespace WarehouseManagementSystem.ViewModels
{
    public class ViewModelBaseRequestClose : ViewModelBase, IRequestClose
    {
        public event EventHandler? RequestClose;

        public void CloseParentWindow()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
