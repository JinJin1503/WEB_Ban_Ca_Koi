using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Entities
{
	public class Report
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ReportId { get; set; }
		public DateTime ReportDate { get; set; }
		public int TotalRevenue { get; set; }
		public int TotalCustomers { get; set; }
		public string Summary { get; set; }
	}
}
