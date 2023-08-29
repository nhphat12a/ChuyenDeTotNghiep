using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public int Price { get; set; }

    public int CategoryId { get; set; }

    public string ProductImage { get; set; } = null!;

    public int Quantity { get; set; }

    public int Discount { get; set; }

    public bool Active { get; set; }

    public string? ProductGallery { get; set; }

    public string? Color { get; set; }

    public string? Size { get; set; }

    public int? DiscountId { get; set; }

    public string? StarRate { get; set; }

    public string? ShortDes { get; set; }

    public int? CollectionId { get; set; }

    public bool? ProductDiscontinue { get; set; }

    public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

    public virtual Category Category { get; set; } = null!;

    public virtual FashionCollection? Collection { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual DiscountBanner? DiscountNavigation { get; set; }

    public virtual ICollection<MostSoldProduct> MostSoldProducts { get; } = new List<MostSoldProduct>();

    public virtual ICollection<OrdersDetail> OrdersDetails { get; } = new List<OrdersDetail>();

    public virtual ICollection<Wishlist> Wishlists { get; } = new List<Wishlist>();
}
