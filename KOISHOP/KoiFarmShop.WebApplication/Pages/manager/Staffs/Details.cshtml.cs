using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.manager.Staffs
{
    // Cho phép cả Quản lý và Nhân viên xem chi tiết
    [Authorize(Policy = AppPolicies.StaffOrManager)]
    public class DetailsModel : PageModel
    {
        private readonly IStaffService _staffService;

        public DetailsModel(IStaffService staffService)
        {
            _staffService = staffService;
        }

        public Staff StaffDetails { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            StaffDetails = await _staffService.GetStaffByIdAsync(id);

            if (StaffDetails == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin nhân viên!";
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}