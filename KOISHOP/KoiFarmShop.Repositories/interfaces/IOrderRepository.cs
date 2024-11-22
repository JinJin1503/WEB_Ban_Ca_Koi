using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface IOrderRepository
	{
		Task<List<Orders>> GetOrders();
		Task<Orders> GetOrderById(int orderId);
		Task AddOrder(Orders order);
		Task UpdateOrder(Orders order);
		Task DeleteOrder(int orderId);
	}
}
