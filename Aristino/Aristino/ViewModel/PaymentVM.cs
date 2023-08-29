using Aristino.Models;

namespace Aristino.ViewModel
{
	public class PaymentVM
	{
		public int PaymentId { get; set; }

		public string PaymentName { get; set; } = null!;

		public bool Allowed { get; set; }

		public virtual ICollection<OrderVM> Orders { get; } = new List<OrderVM>();
	}
}
