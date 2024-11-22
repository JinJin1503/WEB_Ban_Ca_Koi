using System;
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
    }
}
