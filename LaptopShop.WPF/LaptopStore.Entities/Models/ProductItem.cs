using System;
using System.Collections.Generic;

namespace LaptopShop.Entities.Models;

public partial class ProductItem
{
    public int ProductItemId { get; set; }

    public string SerialNumber { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int ProductId { get; set; }

    public int WarehouseId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Product Product { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
