using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.manager.Customers
{
    [Authorize(Roles = "Manager,Staff")]
    public class CreateModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        public CreateModel(ICustomerService customerService, IUserService userService)
        {
            _customerService = customerService;
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
            [StringLength(40, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 40 ký tự.")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Mật khẩu không được để trống")]
            [StringLength(16, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 đến 16 ký tự.")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Họ tên không được để trống")]
            public string CustomerName { get; set; }

            [Required(ErrorMessage = "Số điện thoại không được để trống")]
            [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đúng 10 chữ số.")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Email không được để trống")]
            [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Địa chỉ không được để trống")]
            public string Address { get; set; }
        }

        public void OnGet()
        {
            // Hiển thị form trống
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Kiểm tra Form
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

            // 4. "Đăng nhập nháp" để lấy đối tượng User (mục đích là lấy UserId)
            var loginResult = await _userService.LoginAsync(Input.UserName, Input.Password);
            var createdUser = loginResult.user;

            if (createdUser != null)
            {
                // 5. Tạo thông tin Hồ sơ Khách hàng và gắn UserId vào
                var newCustomer = new Customer
                {
                    CustomerName = Input.CustomerName,
                    Phone = Input.Phone,
                    Email = Input.Email,
                    Address = Input.Address,
                    Points = 0, 
                    UserId = createdUser.UserId
                };

                // Lưu ý: Ngày đăng ký (RegistrationDate) đã được chúng ta cài tự động sinh ra trong file CustomerService rồi!
                await _customerService.AddCustomerAsync(newCustomer);
            }

            // 6. Quay lại trang danh sách và báo thành công
            TempData["SuccessMessage"] = "Thêm khách hàng mới thành công!";
            return RedirectToPage("./Index");
        }
    }
}