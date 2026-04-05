using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KoiFarmShop.WebApplication.Pages.Register
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterModel(IUserService userService, ICustomerService customerService, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _customerService = customerService;
            _passwordHasher = passwordHasher;
        }

        [BindProperty]
        public bool AcceptTerms { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 40 ký tự.")]
        public string UserName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 đến 16 ký tự.")]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Tên khách hàng không được để trống.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Tên khách hàng phải từ 2 đến 50 ký tự.")]
        public string CustomerName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        public string Email { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Nếu vi phạm luật (độ dài, regex), trả về form kèm câu báo lỗi màu đỏ
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Mật khẩu xác nhận không khớp.";
                return Page();
            }

            if (!AcceptTerms)
            {
                ErrorMessage = "Bạn phải đồng ý với điều khoản và điều kiện.";
                return Page();
            }

            // Create the User object
            var user = new User
            {
                UserName = UserName,
                PasswordHasher = _passwordHasher.HashPassword(Password),
                Email = Email // You should also associate the email with the user
            };

            // Register the user by saving it to the database
            var isRegistered = await _userService.RegisterUserAsync(user, Password);
            if (!isRegistered)
            {
                ErrorMessage = "Tên đăng nhập đã tồn tại.";
                return Page();
            }

            // At this point, the user has been saved successfully, and you can get the UserId
            // Create the Customer object and set the UserId to associate the customer with the user
            var customer = new Customer
            {
                CustomerName = CustomerName,
                Email = Email,
                UserId = user.UserId // Link the customer to the user by setting UserId
            };

            // Save the customer data into the database
            await _customerService.AddCustomerAsync(customer);

            return RedirectToPage("/Login/Login"); // Redirect to login page after successful registration
        }

    }

}