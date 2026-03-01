using System;
using System.Collections.Generic;

namespace LaptopShop.Entities.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? ProductItemId { get; set; }

    public string SnapshotProductName { get; set; } = null!;

    public decimal SnapshotPrice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ProductItem? ProductItem { get; set; }
}
