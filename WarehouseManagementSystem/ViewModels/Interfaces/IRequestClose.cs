using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.ViewModels.Interfaces
{
    public interface IRequestClose
    {
        event EventHandler? RequestClose;

        void CloseParentWindow();
    }
}
