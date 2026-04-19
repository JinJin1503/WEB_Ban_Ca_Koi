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
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int  KoiId { get; set; }
		public string KoiName { get; set; }
		public string Origin { get; set; }
		public string Gender { get; set; }
		public int Age { get; set; }
		public float Size { get; set; }
		public string BreedType { get; set; }
		public string Personality { get; set; }
		public int DailyFeed { get; set; }
		public decimal ScreeningRate { get; set; }
		public string HealthStatus { get; set; }
		public string Awards { get; set; }
		[Range(0, 100000000)]
		public int  PricePerKoi { get; set; }
		public int PricePerBatch { get; set; }
		public string ImageURL { get; set; }

		
		public int  CategoryId { get; set; }
		public KoiCategory Category { get; set; }
	}
}
