using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Fax { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Homepage { get; set; } = null!;

    public string? Avatar { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
