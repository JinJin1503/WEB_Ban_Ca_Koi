using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;

namespace KoiFarmShop.WebApplication.Pages.Trangchu
{
	public class DetailsModel : PageModel
	{
		private readonly IKoiFishService _koiFishService; // Dịch vụ lấy cá koi
		private readonly IKoiCategoryService _koiCategoryService; // Dịch vụ lấy danh mục cá koi

		public DetailsModel(IKoiFishService koiFishService, IKoiCategoryService koiCategoryService)
		{
			_koiFishService = koiFishService;
			_koiCategoryService = koiCategoryService;
		}

		public KoiFish KoiFish { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			// Sử dụng dịch vụ để lấy thông tin cá koi theo id
			KoiFish = await _koiFishService.GetKoiByIdAsync(id.Value);

			if (KoiFish == null)
			{
				return NotFound();
			}
			return Page();
		}
	}
}