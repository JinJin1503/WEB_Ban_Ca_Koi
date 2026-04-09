using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
			if (User.Identity?.IsAuthenticated != true)
			{
				return Challenge(new Microsoft.AspNetCore.Authentication.AuthenticationProperties
				{
					RedirectUri = Url.Page("/Product/Index", new { keyword = Keyword })
				}, CookieAuthenticationDefaults.AuthenticationScheme);
			}

			if (!User.IsInRole(AppRoles.Customer))
			{
				return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
			}

			if (KoiId <= 0)
			{
				TempData["ErrorMessage"] = "Sản phẩm không hợp lệ.";
				return RedirectToPage(new { keyword = Keyword });
			}

			int? customerId = User.GetCustomerId();
			if (!customerId.HasValue)
			{
				return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
			}

			try
			{
				await _cartService.AddCartItemToCartAsync(customerId.Value, KoiId, 1, 0, 0, 0);
				TempData["SuccessMessage"] = "Đã thêm sản phẩm vào giỏ hàng.";
			}
			catch
			{
				TempData["ErrorMessage"] = "Không thể thêm sản phẩm vào giỏ hàng.";
			}

			return RedirectToPage(new { keyword = Keyword });
		}
	}
}

