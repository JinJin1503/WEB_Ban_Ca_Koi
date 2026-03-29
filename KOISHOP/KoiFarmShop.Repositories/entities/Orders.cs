using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Entities
{
	public class Orders
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int OrderId { get; set; }
		public DateTime OrderDate { get; set; }

		[ForeignKey("Customer")]
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }

		[ForeignKey("Staff")]
		public int StaffId { get; set; }
		public Staff Staff { get; set; }

	
		public List<OrderDetails> OrderDetails { get; set; }

        public string Status { get; set; }
    }
}
