using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class UserRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();
}
