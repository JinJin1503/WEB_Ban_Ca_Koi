using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiFarmShop.Repositories.Entities;

namespace KoiFarmShop.WebApplication.Pages.Product
{
	public class EditModel : PageModel
	{
		private readonly KoiFarmShop.Repositories.Entities.KoiFarmDbContext _context;

		public EditModel(KoiFarmShop.Repositories.Entities.KoiFarmDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public KoiFish KoiFish { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			KoiFish = await _context.KoiFishs
				.Include(k => k.Category).FirstOrDefaultAsync(m => m.KoiId == id);

			if (KoiFish == null)
			{
				return NotFound();
			}
			LoadCategoryOptions(KoiFish.CategoryId);
			return Page();
		}

		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (KoiFish != null && KoiFish.KoiId <= 0 && id.HasValue)
			{
				KoiFish.KoiId = id.Value;
			}

			NormalizeValidation();

			if (!ModelState.IsValid)
			{
				LoadCategoryOptions(KoiFish?.CategoryId);
				return Page();
			}

			var existingKoiFish = await _context.KoiFishs.FindAsync(KoiFish.KoiId);
			if (existingKoiFish == null)
			{
				return NotFound();
			}

			existingKoiFish.KoiName = KoiFish.KoiName;
			existingKoiFish.Origin = KoiFish.Origin;
			existingKoiFish.Gender = KoiFish.Gender;
			existingKoiFish.Age = KoiFish.Age;
			existingKoiFish.Size = KoiFish.Size;
			existingKoiFish.BreedType = KoiFish.BreedType;
			existingKoiFish.Personality = KoiFish.Personality;
			existingKoiFish.DailyFeed = KoiFish.DailyFeed;
			existingKoiFish.ScreeningRate = KoiFish.ScreeningRate;
			existingKoiFish.HealthStatus = KoiFish.HealthStatus;
			existingKoiFish.Awards = KoiFish.Awards;
			existingKoiFish.PricePerKoi = KoiFish.PricePerKoi;
			existingKoiFish.PricePerBatch = KoiFish.PricePerBatch;
			existingKoiFish.ImageURL = KoiFish.ImageURL;
			existingKoiFish.CategoryId = KoiFish.CategoryId;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!KoiFishExists(KoiFish.KoiId))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToPage("./Index");
		}

		private void LoadCategoryOptions(int? selectedCategoryId = null)
		{
			ViewData["CategoryId"] = new SelectList(_context.KoiCategories, "CategoryId", "CategoryName", selectedCategoryId);
		}

		private bool KoiFishExists(int id)
		{
			return _context.KoiFishs.Any(e => e.KoiId == id);
		}

		private void NormalizeValidation()
		{
			ModelState.Remove("KoiFish.Category");

			if (KoiFish == null)
			{
				ModelState.AddModelError(string.Empty, "Dữ liệu sản phẩm không hợp lệ.");
				return;
			}

			TryValidateModel(KoiFish, nameof(KoiFish));
			ModelState.Remove("KoiFish.Category");

			ValidateRange(nameof(KoiFish.Age), KoiFish.Age, 0, KoiFarmShop.Repositories.Entities.KoiFish.MaxAge);
			ValidateRange(nameof(KoiFish.Size), KoiFish.Size, 0, KoiFarmShop.Repositories.Entities.KoiFish.MaxSize);
			ValidateRange(nameof(KoiFish.DailyFeed), KoiFish.DailyFeed, 0, KoiFarmShop.Repositories.Entities.KoiFish.MaxDailyFeed);
			ValidateRange(nameof(KoiFish.ScreeningRate), KoiFish.ScreeningRate, 0, 100);
			ValidateRange(nameof(KoiFish.PricePerKoi), KoiFish.PricePerKoi, 0, KoiFarmShop.Repositories.Entities.KoiFish.MaxPricePerKoi);
			ValidateRange(nameof(KoiFish.PricePerBatch), KoiFish.PricePerBatch, 0, KoiFarmShop.Repositories.Entities.KoiFish.MaxPricePerBatch);
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

