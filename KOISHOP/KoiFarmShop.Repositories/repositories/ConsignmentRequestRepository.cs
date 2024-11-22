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
	public class ConsignmentRequestRepository : IConsignmentRequestRepository
	{
		private readonly KoiFarmDbContext _context;

		public ConsignmentRequestRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách các yêu cầu gửi hàng
		public async Task<List<ConsignmentRequest>> GetConsignmentRequests()
		{
			try
			{
				return await _context.ConsignmentRequests.ToListAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while retrieving consignment requests.", ex);
			}
		}

		// Lấy yêu cầu gửi hàng theo ID
		public async Task<ConsignmentRequest> GetConsignmentRequestById(int requestId)
		{
			try
			{
				return await _context.ConsignmentRequests
					.FirstOrDefaultAsync(cr => cr.RequestId == requestId);
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while retrieving consignment request with ID {requestId}.", ex);
			}
		}

		// Thêm yêu cầu gửi hàng
		public async Task AddConsignmentRequest(ConsignmentRequest request)
		{
			try
			{
				await _context.ConsignmentRequests.AddAsync(request);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while adding the consignment request.", ex);
			}
		}

		// Cập nhật yêu cầu gửi hàng
		public async Task UpdateConsignmentRequest(ConsignmentRequest request)
		{
			try
			{
				_context.ConsignmentRequests.Update(request);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while updating consignment request with ID {request.RequestId}.", ex);
			}
		}

		// Xóa yêu cầu gửi hàng theo ID
		public async Task DeleteConsignmentRequest(int requestId)
		{
			try
			{
				var request = await _context.ConsignmentRequests
					.FirstOrDefaultAsync(cr => cr.RequestId == requestId);

				if (request != null)
				{
					_context.ConsignmentRequests.Remove(request);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while deleting consignment request with ID {requestId}.", ex);
			}
		}
	}
}
