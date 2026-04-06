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
    // Chỉ Quản lý mới có quyền vào chức năng này
    [Authorize(Policy = AppPolicies.ManagerOnly)]
    public class DeleteModel : PageModel
    {
        private readonly IStaffService _staffService;

        public DeleteModel(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [BindProperty]
        public Staff StaffToDelete { get; set; }

        // Bắt tham số 'id' từ URL
        public async Task<IActionResult> OnGetAsync(int id)
        {
            StaffToDelete = await _staffService.GetStaffByIdAsync(id);
            if (StaffToDelete == null)
            {
                return RedirectToPage("./Index");
            }

            // 1. CHẶN XÓA CHÍNH MÌNH (Chống tự sát)
            var currentStaffId = User.FindFirstValue(AppClaimTypes.StaffId);
            if (!string.IsNullOrEmpty(currentStaffId) && int.Parse(currentStaffId) == id)
            {
                TempData["ErrorMessage"] = "Bảo mật: Bạn không thể tự xóa tài khoản của chính mình!";
                return RedirectToPage("./Index");
            }

            // 2. CHẶN XÓA TÀI KHOẢN GỐC (Root Admin)
            if (StaffToDelete.Email != null && StaffToDelete.Email.Equals("Khanhng5776@ut.edu.vn", System.StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Bảo mật: Không thể xóa tài khoản Quản trị viên gốc của hệ thống!";
                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Kiểm tra bảo mật lần 2 ở hàm Post (Phòng hờ bị hacker dùng Postman bắn API)
            var staffInDb = await _staffService.GetStaffByIdAsync(id);
            if (staffInDb == null) return NotFound();

            var currentStaffId = User.FindFirstValue(AppClaimTypes.StaffId);
            if (!string.IsNullOrEmpty(currentStaffId) && int.Parse(currentStaffId) == id)
            {
                TempData["ErrorMessage"] = "Hành động bị từ chối: Không thể tự xóa mình!";
                return RedirectToPage("./Index");
            }

            if (staffInDb.Email != null && staffInDb.Email.Equals("Khanhng5776@ut.edu.vn", System.StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Hành động bị từ chối: Cố gắng xóa tài khoản gốc!";
                return RedirectToPage("./Index");
            }

            // Nếu vượt qua mọi bài test -> Tiến hành Xóa
            await _staffService.DeleteStaffAsync(id);

            TempData["SuccessMessage"] = "Đã xóa nhân viên thành công!";
            return RedirectToPage("./Index");
        }
    }
}