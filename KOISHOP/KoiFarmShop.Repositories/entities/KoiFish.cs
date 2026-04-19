using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Entities
{
	public class KoiFish
	{
		public const int MaxKoiNameLength = 100;
		public const int MaxOriginLength = 100;
		public const int MaxGenderLength = 20;
		public const int MaxBreedTypeLength = 50;
		public const int MaxPersonalityLength = 100;
		public const int MaxHealthStatusLength = 100;
		public const int MaxAwardsLength = 200;
		public const int MaxImageUrlLength = 500;
		public const int MaxAge = 100;
		public const float MaxSize = 200;
		public const int MaxDailyFeed = 1000;
		public const int MaxPricePerKoi = 100000000;
		public const int MaxPricePerBatch = 1000000000;

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int  KoiId { get; set; }

		[Required(ErrorMessage = "Ten ca Koi khong duoc de trong.")]
		[StringLength(MaxKoiNameLength, ErrorMessage = "Ten ca Koi khong duoc vuot qua 100 ky tu.")]
		public string KoiName { get; set; }

		[Required(ErrorMessage = "Nguon goc khong duoc de trong.")]
		[StringLength(MaxOriginLength, ErrorMessage = "Nguon goc khong duoc vuot qua 100 ky tu.")]
		public string Origin { get; set; }

		[Required(ErrorMessage = "Gioi tinh khong duoc de trong.")]
		[StringLength(MaxGenderLength, ErrorMessage = "Gioi tinh khong duoc vuot qua 20 ky tu.")]
		public string Gender { get; set; }

		[Range(0, MaxAge, ErrorMessage = "Tuoi phai nam trong khoang 0 - 100.")]
		public int Age { get; set; }

		[Range(0, MaxSize, ErrorMessage = "Kich thuoc phai nam trong khoang 0 - 200.")]
		public float Size { get; set; }

		[Required(ErrorMessage = "Loai giong khong duoc de trong.")]
		[StringLength(MaxBreedTypeLength, ErrorMessage = "Loai giong khong duoc vuot qua 50 ky tu.")]
		public string BreedType { get; set; }

		[Required(ErrorMessage = "Tinh cach khong duoc de trong.")]
		[StringLength(MaxPersonalityLength, ErrorMessage = "Tinh cach khong duoc vuot qua 100 ky tu.")]
		public string Personality { get; set; }

		[Range(0, MaxDailyFeed, ErrorMessage = "Luong an hang ngay phai nam trong khoang 0 - 1000.")]
		public int DailyFeed { get; set; }

		[Range(typeof(decimal), "0", "100", ErrorMessage = "Ti le sang loc phai nam trong khoang 0 - 100.")]
		public decimal ScreeningRate { get; set; }

		[Required(ErrorMessage = "Tinh trang suc khoe khong duoc de trong.")]
		[StringLength(MaxHealthStatusLength, ErrorMessage = "Tinh trang suc khoe khong duoc vuot qua 100 ky tu.")]
		public string HealthStatus { get; set; }

		[StringLength(MaxAwardsLength, ErrorMessage = "Giai thuong khong duoc vuot qua 200 ky tu.")]
		public string Awards { get; set; }

		[Range(0, MaxPricePerKoi, ErrorMessage = "Gia moi con phai nam trong khoang 0 - 100000000.")]
		public int  PricePerKoi { get; set; }

		[Range(0, MaxPricePerBatch, ErrorMessage = "Gia theo lo phai nam trong khoang 0 - 1000000000.")]
		public int PricePerBatch { get; set; }

		[Required(ErrorMessage = "Duong dan hinh anh khong duoc de trong.")]
		[StringLength(MaxImageUrlLength, ErrorMessage = "Duong dan hinh anh khong duoc vuot qua 500 ky tu.")]
		public string ImageURL { get; set; }

		
		[Range(1, int.MaxValue, ErrorMessage = "Danh muc san pham khong hop le.")]
		public int  CategoryId { get; set; }
		public KoiCategory Category { get; set; }
	}
}
