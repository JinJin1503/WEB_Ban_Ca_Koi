using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using CartEntities = KoiFarmShop.Repositories.Entities.Cart;  // Alias for Cart class

namespace KoiFarmShop.WebApplication.Pages.cart
{
	public class IndexModel : PageModel
	{
		private readonly ICartService _cartService;
		private readonly IOrderService _orderService;  // Add an order service to create orders
		public IndexModel(ICartService cartService, IOrderService orderService)
		{
			_cartService = cartService;
			_orderService = orderService;
		}

		public List<CartItem> CartItems { get; set; }
		public List<CartEntities> Carts { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			var customerIdString = HttpContext.Session.GetString("CustomerId");

			if (string.IsNullOrEmpty(customerIdString))
			{
				return RedirectToPage("/Login/Login");
			}

			int customerId = int.Parse(customerIdString);
			var cart = await _cartService.GetCartByCustomerIdAsync(customerId);
			CartItems = await _cartService.GetCartItemsAsync(customerId);
			Carts = cart != null ? new List<CartEntities> { cart } : new List<CartEntities>();

			return Page();
		}

		// Method for updating cart item quantities (Edit)
		public async Task<IActionResult> OnPostEditAsync(int cartItemId, int newQuantityPerKoi, int newQuantityPerBatch)
		{
			var customerIdString = HttpContext.Session.GetString("CustomerId");

			if (string.IsNullOrEmpty(customerIdString))
			{
				return RedirectToPage("/Login/Login");
			}

			if (newQuantityPerKoi < 0 || newQuantityPerBatch < 0)
			{
				TempData["ErrorMessage"] = "Số lượng không được nhỏ hơn 0.";
				return RedirectToPage();
			}

			var cartItem = await _cartService.GetCartItemByIdAsync(cartItemId);

			if (cartItem == null)
			{
				TempData["ErrorMessage"] = "Không tìm thấy sản phẩm trong giỏ hàng.";
				return RedirectToPage();
			}

			if (newQuantityPerKoi == 0 && newQuantityPerBatch == 0)
			{
				await _cartService.DeleteCartItemAsync(cartItemId);
				TempData["SuccessMessage"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
				return RedirectToPage();
			}

			cartItem.QuantityPerKoi = newQuantityPerKoi;
			cartItem.QuantityPerBatch = newQuantityPerBatch;

			await _cartService.UpdateCartItemAsync(cartItem);
			TempData["SuccessMessage"] = "Cập nhật giỏ hàng thành công.";

			return RedirectToPage();
		}

		// Method for deleting a cart item
		public async Task<IActionResult> OnPostDeleteAsync(int cartItemId)
		{
			var customerIdString = HttpContext.Session.GetString("CustomerId");

			if (string.IsNullOrEmpty(customerIdString))
			{
				return RedirectToPage("/Login/Login");
			}

			await _cartService.DeleteCartItemAsync(cartItemId);
			TempData["SuccessMessage"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
			return RedirectToPage();
		}

		// Method for processing payment
		public async Task<IActionResult> OnPostPaymentAsync()
		{
			var customerIdString = HttpContext.Session.GetString("CustomerId");

			if (string.IsNullOrEmpty(customerIdString))
			{
				return RedirectToPage("/Login/Login");
			}

			int customerId = int.Parse(customerIdString);

			// Fetch the cart items for this customer
			var cartItems = await _cartService.GetCartItemsAsync(customerId);
			if (cartItems == null || !cartItems.Any())
			{
				TempData["ErrorMessage"] = "Giỏ hàng của bạn không có sản phẩm!";
				return RedirectToPage();
			}

			// Create an order based on the cart items
			var order = new Orders
			{
				CustomerId = customerId,
				StaffId = 1,  // For simplicity, assume the staffId is 1 (you may want to change this)
				OrderDate = DateTime.Now
			};

			// Create order details for each cart item
			List<OrderDetails> orderDetails = cartItems.Select(item => new OrderDetails
			{
				QuantityPerKoi = item.QuantityPerKoi,
				QuantityPerBatch = item.QuantityPerBatch,
				KoiId = item.KoiId,
				PromotionId = 1,
				Status = "Pending",  // Initial status, can change later based on payment success
				PaymentMethod = "Online",  // You can modify this based on actual payment method
				ShippingAddress = "Default Address"  // For simplicity, default address is used
			}).ToList();

			order.OrderDetails = orderDetails;

			// Save the order
			await _orderService.CreateOrderAsync(order);

			// Clear the cart after successful payment
			await _cartService.ClearCartAsync(customerId);

			// Redirect to an order confirmation page
			TempData["SuccessMessage"] = "Thanh toán thành công! Đơn hàng của bạn đã được xử lý.";
			return RedirectToPage("/cart/Confirmation", new { orderId = order.OrderId });
		}
	}
}
