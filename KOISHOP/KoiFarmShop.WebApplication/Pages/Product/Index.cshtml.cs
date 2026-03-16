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

		public IList<KoiFish> KoiFish { get; set; } = new List<KoiFish>();
		public List<KoiFish> Cart { get; set; } = new List<KoiFish>();

		[BindProperty(SupportsGet = true)]
		public string Keyword { get; set; }

		// This will hold the CartItem and also the quantity
		[BindProperty]
		public int KoiId { get; set; }

		public async Task OnGetAsync()
		{
			if (string.IsNullOrWhiteSpace(Keyword))
			{
				KoiFish = await _koiFishService.GetAllKoisAsync();
				return;
			}

			Keyword = Keyword.Trim();
			KoiFish = await _koiFishService.SearchKoiByKeywordAsync(Keyword);
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

			if (KoiId <= 0)
			{
				TempData["ErrorMessage"] = "Sản phẩm không hợp lệ.";
				return RedirectToPage(new { keyword = Keyword });
			}

			int customerId = int.Parse(customerIdString);

			await _cartService.AddCartItemToCartAsync(customerId, KoiId, 1, 0, 0, 0);
			TempData["SuccessMessage"] = "Đã thêm sản phẩm vào giỏ hàng.";

			return RedirectToPage(new { keyword = Keyword });
		}
	}
}
