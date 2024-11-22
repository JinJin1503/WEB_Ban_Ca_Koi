using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace KoiFarmShop.Repositories.Entities
{
	public class Cart
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CartId { get; set; }
		
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }

		public List<CartItem> CartItems { get; set; }
	}
}
