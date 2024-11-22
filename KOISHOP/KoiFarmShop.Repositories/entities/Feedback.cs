using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Entities
{
	public class Feedback
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int FeedbackId { get; set; }
		public int Rating { get; set; }
		public string Comment { get; set; }
		public DateTime FeedbackDate { get; set; }

		// Foreign key references
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }

		public int KoiId { get; set; }
		public KoiFish Koi { get; set; }
	}
}
