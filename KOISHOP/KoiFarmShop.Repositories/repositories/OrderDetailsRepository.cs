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
	public class OrderDetailsRepository : IOrderDetailsRepository
	{
		private readonly KoiFarmDbContext _context;

		public OrderDetailsRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách chi tiết đơn hàng
		public async Task<List<OrderDetails>> GetOrderDetails()
		{
			try
			{
				return await _context.OrderDetails.ToListAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while retrieving order details.", ex);
			}
		}

		// Lấy chi tiết đơn hàng theo ID
		public async Task<OrderDetails> GetOrderDetailById(int orderDetailId)
		{
			try
			{
				return await _context.OrderDetails
					.FirstOrDefaultAsync(od => od.OrderDetailId == orderDetailId);
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while retrieving order detail with ID {orderDetailId}.", ex);
			}
		}

		// Thêm chi tiết đơn hàng mới
		public async Task AddOrderDetail(OrderDetails orderDetail)
		{
			try
			{
				await _context.OrderDetails.AddAsync(orderDetail);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while adding the new order detail.", ex);
			}
		}

		// Cập nhật thông tin chi tiết đơn hàng
		public async Task UpdateOrderDetail(OrderDetails orderDetail)
		{
			try
			{
				_context.OrderDetails.Update(orderDetail);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while updating order detail with ID {orderDetail.OrderDetailId}.", ex);
			}
		}

		// Xóa chi tiết đơn hàng theo ID
		public async Task DeleteOrderDetail(int orderDetailId)
		{
			try
			{
				var orderDetail = await _context.OrderDetails
					.FirstOrDefaultAsync(od => od.OrderDetailId == orderDetailId);

				if (orderDetail != null)
				{
					_context.OrderDetails.Remove(orderDetail);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while deleting order detail with ID {orderDetailId}.", ex);
			}
		}
	}
}
