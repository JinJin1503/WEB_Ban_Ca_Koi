using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Entities
{
	public class CareService
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ServiceId { get; set; }
		public string ServiceDesc { get; set; }
		public int ServicePrice { get; set; }

		// Foreign key references
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }

		public int KoiId { get; set; }
		public KoiFish Koi { get; set; }
	}
}
