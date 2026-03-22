using KoiFarmShop.Repositories.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
	public class ProductsModel : PageModel
	{
		private readonly KoiFarmDbContext _context;

		public ProductsModel(KoiFarmDbContext context)
		{
			_context = context;
		}

		public List<KoiFish> ProductList { get; set; } = new List<KoiFish>();

		public async Task OnGetAsync()
		{
			ProductList = await _context.KoiFishs
				.Include(k => k.Category)
				.ToListAsync();
		}
	}
}
