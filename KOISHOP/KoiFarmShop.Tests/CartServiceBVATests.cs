using Xunit;
using Microsoft.EntityFrameworkCore;
using KoiFarmShop.Repositories;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Implementations;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace KoiFarmShop.Tests
{
    public class CartServiceBVATests
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
        public async Task AddCartItem_MinQuantity_ShouldPass()
        {
            var context = GetDbContext();
            context.KoiFishs.Add(new KoiFish { KoiId = 1 });
            await context.SaveChangesAsync();

            var service = new CartService(context);

            await service.AddCartItemToCartAsync(1, 1, 1, 1, 0, 0);

            var cart = context.Carts.Include(c => c.CartItems).FirstOrDefault();

            Assert.NotNull(cart);
            Assert.Single(cart.CartItems);
        }

        // 2. Quantity = 0 (invalid)
        [Fact]
        public async Task AddCartItem_ZeroQuantity_ShouldThrow()
        {
            var context = GetDbContext();
            context.KoiFishs.Add(new KoiFish { KoiId = 1 });
            await context.SaveChangesAsync();

            var service = new CartService(context);

            await Assert.ThrowsAsync<Exception>(() =>
                service.AddCartItemToCartAsync(1, 1, 0, 0, 0, 0)
            );
        }

        // 3. Quantity âm
        [Fact]
        public async Task AddCartItem_NegativeQuantity_ShouldThrow()
        {
            var context = GetDbContext();
            context.KoiFishs.Add(new KoiFish { KoiId = 1 });
            await context.SaveChangesAsync();

            var service = new CartService(context);

            await Assert.ThrowsAsync<Exception>(() =>
                service.AddCartItemToCartAsync(1, 1, -1, 1, 0, 0)
            );
        }

        // 4. Koi không tồn tại
        [Fact]
        public async Task AddCartItem_KoiNotExist_ShouldThrow()
        {
            var context = GetDbContext();
            var service = new CartService(context);

            await Assert.ThrowsAsync<Exception>(() =>
                service.AddCartItemToCartAsync(1, 999, 1, 1, 0, 0)
            );
        }

        // 5. Quantity lớn (valid)
        [Fact]
        public async Task AddCartItem_LargeQuantity_ShouldPass()
        {
            var context = GetDbContext();
            context.KoiFishs.Add(new KoiFish { KoiId = 1 });
            await context.SaveChangesAsync();

            var service = new CartService(context);

            await service.AddCartItemToCartAsync(1, 1, 1000, 1, 0, 0);

            var cart = context.Carts.Include(c => c.CartItems).FirstOrDefault();

            Assert.NotNull(cart);
            Assert.Single(cart.CartItems);
        }

        // 6. Thêm nhiều item khác nhau
        [Fact]
        public async Task AddCartItem_MultipleDifferentItems_ShouldPass()
        {
            var context = GetDbContext();

            context.KoiFishs.Add(new KoiFish { KoiId = 1 });
            context.KoiFishs.Add(new KoiFish { KoiId = 2 });
            await context.SaveChangesAsync();

            var service = new CartService(context);

            await service.AddCartItemToCartAsync(1, 1, 1, 1, 0, 0);
            await service.AddCartItemToCartAsync(1, 2, 1, 1, 0, 0);

            var cart = context.Carts.Include(c => c.CartItems).FirstOrDefault();

            Assert.Equal(2, cart.CartItems.Count);
        }

        // 7. Thêm cùng koi 2 lần
        [Fact]
        public async Task AddCartItem_SameKoiTwice_ShouldCreateTwoItems()
        {
            var context = GetDbContext();

            context.KoiFishs.Add(new KoiFish { KoiId = 1 });
            await context.SaveChangesAsync();

            var service = new CartService(context);

            await service.AddCartItemToCartAsync(1, 1, 1, 1, 0, 0);
            await service.AddCartItemToCartAsync(1, 1, 1, 1, 0, 0);

            var items = context.CartItems.ToList();

            Assert.Equal(2, items.Count);
        }

        // 8. Checkout cart hợp lệ
        [Fact]
        public async Task Checkout_ValidCart_ShouldClearItems()
        {
            var context = GetDbContext();

            var cart = new Cart
            {
                CartId = 1,
                CustomerId = 1,
                CartItems = new List<CartItem>
                {
                    new CartItem { CartItemId = 1 }
                }
            };

            context.Carts.Add(cart);
            await context.SaveChangesAsync();

            var service = new CartService(context);

            await service.CheckoutAsync(1, "1", "COD");

            var items = context.CartItems.ToList();

            Assert.Empty(items);
        }

        // 9. Checkout cart không tồn tại
        [Fact]
        public async Task Checkout_CartNotExist_ShouldThrow()
        {
            var context = GetDbContext();
            var service = new CartService(context);

            await Assert.ThrowsAsync<Exception>(() =>
                service.CheckoutAsync(999, "1", "COD")
            );
        }

        // 10. Checkout cart rỗng
        [Fact]
        public async Task Checkout_EmptyCart_ShouldPass()
        {
            var context = GetDbContext();

            var cart = new Cart
            {
                CartId = 2,
                CustomerId = 1,
                CartItems = new List<CartItem>()
            };

            context.Carts.Add(cart);
            await context.SaveChangesAsync();

            var service = new CartService(context);

            await service.CheckoutAsync(2, "1", "COD");

            var items = context.CartItems.ToList();

            Assert.Empty(items);
        }
    }
}