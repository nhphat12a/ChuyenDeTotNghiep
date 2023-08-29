using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string BlogTitle { get; set; } = null!;

    public string BlogContent { get; set; } = null!;

    public string? SourceName { get; set; }

    public string? SourceLink { get; set; }

    public string? PostDate { get; set; }

    public string? Thumbnail { get; set; }

    public int? BlogLike { get; set; }

    public int? BlogViews { get; set; }

    public int? CustomerId { get; set; }

    public virtual ICollection<BlogComment> BlogComments { get; } = new List<BlogComment>();

    public virtual Customer? Customer { get; set; }
}
