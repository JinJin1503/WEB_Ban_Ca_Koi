using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface IPromotionRepository
	{
		Task<List<Promotion>> GetPromotions();
		Task<Promotion> GetPromotionById(int promotionId);
		Task AddPromotion(Promotion promotion);
		Task UpdatePromotion(Promotion promotion);
		Task DeletePromotion(int promotionId);
	}
}
