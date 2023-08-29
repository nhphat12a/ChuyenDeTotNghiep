using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
    public class ColorVM
    {
        public int ColorId { get; set; }
        [Required(ErrorMessage ="Tên Màu Không Thể Để Trống")]
        public string ColorName { get; set; } = null!;
    }
}
