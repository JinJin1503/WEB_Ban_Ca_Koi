using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
    public interface IKoiCategoryService
    {
        Task<IEnumerable<KoiCategory>> GetAllCategoriesAsync(); // Lấy tất cả danh mục Koi
        Task<KoiCategory> GetCategoryByIdAsync(int categoryId); // Lấy danh mục Koi theo ID
        Task CreateCategoryAsync(KoiCategory category); // Tạo danh mục Koi mới
        Task UpdateCategoryAsync(int categoryId, KoiCategory category); // Cập nhật danh mục Koi
        Task DeleteCategoryAsync(int categoryId); // Xóa danh mục Koi
    }

}
