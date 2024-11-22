using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoiFarmShop.Repositories.Repositories
{
    // KoiCategoryRepository.cs
    public class KoiCategoryRepository : IKoiCategoryRepository
    {
        private readonly KoiFarmDbContext _context;

        public KoiCategoryRepository(KoiFarmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KoiCategory>> GetAllCategoriesAsync()
        {
            return await _context.KoiCategories.ToListAsync();
        }

        public async Task<KoiCategory> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.KoiCategories.FindAsync(categoryId);
        }

        public async Task AddCategoryAsync(KoiCategory category)
        {
            await _context.KoiCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(KoiCategory category)
        {
            _context.KoiCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await GetCategoryByIdAsync(categoryId);
            if (category != null)
            {
                _context.KoiCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }

}
