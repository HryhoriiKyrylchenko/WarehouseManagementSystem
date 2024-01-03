using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Enums;

namespace WarehouseManagementSystem.Services
{
    public class AccessManager
    {
        private static Dictionary<UserRolesEnum, List<UserPermissions>> RolePermissions;

        static AccessManager()
        {
            RolePermissions = new Dictionary<UserRolesEnum, List<UserPermissions>>();

            RolePermissions[UserRolesEnum.ADMIN] = new List<UserPermissions> { UserPermissions.ViewAllData, 
                                                                                UserPermissions.ManageAllData };
            RolePermissions[UserRolesEnum.DIRECTOR] = new List<UserPermissions> { UserPermissions.ViewAllData, 
                                                                                UserPermissions.CreateAllReports };
            RolePermissions[UserRolesEnum.MANAGER] = new List<UserPermissions> { UserPermissions.ViewAllData, 
                                                                                UserPermissions.AddProducts, 
                                                                                UserPermissions.EditProducts, 
                                                                                UserPermissions.ManageAllReceipts, 
                                                                                UserPermissions.ManageAllShipments, 
                                                                                UserPermissions.CreateAllReports };
            RolePermissions[UserRolesEnum.WAREHOUSE_WORKER] = new List<UserPermissions> { UserPermissions.ViewProducts, 
                                                                                UserPermissions.AddProducts, 
                                                                                UserPermissions.ViewSelfReceipts, 
                                                                                UserPermissions.ManageSelfReceipts,
                                                                                UserPermissions.ViewSelfShipments, 
                                                                                UserPermissions.ManageSelfShipments, 
                                                                                UserPermissions.ViewSelfReports, 
                                                                                UserPermissions.CreateSelfReports };
            RolePermissions[UserRolesEnum.ACCOUNTANT] = new List<UserPermissions> { UserPermissions.ViewProducts, 
                                                                                UserPermissions.ViewAllReceipts, 
                                                                                UserPermissions.ViewAllShipments, 
                                                                                UserPermissions.ViewSelfReports, 
                                                                                UserPermissions.CreateAllReports };
            RolePermissions[UserRolesEnum.SALESPERSON] = new List<UserPermissions> { UserPermissions.ViewProducts, 
                                                                                UserPermissions.ViewSelfReceipts, 
                                                                                UserPermissions.ManageSelfReceipts, 
                                                                                UserPermissions.ViewSelfShipments, 
                                                                                UserPermissions.ManageSelfShipments, 
                                                                                UserPermissions.ViewSelfReports, 
                                                                                UserPermissions.CreateSelfReports };
            RolePermissions[UserRolesEnum.GUEST] = new List<UserPermissions> { UserPermissions.None };
            RolePermissions[UserRolesEnum.UNDEFINED] = new List<UserPermissions> { UserPermissions.None };
        }

        public static bool CanUserPerformAction(UserRolesEnum userRole, UserPermissions permission)
        {
            if (RolePermissions.TryGetValue(userRole, out var userPermissions))
            {
                return userPermissions.Contains(permission);
            }

            return false;
        }

    }
}
