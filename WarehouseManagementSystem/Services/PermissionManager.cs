using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Attributes;
using WarehouseManagementSystem.Enums;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.ViewModels.Helpers;

namespace WarehouseManagementSystem.Services
{
    public class PermissionManager
    {
        private readonly UserRolesEnum currentUserRole;

        public PermissionManager(UserRolesEnum userRole)
        {
            currentUserRole = userRole;
        }

        public bool CanExecute(object parameter, MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            var customPermissionAttributes = methodInfo.GetCustomAttributes(typeof(AccessPermissionAttribute), true)
                                                      .Cast<AccessPermissionAttribute>();

            var permissions = customPermissionAttributes.SelectMany(attr => attr.Permissions).ToArray();

            var result = AccessManager.CanUserPerformAction(currentUserRole, permissions);

            return result;
        }
    }
}
