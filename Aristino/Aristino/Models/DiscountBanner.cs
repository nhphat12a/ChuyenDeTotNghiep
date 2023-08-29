using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class DiscountBanner
{
    public int DiscountId { get; set; }

    public string? DiscountName { get; set; }

    public string? DiscountDescription { get; set; }

    public int? DiscountRate { get; set; }

    public string? Banner { get; set; }

    public int? CategoryId { get; set; }

    public string? StartSale { get; set; }

    public string? EndSale { get; set; }

    public bool? DisableDiscount { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
