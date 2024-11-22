using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Entities
{
	public class ConsignmentKoi
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ConsignmentKoiId { get; set; }
		public int AgreedPrice { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		
		public int RequestId { get; set; }
		public ConsignmentRequest ConsignmentRequest { get; set; }

		public int KoiId { get; set; }
		public KoiFish Koi { get; set; }
	}
}
