using Aristino.Models;
using System.ComponentModel.DataAnnotations;

namespace Aristino.ViewModel
{
	public class UserRoleVM
	{
		public int RoleId { get; set; }

		public string RoleName { get; set; } = null!;

		public virtual ICollection<AccountVM> Accounts { get; } = new List<AccountVM>();
	}
}
