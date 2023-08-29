using Aristino.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Tên Sản Phẩm Không Thể Bỏ Trống")]
        [StringLength(100,ErrorMessage ="Tên Sản Phẩm Không Được Quá 100 Ký Tự")]
        public string ProductName { get; set; } = null!;
        [Required(ErrorMessage = "Description cannot be null")]

        public string ProductDescription { get; set; } = null!;


        [Range(0, int.MaxValue, ErrorMessage = "Giá Tiền Không Hợp Lệ")]
        [Required(ErrorMessage = "Giá Tiền Không Thể Bỏ Trống")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0}")]
        public int? Price { get; set; }
        [Required(ErrorMessage = "Danh Mục Không Thể Trống")]
        public int? CategoryId { get; set; }
        public string? ProductImage { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số Lượng Không Hợp Lệ")]
        [Required(ErrorMessage = "Số Lượng Không Thể Bỏ Trống")]
        public int? Quantity { get; set; }

        public int? Discount { get; set; }
        public int? DiscountId { get; set; }
        public bool Active { get; set; }
        public string? ProductGallery { get; set; }
        [Required(ErrorMessage ="Mô Tả Ngắn Không Được Bỏ Trông")]
        [StringLength(200,MinimumLength =10,ErrorMessage =("Mô Tả Ngắn Chứa Tổi Thiểu 10 Ký Tự Và Tối Đa 200 Ký Tự"))]
		public string? ShortDes { get; set; }
		public string? StarRate { get; set; }
        public int? CollectionId { get; set; }
        public bool ProductDiscontinue { get; set; }
        public virtual FashionCollectionVM? Collection { get; set; }
        public virtual CategoryVM? Category { get; set; } = null!;
        public virtual DiscountBannerVM? DiscountNavigation { get; set; }
        [Required(ErrorMessage = "Hình Không Thể Bỏ Trống")]
        public List<IFormFile> UploadImage { get; set; }
        [Required(ErrorMessage = "Màu Sắc Không Thể Bỏ Trống")]
        public List<string> ColorList { get; set; }
        [Required(ErrorMessage = "Size Không Thể Bỏ Trống")]
        public List<string> SizeList { get; set; }
    }
}
