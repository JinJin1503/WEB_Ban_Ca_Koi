using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services;
using KoiFarmShop.Services.Implementations;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;

namespace KoiFarmShop.WebApplication.Pages.Login
{
    [IgnoreAntiforgeryToken]
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
            // Kiểm tra thông tin đăng nhập từ DB
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

            // 2. KIỂM TRA NHÂN VIÊN VÀ PHÂN QUYỀN
            var staff = await _staffService.GetStaffByUserIdAsync(user.UserId);
            if (staff != null)
            {
                if (!string.IsNullOrEmpty(staff.Role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, staff.Role));
                }

                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("CustomerId", "");

                // 3. PHÁT THẺ BÀI (ĐĂNG NHẬP COOKIE CHÍNH THỨC)
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                // Chuyển hướng vào trang quản trị
                return RedirectToPage("/Manager/Index");
            }

            // 4. NẾU KHÔNG PHẢI NHÂN VIÊN -> KIỂM TRA KHÁCH HÀNG
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

            // Phát thẻ bài cho khách hàng
            var customerIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(customerIdentity));

            // Chuyển hướng về trang chủ
            return RedirectToPage("/Trangchu/Index");
        }

    }
}