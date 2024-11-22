using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
	public class DetailsStaffModel : PageModel
	{
		private readonly IStaffService _staffService;

		// Constructor nhận vào StaffService
		public DetailsStaffModel(IStaffService staffService)
		{
			_staffService = staffService;
		}

		public Staff StaffDetails { get; set; }

		public async Task<IActionResult> OnGetAsync(int staffId)
		{
			// Lấy thông tin chi tiết nhân viên từ DB
			StaffDetails = await _staffService.GetStaffByIdAsync(staffId);
			if (StaffDetails == null)
			{
				return NotFound(); // Nếu không tìm thấy nhân viên
			}
			return Page();
		}
	}
}
