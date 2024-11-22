using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;

namespace KoiFarmShop.WebApplication.Pages.Cart
{
    public class DetailsModel : PageModel
    {
     
        private readonly ICartService _cartService;

        public DetailsModel(ICartService cartService)
        {
            _cartService = cartService;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Cart = await _cartService.GetCartByCustomerIdAsync(id.Value);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return Page();
        }
        public KoiFarmShop.Repositories.Entities.Cart Cart { get; set; }

        // This will handle removing an item from the cart
        public async Task<IActionResult> OnPostRemoveAsync(int cartItemId, int cartId)
        {
            try
            {
                await _cartService.RemoveFromCartAsync(cartId, cartItemId);
                return RedirectToPage("./Details", new { id = cartId });
            }
            catch
            {
                return RedirectToPage("./Details", new { id = cartId });
            }
        }
    }
}