using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Passwords { get; set; } = null!;

    public int RoleId { get; set; }

    public bool AccountStatus { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Email { get; set; }

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual UserRole Role { get; set; } = null!;
}
