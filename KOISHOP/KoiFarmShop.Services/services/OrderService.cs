using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // =========================
        // CREATE ORDER
        // =========================
        public async Task CreateOrderAsync(Orders order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (order.OrderDetails == null || !order.OrderDetails.Any())
                throw new ArgumentException("Order must have at least one detail");

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
        }

        // =========================
        // GET ORDER BY ID
        // =========================
        public async Task<Orders> GetOrderByIdAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid OrderId");

            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Koi)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                throw new Exception("Order not found");

            // tính tổng tiền
            order.TotalPrice = order.OrderDetails.Sum(x => x.TotalAmount);

            return order;
        }

        // =========================
        // UPDATE STATUS
        // =========================
        public async Task UpdateStatus(int orderId, string status)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid OrderId");

            var order = await _dbContext.Orders.FindAsync(orderId);

            if (order == null)
                throw new Exception("Order not found");

            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status is required");

            var validStatus = new List<string>
            {
                "Pending",
                "Shipping",
                "Approved",
                "Completed"
            };

            if (!validStatus.Contains(status))
                throw new ArgumentException("Invalid status");

            order.Status = status;
            await _dbContext.SaveChangesAsync();
        }

        // =========================
        // GET ALL ORDERS
        // =========================
        public async Task<List<Orders>> GetAllOrdersAsync()
        {
            var orders = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Koi)
                .ToListAsync();

            foreach (var order in orders)
            {
                order.TotalPrice = order.OrderDetails.Sum(x => x.TotalAmount);
            }

            return orders;
        }
    }
}