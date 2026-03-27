using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

// CÁC THƯ VIỆN BẮT BUỘC PHẢI THÊM ĐỂ LÀM BẢO MẬT:
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;

namespace KoiFarmShop.WebApplication.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IStaffService _staffService;

        public LoginModel(IUserService userService, IPasswordHasher passwordHasher, ICustomerService customerService, IStaffService staffService)
        {
            _userService = userService;
            _customerService = customerService;
            _passwordHasher = passwordHasher;
            _staffService = staffService;
        }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Logic khi người dùng truy cập trang lần đầu (GET)
        }

        // Đã bỏ các tham số thừa trong ngoặc, vì UserName và Password đã dùng [BindProperty]
        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra đăng nhập
            var user = await _userService.LoginAsync(UserName, Password);

            if (user == null)
            {
                ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return Page();
            }

            // 1. CHUẨN BỊ THẺ ĐỊNH DANH (CLAIMS) - Bắt buộc cho [Authorize]
            var claims = new List<Claim>
            {
                // Lưu UserId vào định danh để trang Ký gửi Bán có thể lấy ra dùng
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            string redirectUrl = "";

            // Kiểm tra xem có phải là Nhân viên/Quản lý không
            var staff = await _staffService.GetStaffByUserIdAsync(user.UserId);

            if (staff != null)
            {
                // Thêm quyền Role cho Nhân viên (để sau này có thể phân quyền admin)
                claims.Add(new Claim(ClaimTypes.Role, staff.Role));

                // Vẫn giữ lại Session của bạn để không làm hỏng code cũ
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("CustomerId", "");

                redirectUrl = "/Manager/Index";
            }
            else
            {
                // Nếu không phải Staff thì kiểm tra Khách hàng
                var customer = await _customerService.GetCustomerByUserIdAsync(user.UserId);

                if (customer == null)
                {
                    ErrorMessage = "Khách hàng không tồn tại.";
                    return Page();
                }

                claims.Add(new Claim(ClaimTypes.Role, "Customer"));

                // Vẫn giữ lại Session của bạn
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("CustomerId", customer.CustomerId.ToString());

                redirectUrl = "/Trangchu/Index";
            }

            // 2. PHÁT COOKIE XÁC THỰC BẢO MẬT (QUAN TRỌNG NHẤT)
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Lưu đăng nhập cả khi tắt trình duyệt
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Chuyển hướng theo đúng Role
            return RedirectToPage(redirectUrl);
        }
    }
}