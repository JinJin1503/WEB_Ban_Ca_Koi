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

			ValidateRange(nameof(KoiFish.Age), KoiFish.Age, 0, 100);
			ValidateRange(nameof(KoiFish.Size), KoiFish.Size, 0, 200);
			ValidateRange(nameof(KoiFish.DailyFeed), KoiFish.DailyFeed, 0, 1000);
			ValidateRange(nameof(KoiFish.ScreeningRate), KoiFish.ScreeningRate, 0, 100);
			ValidateRange(nameof(KoiFish.PricePerKoi), KoiFish.PricePerKoi, 0, 100000000);
			ValidateRange(nameof(KoiFish.PricePerBatch), KoiFish.PricePerBatch, 0, 1000000000);
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
	
		private void ValidateRange(string propertyName, int value, int min, int max)
		{
			if (value < min || value > max)
			{
				ModelState.AddModelError($"KoiFish.{propertyName}",
					$"{propertyName} phải nằm trong khoảng {min} - {max}.");
			}
		}

		private void ValidateRange(string propertyName, float value, float min, float max)
		{
			if (value < min || value > max)
			{
				ModelState.AddModelError($"KoiFish.{propertyName}",
					$"{propertyName} phải nằm trong khoảng {min} - {max}.");
			}
		}

		private void ValidateRange(string propertyName, decimal value, decimal min, decimal max)
		{
			if (value < min || value > max)
			{
				ModelState.AddModelError($"KoiFish.{propertyName}",
					$"{propertyName} phải nằm trong khoảng {min} - {max}.");
			}
		}
	}
}