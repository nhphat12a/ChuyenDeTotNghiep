using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class ProductGallery
{
    public int GalleryId { get; set; }

    public int? ProductId { get; set; }

    public string? ImageName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Product? Product { get; set; }
}
