using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
	public class EditStaffModel : PageModel
	{
		private readonly IStaffService _staffService;

		// Constructor nhận vào StaffService
		public EditStaffModel(IStaffService staffService)
		{
			_staffService = staffService;
		}

		[BindProperty]
		public Staff ExistingStaff { get; set; }

		public async Task<IActionResult> OnGetAsync(int staffId)
		{
			// Lấy thông tin nhân viên từ DB
			ExistingStaff = await _staffService.GetStaffByIdAsync(staffId);
			if (ExistingStaff == null)
			{
				return NotFound(); // Nếu không tìm thấy nhân viên
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			// Cập nhật nhân viên
			await _staffService.UpdateStaffAsync(ExistingStaff);
			return RedirectToPage("./Index"); // Redirect về danh sách sau khi cập nhật thành công
		}
	}
}
