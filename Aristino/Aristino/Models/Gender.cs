using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class Gender
{
    public int GenderId { get; set; }

    public string? GenderName { get; set; }

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();
}
