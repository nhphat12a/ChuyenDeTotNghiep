using Aristino.Models;

namespace Aristino.ViewModel
{
	public class TransactionStatusVM
	{
		public int TransacId { get; set; }

		public string TransacName { get; set; } = null!;

		public string TransacDes { get; set; } = null!;

		public virtual ICollection<OrderVM> Orders { get; } = new List<OrderVM>();
	}
}
