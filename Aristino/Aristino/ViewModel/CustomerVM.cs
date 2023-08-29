using Aristino.Models;
using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
	public class CustomerVM
	{
		public int CustomersId { get; set; }

		[Required(ErrorMessage ="Họ Không Được Rỗng")]
		public string FirstName { get; set; } = null!;
		[Required(ErrorMessage = "Tên Không Được Rỗng")]
		public string LastName { get; set; } = null!;

		public DateTime? Dob { get; set; }

		public string? CustomerAddress { get; set; }
		[Required(ErrorMessage = "Số Điện Thoại Không Được Rỗng")]
		public int? PhoneNumber { get; set; }

		public string? Email { get; set; }

        public int? StatusId { get; set; }
        public string? Avatar { get; set; }

		public DateTime? CreatedDate { get; set; }

		public int? PostalCode { get; set; }

		public string? Country { get; set; }
        public int? GenderId { get; set; }
		[StringLength(16)]
        public string? CardNumber { get; set; }

        public string? CardOwner { get; set; }

        public string? ExpiredDate { get; set; }
        public int? Cvc { get; set; }

        public virtual ICollection<AccountVM> Accounts { get; } = new List<AccountVM>();
        public virtual ICollection<CartVM> Carts { get; } = new List<CartVM>();
        public virtual ICollection<OrderVM> Orders { get; } = new List<OrderVM>();
        public virtual ICollection<WishlistVM> Wishlists { get; } = new List<WishlistVM>();
        public virtual GenderVM? Gender { get; set; }
        public virtual UserStatusVM? Status { get; set; }
        public List<IFormFile>? UploadImage { get; set; }
	}
}
