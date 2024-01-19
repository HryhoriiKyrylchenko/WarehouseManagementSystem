using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Services
{
    public class EventService
    {
        public static event EventHandler<bool>? VisibilityChanged;

        public static void RaiseVisibilityChanged(bool isVisible)
        {
            VisibilityChanged?.Invoke(null, isVisible);
        }
    }
}
