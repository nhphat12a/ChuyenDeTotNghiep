using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class UserStatus
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();
}
