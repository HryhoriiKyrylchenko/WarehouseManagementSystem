using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Enums;

namespace WarehouseManagementSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessPermissionAttribute : Attribute
    {
        public UserPermissionEnum[] Permissions { get; }

        public AccessPermissionAttribute(params UserPermissionEnum[] permissions)
        {
            Permissions = permissions;
        }
    }
}
