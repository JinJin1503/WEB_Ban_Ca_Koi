using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Entities
{
	public class CartItem
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CartItemId { get; set; }
		public int QuantityPerKoi { get; set; }
		public int QuantityPerBatch { get; set; }
		public DateTime DateAdded { get; set; }

		// Thuộc tính tổng tiền cho từng CartItem
		[NotMapped] // Không lưu vào DB, chỉ tính toán
		public int TotalPrice
		{
			get
			{
				// Tính tổng tiền = giá cá nhân * số lượng cá + giá lô * số lượng lô
				return (Koi.PricePerKoi * QuantityPerKoi) + (Koi.PricePerBatch * QuantityPerBatch);
			}
			 set { } // Không cho phép set trực tiếp
		}
		public int CartId { get; set; }
		public Cart Cart { get; set; }

		public int KoiId { get; set; }
		public KoiFish Koi { get; set; }
	}
}
