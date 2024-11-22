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
	public class OrderRepository : IOrderRepository
	{
		private readonly KoiFarmDbContext _context;

		public OrderRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách tất cả đơn hàng
		public async Task<List<Orders>> GetOrders()
		{
			try
			{
				return await _context.Orders.ToListAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while retrieving orders.", ex);
			}
		}

		// Lấy thông tin đơn hàng theo ID
		public async Task<Orders> GetOrderById(int orderId)
		{
			try
			{
				return await _context.Orders
					.FirstOrDefaultAsync(o => o.OrderId == orderId);
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while retrieving order with ID {orderId}.", ex);
			}
		}

		// Thêm đơn hàng mới
		public async Task AddOrder(Orders order)
		{
			try
			{
				await _context.Orders.AddAsync(order);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while adding the new order.", ex);
			}
		}

		// Cập nhật thông tin đơn hàng
		public async Task UpdateOrder(Orders order)
		{
			try
			{
				_context.Orders.Update(order);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while updating order with ID {order.OrderId}.", ex);
			}
		}

		// Xóa đơn hàng theo ID
		public async Task DeleteOrder(int orderId)
		{
			try
			{
				var order = await _context.Orders
					.FirstOrDefaultAsync(o => o.OrderId == orderId);

				if (order != null)
				{
					_context.Orders.Remove(order);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while deleting order with ID {orderId}.", ex);
			}
		}
	}
}
