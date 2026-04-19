using System;
using System.Threading.Tasks;
using Xunit;
using KoiFarmShop.Repositories;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services;
using Microsoft.EntityFrameworkCore;

namespace KoiFarmShop.Tests
{
    public class OrderServiceBVATest
    {
        private KoiFarmDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<KoiFarmDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new KoiFarmDbContext(options);
        }

        // 1. Min boundary (valid)
        [Fact]
        public async Task UpdateStatus_OrderId_MinBoundary_ShouldPass()
        {
            var context = GetDbContext();
            context.Orders.Add(new Orders { OrderId = 1, Status = "Pending" });
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await service.UpdateStatus(1, "Shipping");

            var order = await context.Orders.FindAsync(1);
            Assert.Equal("Shipping", order.Status);
        }

        // 2. OrderId = 0 (invalid)
        [Fact]
        public async Task UpdateStatus_OrderId_Zero_ShouldThrow()
        {
            var context = GetDbContext();
            var service = new OrderService(context);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.UpdateStatus(0, "Shipping")
            );
        }

        // 3. OrderId âm
        [Fact]
        public async Task UpdateStatus_OrderId_Negative_ShouldThrow()
        {
            var context = GetDbContext();
            var service = new OrderService(context);

            await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateStatus(-1, "Shipping")
            );
        }

        // 4. Order không tồn tại
        [Fact]
        public async Task UpdateStatus_OrderNotFound_ShouldThrow()
        {
            var context = GetDbContext();
            var service = new OrderService(context);

            await Assert.ThrowsAsync<Exception>(() =>
                service.UpdateStatus(999, "Shipping")
            );
        }

        // 5. Status hợp lệ
        [Fact]
        public async Task UpdateStatus_ValidStatus_ShouldPass()
        {
            var context = GetDbContext();
            context.Orders.Add(new Orders { OrderId = 2, Status = "Pending" });
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await service.UpdateStatus(2, "Approved");

            var order = await context.Orders.FindAsync(2);
            Assert.Equal("Approved", order.Status);
        }

        // 6. Status invalid
        [Fact]
        public async Task UpdateStatus_InvalidStatus_ShouldThrow()
        {
            var context = GetDbContext();
            context.Orders.Add(new Orders { OrderId = 3, Status = "Pending" });
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateStatus(3, "Done")
            );
        }

        // 7. Status null
        [Fact]
        public async Task UpdateStatus_NullStatus_ShouldThrow()
        {
            var context = GetDbContext();
            context.Orders.Add(new Orders { OrderId = 4, Status = "Pending" });
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateStatus(4, null)
            );
        }

        // 8. Status empty
        [Fact]
        public async Task UpdateStatus_EmptyStatus_ShouldThrow()
        {
            var context = GetDbContext();
            context.Orders.Add(new Orders { OrderId = 5, Status = "Pending" });
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateStatus(5, "")
            );
        }

        // 9. Status same value
        [Fact]
        public async Task UpdateStatus_SameStatus_ShouldPass()
        {
            var context = GetDbContext();
            context.Orders.Add(new Orders { OrderId = 6, Status = "Shipping" });
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await service.UpdateStatus(6, "Shipping");

            var order = await context.Orders.FindAsync(6);
            Assert.Equal("Shipping", order.Status);
        }

        // 10. Update nhiều lần
        [Fact]
        public async Task UpdateStatus_MultipleTimes_ShouldKeepLatest()
        {
            var context = GetDbContext();
            context.Orders.Add(new Orders { OrderId = 7, Status = "Pending" });
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await service.UpdateStatus(7, "Shipping");
            await service.UpdateStatus(7, "Completed");

            var order = await context.Orders.FindAsync(7);
            Assert.Equal("Completed", order.Status);
        }
    }
}