using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoiFarmShop.Repositories
{
	public class CartRepository : ICartRepository
	{
		private readonly KoiFarmDbContext _context;

		public CartRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy giỏ hàng của khách hàng (nếu có)
		public async Task<Cart> GetCartByCustomerIdAsync(int customerId)
		{
			return await _context.Carts
								 .Include(c => c.CartItems)
								 .ThenInclude(ci => ci.Koi) // Liên kết đến KoiFish để lấy chi tiết sản phẩm
								 .FirstOrDefaultAsync(c => c.CustomerId == customerId);
		}
		// Thêm giỏ hàng mới
		public async Task AddCartAsync(Cart cart)
		{
			await _context.Carts.AddAsync(cart);
			await _context.SaveChangesAsync();
		}
		// Cập nhật giỏ hàng
		public async Task UpdateCartAsync(Cart cart)
		{
			_context.Carts.Update(cart);
			await _context.SaveChangesAsync();
		}
		// Xóa giỏ hàng của khách hàng khi tài khoản bị xóa
		public async Task DeleteCartByCustomerIdAsync(int customerId)
		{
			var cart = await _context.Carts
									  .FirstOrDefaultAsync(c => c.CustomerId == customerId);

			if (cart != null)
			{
				_context.Carts.Remove(cart);
				await _context.SaveChangesAsync();
			}
		}
		public async Task<Cart> GetCartByIdAsync(int cartId)
		{
			// Truy vấn giỏ hàng từ cơ sở dữ liệu theo CartId
			return await _context.Carts
								 .Include(c => c.CartItems)  // Bao gồm các sản phẩm trong giỏ hàng
								 .FirstOrDefaultAsync(c => c.CartId == cartId);  // Lấy giỏ hàng có CartId tương ứng
		}

	}

}