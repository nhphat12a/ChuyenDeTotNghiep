using Aristino.Models;
using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
    public class FashionCollectionVM
    {
        public int CollectionId { get; set; }
        [Required(ErrorMessage ="Tên Bộ Sưu Tập Không Được Để Trống")]
        [StringLength(200,MinimumLength=5,ErrorMessage ="Tên Bộ Sưu Tập Dài Tối Thiểu 5 Ký Tự Và Tối Đa 200 Ký Tự")]
        public string? CollectionName { get; set; }
        [Required(ErrorMessage ="Mô Tả Không Được Để Trống")]
        [StringLength(1000,MinimumLength =10,ErrorMessage ="Mô Tả Dài Tối Thiểu 10 Ký Tự Và Tối Đa 300 Ký Tự")]
        public string? CollectionDes { get; set; }
        public string? CollectionThumbnail { get; set; }
        public bool? CollectionDisable { get; set; }
        [Required(ErrorMessage = "Hình Không Được Bỏ Trống")]
        public List<IFormFile> UploadImage { get; set; }
        public virtual ICollection<ProductVM> Products { get; } = new List<ProductVM>();
    }
}
