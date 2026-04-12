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
        private readonly IUserService _userService; 

        public EditModel(IStaffService staffService, IUserService userService)
        {
            _staffService = staffService;
            _userService = userService;
        }

        [BindProperty]
        public Staff ExistingStaff { get; set; }

        [BindProperty]
        public bool IsLocked { get; set; }

        [BindProperty]
        public int FailedAttemptCount { get; set; }

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

            // Lấy thông tin tài khoản (User) để hiển thị trạng thái khóa
            var user = await _userService.GetUserByIdAsync(ExistingStaff.UserId);
            if (user != null)
            {
                IsLocked = user.IsLocked;
                FailedAttemptCount = user.FailedAttemptCount;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

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

            // 1. Cập nhật bảng Staff
            staffInDb.StaffName = ExistingStaff.StaffName;
            staffInDb.Role = ExistingStaff.Role;
            staffInDb.Phone = ExistingStaff.Phone;
            staffInDb.Email = ExistingStaff.Email;
            staffInDb.Salary = ExistingStaff.Salary;

            await _staffService.UpdateStaffAsync(staffInDb);

            // 2. Cập nhật bảng User (Tính năng mở khóa)
            var userInDb = await _userService.GetUserByIdAsync(staffInDb.UserId);
            if (userInDb != null)
            {
                userInDb.Email = ExistingStaff.Email; 
                userInDb.IsLocked = IsLocked;

                if (!IsLocked)
                {
                    userInDb.FailedAttemptCount = 0; // Tự động reset về 0 nếu Admin gạt mở khóa
                }
                else
                {
                    userInDb.FailedAttemptCount = FailedAttemptCount;
                }

                await _userService.UpdateUserAsync(userInDb);
            }

            TempData["SuccessMessage"] = "Cập nhật thông tin nhân viên thành công!";
            return RedirectToPage("./Index");
        }
    }
}