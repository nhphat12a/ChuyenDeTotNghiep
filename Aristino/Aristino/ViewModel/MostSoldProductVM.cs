using Aristino.Models;

namespace Aristino.ViewModel
{
    public class MostSoldProductVM
    {
        public int Id { get; set; }

        public int? ProductId { get; set; }

        public int Sold { get; set; }

        public bool IsReset { get; set; }

        public virtual ProductVM? Product { get; set; }
    }
}
