using Aristino.Models;
using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
	public class BlogVM
	{
		public int BlogId { get; set; }
		[Required(ErrorMessage ="Title Cannot Be Null")]
		[StringLength(200,MinimumLength =5,ErrorMessage = "Title Have A Minimum Length Of 5 And A Maximum Of 200")]

		public string BlogTitle { get; set; } = null!;
		[Required(ErrorMessage ="Content Cannot Be Null")]
		public string BlogContent { get; set; } = null!;

		public string? SourceName { get; set; }

		public string? SourceLink { get; set; }

		public string? PostDate { get; set; }
        public string? Thumbnail { get; set; }
		public int? BlogLike { get; set; }
        public int? CustomerId { get; set; }
        public int? BlogViews { get; set; }
		public List<IFormFile> UploadImage { get; set; }
		public virtual ICollection<BlogCommentVM> BlogComments { get; } = new List<BlogCommentVM>();
		public virtual Customer? Customer { get; set; }
    }
}
