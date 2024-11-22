using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Dichvu
{
	public class IndexModel : PageModel
	{
		private readonly ICareServiceService _careService;

		public IndexModel(ICareServiceService careService)
		{
			_careService = careService;
		}

		public IList<CareService> CareService { get; set; }

		public async Task OnGetAsync()
		{
			// Nếu muốn dùng phương pháp gốc mà không sửa code của bạn:
			CareService = await _careService.GetAllCareServicesAsync();
		}
	}
}
