using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.dichvukiban
{
    [Authorize] // Yêu cầu đăng nhập mới được vào trang này
    [IgnoreAntiforgeryToken] // Hỗ trợ Postman test không bị lỗi 400
    public class IndexModel : PageModel
    {
        private readonly IConsignmentRequestService _consignmentRequestService;
        private readonly ICustomerService _customerService;

        public IndexModel(IConsignmentRequestService consignmentRequestService, ICustomerService customerService)
        {
            _consignmentRequestService = consignmentRequestService;
            _customerService = customerService;
        }

        // Khai báo 1 đối tượng duy nhất để hứng dữ liệu từ Form
        [BindProperty]
        public ConsignmentRequest ConsignmentRequest { get; set; }

        public void OnGet()
        {
            // Chỉ hiển thị form giao diện
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Bỏ qua các trường bắt buộc nhưng form không có (tránh lỗi ModelState)
            ModelState.Remove("ConsignmentRequest.CustomerId");
            ModelState.Remove("ConsignmentRequest.Customer");
            ModelState.Remove("ConsignmentRequest.Status");
            ModelState.Remove("ConsignmentRequest.ConsignmentType");
            ModelState.Remove("ConsignmentRequest.Certificate");
            ModelState.Remove("ConsignmentRequest.Notes");
            ModelState.Remove("ConsignmentRequest.KoiImage"); // Tạm thời bỏ qua ảnh
            ModelState.Remove("ConsignmentRequest.ConsignmentDate"); // Ký gửi bán có thể lấy ngày hiện tại

            if (!ModelState.IsValid)
            {
                return Page(); // Nếu điền thiếu, load lại trang báo lỗi
            }

            // 2. Xác định Khách hàng đang đăng nhập
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                var customer = await _customerService.GetCustomerByUserIdAsync(userId);
                if (customer != null)
                {
                    ConsignmentRequest.CustomerId = customer.CustomerId;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không tìm thấy hồ sơ khách hàng của bạn.");
                    return Page();
                }
            }
            else
            {
                // MẸO: Nếu test Postman chưa lấy được User Cookie, tạm thời gán ID = 1
                ConsignmentRequest.CustomerId = 1;
            }

            // 3. Gán các giá trị mặc định cho Ký gửi Bán
            ConsignmentRequest.RequestDate = DateTime.Now;
            ConsignmentRequest.ConsignmentDate = DateTime.Now; // Ngày gửi
            ConsignmentRequest.Status = "Chờ duyệt";
            ConsignmentRequest.ConsignmentType = "Bán"; // Đánh dấu là dịch vụ Bán

            // 4. Lưu vào Database
            await _consignmentRequestService.AddConsignmentRequestAsync(ConsignmentRequest);

            // 5. Thành công -> Chuyển hướng sang trang Lịch sử
            return RedirectToPage("/history_dichvu/History");
        }
    }
}