using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class TransactionStatus
{
    public int TransacId { get; set; }

    public string TransacName { get; set; } = null!;

    public string TransacDes { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
