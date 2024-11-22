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
	public class CareServiceRepository : ICareServiceRepository
	{
		private readonly KoiFarmDbContext _context;

		public CareServiceRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách tất cả CareServices
		public async Task<List<CareService>> GetCareServices()
		{
			try
			{
				return await _context.CareServices.ToListAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while retrieving care services.", ex);
			}
		}

		// Lấy CareService theo ID
		public async Task<CareService> GetCareServiceById(int serviceId)
		{
			try
			{
				return await _context.CareServices
					.FirstOrDefaultAsync(s => s.ServiceId == serviceId);
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while retrieving care service with ID {serviceId}.", ex);
			}
		}

		// Thêm một CareService mới
		public async Task AddCareService(CareService service)
		{
			try
			{
				await _context.CareServices.AddAsync(service);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while adding the care service.", ex);
			}
		}

		// Cập nhật một CareService
		public async Task UpdateCareService(CareService service)
		{
			try
			{
				_context.CareServices.Update(service);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while updating care service with ID {service.ServiceId}.", ex);
			}
		}

		// Xóa CareService theo ID
		public async Task DeleteCareService(int serviceId)
		{
			try
			{
				var service = await _context.CareServices
					.FirstOrDefaultAsync(s => s.ServiceId == serviceId);

				if (service != null)
				{
					_context.CareServices.Remove(service);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while deleting care service with ID {serviceId}.", ex);
			}
		}
	}
}
