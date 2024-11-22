using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
    // KoiCategoryRepository.cs
    public interface IKoiCategoryRepository
    {
        Task<IEnumerable<KoiCategory>> GetAllCategoriesAsync(); // Lấy tất cả danh mục Koi
        Task<KoiCategory> GetCategoryByIdAsync(int categoryId); // Lấy danh mục Koi theo ID
        Task AddCategoryAsync(KoiCategory category); // Thêm một danh mục Koi mới
        Task UpdateCategoryAsync(KoiCategory category); // Cập nhật danh mục Koi
        Task DeleteCategoryAsync(int categoryId); // Xóa danh mục Koi
    }

}
