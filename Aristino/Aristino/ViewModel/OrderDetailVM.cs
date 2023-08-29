using Aristino.Models;

namespace Aristino.ViewModel
{
	public class OrderDetailVM
	{
		public int OdersDetailId { get; set; }

		public int OrderId { get; set; }

		public int ProductId { get; set; }

		public int Quantity { get; set; }

		public int Price { get; set; }
        public int ProductPrice { get; set; }
        public int Discount { get; set; }

		public string Size { get; set; }

		public string Color { get; set; }
		public virtual ProductVM Product { get; set; } = null!;
        public virtual OrderVM Order { get; set; } = null!;
        public List<int> ProductIdList { get; set; }
		public List<string> ColorList { get; set; }
		public List<string> SizeList { get; set; }
		public List<int> QuantityList { get; set; }
		public int PaymentId { get; set; }
	}
}
