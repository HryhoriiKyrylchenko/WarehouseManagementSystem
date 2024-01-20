using Newtonsoft.Json.Linq;
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
        private static Dictionary<UserRolesEnum, List<UserPermissionEnum>> RolePermissions;

        static AccessManager()
        {
            RolePermissions = new Dictionary<UserRolesEnum, List<UserPermissionEnum>>();

            RolePermissions[UserRolesEnum.ADMIN] = new List<UserPermissionEnum> { UserPermissionEnum.ViewAllData, 
                                                                                UserPermissionEnum.ManageAllData };
            RolePermissions[UserRolesEnum.DIRECTOR] = new List<UserPermissionEnum> { UserPermissionEnum.ViewAllData, 
                                                                                UserPermissionEnum.CreateAllReports };
            RolePermissions[UserRolesEnum.MANAGER] = new List<UserPermissionEnum> { UserPermissionEnum.ViewAllData, 
                                                                                UserPermissionEnum.AddProducts, 
                                                                                UserPermissionEnum.EditProducts, 
                                                                                UserPermissionEnum.ManageAllReceipts, 
                                                                                UserPermissionEnum.ManageAllShipments, 
                                                                                UserPermissionEnum.CreateAllReports };
            RolePermissions[UserRolesEnum.WAREHOUSE_WORKER] = new List<UserPermissionEnum> { UserPermissionEnum.ViewProducts, 
                                                                                UserPermissionEnum.AddProducts, 
                                                                                UserPermissionEnum.ViewSelfReceipts, 
                                                                                UserPermissionEnum.ManageSelfReceipts,
                                                                                UserPermissionEnum.ViewSelfShipments, 
                                                                                UserPermissionEnum.ManageSelfShipments, 
                                                                                UserPermissionEnum.ViewSelfReports, 
                                                                                UserPermissionEnum.CreateSelfReports };
            RolePermissions[UserRolesEnum.ACCOUNTANT] = new List<UserPermissionEnum> { UserPermissionEnum.ViewProducts, 
                                                                                UserPermissionEnum.ViewAllReceipts, 
                                                                                UserPermissionEnum.ViewAllShipments, 
                                                                                UserPermissionEnum.ViewSelfReports, 
                                                                                UserPermissionEnum.CreateAllReports };
            RolePermissions[UserRolesEnum.SALESPERSON] = new List<UserPermissionEnum> { UserPermissionEnum.ViewProducts, 
                                                                                UserPermissionEnum.ViewSelfReceipts, 
                                                                                UserPermissionEnum.ManageSelfReceipts, 
                                                                                UserPermissionEnum.ViewSelfShipments, 
                                                                                UserPermissionEnum.ManageSelfShipments, 
                                                                                UserPermissionEnum.ViewSelfReports, 
                                                                                UserPermissionEnum.CreateSelfReports };
            RolePermissions[UserRolesEnum.GUEST] = new List<UserPermissionEnum> { UserPermissionEnum.None };
            RolePermissions[UserRolesEnum.UNDEFINED] = new List<UserPermissionEnum> { UserPermissionEnum.None };
        }

        public static bool CanUserPerformAction(UserRolesEnum userRole, UserPermissionEnum[] permissions)
        {
            if (RolePermissions.TryGetValue(userRole, out var userPermissions))
            {
                return userPermissions.Any(value => permissions.Contains(value));
            }

            return false;
        }

    }
}
