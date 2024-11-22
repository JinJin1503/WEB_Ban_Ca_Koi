using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface IConsignmentRequestRepository
	{
		Task<List<ConsignmentRequest>> GetConsignmentRequests();
		Task<ConsignmentRequest> GetConsignmentRequestById(int requestId);
		Task AddConsignmentRequest(ConsignmentRequest request);
		Task UpdateConsignmentRequest(ConsignmentRequest request);
		Task DeleteConsignmentRequest(int requestId);
	}
}
