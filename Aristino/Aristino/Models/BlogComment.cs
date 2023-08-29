using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class BlogComment
{
    public int BlogCommentId { get; set; }

    public int? CustomerId { get; set; }

    public string? Title { get; set; }

    public string? CommentContent { get; set; }

    public int? BlogId { get; set; }

    public string? CommentDate { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual Customer? Customer { get; set; }
}
