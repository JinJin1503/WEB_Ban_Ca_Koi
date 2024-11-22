using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Repositories
{
	public class KoiFishRepository : IKoiFishRepository
	{
		private readonly KoiFarmDbContext _context;

		public KoiFishRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách tất cả KoiFish
		public async Task<List<KoiFish>> GetKoiFishes()
		{
			try
			{
				return await _context.KoiFishs.ToListAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while retrieving KoiFish.", ex);
			}
		}

		// Lấy một KoiFish theo ID
		public async Task<KoiFish> GetKoiFishById(int koiId)
		{
			try
			{
				return await _context.KoiFishs
					.FirstOrDefaultAsync(k => k.KoiId == koiId);
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while retrieving KoiFish with ID {koiId}.", ex);
			}
		}

		// Thêm một KoiFish mới
		public async Task AddKoiFish(KoiFish koi)
		{
			try
			{
				await _context.KoiFishs.AddAsync(koi);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while adding the new KoiFish.", ex);
			}
		}

		// Cập nhật thông tin KoiFish
		public async Task UpdateKoiFish(KoiFish koi)
		{
			try
			{
				_context.KoiFishs.Update(koi);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while updating KoiFish with ID {koi.KoiId}.", ex);
			}
		}

		// Xóa KoiFish theo ID
		public async Task DeleteKoiFish(int koiId)
		{
			try
			{
				var koi = await _context.KoiFishs
					.FirstOrDefaultAsync(k => k.KoiId == koiId);

				if (koi != null)
				{
					_context.KoiFishs.Remove(koi);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while deleting KoiFish with ID {koiId}.", ex);
			}
		}

		// Thêm phương thức trong KoiFishRepository
		public async Task<List<KoiFish>> GetKoiFishByIdsAsync(List<int> koiId)
		{
			try
			{
				return await _context.KoiFishs
					.Where(k => koiId.Contains(k.KoiId))
					.Include(k => k.Category)  // Bao gồm thông tin về category nếu cần thiết
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while retrieving KoiFish by IDs.", ex);
			}
		}












	}
}
