using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class AristinoPolicy
{
    public int PolicyId { get; set; }

    public string? PolicyTitle { get; set; }

    public string? PolicyContent { get; set; }
}
