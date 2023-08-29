using Aristino.Models;
using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
	public class AccountVM
	{
		public int AccountId { get; set; }
		[Required(ErrorMessage ="tên Người Dùng Không Thể Bị Bỏ Trống")]
		[StringLength(20,MinimumLength =5,ErrorMessage ="Tên Người Dùng Chứa Ít Nhất 5 Ký Tự Và Tối Đa 20 Ký Tự")]
		public string Username { get; set; } = null!;
		[Required(ErrorMessage ="Mật Khẩu Không Thể Rỗng")]
		[MinLength(5,ErrorMessage ="Mật Khẩu Quá Ngắn")]
		public string Passwords { get; set; } = null!;
		public string? ConfirmPasswords { get; set; } = null!;
		[Required(ErrorMessage ="Họ Không Thể Bỏ Trống")]
		[StringLength(50,MinimumLength =2,ErrorMessage = "Họ Chỉ Chứa Ít Nhất 5 Ký Tự Và Tối Đa 20 Ký Tự")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "Tên Không Thể Bị Bỏ Trống")]
		[StringLength(50, MinimumLength = 2, ErrorMessage = "Tên Chỉ Chứa Ít Nhất 5 Ký Tự Và Tối Đa 20 Ký Tự")]
		public string LastName { get; set; }
		[Required(ErrorMessage ="Email Không Thể Bỏ Trống")]
		public string Email { get; set; }
		[Required(ErrorMessage ="Số Điện Thoại Không Được Rỗng")]
		public string? PhoneNumber { get; set; }
		[Required(ErrorMessage ="Vui Lòng Nhập Ngày Sinh")]
        public DateTime? Dob { get; set; }
        public int? RoleId { get; set; }
        public bool? AccountStatus { get; set; }
		public int? CustomerId { get; set; }
		public int? GenderId { get; set; }
		public DateTime? CreatedDate { get; set; }
		public virtual UserRoleVM? Role { get; set; } = null!;
	}
}
