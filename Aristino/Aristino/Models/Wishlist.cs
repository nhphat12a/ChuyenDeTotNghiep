using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class Wishlist
{
    public int WishlistId { get; set; }

    public int? CustomerId { get; set; }

    public int? ProductId { get; set; }

    public string? Color { get; set; }

    public string? Size { get; set; }

    public int? Quantity { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
