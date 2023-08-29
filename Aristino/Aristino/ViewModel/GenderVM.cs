using Aristino.Models;

namespace Aristino.ViewModel
{
    public class GenderVM
    {
        public int GenderId { get; set; }

        public string? GenderName { get; set; }

        public virtual ICollection<CustomerVM> Customers { get; } = new List<CustomerVM>();
    }
}
