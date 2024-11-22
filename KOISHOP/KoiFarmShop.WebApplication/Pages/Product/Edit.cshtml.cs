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
           ViewData["CategoryId"] = new SelectList(_context.KoiCategories, "CategoryId", "CategoryId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(KoiFish).State = EntityState.Modified;

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

        private bool KoiFishExists(int id)
        {
            return _context.KoiFishs.Any(e => e.KoiId == id);
        }
    }
}
