using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class FashionCollection
{
    public int CollectionId { get; set; }

    public string? CollectionName { get; set; }

    public string? CollectionDes { get; set; }

    public string? CollectionThumbnail { get; set; }

    public bool? CollectionDisable { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
