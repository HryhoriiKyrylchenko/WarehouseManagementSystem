﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Models.Builders
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
