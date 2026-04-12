using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiFarmShop.Repositories.Entities;

namespace KoiFarmShop.WebApplication.Pages.Product
{
	public class CreateModel : PageModel
	{
		private readonly KoiFarmShop.Repositories.Entities.KoiFarmDbContext _context;

		public CreateModel(KoiFarmShop.Repositories.Entities.KoiFarmDbContext context)
		{
			_context = context;
		}

		public IActionResult OnGet()
		{
			LoadCategoryOptions();
			return Page();
		}

		[BindProperty]
		public KoiFish KoiFish { get; set; }

		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			NormalizeValidation();

			if (!ModelState.IsValid)
			{
				LoadCategoryOptions(KoiFish?.CategoryId);
				return Page();
			}

			_context.KoiFishs.Add(KoiFish);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}

		private void LoadCategoryOptions(int? selectedCategoryId = null)
		{
			ViewData["CategoryId"] = new SelectList(_context.KoiCategories, "CategoryId", "CategoryName", selectedCategoryId);
		}

		private void NormalizeValidation()
		{
			ModelState.Remove("KoiFish.Category");

			if (KoiFish == null)
			{
				ModelState.AddModelError(string.Empty, "Dữ liệu sản phẩm không hợp lệ.");
				return;
			}

			ValidateNonNegative(nameof(KoiFish.Age), KoiFish.Age);
			ValidateNonNegative(nameof(KoiFish.Size), KoiFish.Size);
			ValidateNonNegative(nameof(KoiFish.DailyFeed), KoiFish.DailyFeed);
			ValidateNonNegative(nameof(KoiFish.ScreeningRate), KoiFish.ScreeningRate);
			ValidateNonNegative(nameof(KoiFish.PricePerKoi), KoiFish.PricePerKoi);
			ValidateNonNegative(nameof(KoiFish.PricePerBatch), KoiFish.PricePerBatch);
		}

		private void ValidateNonNegative(string propertyName, int value)
		{
			if (value < 0)
			{
				ModelState.AddModelError($"KoiFish.{propertyName}", $"{propertyName} phải lớn hơn hoặc bằng 0.");
			}
		}

		private void ValidateNonNegative(string propertyName, float value)
		{
			if (value < 0)
			{
				ModelState.AddModelError($"KoiFish.{propertyName}", $"{propertyName} phải lớn hơn hoặc bằng 0.");
			}
		}

		private void ValidateNonNegative(string propertyName, decimal value)
		{
			if (value < 0)
			{
				ModelState.AddModelError($"KoiFish.{propertyName}", $"{propertyName} phải lớn hơn hoặc bằng 0.");
			}
		}
	}
}

