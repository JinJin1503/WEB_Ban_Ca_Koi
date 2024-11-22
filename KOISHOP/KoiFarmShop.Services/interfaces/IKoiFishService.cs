using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface IKoiFishService
	{
		Task<List<KoiFish>> GetAllKoisAsync();
		Task<KoiFish> GetKoiByIdAsync(int koiId);
		Task<List<KoiFish>> GetKoisByCategoryIdAsync(int categoryId);
		Task AddKoiAsync(KoiFish koi);
		Task UpdateKoiAsync(KoiFish koi);
		Task DeleteKoiAsync(int koiId);
		Task<List<KoiFish>> SearchKoiByCriteriaAsync(string breedType, string category); // Tìm kiếm theo tiêu chí

		// Thêm phương thức CompareKoiAsync
		Task<List<KoiFish>> CompareKoiAsync(List<int> koiIds);
	}
}
