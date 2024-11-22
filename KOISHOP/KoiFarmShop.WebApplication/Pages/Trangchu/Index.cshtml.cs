using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;

namespace KoiFarmShop.WebApplication.Pages.Trangchu
{
	public class IndexModel : PageModel
	{
		private readonly IKoiFishService _koiFishService;

		public IndexModel(IKoiFishService koiFishService)
		{
			_koiFishService = koiFishService;
		}

		public IList<KoiFish> KoiFish { get; set; }

		public async Task OnGetAsync()
		{
			// Lấy danh sách KoiFish từ service
			KoiFish = await _koiFishService.GetAllKoisAsync();
		}
	}
}