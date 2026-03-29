using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface IConsignmentRequestService
	{
		Task<List<ConsignmentRequest>> GetAllConsignmentRequestsAsync();
		Task<ConsignmentRequest> GetConsignmentRequestByIdAsync(int requestId);
		Task AddConsignmentRequestAsync(ConsignmentRequest request);
		Task UpdateConsignmentRequestAsync(ConsignmentRequest request);
		Task DeleteConsignmentRequestAsync(int requestId);
		Task ApproveRequestAsync(int requestId); // Phê duyệt yêu cầu ký gửi
        Task RejectRequestAsync(int requestId);
    }
}
