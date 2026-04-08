using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.manager.MyProfile
{
    [Authorize(Roles = "Manager,Staff")]
    public class IndexModel : PageModel
    {
        private readonly IStaffService _staffService;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;

        public IndexModel(IStaffService staffService, IUserService userService, IPasswordHasher passwordHasher)
        {
            _staffService = staffService;
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        [BindProperty]
        public ProfileInputModel Input { get; set; }

        public class ProfileInputModel
        {
            public int StaffId { get; set; }
            public int UserId { get; set; }

            // --- Các trường READ-ONLY (Chỉ xem) ---
            public string StaffName { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public int Salary { get; set; }
            public string UserName { get; set; }

            // --- Các trường CHO PHÉP SỬA ---
            [Required(ErrorMessage = "Số điện thoại không được để trống")]
            [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải đúng 10 số.")]
            public string Phone { get; set; }

            [StringLength(16, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 đến 16 ký tự.")]
            public string NewPassword { get; set; }

            [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy UserId từ Session (hoặc Claim) mà bạn đã set lúc Login
            var userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToPage("/Login/Login");
            }

            // Lấy thông tin Staff và User
            var staff = await _staffService.GetStaffByUserIdAsync(userId);
            var user = await _userService.GetUserByIdAsync(userId);

            if (staff == null || user == null)
            {
                return NotFound("Không tìm thấy thông tin nhân viên.");
            }

            // Đổ dữ liệu vào Form
            Input = new ProfileInputModel
            {
                StaffId = staff.StaffId,
                UserId = user.UserId,
                StaffName = staff.StaffName,
                Email = staff.Email,
                Role = staff.Role,
                Salary = staff.Salary,
                UserName = user.UserName,
                Phone = staff.Phone
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Nếu form lỗi, cần load lại các trường Read-only để tránh bị null trên giao diện
                return await ReloadReadOnlyFieldsAndReturnPage();
            }

            // 1. Cập nhật bảng Staff (Chỉ cập nhật Số điện thoại)
            var staff = await _staffService.GetStaffByUserIdAsync(Input.UserId);
            if (staff != null)
            {
                staff.Phone = Input.Phone;
                await _staffService.UpdateStaffAsync(staff);
            }

            // 2. Cập nhật Mật khẩu bảng User (Nếu có nhập mật khẩu mới)
            if (!string.IsNullOrEmpty(Input.NewPassword))
            {
                var user = await _userService.GetUserByIdAsync(Input.UserId);
                if (user != null)
                {
                    user.PasswordHasher = _passwordHasher.HashPassword(Input.NewPassword);
                    await _userService.UpdateUserAsync(user);
                }
            }

            TempData["SuccessMessage"] = "Cập nhật hồ sơ cá nhân thành công!";
            return RedirectToPage("./Index");
        }

        // Hàm hỗ trợ để load lại data cho các ô bị khóa (vì các ô Read-only sẽ không gửi data về server khi submit form)
        private async Task<IActionResult> ReloadReadOnlyFieldsAndReturnPage()
        {
            var staff = await _staffService.GetStaffByUserIdAsync(Input.UserId);
            var user = await _userService.GetUserByIdAsync(Input.UserId);

            if (staff != null && user != null)
            {
                Input.StaffName = staff.StaffName;
                Input.Email = staff.Email;
                Input.Role = staff.Role;
                Input.Salary = staff.Salary;
                Input.UserName = user.UserName;
            }
            return Page();
        }
    }
}