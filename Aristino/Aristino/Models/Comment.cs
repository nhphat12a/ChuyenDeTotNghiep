using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string Title { get; set; } = null!;

    public int CustomerId { get; set; }

    public string CommentedDate { get; set; } = null!;

    public int? StarRating { get; set; }

    public string? CommentContent { get; set; }

    public int? ProductId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product? Product { get; set; }
}
