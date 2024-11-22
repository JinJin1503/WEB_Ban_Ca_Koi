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
    public class DeleteModel : PageModel
    {
        private readonly ICartService _cartService;

        public DeleteModel(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Thực thể Cart để bind dữ liệu khi gửi form
        [BindProperty]
        public KoiFarmShop.Repositories.Entities.Cart Cart { get; set; }

        public async Task<IActionResult> OnGetDeleteAsync(int cartItemId)
        {
            var customerId = 1; // Replace with actual customer ID
            await _cartService.RemoveFromCartAsync(customerId, cartItemId);
            return RedirectToPage("/Cart");
        }
    }
}
