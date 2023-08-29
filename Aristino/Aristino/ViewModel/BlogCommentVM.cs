using Aristino.Models;
using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
	public class BlogCommentVM
	{
		public int BlogCommentId { get; set; }

		public int? CustomerId { get; set; }
		[Required(ErrorMessage ="Tiêu Đề Comment Không Thể Trống")]
		[StringLength(50,ErrorMessage ="Tiêu Đề Không Được Dài Quá 50 Ký Tụ")]
		public string? Title { get; set; }
		[StringLength(300, ErrorMessage = "Comment Can Only Contain Up To 300 Words")]
		public string? CommentContent { get; set; }

		public int? BlogId { get; set; }

		public string? CommentDate { get; set; }

		public virtual BlogVM? Blog { get; set; }

		public virtual CustomerVM? Customer { get; set; }
	}
}
