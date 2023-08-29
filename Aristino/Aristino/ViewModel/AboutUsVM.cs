using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
	public class AboutUsVM
	{
		public int Id { get; set; }
		[Required(ErrorMessage ="Địa Chỉ Không Được Bỏ Trống")]
		[StringLength(500,MinimumLength =5,ErrorMessage ="Địa Chỉ Chứa Tối Thiểu 5 Ký Tự Và Tối Đa 500 Ký Tự")]
		public string? ShopAddress { get; set; }
		[Required(ErrorMessage ="Tên Thành Phố Không Được Để Trống")]
		[StringLength(100,ErrorMessage ="Tên Thành Phố Chứa Tối Đa 100 Ký Tự")]
		public string? City { get; set; }
		[Required(ErrorMessage ="Tên Tỉnh Không Được Bỏ Trống")]
		[StringLength(100,ErrorMessage ="Tên Tỉnh Chứa Tối Đa 10 Ký Tự")]
		public string? Province { get; set; }
		[Required(ErrorMessage ="Số Điện Thoại Không Được Bỏ Trống")]
		[StringLength(20,ErrorMessage ="Số Điện Thoại Dài Tối Đa 20 Số")]
		public string? PhoneNumber { get; set; }
		[Required(ErrorMessage ="Email Không Được Bỏ Trống")]
		[StringLength(50,ErrorMessage ="Email Dài Tối Đa 50 Ký Tự")]
		public string? Email { get; set; }
	}
}
