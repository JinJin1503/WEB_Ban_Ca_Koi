using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
	public class DeleteStaffModel : PageModel
	{
		private readonly IStaffService _staffService;

		// Constructor nhận vào StaffService
		public DeleteStaffModel(IStaffService staffService)
		{
			_staffService = staffService;
		}

		[BindProperty]
		public Staff StaffToDelete { get; set; }

		public async Task<IActionResult> OnGetAsync(int staffId)
		{
			// Lấy thông tin nhân viên từ DB
			StaffToDelete = await _staffService.GetStaffByIdAsync(staffId);
			if (StaffToDelete == null)
			{
				return NotFound(); // Nếu không tìm thấy nhân viên
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int staffId)
		{
			// Xóa nhân viên
			await _staffService.DeleteStaffAsync(staffId);
			return RedirectToPage("./Index"); // Redirect về danh sách sau khi xóa thành công
		}
	}
}
