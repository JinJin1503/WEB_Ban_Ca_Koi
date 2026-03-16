using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface IKoiFishRepository
	{
		Task<List<KoiFish>> GetKoiFishes();
		Task<KoiFish> GetKoiFishById(int koiId);
		Task AddKoiFish(KoiFish koi);
		Task UpdateKoiFish(KoiFish koi);
		Task DeleteKoiFish(int koiId);

		Task<List<KoiFish>> GetKoiFishByIdsAsync(List<int> koiId);

		Task<List<KoiFish>> SearchKoiFishAsync(string keyword);



	}
}
