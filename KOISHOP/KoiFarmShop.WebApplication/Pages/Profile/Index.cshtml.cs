using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.Repositories.Entities;

namespace KoiFarmShop.WebApplication.Pages.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher; // Thêm Hasher để xử lý đổi mật khẩu

        public IndexModel(ICustomerService customerService, IUserService userService, IPasswordHasher passwordHasher)
        {
            _customerService = customerService;
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        // 1. ViewModel cho Form Hồ sơ
        public class ProfileViewModel
        {
            public string FullName { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }

        // 2. ViewModel cho Form Đổi mật khẩu
        public class ChangePasswordViewModel
        {
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }

        [BindProperty]
        public ProfileViewModel UserProfile { get; set; }

        [BindProperty]
        public ChangePasswordViewModel PasswordModel { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return RedirectToPage("/Login/Login");

            int userId = int.Parse(userIdString);

            // Gọi DB lấy thông tin hiển thị lên view
            var customer = await _customerService.GetCustomerByUserIdAsync(userId);
            var user = await _userService.GetUserByIdAsync(userId);

            UserProfile = new ProfileViewModel
            {
                FullName = customer?.CustomerName ?? user?.UserName,
                Address = customer?.Address,
                Phone = customer?.Phone,
                Email = customer?.Email ?? user?.Email
            };

            return Page();
        }

        // HÀM XỬ LÝ 1: LƯU HỒ SƠ
        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return RedirectToPage("/Login/Login");

            int userId = int.Parse(userIdString);
            var customer = await _customerService.GetCustomerByUserIdAsync(userId);

            if (customer != null)
            {
                customer.CustomerName = UserProfile.FullName;
                customer.Address = UserProfile.Address;
                customer.Phone = UserProfile.Phone;
                customer.Email = UserProfile.Email;

                await _customerService.UpdateCustomerAsync(customer);
                SuccessMessage = "Cập nhật hồ sơ thành công!";
            }
            return RedirectToPage();
        }

        // HÀM XỬ LÝ 2: LƯU CÀI ĐẶT
        public async Task<IActionResult> OnPostUpdateSettingsAsync()
        {
            // Tương lai nếu DB có bảng Settings, bạn gọi DB ở đây
            SuccessMessage = "Đã lưu cấu hình thông báo!";
            return RedirectToPage();
        }

        // HÀM XỬ LÝ 3: ĐỔI MẬT KHẨU
        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString);

            var user = await _userService.GetUserByIdAsync(userId);

            // 1. Kiểm tra mật khẩu cũ có đúng không
            if (!_passwordHasher.VerifyHashedPassword(user.PasswordHasher, PasswordModel.CurrentPassword))
            {
                ErrorMessage = "Mật khẩu hiện tại không chính xác!";
                return RedirectToPage();
            }

            // 2. Kiểm tra mật khẩu mới và xác nhận
            if (PasswordModel.NewPassword != PasswordModel.ConfirmPassword)
            {
                ErrorMessage = "Mật khẩu xác nhận không khớp!";
                return RedirectToPage();
            }

            // 3. Hash mật khẩu mới và lưu xuống DB
            user.PasswordHasher = _passwordHasher.HashPassword(PasswordModel.NewPassword);
            await _userService.UpdateUserAsync(user);

            SuccessMessage = "Đổi mật khẩu thành công!";
            return RedirectToPage();
        }
    }
}