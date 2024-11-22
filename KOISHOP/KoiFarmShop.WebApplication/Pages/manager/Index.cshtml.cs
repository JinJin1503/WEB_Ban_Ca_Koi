using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
	public class IndexModel : PageModel
	{
		private readonly IStaffService _staffService;

		// Constructor nhận vào StaffService
		public IndexModel(IStaffService staffService)
		{
			_staffService = staffService;
		}

		public List<Staff> StaffList { get; set; }

		public async Task OnGetAsync()
		{
			// Lấy danh sách nhân viên từ DB
			StaffList = await _staffService.GetAllStaffAsync();
		}
	}
}
