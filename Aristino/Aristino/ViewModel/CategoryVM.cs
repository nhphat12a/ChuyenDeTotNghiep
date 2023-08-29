using Aristino.Models;
using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Name cannot be null")]

        public string CategoryName { get; set; } = null!;

        public string? CategoryDescription { get; set; }

        public string? Avatar { get; set; }

        public virtual ICollection<ProductVM> ProductVMs { get; } = new List<ProductVM>();
        public List<IFormFile>? UploadImage { get; set; }
    }
}
