using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;

namespace KoiFarmShop.WebApplication.Pages.Trangchu
{
	public class EditModel : PageModel
	{
		private readonly IKoiFishService _koiFishService; // Sử dụng service cho Koi Fish
		private readonly IKoiCategoryService _koiCategoryService; // Sử dụng service cho Category

		public EditModel(IKoiFishService koiFishService, IKoiCategoryService koiCategoryService)
		{
			_koiFishService = koiFishService;
			_koiCategoryService = koiCategoryService;
		}

		[BindProperty]
		public KoiFish KoiFish { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			// Thay thế việc truy vấn từ _context bằng cách sử dụng service
			KoiFish = await _koiFishService.GetKoiByIdAsync(id.Value);

			if (KoiFish == null)
			{
				return NotFound();
			}

			// Lấy danh mục từ service thay vì _context
			ViewData["CategoryId"] = new SelectList(await _koiCategoryService.GetAllCategoriesAsync(), "CategoryId", "CategoryName");
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			// Cập nhật thông tin cá koi qua service thay vì _context
			try
			{
				await _koiFishService.UpdateKoiAsync(KoiFish);
			}
			catch (Exception ex)
			{
				// Xử lý lỗi khi cập nhật
				throw new Exception("An error occurred while updating the Koi fish.", ex);
			}

			return RedirectToPage("./Index");
		}
	}
}