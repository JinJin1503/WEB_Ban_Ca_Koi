using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
    [Authorize(Policy = AppPolicies.ManagerOnly)]
    public class ManageConsignmentModel : PageModel
    {
        private readonly IConsignmentRequestService _consignmentService;

        // Định nghĩa các hằng số trạng thái để dễ quản lý và tránh lỗi typo
        private const string STATUS_RECEIVED = "Đã nhận cá";
        private const string STATUS_CARING = "Đang chăm sóc";
        private const string STATUS_SOLD = "Đã bán";

        public ManageConsignmentModel(IConsignmentRequestService consignmentService)
        {
            _consignmentService = consignmentService;
        }

        public List<ConsignmentRequest> Requests { get; set; } = new();

        public async Task OnGetAsync()
        {
            Requests = await _consignmentService.GetAllConsignmentRequestsAsync();
        }



        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            await _consignmentService.ApproveRequestAsync(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            await _consignmentService.RejectRequestAsync(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostReceiveKoiAsync(int id)
            => await UpdateStatusAsync(id, STATUS_RECEIVED);

        public async Task<IActionResult> OnPostStartCareAsync(int id)
            => await UpdateStatusAsync(id, STATUS_CARING);

        public async Task<IActionResult> OnPostSoldAsync(int id)
            => await UpdateStatusAsync(id, STATUS_SOLD);



        /// Hàm dùng chung để cập nhật trạng thái đơn ký gửi
        private async Task<IActionResult> UpdateStatusAsync(int id, string status)
        {
            var request = await _consignmentService.GetConsignmentRequestByIdAsync(id);
            if (request != null)
            {
                request.Status = status;
                await _consignmentService.UpdateConsignmentRequestAsync(request);
            }
            return RedirectToPage();
        }


        /// Dịch trạng thái hiển thị cho người dùng
        public string TranslateStatus(string status) => (status?.ToLower()) switch
        {
            "approved" => "Đã duyệt",
            "rejected" => "Từ chối",
            "pending" => "Chờ duyệt",
            null or "" => "Không xác định",
            _ => status // Trả về nguyên bản nếu là tiếng Việt hoặc trạng thái khác
        };


    }
}