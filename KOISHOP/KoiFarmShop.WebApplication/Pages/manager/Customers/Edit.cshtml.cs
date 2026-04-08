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
    public class EditModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;

        public EditModel(ICustomerService customerService, IUserService userService)
        {
            _customerService = customerService;
            _userService = userService;
        }

        [BindProperty]
        public EditInputModel Input { get; set; }

        public class EditInputModel
        {
            public int CustomerId { get; set; }

            [Required(ErrorMessage = "Họ tên không được để trống")]
            public string CustomerName { get; set; }

            [Required(ErrorMessage = "Số điện thoại không được để trống")]
            [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải 10 số.")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Email không được để trống")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Địa chỉ không được để trống")]
            public string Address { get; set; }

            // Các thuộc tính của User để quản lý trạng thái tài khoản
            public int UserId { get; set; }
            public bool IsLocked { get; set; }
            public int FailedAttemptCount { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _customerService.GetCustomerByIdAsync(id.Value);
            if (customer == null) return NotFound();

            var user = await _userService.GetUserByIdAsync(customer.UserId);

            // Đổ dữ liệu cũ vào form
            Input = new EditInputModel
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                UserId = customer.UserId,
                IsLocked = user?.IsLocked ?? false,
                FailedAttemptCount = user?.FailedAttemptCount ?? 0
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // 1. Cập nhật bảng Customer
            var customer = await _customerService.GetCustomerByIdAsync(Input.CustomerId);
            if (customer == null) return NotFound();

            customer.CustomerName = Input.CustomerName;
            customer.Phone = Input.Phone;
            customer.Email = Input.Email;
            customer.Address = Input.Address;

            await _customerService.UpdateCustomerAsync(customer);

            // 2. Cập nhật bảng User (Xử lý Mở khóa tài khoản)
            var user = await _userService.GetUserByIdAsync(Input.UserId);
            if (user != null)
            {
                user.Email = Input.Email; // Đồng bộ email nếu cần
                user.IsLocked = Input.IsLocked;

                // Nếu Admin chủ động tích chọn "Mở khóa", ta cũng nên reset số lần nhập sai về 0
                if (!Input.IsLocked)
                {
                    user.FailedAttemptCount = 0;
                }
                else
                {
                    user.FailedAttemptCount = Input.FailedAttemptCount;
                }

                await _userService.UpdateUserAsync(user);
            }

            TempData["SuccessMessage"] = "Cập nhật thông tin khách hàng thành công!";
            return RedirectToPage("./Index");
        }
    }
}