using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface ICareServiceRepository
	{
		Task<List<CareService>> GetCareServices();
		Task<CareService> GetCareServiceById(int serviceId);
		Task AddCareService(CareService service);
		Task UpdateCareService(CareService service);
		Task DeleteCareService(int serviceId);
	}
}
