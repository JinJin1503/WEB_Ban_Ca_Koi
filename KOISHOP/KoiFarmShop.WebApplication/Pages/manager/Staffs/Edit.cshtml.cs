using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.manager.Staffs
{
    // Chỉ Quản lý mới có quyền sửa nhân viên
    [Authorize(Policy = AppPolicies.ManagerOnly)]
    public class EditModel : PageModel
    {
        private readonly IStaffService _staffService;

        public EditModel(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [BindProperty]
        public Staff ExistingStaff { get; set; }

        // Bắt tham số 'id' từ URL truyền vào
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var currentStaffId = User.FindFirstValue(AppClaimTypes.StaffId);
            if (!string.IsNullOrEmpty(currentStaffId) && int.Parse(currentStaffId) == id)
            {
                TempData["ErrorMessage"] = "Bảo mật: Bạn không được phép tự chỉnh sửa thông tin của chính mình ở đây!";
                return RedirectToPage("./Index");
            }

            ExistingStaff = await _staffService.GetStaffByIdAsync(id);
            if (ExistingStaff == null)
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // BẮT BUỘC: Lấy lại nhân viên cũ từ DB để không bị mất các trường bị ẩn (UserId, Email, JoinDate...)
            var staffInDb = await _staffService.GetStaffByIdAsync(ExistingStaff.StaffId);
            if (staffInDb == null)
            {
                return NotFound();
            }

            var currentStaffId = User.FindFirstValue(AppClaimTypes.StaffId);
            if (!string.IsNullOrEmpty(currentStaffId) && int.Parse(currentStaffId) == staffInDb.StaffId)
            {
                TempData["ErrorMessage"] = "Hành động bị từ chối: Không thể tự cập nhật dữ liệu của mình!";
                return RedirectToPage("./Index");
            }

            // Chỉ cập nhật những trường được phép sửa trên Form
            staffInDb.StaffName = ExistingStaff.StaffName;
            staffInDb.Role = ExistingStaff.Role;
            staffInDb.Phone = ExistingStaff.Phone;
            staffInDb.Email = ExistingStaff.Email;
            staffInDb.Salary = ExistingStaff.Salary;

            await _staffService.UpdateStaffAsync(staffInDb);

            // Báo thành công ra ngoài Index
            TempData["SuccessMessage"] = "Cập nhật thông tin nhân viên thành công!";
            return RedirectToPage("./Index");
        }
    }
}