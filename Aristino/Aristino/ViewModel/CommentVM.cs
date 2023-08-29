using Aristino.Models;
using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
    public class CommentVM
    {
        public int CommentId { get; set; }
        [Required(ErrorMessage ="Title Cannot Be Null")]
        [StringLength(100,ErrorMessage = "Tiêu Đề Không Được Để Trống")]
        public string Title { get; set; } = null!;

        public int? CustomerId { get; set; }
        [StringLength(300,ErrorMessage ="Bình Luận Chỉ Có Thể Có Tối Đa 300 Ký Tự")]
        public string? CommentContent { get; set; }

        public string? CommentedDate { get; set; } = null!;
		public int? ProductId { get; set; }
        [Required(ErrorMessage ="Vui Lòng Đánh Giá Sao Sản Phẩm")]
		public int? StarRating { get; set; }

        public virtual CustomerVM? Customer { get; set; } = null!;
    }
}
