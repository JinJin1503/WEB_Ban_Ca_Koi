using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface IOrderDetailsRepository
	{
		Task<List<OrderDetails>> GetOrderDetails();
		Task<OrderDetails> GetOrderDetailById(int orderDetailId);
		Task AddOrderDetail(OrderDetails orderDetail);
		Task UpdateOrderDetail(OrderDetails orderDetail);
		Task DeleteOrderDetail(int orderDetailId);
	}
}
