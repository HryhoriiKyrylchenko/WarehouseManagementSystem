using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Enums
{
    public enum UserPermissionEnum
    {
        None,
        ViewProducts,
        AddProducts,
        EditProducts,
        ViewSelfReceipts,
        ViewAllReceipts,
        ManageSelfReceipts,
        ManageAllReceipts,
        ViewSelfShipments,
        ViewAllShipments,
        ManageSelfShipments,
        ManageAllShipments,
        ViewSelfReports,
        ViewAllReports,
        CreateSelfReports,
        CreateAllReports,
        ViewAllData,
        ManageAllData
    }
}
