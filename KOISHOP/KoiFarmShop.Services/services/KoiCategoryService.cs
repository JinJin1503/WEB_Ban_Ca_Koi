using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using KoiFarmShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Services
{
    public class KoiCategoryService : IKoiCategoryService
    {
        private readonly IKoiCategoryRepository _repository;
        public KoiCategoryService(IKoiCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<KoiCategory>> GetAllCategoriesAsync()
        {
            var categories = await _repository.GetAllCategoriesAsync();
            return categories.Select(c => new KoiCategory
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryDesc = c.CategoryDesc
            });
        }

        public async Task<KoiCategory> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _repository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                return null;
            }
            return new KoiCategory
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDesc = category.CategoryDesc
            };
        }

        public async Task CreateCategoryAsync(KoiCategory category)
        {
            var categories = new KoiCategory
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDesc = category.CategoryDesc
            };
            await _repository.AddCategoryAsync(category);
        }

        public async Task UpdateCategoryAsync(int categoryId, KoiCategory category)
        {
            var categories = await _repository.GetCategoryByIdAsync(categoryId);
            if (category != null)
            {
                category.CategoryName = category.CategoryName;
                category.CategoryDesc = category.CategoryDesc;
                await _repository.UpdateCategoryAsync(category);
            }
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await _repository.DeleteCategoryAsync(categoryId);
        }
    }

}
