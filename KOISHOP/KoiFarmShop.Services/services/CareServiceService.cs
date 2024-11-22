using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Implementations
{
	public class CareServiceService : ICareServiceService
	{
		private readonly KoiFarmDbContext _context;

		// Constructor nhận vào KoiFarmDbContext để làm việc với database
		public CareServiceService(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy tất cả các dịch vụ chăm sóc
		public async Task<List<CareService>> GetAllCareServicesAsync()
		{
			return await _context.CareServices.ToListAsync();
		}

		// Lấy dịch vụ chăm sóc theo ID
		public async Task<CareService> GetCareServiceByIdAsync(int serviceId)
		{
			return await _context.CareServices
				.FirstOrDefaultAsync(s => s.ServiceId == serviceId);
		}

		// Thêm dịch vụ chăm sóc mới
		public async Task AddCareServiceAsync(CareService service)
		{
			_context.CareServices.Add(service);
			await _context.SaveChangesAsync();
		}

		// Cập nhật dịch vụ chăm sóc
		public async Task UpdateCareServiceAsync(CareService service)
		{
			_context.CareServices.Update(service);
			await _context.SaveChangesAsync();
		}

		// Xóa dịch vụ chăm sóc
		public async Task DeleteCareServiceAsync(int serviceId)
		{
			var service = await _context.CareServices
				.FirstOrDefaultAsync(s => s.ServiceId == serviceId);

			if (service != null)
			{
				_context.CareServices.Remove(service);
				await _context.SaveChangesAsync();
			}
		}

		// Lấy dịch vụ chăm sóc của khách hàng theo CustomerId
		public async Task<List<CareService>> GetCareServicesByCustomerIdAsync(int customerId)
		{
			return await _context.CareServices
				.Where(s => s.CustomerId == customerId)
				.ToListAsync();
		}
	}
}
