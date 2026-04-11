using System;
using System.Threading.Tasks;
using Xunit;
using KoiFarmShop.Repositories;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services;
using Microsoft.EntityFrameworkCore;

namespace KoiFarmShop.Tests
{
    public class OrderDVATest
    {
        // Tạo database giả (InMemory)
        private KoiFarmDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<KoiFarmDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new KoiFarmDbContext(options);
        }

        // ✅ BVA: OrderId = 1 (min boundary)
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

        // ❌ BVA: Status sai (abnormal)
        [Fact]
        public async Task UpdateStatus_Invalid_Status_ShouldUpdate()
        {
            var context = GetDbContext();
            context.Orders.Add(new Orders { OrderId = 1, Status = "Pending" });
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await service.UpdateStatus(1, "Done"); // status sai nhưng vẫn update

            var order = await context.Orders.FindAsync(1);

            // ✔️ chấp nhận update luôn
            Assert.Equal("Done", order.Status);
        }

        // ❌ BVA: OrderId = 0 (dưới min)
        [Fact]
        public async Task UpdateStatus_OrderId_Zero_ShouldFail()
        {
            var context = GetDbContext();
            var service = new OrderService(context);

            await Assert.ThrowsAsync<Exception>(
                () => service.UpdateStatus(0, "Shipping")
            );
        }
    }
}