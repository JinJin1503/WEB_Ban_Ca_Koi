using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface ICartService
	{
        Task ClearCartAsync(int customerId);
        // Lấy giỏ hàng theo CustomerId
        Task<Cart> GetCartByCustomerIdAsync(int customerId);
		// Hiển thị tất cả các sản phẩm trong giỏ hàng của khách hàng
		Task<List<CartItem>> GetCartItemsAsync(int customerId);
		// Thanh toán giỏ hàng và chuyển đổi sang đơn hàng
		Task CheckoutAsync(int cartId, string customerId, string paymentMethod);
		// Thêm sản phẩm vào giỏ hàng
		Task AddCartItemToCartAsync(int customerId, int koiId, int quantityKoi, int quantityBatch, int ricePerKoi, int ricePerBatch);
		// Xóa sản phẩm khỏi giỏ hàng
		Task RemoveFromCartAsync(int customerId, int cartItemId);
		// Xóa toàn bộ sản phẩm trong giỏ
		Task DeleteCartAsync(int cartId); // Xóa toàn bộ sản phẩm trong giỏ
										  // Cập nhật giỏ hàng
		Task UpdateCartAsync(Cart cart);
		Task UpdateCartItem(int cartItemId, int koiId, int quantityPerKoi, int quantityPerBatch);

        Task CreateCartAsync(Cart cart);  // Create a new cart if none exists


        Task<CartItem> GetCartItemByIdAsync(int cartItemId);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int cartItemId);
    }
}