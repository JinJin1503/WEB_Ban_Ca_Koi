using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager.Consignment
{
    [Authorize(Policy = AppPolicies.StaffOrManager)]
    [IgnoreAntiforgeryToken]
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

        public async Task<IActionResult> OnPostApproveAsync(int id, int? fee, int? commission, string notes)
        {
            var request = await _consignmentService.GetConsignmentRequestByIdAsync(id);
            if (request != null)
            {
                if (request.ConsignmentType == "Chăm sóc" && fee.HasValue)
                {
                    request.ConsignmentFee = fee.Value;
                }
                // LOGIC MỚI: Nếu là Đơn Bán và Admin có nhập Hoa hồng -> Mượn cột Duration để lưu
                else if (request.ConsignmentType == "Bán" && commission.HasValue)
                {
                    request.ConsignmentDuration = commission.Value;
                }

                request.Notes = notes;
                await _consignmentService.UpdateConsignmentRequestAsync(request);
            }

            await _consignmentService.ApproveRequestAsync(id);
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            await _consignmentService.RejectRequestAsync(id);
            // Reload lại đúng trang chi tiết của ID này
            return RedirectToPage(new { id });
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
            return RedirectToPage(new { id }); // Reload lại trang hiện tại
        }

        // 3. Hàm xử lý nút ĐANG CHĂM SÓC
        public async Task<IActionResult> OnPostStartCareAsync(int id)
        {
            var request = await _consignmentService.GetConsignmentRequestByIdAsync(id);
            if (request != null)
            {
                request.Status = "Đang chăm sóc";
                // THÊM DÒNG NÀY: Chốt ngày giờ bắt đầu chăm sóc là ngay lúc Admin bấm nút
                request.ConsignmentDate = System.DateTime.Now;

                await _consignmentService.UpdateConsignmentRequestAsync(request);
            }
            return RedirectToPage(new { id });
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
            return RedirectToPage(new { id });
        }


    }
}
