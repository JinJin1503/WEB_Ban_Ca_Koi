using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface ICartRepository
	{
		Task<Cart> GetCartByCustomerIdAsync(int customerId);
		Task AddCartAsync(Cart cart);
		Task UpdateCartAsync(Cart cart);
		// Xóa giỏ hàng của khách hàng khi tài khoản bị xóa
		Task DeleteCartByCustomerIdAsync(int customerId);
		Task<Cart> GetCartByIdAsync(int cartId);
	}
}