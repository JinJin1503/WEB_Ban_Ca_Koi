using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Login
{
    [IgnoreAntiforgeryToken] // Giữ lại của bạn để test Postman không bị lỗi
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

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.LoginAsync(UserName, Password);

            if (user == null)
            {
                ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return Page();
            }

            // 1. TẠO DANH SÁCH THÔNG TIN CƠ BẢN (CLAIMS)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            string redirectUrl = "";

            // 2. KIỂM TRA NHÂN VIÊN VÀ PHÂN QUYỀN
            var staff = await _staffService.GetStaffByUserIdAsync(user.UserId);

            if (staff != null)
            {
                // Kiểm tra an toàn trước khi add Role (Logic của bạn)
                if (!string.IsNullOrEmpty(staff.Role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, staff.Role));
                }

                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("CustomerId", "");

                redirectUrl = "/Manager/Index";
            }
            else
            {
                // 3. NẾU KHÔNG PHẢI NHÂN VIÊN -> KIỂM TRA KHÁCH HÀNG
                var customer = await _customerService.GetCustomerByUserIdAsync(user.UserId);
                if (customer == null)
                {
                    ErrorMessage = "Khách hàng không tồn tại.";
                    return Page();
                }

                // Gắn quyền mặc định là Customer cho khách hàng
                claims.Add(new Claim(ClaimTypes.Role, "Customer"));

                // Lưu Session cho khách hàng
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("CustomerId", customer.CustomerId.ToString());

                redirectUrl = "/Trangchu/Index";
            }

            // 4. PHÁT THẺ BÀI (Gộp code SignIn của cả 2 nhánh cho gọn & thêm IsPersistent của main)
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