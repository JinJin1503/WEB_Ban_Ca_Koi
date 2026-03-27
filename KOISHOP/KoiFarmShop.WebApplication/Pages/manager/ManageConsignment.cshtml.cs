using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
    [Authorize(Roles = "Manager, Staff")]
    public class ManageConsignmentModel : PageModel
    {
        private readonly IConsignmentRequestService _consignmentService;

        public ManageConsignmentModel(IConsignmentRequestService consignmentService)
        {
            _consignmentService = consignmentService;
        }

        public List<ConsignmentRequest> Requests { get; set; }

        public async Task OnGetAsync()
        {
            // Lấy toàn bộ danh sách đơn ký gửi (Bán & Chăm sóc)
            Requests = await _consignmentService.GetAllConsignmentRequestsAsync();
        }

        // Logic phê duyệt đơn hàng nhanh
        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            await _consignmentService.ApproveRequestAsync(id); //
            return RedirectToPage();
        }
    }
}