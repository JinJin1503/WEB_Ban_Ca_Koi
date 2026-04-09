using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using SecurityClaim = System.Security.Claims.Claim;

namespace KoiFarmShop.WebApplication.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly IStaffService _staffService;

        public LoginModel(IUserService userService, ICustomerService customerService, IStaffService staffService)
        {
            _userService = userService;
            _customerService = customerService;
            _staffService = staffService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 40 ký tự.")]
        public string UserName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 đến 16 ký tự.")]
        public string Password { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToLocalOrDefault(GetDefaultRedirectPage());
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.LoginAsync(UserName, Password);

            if (user == null)
            {
                ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return Page();
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            var staff = await _staffService.GetStaffByUserIdAsync(user.UserId);
            if (staff != null)
            {
                string normalizedRole = NormalizeStaffRole(staff.Role);
                var claims = new List<SecurityClaim>
                {
                    new SecurityClaim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new SecurityClaim(ClaimTypes.Name, user.UserName),
                    new SecurityClaim(ClaimTypes.Role, normalizedRole),
                    new SecurityClaim(AppClaimTypes.StaffId, staff.StaffId.ToString())
                };

                await SignInUserAsync(claims);

                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.Remove("CustomerId");

                if (normalizedRole == AppRoles.Manager || normalizedRole == (AppRoles.Staff))
                {
                    return RedirectToLocalOrDefault("/manager/Index");
                }

                 return RedirectToLocalOrDefault("/Trangchu/Index");
            }

            var customer = await _customerService.GetCustomerByUserIdAsync(user.UserId);
            if (customer == null)
            {
                ErrorMessage = "Khách hàng không tồn tại.";
                return Page();
            }

            var customerClaims = new List<SecurityClaim>
            {
                new SecurityClaim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new SecurityClaim(ClaimTypes.Name, user.UserName),
                new SecurityClaim(ClaimTypes.Role, AppRoles.Customer),
                new SecurityClaim(AppClaimTypes.CustomerId, customer.CustomerId.ToString())
            };

            await SignInUserAsync(customerClaims);

            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("CustomerId", customer.CustomerId.ToString());

            return RedirectToLocalOrDefault("/Trangchu/Index");
        }
        private async Task SignInUserAsync(IEnumerable<SecurityClaim> claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });
        }

        private string NormalizeStaffRole(string role)
        {
            if (string.Equals(role, "Manager", StringComparison.OrdinalIgnoreCase))
            {
                return AppRoles.Manager;
            }

            return AppRoles.Staff;
        }

        private IActionResult RedirectToLocalOrDefault(string defaultPage)
        {
            if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return LocalRedirect(ReturnUrl);
            }

            return RedirectToPage(defaultPage);
        }

        private string GetDefaultRedirectPage()
        {
            if (User.IsInRole(AppRoles.Manager) || User.IsInRole(AppRoles.Staff))
            {
                return "/manager/Index";
            }

            return "/Trangchu/Index";
        }
    }
}
