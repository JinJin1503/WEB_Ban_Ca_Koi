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
		private const string ProductErrorMessageSessionKey = "ProductIndexErrorMessage";
		private const string ProductErrorMessageCookieName = "KoiFarmShopProductErrorMessage";

		public IndexModel(IKoiFishService koiFishService, ICartService cartService)
		{
			_koiFishService = koiFishService;
			_cartService = cartService;
		}

		public IList<KoiFish> KoiFish { get; set; } = new List<KoiFish>();
		public List<KoiFish> Cart { get; set; } = new List<KoiFish>();
		public string ErrorMessage { get; private set; }

		[BindProperty(SupportsGet = true)]
		public string Keyword { get; set; }

		// This will hold the CartItem and also the quantity
		[BindProperty]
		public int KoiId { get; set; }

		public async Task OnGetAsync()
		{
			RestorePendingErrorMessage();

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
				SetErrorMessage("Sản phẩm không hợp lệ.");
				return RedirectToProductIndex();
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

		private IActionResult RedirectToProductIndex()
		{
			if (string.IsNullOrWhiteSpace(Keyword))
			{
				return Redirect("/Product/Index");
			}

			return Redirect($"/Product/Index?keyword={Uri.EscapeDataString(Keyword)}");
		}

		private void SetErrorMessage(string message)
		{
			TempData["ErrorMessage"] = message;
			HttpContext.Session.SetString(ProductErrorMessageSessionKey, message);
			Response.Cookies.Append(ProductErrorMessageCookieName, message, new CookieOptions
			{
				HttpOnly = true,
				IsEssential = true,
				MaxAge = TimeSpan.FromMinutes(5),
				SameSite = SameSiteMode.Lax
			});
		}

		private void RestorePendingErrorMessage()
		{
			var message = Request.Cookies[ProductErrorMessageCookieName]
				?? HttpContext.Session.GetString(ProductErrorMessageSessionKey)
				?? TempData.Peek("ErrorMessage")?.ToString();

			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}

			ErrorMessage = message;
			TempData["ErrorMessage"] = message;
			Response.Cookies.Delete(ProductErrorMessageCookieName);
			HttpContext.Session.Remove(ProductErrorMessageSessionKey);
		}
	}
}

