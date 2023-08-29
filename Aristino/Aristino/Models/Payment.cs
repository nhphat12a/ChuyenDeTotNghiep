using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string PaymentName { get; set; } = null!;

    public bool Allowed { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
