using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.historydichvu
{
    [Authorize] // Bắt buộc đăng nhập
    public class IndexModel : PageModel
    {
        private readonly IConsignmentRequestService _consignmentService;
        private readonly ICustomerService _customerService;

        public IndexModel(IConsignmentRequestService consignmentService, ICustomerService customerService)
        {
            _consignmentService = consignmentService;
            _customerService = customerService;
        }

        // Danh sách chứa TẤT CẢ đơn hàng của khách
        public List<ConsignmentRequest> MyRequests { get; set; } = new List<ConsignmentRequest>();

        public async Task OnGetAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                var customer = await _customerService.GetCustomerByUserIdAsync(userId);
                if (customer != null)
                {
                    // Lấy tất cả và lọc ra đơn của user này, xếp đơn mới nhất lên đầu
                    var allRequests = await _consignmentService.GetAllConsignmentRequestsAsync();
                    MyRequests = allRequests.Where(r => r.CustomerId == customer.CustomerId)
                                            .OrderByDescending(r => r.RequestDate)
                                            .ToList();
                }
            }
        }
    }
}