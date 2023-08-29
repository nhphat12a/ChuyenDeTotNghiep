using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
    public class SizeVM
    {
        public int SizeId { get; set; }
        [Required(ErrorMessage ="Size Không Được Bỏ Trống")]
        [StringLength(10,ErrorMessage ="Tên Size Chứa Tối Đa 10 Ký Tự")]
        public string SizeName { get; set; } = null!;
    }
}
