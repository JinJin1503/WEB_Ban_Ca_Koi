using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface IOrderService
	{
		Task<Orders> GetOrderByIdAsync(int orderId);
        Task CreateOrderAsync(Orders order);
        Task UpdateStatus(int orderId, string status);
        Task<List<Orders>> GetAllOrdersAsync();
    }
}
