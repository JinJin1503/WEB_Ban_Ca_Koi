using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Repositories
{
	public class PromotionRepository : IPromotionRepository
	{
		private readonly KoiFarmDbContext _context;

		public PromotionRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách tất cả các chương trình khuyến mãi
		public async Task<List<Promotion>> GetPromotions()
		{
			try
			{
				return await _context.Promotions.ToListAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while retrieving promotions.", ex);
			}
		}

		// Lấy chương trình khuyến mãi theo ID
		public async Task<Promotion> GetPromotionById(int promotionId)
		{
			try
			{
				return await _context.Promotions
					.FirstOrDefaultAsync(p => p.PromotionId == promotionId);
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while retrieving promotion with ID {promotionId}.", ex);
			}
		}

		// Thêm một chương trình khuyến mãi mới
		public async Task AddPromotion(Promotion promotion)
		{
			try
			{
				await _context.Promotions.AddAsync(promotion);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while adding the promotion.", ex);
			}
		}

		// Cập nhật một chương trình khuyến mãi
		public async Task UpdatePromotion(Promotion promotion)
		{
			try
			{
				_context.Promotions.Update(promotion);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while updating promotion with ID {promotion.PromotionId}.", ex);
			}
		}

		// Xóa chương trình khuyến mãi theo ID
		public async Task DeletePromotion(int promotionId)
		{
			try
			{
				var promotion = await _context.Promotions
					.FirstOrDefaultAsync(p => p.PromotionId == promotionId);

				if (promotion != null)
				{
					_context.Promotions.Remove(promotion);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while deleting promotion with ID {promotionId}.", ex);
			}
		}
	}
}
