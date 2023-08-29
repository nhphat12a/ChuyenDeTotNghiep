using Aristino.Models;

namespace Aristino.ViewModel
{
    public class UserStatusVM
    {
        public int StatusId { get; set; }

        public string? StatusName { get; set; }

        public virtual ICollection<CustomerVM> Customers { get; } = new List<CustomerVM>();
    }
}
