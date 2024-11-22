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
	public class ConsignmentKoiRepository : IConsignmentKoiRepository
	{
		private readonly KoiFarmDbContext _context;

		public ConsignmentKoiRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách các Koi consignments
		public async Task<List<ConsignmentKoi>> GetConsignmentKois()
		{
			try
			{
				return await _context.ConsignmentKois.ToListAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while retrieving consignment kois.", ex);
			}
		}

		// Lấy Koi consignment theo ID
		public async Task<ConsignmentKoi> GetConsignmentKoiById(int consignmentKoiId)
		{
			try
			{
				return await _context.ConsignmentKois
					.FirstOrDefaultAsync(ck => ck.ConsignmentKoiId == consignmentKoiId);
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while retrieving consignment koi with ID {consignmentKoiId}.", ex);
			}
		}

		// Thêm Koi consignment
		public async Task AddConsignmentKoi(ConsignmentKoi consignmentKoi)
		{
			try
			{
				await _context.ConsignmentKois.AddAsync(consignmentKoi);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while adding the consignment koi.", ex);
			}
		}

		// Cập nhật Koi consignment
		public async Task UpdateConsignmentKoi(ConsignmentKoi consignmentKoi)
		{
			try
			{
				_context.ConsignmentKois.Update(consignmentKoi);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while updating consignment koi with ID {consignmentKoi.ConsignmentKoiId}.", ex);
			}
		}

		// Xóa Koi consignment theo ID
		public async Task DeleteConsignmentKoi(int consignmentKoiId)
		{
			try
			{
				var consignmentKoi = await _context.ConsignmentKois
					.FirstOrDefaultAsync(ck => ck.ConsignmentKoiId == consignmentKoiId);

				if (consignmentKoi != null)
				{
					_context.ConsignmentKois.Remove(consignmentKoi);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while deleting consignment koi with ID {consignmentKoiId}.", ex);
			}
		}
	}
}
