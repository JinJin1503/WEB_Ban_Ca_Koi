using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiFarmShop.Repositories;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services;
using Microsoft.EntityFrameworkCore;

namespace KoiFarmShop.Tests
{
    public class OrderServiceTests
    {
        private KoiFarmDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<KoiFarmDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new KoiFarmDbContext(options);
        }

        [Fact]
        public async Task UpdateStatus_Should_Update_Order_Status()
        {
            // Arrange
            var context = GetDbContext();
            context.Orders.Add(new Orders { OrderId = 1, Status = "Pending" });
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            // Act
            await service.UpdateStatus(1, "Shipping");

            // Assert
            var order = await context.Orders.FindAsync(1);
            Assert.Equal("Shipping", order.Status);
        }
        // đơn không tồn tại 
        [Fact]
        public async Task UpdateStatus_Should_Not_Update_When_Order_Not_Exist()
        {
            var context = GetDbContext();
            var service = new OrderService(context);

            await Assert.ThrowsAsync<Exception>(
                () => service.UpdateStatus(999, "Shipping")
            );
        }

        // test tao đơn hàng 
        [Fact]
        public async Task CreateOrder_Should_Add_New_Order()
        {
            var context = GetDbContext();

            var order = new Orders { Status = "Pending" };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            Assert.Equal(1, context.Orders.Count());
        }
        //Test lấy danh sách đơn hàng

        [Fact]
        public async Task GetAllOrders_Should_Return_List()
        {
            var context = GetDbContext();
            context.Orders.Add(new Orders { OrderId = 1 });
            context.Orders.Add(new Orders { OrderId = 2 });
            await context.SaveChangesAsync();

            var service = new OrderService(context);
            var result = await service.GetAllOrdersAsync();

            Assert.Equal(2, result.Count);
        }
    }
}
