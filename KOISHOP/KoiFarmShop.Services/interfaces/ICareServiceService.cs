using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface ICareServiceService
	{
		Task<List<CareService>> GetAllCareServicesAsync();
		Task<CareService> GetCareServiceByIdAsync(int serviceId);
		Task AddCareServiceAsync(CareService service);
		Task UpdateCareServiceAsync(CareService service);
		Task DeleteCareServiceAsync(int serviceId);
		Task<List<CareService>> GetCareServicesByCustomerIdAsync(int customerId); // Lấy dịch vụ chăm sóc theo khách hàng

	}
}
