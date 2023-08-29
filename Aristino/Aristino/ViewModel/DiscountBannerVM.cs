using Aristino.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
	public class DiscountBannerVM
	{
		public int DiscountId { get; set; }

		[Required(ErrorMessage = "Discount Name Cannot Be Null")]

		public string? DiscountName { get; set; }

		public string? DiscountDescription { get; set; }
		[Required(ErrorMessage = "Rate Cannot Be Null or Invalid")]
		[Range(0, 100, ErrorMessage = "Discount Rate Range Must Be From 0 to 100")]
		public int? DiscountRate { get; set; }
		public string? Banner { get; set; }
		[Required(ErrorMessage = "Category Cannot Be null")]
		public int? CategoryId { get; set; }
		public string? StartSale { get; set; }
		public string? EndSale { get; set; }
		[Required(ErrorMessage ="Giờ Hết Hạn Không Thể Bỏ Trống")]
		public DateTime EndTime { get; set; }
        [Required(ErrorMessage = "Ngày Hết Hạn Không Thể Bỏ Trống")]
        public DateTime EndDate { get; set; }

		public bool? DisableDiscount { get; set; }
		public virtual CategoryVM? Category { get; set; }
		[Required(ErrorMessage = "Image Cannot be null")]
		public List<IFormFile>? UploadImage { get; set; }
		public virtual ICollection<ProductVM> Products { get; } = new List<ProductVM>();
	}
}
