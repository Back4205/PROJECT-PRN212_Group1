using System;
using System.Collections.Generic;

namespace LaptopShop.Entities.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Shipment? Shipment { get; set; }
    public decimal TotalAmountOrder 
    {
        get
        {
           
            if (OrderItems == null) return 0;

          
            return OrderItems.Sum(item => item.SnapshotPrice);
        }
    }
}
