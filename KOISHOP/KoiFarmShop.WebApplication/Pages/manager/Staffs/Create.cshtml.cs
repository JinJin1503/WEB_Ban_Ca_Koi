using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.WebApplication.Security;

namespace KoiFarmShop.WebApplication.Pages.manager.Staffs
{
    [Authorize(Policy = AppPolicies.ManagerOnly)]
    public class CreateModel : PageModel
    {
        private readonly IStaffService _staffService;
        private readonly IUserService _userService;

        // Tiêm cả 2 Service vào để thao tác với 2 bảng
        public CreateModel(IStaffService staffService, IUserService userService)
        {
            _staffService = staffService;
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        // Class chứa các trường nhập liệu trên Form
        public class InputModel
        {
            [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
            [StringLength(40, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 40 ký tự.")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Mật khẩu không được để trống")]
            [StringLength(16, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 đến 16 ký tự.")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Họ tên không được để trống")]
            public string StaffName { get; set; }

            [Required(ErrorMessage = "Số điện thoại không được để trống")]
            [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đúng 10 chữ số.")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Email không được để trống")]
            [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Vui lòng chọn vai trò")]
            public string Role { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập lương")]
            [Range(0, double.MaxValue, ErrorMessage = "Mức lương không được là số âm.")]
            public int Salary { get; set; }
        }

        public void OnGet()
        {
            // Hiển thị form trống khi vừa vào trang
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Kiểm tra xem người dùng nhập đủ Form chưa
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // 2. Kiểm tra tên đăng nhập có bị trùng không
            if (!await _userService.IsUserNameValidAsync(Input.UserName))
            {
                ModelState.AddModelError("Input.UserName", "Tên đăng nhập này đã có người sử dụng!");
                return Page();
            }

            // 3. Tạo tài khoản User trước
            var newUser = new User
            {
                UserName = Input.UserName,
                Email = Input.Email
            };

            var registerSuccess = await _userService.RegisterUserAsync(newUser, Input.Password);
            if (!registerSuccess)
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo tài khoản hệ thống.");
                return Page();
            }

            // 4. Đăng nhập nháp để lấy đối tượng User (lấy cái UserId vừa sinh ra)
            var loginResult = await _userService.LoginAsync(Input.UserName, Input.Password);
            var createdUser = loginResult.user;

            if (createdUser != null)
            {
                // 5. Tạo thông tin Nhân viên và gắn UserId vào
                var newStaff = new Staff
                {
                    StaffName = Input.StaffName,
                    Phone = Input.Phone,
                    Email = Input.Email,
                    Role = Input.Role,
                    Salary = Input.Salary,
                    JoinDate = DateTime.Now,
                    UserId = createdUser.UserId // Móc nối 2 bảng với nhau ở đây
                };

                await _staffService.AddStaffAsync(newStaff);
            }

            // 6. Quay lại trang danh sách nhân viên
            TempData["SuccessMessage"] = "Thêm nhân viên mới thành công!";
            return RedirectToPage("./Index");
        }
    }
}