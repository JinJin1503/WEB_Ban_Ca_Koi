using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KoiFarmShop.Repositories.Entities;

namespace KoiFarmShop.WebApplication.Pages.Product
{
    public class DetailsModel : PageModel
    {
        private readonly KoiFarmShop.Repositories.Entities.KoiFarmDbContext _context;

        public DetailsModel(KoiFarmShop.Repositories.Entities.KoiFarmDbContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
