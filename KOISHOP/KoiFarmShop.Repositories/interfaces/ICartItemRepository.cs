using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface ICartItemRepository
	{
		Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);
		Task AddCartItemAsync(CartItem cartItem);
		Task UpdateCartItemAsync(CartItem cartItem);
		Task DeleteCartItemAsync(int cartItemId);
		Task<CartItem> GetCartItemAsync(int cartId, int koiId);
	}
}