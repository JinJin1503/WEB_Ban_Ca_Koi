using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using KoiFarmShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Services
{
	public class KoiFishService : IKoiFishService
	{
		private readonly IKoiFishRepository _koiFishRepository;

		public KoiFishService(IKoiFishRepository koiFishRepository)
		{
			_koiFishRepository = koiFishRepository;
		}

		public async Task<List<KoiFish>> GetAllKoisAsync()
		{
			try
			{
				var koiFishList = await _koiFishRepository.GetKoiFishes();
				return koiFishList.Where(IsValidKoi).ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while retrieving all Koi fish.", ex);
			}
		}

		public async Task<KoiFish> GetKoiByIdAsync(int koiId)
		{
			try
			{
				var koi = await _koiFishRepository.GetKoiFishById(koiId);
				return IsValidKoi(koi) ? koi : null;
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while retrieving Koi fish with ID {koiId}.", ex);
			}
		}

		public async Task<List<KoiFish>> GetKoisByCategoryIdAsync(int categoryId)
		{
			try
			{
				var koiFishList = await GetAllKoisAsync();
				return koiFishList
					.Where(k => k.CategoryId == categoryId)
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while retrieving Koi fish in category ID {categoryId}.", ex);
			}
		}

		public async Task<List<KoiFish>> SearchKoiByKeywordAsync(string keyword)
		{
			try
			{
				var koiFishList = await _koiFishRepository.SearchKoiFishAsync(keyword);
				return koiFishList.Where(IsValidKoi).ToList();
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while searching Koi fish with keyword '{keyword}'.", ex);
			}
		}

		public async Task AddKoiAsync(KoiFish koi)
		{
			ValidateKoi(koi);

			try
			{
				await _koiFishRepository.AddKoiFish(koi);
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while adding a new Koi fish.", ex);
			}
		}

		public async Task UpdateKoiAsync(KoiFish koi)
		{
			ValidateKoi(koi);

			try
			{
				await _koiFishRepository.UpdateKoiFish(koi);
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while updating Koi fish with ID {koi.KoiId}.", ex);
			}
		}

		public async Task DeleteKoiAsync(int koiId)
		{
			try
			{
				await _koiFishRepository.DeleteKoiFish(koiId);
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while deleting Koi fish with ID {koiId}.", ex);
			}
		}

		public async Task<List<KoiFish>> SearchKoiByCriteriaAsync(string breedType, string category)
		{
			try
			{
				var koiFishList = await GetAllKoisAsync();
				return koiFishList.Where(k =>
					(string.IsNullOrEmpty(breedType) || k.BreedType.Equals(breedType, StringComparison.OrdinalIgnoreCase)) &&
					(string.IsNullOrEmpty(category) || k.Category.CategoryName.Equals(category, StringComparison.OrdinalIgnoreCase)))
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while searching for Koi fish by criteria.", ex);
			}
		}
		public async Task<List<KoiFish>> CompareKoiAsync(List<int> koiId)
		{
			try
			{
				var koiFishList = await _koiFishRepository.GetKoiFishByIdsAsync(koiId);
				return koiFishList.Where(IsValidKoi).ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while comparing Koi fish.", ex);
			}
		}

		private static void ValidateKoi(KoiFish koi)
		{
			if (koi == null)
			{
				throw new ValidationException("Du lieu san pham khong hop le.");
			}

			var validationResults = GetValidationResults(koi);
			if (validationResults.Any())
			{
				throw new ValidationException(string.Join("; ", validationResults.Select(result => result.ErrorMessage)));
			}
		}

		private static bool IsValidKoi(KoiFish koi)
		{
			return koi != null && !GetValidationResults(koi).Any();
		}

		private static List<ValidationResult> GetValidationResults(KoiFish koi)
		{
			var validationResults = new List<ValidationResult>();
			if (koi == null)
			{
				validationResults.Add(new ValidationResult("Du lieu san pham khong hop le."));
				return validationResults;
			}

			var validationContext = new ValidationContext(koi);
			Validator.TryValidateObject(koi, validationContext, validationResults, true);
			return validationResults;
		}





	}
}
