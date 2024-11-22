using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface IOrderDetailsService
	{
		Task<List<OrderDetails>> GetOrderDetailsByOrderIdAsync(int orderId);
		Task<OrderDetails> GetOrderDetailByIdAsync(int orderDetailId);
		Task AddOrderDetailAsync(OrderDetails orderDetail);
		Task UpdateOrderDetailAsync(OrderDetails orderDetail);
		Task DeleteOrderDetailAsync(int orderDetailId);
	}
}
