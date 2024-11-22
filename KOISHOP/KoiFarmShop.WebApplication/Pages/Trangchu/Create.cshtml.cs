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
			// Lấy danh sách danh mục Koi từ service
			var categories = await _koiCategoryService.GetAllCategoriesAsync();
			CategoryList = new SelectList(categories, "CategoryId", "CategoryName");

			return Page();
		}

		[BindProperty]
		public KoiFish KoiFish { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			// Thêm mới KoiFish qua service
			await _koiFishService.AddKoiAsync(KoiFish);

			return RedirectToPage("./Index");
		}
	}
}