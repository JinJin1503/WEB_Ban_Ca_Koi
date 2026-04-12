using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
    [Authorize(Policy = AppPolicies.StaffOrManager)]
    public class DetailsConsignmentModel : PageModel
    {
        private readonly IConsignmentRequestService _consignmentService;

        public DetailsConsignmentModel(IConsignmentRequestService consignmentService)
        {
            _consignmentService = consignmentService;
        }

        public ConsignmentRequest ConsignmentRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ConsignmentRequest = await _consignmentService.GetConsignmentRequestByIdAsync(id);

            if (ConsignmentRequest == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            await _consignmentService.ApproveRequestAsync(id);
            return RedirectToPage("/manager/ManageConsignment");
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
           
            await _consignmentService.RejectRequestAsync(id);

           
            return RedirectToPage("/manager/ManageConsignment");
        }

        // 2. Hàm xử lý nút ĐÃ NHẬN CÁ
        public async Task<IActionResult> OnPostReceiveKoiAsync(int id)
        {
            var request = await _consignmentService.GetConsignmentRequestByIdAsync(id);
            if (request != null)
            {
                request.Status = "Đã nhận cá";
                await _consignmentService.UpdateConsignmentRequestAsync(request);
            }
            return RedirectToPage(new { id = id }); // Reload lại trang hiện tại
        }

        // 3. Hàm xử lý nút ĐANG CHĂM SÓC
        public async Task<IActionResult> OnPostStartCareAsync(int id)
        {
            var request = await _consignmentService.GetConsignmentRequestByIdAsync(id);
            if (request != null)
            {
                request.Status = "Đang chăm sóc";
                await _consignmentService.UpdateConsignmentRequestAsync(request);
            }
            return RedirectToPage(new { id = id });
        }

        // 4. Hàm xử lý nút ĐÃ BÁN
        public async Task<IActionResult> OnPostSoldAsync(int id)
        {
            var request = await _consignmentService.GetConsignmentRequestByIdAsync(id);
            if (request != null)
            {
                request.Status = "Đã bán";
                await _consignmentService.UpdateConsignmentRequestAsync(request);
            }
            return RedirectToPage(new { id = id });
        }
    }
}
