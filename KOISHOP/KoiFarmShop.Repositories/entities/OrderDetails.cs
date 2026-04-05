using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace KoiFarmShop.Repositories.Entities
{
	public class OrderDetails
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int OrderDetailId { get; set; }
		public int QuantityPerKoi { get; set; }
		public int QuantityPerBatch { get; set; }
		public string Status { get; set; }
		public string PaymentMethod { get; set; }
		public string ShippingAddress { get; set; }

		// Foreign key references
		[ForeignKey("Order")]
		public int OrderId { get; set; }
		public Orders Order { get; set; }
		[ForeignKey("Koi")]
		public int KoiId { get; set; }
		public KoiFish Koi { get; set; }
		[ForeignKey("Promotions")]
		public int PromotionId { get; set; }
		public Promotion Promotions { get; set; }

		[NotMapped] // Không lưu vào DB, chỉ tính toán
		public int TotalAmount
		{
			get
			{
				// Kiểm tra null trước khi truy cập vào Koi và tính toán
				if (Koi == null)
				{
					return 0; // Hoặc có thể trả về giá trị mặc định phù hợp
				}

				// Tính tổng tiền = giá cá nhân * số lượng cá + giá lô * số lượng lô
				return (Koi.PricePerKoi * QuantityPerKoi) + (Koi.PricePerBatch * QuantityPerBatch);
			}
			set { } // Không cho phép set trực tiếp
		}



	}
}
