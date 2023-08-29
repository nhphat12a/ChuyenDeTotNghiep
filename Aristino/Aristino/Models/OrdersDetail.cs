using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class OrdersDetail
{
    public int OdersDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    public int Discount { get; set; }

    public string Size { get; set; } = null!;

    public string Color { get; set; } = null!;

    public int? ProductPrice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
