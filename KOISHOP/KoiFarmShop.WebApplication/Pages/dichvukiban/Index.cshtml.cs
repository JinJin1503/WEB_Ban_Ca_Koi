using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Hosting; 
using System.IO; 

namespace KoiFarmShop.WebApplication.Pages.dichvukiban
{
    [Authorize] // Yêu cầu đăng nhập mới được vào trang này
    [IgnoreAntiforgeryToken] 
    public class IndexModel : PageModel
    {
        private readonly IConsignmentRequestService _consignmentRequestService;
        private readonly ICustomerService _customerService;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(IConsignmentRequestService consignmentRequestService, ICustomerService customerService, IWebHostEnvironment environment)
        {
            _consignmentRequestService = consignmentRequestService;
            _customerService = customerService;
            _environment = environment;
        }

        // Khai báo 1 đối tượng duy nhất để hứng dữ liệu từ Form
        [BindProperty]
        public ConsignmentRequest ConsignmentRequest { get; set; }
        // Thêm biến này để hứng file ảnh thật từ HTML gửi lên
        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

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
                return Page(); 
            }
            if (ConsignmentRequest.ConsignmentFee < 0)
            {
                ModelState.AddModelError("ConsignmentRequest.ConsignmentFee", "Giá bán không được là số âm.");
                return Page();
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
              
                ConsignmentRequest.CustomerId = 1;
            }

            // 3. Gán các giá trị mặc định cho Ký gửi Bán
            ConsignmentRequest.RequestDate = DateTime.Now;
            ConsignmentRequest.ConsignmentDate = DateTime.Now; // Ngày gửi
            ConsignmentRequest.Status = "Chờ duyệt";
            ConsignmentRequest.ConsignmentType = "Bán"; // Đánh dấu là dịch vụ Bán

            // XỬ LÝ UPLOAD ẢNH
            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                // 1. Tạo tên file độc nhất để không bị trùng
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUpload.FileName);

                // 2. Đường dẫn lưu file (Thư mục wwwroot/images/consignment)
                string uploadFolder = Path.Combine(_environment.WebRootPath, "images", "consignment");

                // Tạo thư mục nếu chưa có
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string filePath = Path.Combine(uploadFolder, fileName);

                // 3. Copy file vào ổ cứng
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(fileStream);
                }

                // 4. Gán đường dẫn vào ConsignmentRequest để lưu xuống DB
                ConsignmentRequest.KoiImage = "/images/consignment/" + fileName;
            }

            // 4. Lưu vào Database
            await _consignmentRequestService.AddConsignmentRequestAsync(ConsignmentRequest);

            // 5. Thành công -> Chuyển hướng sang trang Lịch sử
            return RedirectToPage("/history_dichvu/History");
        }
    }
}