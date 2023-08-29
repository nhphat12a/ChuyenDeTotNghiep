using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? CategoryDescription { get; set; }

    public string? Avatar { get; set; }

    public virtual ICollection<DiscountBanner> DiscountBanners { get; } = new List<DiscountBanner>();

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
