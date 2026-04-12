using Xunit;
using Microsoft.EntityFrameworkCore;
using KoiFarmShop.Repositories;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Implementations;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class CartServiceTests
{
    private KoiFarmDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<KoiFarmDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new KoiFarmDbContext(options);
    }
    [Fact]
    public async Task AddCartItem_MinQuantity_ShouldPass()
    {
        var context = GetDbContext();

        context.KoiFishs.Add(new KoiFish { KoiId = 1 });
        await context.SaveChangesAsync();

        var service = new CartService(context);

        await service.AddCartItemToCartAsync(1, 1, 1, 1, 0, 0);

        var cart = context.Carts.FirstOrDefault();
        Assert.NotNull(cart);
        Assert.Single(cart.CartItems);
    }
    [Fact]
    public async Task AddCartItem_QuantityZero_ShouldStillAdd() // do code bạn chưa validate
    {
        var context = GetDbContext();

        context.KoiFishs.Add(new KoiFish { KoiId = 1 });
        await context.SaveChangesAsync();

        var service = new CartService(context);

        await service.AddCartItemToCartAsync(1, 1, 0, 0, 0, 0);

        var cart = context.Carts.FirstOrDefault();
        Assert.Single(cart.CartItems);
    }
    [Fact]
    public async Task Checkout_CartNotExist_ShouldThrow()
    {
        var context = GetDbContext();
        var service = new CartService(context);

        await Assert.ThrowsAsync<Exception>(
            () => service.CheckoutAsync(999, "1", "COD")
        );
    }
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
}