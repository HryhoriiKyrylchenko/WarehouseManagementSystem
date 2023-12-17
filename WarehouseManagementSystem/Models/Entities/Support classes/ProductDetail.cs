using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Entities.Support_classes
{
    public class ProductDetail
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public ProductDetail(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
