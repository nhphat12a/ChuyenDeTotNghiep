using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public int CustomerId { get; set; }

    public int PaymentId { get; set; }

    public DateTime ShipDate { get; set; }

    public int TransacId { get; set; }

    public bool IsPaid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? TotalPrice { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrdersDetail> OrdersDetails { get; } = new List<OrdersDetail>();

    public virtual Payment Payment { get; set; } = null!;

    public virtual TransactionStatus Transac { get; set; } = null!;
}
