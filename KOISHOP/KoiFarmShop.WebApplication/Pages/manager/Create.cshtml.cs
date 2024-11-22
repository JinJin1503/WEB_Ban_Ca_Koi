using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
	public class CreateStaffModel : PageModel
	{
		private readonly IStaffService _staffService;

		// Constructor nhận vào StaffService
		public CreateStaffModel(IStaffService staffService)
		{
			_staffService = staffService;
		}

		[BindProperty]
		public Staff NewStaff { get; set; }

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			// Thêm nhân viên mới vào cơ sở dữ liệu
			await _staffService.AddStaffAsync(NewStaff);
			return RedirectToPage("./Index"); // Redirect về danh sách sau khi thêm thành công
		}
	}
}
