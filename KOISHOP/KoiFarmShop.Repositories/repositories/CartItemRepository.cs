using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoiFarmShop.Repositories
{
	public class CartItemRepository : ICartItemRepository
	{
		private readonly KoiFarmDbContext _context;

		public CartItemRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId)
		{
			return await _context.CartItems
								 .Include(ci => ci.Koi)
								 .Where(ci => ci.CartId == cartId)
								 .ToListAsync();
		}

		public async Task AddCartItemAsync(CartItem cartItem)
		{
			await _context.CartItems.AddAsync(cartItem);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateCartItemAsync(CartItem cartItem)
		{
			_context.CartItems.Update(cartItem);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCartItemAsync(int cartItemId)
		{
			var cartItem = await _context.CartItems.FindAsync(cartItemId);
			if (cartItem != null)
			{
				_context.CartItems.Remove(cartItem);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<CartItem> GetCartItemAsync(int cartId, int koiId)
		{
			return await _context.CartItems
								 .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.KoiId == koiId);
		}
	}

}