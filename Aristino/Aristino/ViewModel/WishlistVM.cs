using Aristino.Models;

namespace Aristino.ViewModel
{
    public class WishlistVM
    {
        public int WishlistId { get; set; }

        public int? CustomerId { get; set; }

        public int? ProductId { get; set; }
		public string? Color { get; set; }

		public string? Size { get; set; }
		public int? Quantity { get; set; }
		public virtual CustomerVM? Customer { get; set; }

        public virtual ProductVM? Product { get; set; }
    }
}
