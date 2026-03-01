using System;
using System.Collections.Generic;

namespace LaptopShop.Entities.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string WarehouseName { get; set; } = null!;

    public string Location { get; set; } = null!;

    public int ManagerUserId { get; set; }

    public virtual User ManagerUser { get; set; } = null!;

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
}
