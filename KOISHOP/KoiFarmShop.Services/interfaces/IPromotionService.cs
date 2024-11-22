using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface IPromotionService
	{
		Task<List<Promotion>> GetPromotionsAsync();
		Task<Promotion> GetPromotionByIdAsync(int  promotionId);
		Task AddPromotionAsync(Promotion promotion);
		Task UpdatePromotionAsync(Promotion promotion);
		Task DeletePromotionAsync(int promotionId);
		Task ApplyPromotionToOder(int orderId, int promotionId);
	}
}
