using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KoiFarmShop.WebApplication.Pages.Dichvuchamsoc
{
    [Authorize] // Yêu cầu đăng nhập mới được vào trang này
    [IgnoreAntiforgeryToken] // Hỗ trợ Postman test không bị lỗi 400
    public class IndexCareModel : PageModel
    {
        private readonly ICareServiceService _careService;
        private readonly IConsignmentRequestService _consignmentService;
        private readonly ICustomerService _customerService; // Thêm service để lấy thông tin khách hàng

        public IndexCareModel(ICareServiceService careService, IConsignmentRequestService consignmentService, ICustomerService customerService)
        {
            _careService = careService;
            _consignmentService = consignmentService;
            _customerService = customerService;
        }

        [BindProperty]
        public ConsignmentRequest ConsignmentRequest { get; set; }

        public IList<CareService> CareServices { get; set; }

        public async Task OnGetAsync()
        {
            // Tải danh sách các gói chăm sóc (nếu có) để hiển thị ra giao diện
            CareServices = await _careService.GetAllCareServicesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Dọn dẹp các lỗi validation ảo do form không có nhập các trường này
            ModelState.Remove("ConsignmentRequest.CustomerId");
            ModelState.Remove("ConsignmentRequest.Customer");
            ModelState.Remove("ConsignmentRequest.Status");
            ModelState.Remove("ConsignmentRequest.ConsignmentType");
            ModelState.Remove("ConsignmentRequest.Certificate");
            ModelState.Remove("ConsignmentRequest.Notes");
            ModelState.Remove("ConsignmentRequest.KoiImage"); // Tạm bỏ qua ảnh
            ModelState.Remove("ConsignmentRequest.ConsignmentFee"); // Form chăm sóc không có nhập phí ngay lúc đầu

            if (!ModelState.IsValid)
            {
                // Nếu form không hợp lệ, load lại danh sách dịch vụ và hiển thị lỗi
                CareServices = await _careService.GetAllCareServicesAsync();
                return Page();
            }

            // 2. Lấy ID của Khách hàng đang đăng nhập từ Cookie
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
                    CareServices = await _careService.GetAllCareServicesAsync();
                    return Page();
                }
            }
            else
            {
                // Fallback: Nếu test Postman chưa gắn Cookie đúng cách, tạm lấy ID = 1
                ConsignmentRequest.CustomerId = 1;
            }

            // 3. Gán các giá trị mặc định hệ thống cho Ký gửi Chăm sóc
            ConsignmentRequest.RequestDate = DateTime.Now;
            ConsignmentRequest.Status = "Chờ duyệt";
            ConsignmentRequest.ConsignmentType = "Chăm sóc"; // Đánh dấu đây là đơn Chăm sóc

            // 4. Lưu đơn ký gửi vào Database
            await _consignmentService.AddConsignmentRequestAsync(ConsignmentRequest);

            // 5. Thành công -> Chuyển hướng sang trang Lịch sử ký gửi 
            return RedirectToPage("/history_dichvu/History");
        }
    }
}