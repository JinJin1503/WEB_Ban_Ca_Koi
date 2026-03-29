using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Implementations
{
	public class ConsignmentRequestService : IConsignmentRequestService
	{
		private readonly KoiFarmDbContext _context;

		public ConsignmentRequestService(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách tất cả yêu cầu ký gửi
		public async Task<List<ConsignmentRequest>> GetAllConsignmentRequestsAsync()
		{
			return await _context.ConsignmentRequests
								 .Include(c => c.Customer) // Bao gồm thông tin khách hàng
								 .ToListAsync();
		}

		// Lấy thông tin yêu cầu ký gửi theo ID
		public async Task<ConsignmentRequest> GetConsignmentRequestByIdAsync(int requestId)
		{
			return await _context.ConsignmentRequests
								 .Include(c => c.Customer) // Bao gồm thông tin khách hàng
								 .FirstOrDefaultAsync(r => r.RequestId == requestId);
		}

		// Thêm yêu cầu ký gửi mới
		public async Task AddConsignmentRequestAsync(ConsignmentRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			await _context.ConsignmentRequests.AddAsync(request);
			await _context.SaveChangesAsync();
		}

		// Cập nhật yêu cầu ký gửi
		public async Task UpdateConsignmentRequestAsync(ConsignmentRequest request)
		{
			var existingRequest = await _context.ConsignmentRequests.FindAsync(request.RequestId);
			if (existingRequest == null)
				throw new Exception("Yêu cầu ký gửi không tồn tại.");

			_context.Entry(existingRequest).CurrentValues.SetValues(request);
			await _context.SaveChangesAsync();
		}

		// Xóa yêu cầu ký gửi theo ID
		public async Task DeleteConsignmentRequestAsync(int requestId)
		{
			var request = await _context.ConsignmentRequests.FindAsync(requestId);
			if (request == null)
				throw new Exception("Yêu cầu ký gửi không tồn tại.");

			_context.ConsignmentRequests.Remove(request);
			await _context.SaveChangesAsync();
		}

		// Phê duyệt yêu cầu ký gửi
		public async Task ApproveRequestAsync(int requestId)
		{
			var request = await _context.ConsignmentRequests.FindAsync(requestId);
			if (request == null)
				throw new Exception("Yêu cầu ký gửi không tồn tại.");

			// Giả sử có một trường Status để cập nhật trạng thái
			request.Status = "Approved";
			_context.ConsignmentRequests.Update(request);
			await _context.SaveChangesAsync();
		}
        public async Task RejectRequestAsync(int requestId)
        {
            var request = await _context.ConsignmentRequests.FindAsync(requestId);
            if (request == null)
                throw new Exception("Yêu cầu ký gửi không tồn tại.");

            // Cập nhật trạng thái thành "Từ chối" hoặc "Rejected"
            request.Status = "Từ chối";

            // Tùy chọn: Có thể lưu thêm ghi chú tại sao bị từ chối vào cột Remarks hoặc Notes
            _context.ConsignmentRequests.Update(request);
            await _context.SaveChangesAsync();
        }
    }
}
