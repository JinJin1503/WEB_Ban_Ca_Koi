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

namespace KoiFarmShop.WebApplication.Pages.Cart
{
    public class EditModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly IKoiFishService _koiFishService;

        public EditModel(ICartService cartService, IKoiFishService koiFishService)
        {
            _cartService = cartService;
            _koiFishService = koiFishService;
        }
        [BindProperty]
        public CartItem CartItem { get; set; }
        public List<CartItem> CartItems { get; set; }

        public CartEntities Cart { get; set; }
        public async Task OnGetAsync()
        {
            int customerId = 1; // Replace with actual customer ID
            CartItems = await _cartService.GetCartItemsAsync(customerId);
        }
        public async Task<IActionResult> OnPostUpdateCartAsync()
        {
            try
            {
                await _cartService.UpdateCartItem(CartItem.CartItemId, CartItem.KoiId, CartItem.QuantityPerKoi, CartItem.QuantityPerBatch);
                TempData["SuccessMessage"] = "Cập nhật giỏ hàng thành công.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Không thể cập nhật giỏ hàng.";
            }

            return RedirectToPage("/cart/Index");

        }
    }
}
