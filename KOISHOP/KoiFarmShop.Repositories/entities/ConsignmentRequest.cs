using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Entities
{
	public class ConsignmentRequest
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RequestId { get; set; }
		public DateTime RequestDate { get; set; }
		public string Status { get; set; }
		public int ConsignmentFee { get; set; }
		public string ConsignmentType { get; set; }
		public string Certificate { get; set; }
		public string Notes { get; set; }

		
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }

		public List<ConsignmentKoi> ConsignmentKois { get; set; }
	}
}
