using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KoiFarmShop.WebApplication.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly IKoiFishService _koiFishService;
        private readonly ICartService _cartService;  // Add ICartService for cart functionality

        public IndexModel(IKoiFishService koiFishService, ICartService cartService)
        {
            _koiFishService = koiFishService;
            _cartService = cartService;
        }

        public IList<KoiFish> KoiFish { get; set; }
        public List<KoiFish> Cart { get; set; } = new List<KoiFish>();

        // This will hold the CartItem and also the quantity
        [BindProperty]
        public int KoiId { get; set; }

        public async Task OnGetAsync()
        {
            KoiFish = await _koiFishService.GetAllKoisAsync();
        }

        // This is the handler method for adding to the cart
        public async Task<IActionResult> OnPostAddToCartAsync()
        {
            // Get the CustomerId from the session (you may want to handle the case where it's not found)
            var customerIdString = HttpContext.Session.GetString("CustomerId");

            if (string.IsNullOrEmpty(customerIdString))
            {
                // Redirect to login if CustomerId is not found in the session
                return RedirectToPage("/Login/Login");
            }

            int customerId = int.Parse(customerIdString);

            // Call the service to add the Koi to the cart
            var cart = await _cartService.GetCartByCustomerIdAsync(customerId);

            if (cart == null)
            {
                // If no cart exists for the customer, create a new one
                cart = new Repositories.Entities.Cart
                {
                    CustomerId = customerId,
                    CartItems = new List<CartItem>()
                };

                // Add a new cart entry (assuming your cart creation logic is handled by the service)
                await _cartService.CreateCartAsync(cart);
            }

            // Add the Koi to the cart (assuming the cartItem logic is handled here)
            await _cartService.AddCartItemToCartAsync(customerId, KoiId, 1, 1, 0, 0); // Example: 1 Koi, no batches

            // After adding to the cart, redirect back to the current page
            return RedirectToPage();
        }
    }
}
