using Aristino.Models;
using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
	public class OrderVM
	{
		public int OrderId { get; set; }

		public string OrderNumber { get; set; } = null!;

		public int CustomerId { get; set; }

		public int PaymentId { get; set; }

		public DateTime ShipDate { get; set; }
		[Required(ErrorMessage ="Please Choose Order Status")]
		public int? TransacId { get; set; }
        public int? TotalPrice { get; set; }
        public bool IsPaid { get; set; }
		public DateTime? OrderDate { get; set; }

		public DateTime? PaymentDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public virtual CustomerVM? Customer { get; set; } = null!;

		public virtual ICollection<OrderDetailVM> OrdersDetails { get; } = new List<OrderDetailVM>();

		public virtual PaymentVM? Payment { get; set; } = null!;

		public virtual TransactionStatusVM? Transac { get; set; } = null!;
	}
}
