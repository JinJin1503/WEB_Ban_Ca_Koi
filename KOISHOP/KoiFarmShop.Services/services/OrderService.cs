using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace KoiFarmShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly KoiFarmDbContext _dbContext;

        public OrderService(KoiFarmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Method to create a new order
        public async Task CreateOrderAsync(Orders order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            // Add the order to the database
            _dbContext.Orders.Add(order);

            // Add the order details associated with the order
            foreach (var orderDetail in order.OrderDetails)
            {
                _dbContext.OrderDetails.Add(orderDetail);
            }

            // Save changes to the database asynchronously
            await _dbContext.SaveChangesAsync();
        }

        // Method to get an order by its ID
        public async Task<Orders> GetOrderByIdAsync(int orderId)
        {
            // Fetch the order along with its details from the database
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Koi)  // Including the related Koi details
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            return order;
        }
        public async Task UpdateStatus(int orderId, string status)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            order.Status = status;
            await _dbContext.SaveChangesAsync();
        }

		public async Task<List<Orders>> GetAllOrdersAsync()
		{
			var orders = await _dbContext.Orders
				.Include(o => o.OrderDetails)
				.ToListAsync();

			foreach (var order in orders)
			{
				var details = _dbContext.OrderDetails
					.Include(x => x.Koi) // 🔥 QUAN TRỌNG
					.Where(x => x.OrderId == order.OrderId)
					.ToList();

				order.TotalPrice = details.Sum(x => x.TotalAmount);
			}

			return orders;
		}

	}
}
