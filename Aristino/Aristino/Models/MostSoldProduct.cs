using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class MostSoldProduct
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int Sold { get; set; }

    public bool IsReset { get; set; }

    public virtual Product? Product { get; set; }
}
