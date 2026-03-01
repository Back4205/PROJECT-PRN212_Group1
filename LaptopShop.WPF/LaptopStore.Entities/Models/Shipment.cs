using System;
using System.Collections.Generic;

namespace LaptopShop.Entities.Models;

public partial class Shipment
{
    public int ShipmentId { get; set; }

    public int OrderId { get; set; }

    public DateTime? ShipDate { get; set; }

    public string ShipAddress { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
