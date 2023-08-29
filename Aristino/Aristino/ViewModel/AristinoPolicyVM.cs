using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
	public class AristinoPolicyVM
	{
		public int PolicyId { get; set; }
		[Required(ErrorMessage ="Tên Chính Sách Không Được Rỗng")]
		[StringLength(200,MinimumLength =10,ErrorMessage ="Tên Chính Sách Dài Tối Thiểu 10 Ký Tự Và Tối Đa 200 Ký Tự")]
		public string? PolicyTitle { get; set; }
		[Required(ErrorMessage ="Nội Dung Chính Sách Không Được Rỗng")]
		public string? PolicyContent { get; set; }
	}
}
