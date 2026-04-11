using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.manager.Customers
{
    [Authorize(Roles = "Manager,Staff")]
    public class DetailsModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;

        public DetailsModel(ICustomerService customerService, IUserService userService)
        {
            _customerService = customerService;
            _userService = userService;
        }

        public Customer Customer { get; set; }
        public User AccountInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Báo lỗi 404 nếu không có ID truyền vào
            }

            // 1. Lấy thông tin khách hàng
            Customer = await _customerService.GetCustomerByIdAsync(id.Value);

            if (Customer == null)
            {
                return NotFound(); // Không tìm thấy khách hàng trong DB
            }

            // 2. Lấy thêm thông tin tài khoản đăng nhập dựa vào UserId
            if (Customer.UserId > 0)
            {
                AccountInfo = await _userService.GetUserByIdAsync(Customer.UserId);
            }

            return Page();
        }
    }
}