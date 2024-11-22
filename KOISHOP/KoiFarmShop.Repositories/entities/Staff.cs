using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Entities
{
	public class Staff
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int StaffId { get; set; }
		public string StaffName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
		public DateTime JoinDate { get; set; }
		public int Salary { get; set; }

	
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
