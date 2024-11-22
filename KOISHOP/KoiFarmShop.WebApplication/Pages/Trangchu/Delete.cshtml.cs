using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;

namespace KoiFarmShop.WebApplication.Pages.Trangchu
{
	public class DeleteModel : PageModel
	{
		private readonly IKoiFishService _koiFishService;

		public DeleteModel(IKoiFishService koiFishService)
		{
			_koiFishService = koiFishService;
		}

		[BindProperty]
		public KoiFish KoiFish { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			// Sử dụng dịch vụ để lấy thông tin KoiFish
			KoiFish = await _koiFishService.GetKoiByIdAsync(id.Value);

			if (KoiFish == null)
			{
				return NotFound();
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			try
			{
				// Sử dụng dịch vụ để xóa KoiFish
				await _koiFishService.DeleteKoiAsync(id.Value);
			}
			catch (Exception ex)
			{
				// Log lỗi hoặc hiển thị thông báo lỗi
				ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
				return Page();
			}

			return RedirectToPage("./Index");
		}
	}
}