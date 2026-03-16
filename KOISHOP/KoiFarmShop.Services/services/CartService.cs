using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoiFarmShop.Services.Implementations
{
	public class CartService : ICartService
	{
		private readonly KoiFarmDbContext _context;

		public CartService(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy giỏ hàng theo CustomerId
		public async Task<Cart> GetCartByCustomerIdAsync(int customerId)
		{
			return await _context.Carts
								 .Include(c => c.CartItems)
								 .ThenInclude(ci => ci.Koi)  // Include koi details in CartItem
								 .FirstOrDefaultAsync(c => c.CustomerId == customerId);
		}

		// Hiển thị tất cả các sản phẩm trong giỏ hàng của khách hàng
		public async Task<List<CartItem>> GetCartItemsAsync(int customerId)
		{
			var cart = await GetCartByCustomerIdAsync(customerId);
			return cart?.CartItems ?? new List<CartItem>();  // If no cart exists, return empty list
		}

		// Thanh toán giỏ hàng và chuyển đổi sang đơn hàng
		public async Task CheckoutAsync(int cartId, string customerId, string paymentMethod)
		{
			var cart = await _context.Carts
									  .Include(c => c.CartItems)
									  .FirstOrDefaultAsync(c => c.CartId == cartId);

			if (cart == null)
				throw new Exception("Cart not found.");

			// Implement the checkout logic here. Example:
			// 1. Create an Order.
			// 2. Mark the Cart as checked out.
			// 3. Handle payment.

			// For simplicity, let's assume we just delete the cart items.
			_context.CartItems.RemoveRange(cart.CartItems);
			await _context.SaveChangesAsync();
		}

		public async Task CreateCartAsync(Cart cart)
		{
			await _context.Carts.AddAsync(cart);
			await _context.SaveChangesAsync();
		}

		// Thêm sản phẩm vào giỏ hàng
		public async Task AddCartItemToCartAsync(int customerId, int koiId, int quantityKoi, int quantityBatch, int ricePerKoi, int ricePerBatch)
		{
			var cart = await _context.Carts
									  .Include(c => c.CartItems)
									  .FirstOrDefaultAsync(c => c.CustomerId == customerId);

			if (cart == null)
			{
				// If no cart exists, create one
				cart = new Cart
				{
					CustomerId = customerId,
					CartItems = new List<CartItem>()
				};
				_context.Carts.Add(cart);
				await _context.SaveChangesAsync();
			}
			else if (cart.CartItems == null)
			{
				cart.CartItems = new List<CartItem>();
			}

			var koi = await _context.KoiFishs.FindAsync(koiId);
			if (koi == null)
				throw new Exception("Koi not found.");

			var cartItem = new CartItem
			{
				KoiId = koiId,
				QuantityPerKoi = quantityKoi,
				QuantityPerBatch = quantityBatch,
				DateAdded = DateTime.UtcNow,
				CartId = cart.CartId
			};

			await _context.CartItems.AddAsync(cartItem);
			await _context.SaveChangesAsync();
		}

		// Xóa sản phẩm khỏi giỏ hàng
		public async Task RemoveFromCartAsync(int customerId, int cartItemId)
		{
			var cart = await _context.Carts
									  .Include(c => c.CartItems)
									  .FirstOrDefaultAsync(c => c.CustomerId == customerId);

			if (cart == null)
				throw new Exception("Cart not found.");

			var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);

			if (cartItem == null)
				throw new Exception("Cart item not found.");

			_context.CartItems.Remove(cartItem);
			await _context.SaveChangesAsync();
		}

		// Xóa toàn bộ sản phẩm trong giỏ
		public async Task DeleteCartAsync(int cartId)
		{
			var cart = await _context.Carts
									  .Include(c => c.CartItems)
									  .FirstOrDefaultAsync(c => c.CartId == cartId);

			if (cart == null)
				throw new Exception("Cart not found.");

			_context.CartItems.RemoveRange(cart.CartItems);
			_context.Carts.Remove(cart);
			await _context.SaveChangesAsync();
		}

		// Cập nhật giỏ hàng
		public async Task UpdateCartAsync(Cart cart)
		{
			var existingCart = await _context.Carts
											 .Include(c => c.CartItems)
											 .FirstOrDefaultAsync(c => c.CartId == cart.CartId);

			if (existingCart == null)
				throw new Exception("Cart not found.");

			existingCart.CartItems = cart.CartItems;
			_context.Carts.Update(existingCart);
			await _context.SaveChangesAsync();
		}

		// Cập nhật CartItem trong giỏ
		public async Task UpdateCartItem(int cartItemId, int koiId, int quantityPerKoi, int quantityPerBatch)
		{
			var cartItem = await _context.CartItems
										  .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

			if (cartItem == null)
				throw new Exception("Cart item not found.");

			var koi = await _context.KoiFishs.FindAsync(koiId);
			if (koi == null)
				throw new Exception("Koi not found.");

			cartItem.KoiId = koiId;
			cartItem.QuantityPerKoi = quantityPerKoi;
			cartItem.QuantityPerBatch = quantityPerBatch;

			_context.CartItems.Update(cartItem);
			await _context.SaveChangesAsync();
		}

		public async Task<CartItem> GetCartItemByIdAsync(int cartItemId)
		{
			return await _context.CartItems
				.FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
		}

		public async Task UpdateCartItemAsync(CartItem cartItem)
		{
			if (cartItem.QuantityPerKoi < 0 || cartItem.QuantityPerBatch < 0)
			{
				throw new Exception("Quantity cannot be negative.");
			}

			_context.CartItems.Update(cartItem);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCartItemAsync(int cartItemId)
		{
			var cartItem = await GetCartItemByIdAsync(cartItemId);
			if (cartItem != null)
			{
				_context.CartItems.Remove(cartItem);
				await _context.SaveChangesAsync();
			}
		}

		public async Task ClearCartAsync(int customerId)
		{
			// Fetch all cart items associated with the customer
			var cartItems = await _context.CartItems
											.Where(ci => ci.Cart.CustomerId == customerId)
											.ToListAsync();

			// Remove all the cart items
			_context.CartItems.RemoveRange(cartItems);

			// Save changes to the database
			await _context.SaveChangesAsync();
		}
	}
}
