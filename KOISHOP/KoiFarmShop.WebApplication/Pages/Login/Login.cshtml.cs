using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services;
using KoiFarmShop.Services.Implementations;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

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
		public async Task<IActionResult> OnPostAsync(User users, string UserName, string Password)
		{
			// Attempt to login the user with provided credentials
			var user = await _userService.LoginAsync(UserName, Password);

			if (user == null)
			{
				// If login fails, return an error message
				ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
				return Page();
			}

			// Check if the user exists in the Staff table
			var staff = await _staffService.GetStaffByUserIdAsync(user.UserId);

			if (staff != null)
			{
				// If the user is found in the staff table, check for roles
				if (staff.Role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
				{
					// If the user is a Manager, redirect to the Manager page
					HttpContext.Session.SetString("UserName", user.UserName);
					HttpContext.Session.SetString("UserId", user.UserId.ToString());
					HttpContext.Session.SetString("CustomerId", "");  // No customer ID for staff roles
					return RedirectToPage("/Manager/Index");
				}

				// If the user is a staff member (but not a manager), redirect to the staff page
				HttpContext.Session.SetString("UserName", user.UserName);
				HttpContext.Session.SetString("UserId", user.UserId.ToString());
				HttpContext.Session.SetString("CustomerId", "");  // No customer ID for staff roles
				return RedirectToPage("/Manager/Index");
			}

			// If not a staff member, check if the user is a customer
			var customer = await _customerService.GetCustomerByUserIdAsync(user.UserId);

			if (customer == null)
			{
				// Handle the case where the customer does not exist (Optional)
				ErrorMessage = "Khách hàng không tồn tại.";
				return Page();
			}

			// Store the UserName, UserId, and CustomerId in the session for customers
			HttpContext.Session.SetString("UserName", user.UserName);
			HttpContext.Session.SetString("UserId", user.UserId.ToString());
			HttpContext.Session.SetString("CustomerId", customer.CustomerId.ToString());

			// Redirect the customer to the home page
			return RedirectToPage("/Trangchu/Index");
		}



	}
}