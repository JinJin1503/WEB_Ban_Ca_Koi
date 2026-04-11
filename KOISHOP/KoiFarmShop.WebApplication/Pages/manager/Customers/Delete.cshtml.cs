using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.manager.Customers
{
    [Authorize(Roles = "Manager,Staff")]
    public class DeleteModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;

        public DeleteModel(ICustomerService customerService, IUserService userService)
        {
            _customerService = customerService;
            _userService = userService;
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            // Lấy thông tin hiển thị lên trang xác nhận
            Customer = await _customerService.GetCustomerByIdAsync(id.Value);

            if (Customer == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _customerService.GetCustomerByIdAsync(id.Value);

            if (customer != null)
            {
                try
                {
                    // Lưu lại UserId trước khi xóa khách hàng
                    int userIdToDelete = customer.UserId;

                    // 1. Tiến hành xóa khách hàng
                    await _customerService.DeleteCustomerAsync(id.Value);

                    // 2. Xóa luôn tài khoản đăng nhập liên kết để dọn sạch rác Database
                    if (userIdToDelete > 0)
                    {
                        await _userService.DeleteUserAsync(userIdToDelete);
                    }

                    TempData["SuccessMessage"] = $"Đã xóa khách hàng {customer.CustomerName} thành công!";
                    return RedirectToPage("./Index");
                }
                catch (Exception)
                {
                    // BẮT LỖI AN TOÀN: Nếu khách hàng đã có Đơn hàng/Hóa đơn, DB sẽ không cho xóa
                    TempData["ErrorMessage"] = "Không thể xóa khách hàng này vì họ đã phát sinh dữ liệu giao dịch (đơn hàng, ký gửi...). Lời khuyên: Hãy sử dụng chức năng 'Khóa tài khoản' ở mục Sửa.";
                    return RedirectToPage("./Index");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}