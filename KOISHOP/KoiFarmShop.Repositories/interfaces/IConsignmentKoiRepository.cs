using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface IConsignmentKoiRepository
	{
		Task<List<ConsignmentKoi>> GetConsignmentKois();
		Task<ConsignmentKoi> GetConsignmentKoiById(int consignmentKoiId);
		Task AddConsignmentKoi(ConsignmentKoi consignmentKoi);
		Task UpdateConsignmentKoi(ConsignmentKoi consignmentKoi);
		Task DeleteConsignmentKoi(int consignmentKoiId);
	}
}
