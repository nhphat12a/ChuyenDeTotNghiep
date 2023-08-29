using Aristino.Models;

namespace Aristino.ViewModel
{
    public class CartVM
    {
        public int CartId { get; set; }

        public int? ProductId { get; set; }

        public int? CustomerId { get; set; }

        public string? TotalPrice { get; set; }

        public int? Quantity { get; set; }
		public string? Size { get; set; }

		public string? Color { get; set; }

		public virtual CustomerVM? Customer { get; set; }

        public virtual ProductVM? Product { get; set; }
        public List<int> ProductIdList { get; set; }
        public List<string> SizeList { get; set; }
        public List<string> ColorList { get; set; }
    }
}
