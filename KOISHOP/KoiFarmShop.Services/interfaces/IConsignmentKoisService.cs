using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface IConsignmentKoiService
	{
		Task<List<ConsignmentKoi>> GetConsignmentKoiByConsignmentIdAsync(int consignmentId);
		Task<ConsignmentKoi> GetConsignmentKoiByIdAsync(int consignmentKoiId);
		Task AddConsignmentKoiAsync(ConsignmentKoi consignmentKoi);
		Task UpdateConsignmentKoiAsync(ConsignmentKoi consignmentKoi);
		Task DeleteConsignmentKoiAsync(int consignmentKoiId);
	}
}
