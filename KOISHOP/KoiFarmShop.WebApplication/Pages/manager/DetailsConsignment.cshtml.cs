using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
    [Authorize(Roles = "Quản lý, Nhân viên bán hàng")] 
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

        // Tạo luôn hàm Duyệt ở trong trang chi tiết để Admin tiện thao tác
        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            await _consignmentService.ApproveRequestAsync(id);
            return RedirectToPage("/manager/ManageConsignment");
        }
        // THÊM HÀM NÀY ĐỂ XỬ LÝ TỪ CHỐI
        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            // Gọi hàm từ chối trong Service
            await _consignmentService.RejectRequestAsync(id);

            // Sau khi từ chối xong thì đá về trang danh sách
            return RedirectToPage("/manager/ManageConsignment");
        }
    }
}