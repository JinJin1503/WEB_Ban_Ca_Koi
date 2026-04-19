using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;

namespace KoiFarmShop.WebApplication.Pages.Trangchu
{
	public class CreateModel : PageModel
	{
		private readonly IKoiFishService _koiFishService;
		private readonly IKoiCategoryService _koiCategoryService;

		public CreateModel(IKoiFishService koiFishService, IKoiCategoryService koiCategoryService)
		{
			_koiFishService = koiFishService;
			_koiCategoryService = koiCategoryService;
		}

		public SelectList CategoryList { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			await LoadCategoryOptionsAsync();
			return Page();
		}

		[BindProperty]
		public KoiFish KoiFish { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("KoiFish.Category");
			if (KoiFish == null)
			{
				ModelState.AddModelError(string.Empty, "Du lieu san pham khong hop le.");
			}
			else
			{
				TryValidateModel(KoiFish, nameof(KoiFish));
				ModelState.Remove("KoiFish.Category");
			}

			if (!ModelState.IsValid)
			{
				await LoadCategoryOptionsAsync(KoiFish?.CategoryId);
				return Page();
			}

			// Thêm mới KoiFish qua service
			await _koiFishService.AddKoiAsync(KoiFish);

			return RedirectToPage("./Index");
		}

		private async Task LoadCategoryOptionsAsync(int? selectedCategoryId = null)
		{
			var categories = await _koiCategoryService.GetAllCategoriesAsync();
			CategoryList = new SelectList(categories, "CategoryId", "CategoryName", selectedCategoryId);
			ViewData["CategoryId"] = CategoryList;
		}
	}
}
