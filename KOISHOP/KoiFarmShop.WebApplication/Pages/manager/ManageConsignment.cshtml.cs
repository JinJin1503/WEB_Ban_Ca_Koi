using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Implementations;
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

        public ManageConsignmentModel(IConsignmentRequestService consignmentService)
        {
            _consignmentService = consignmentService;
        }

        public List<ConsignmentRequest> Requests { get; set; }

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
        {
            var request = await _consignmentService.GetConsignmentRequestByIdAsync(id);
            if (request != null)
            {
                request.Status = "Đã nhận cá";
                await _consignmentService.UpdateConsignmentRequestAsync(request);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostStartCareAsync(int id)
        {
            var request = await _consignmentService.GetConsignmentRequestByIdAsync(id);
            if (request != null)
            {
                request.Status = "Đang chăm sóc";
                await _consignmentService.UpdateConsignmentRequestAsync(request);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSoldAsync(int id)
        {
            var request = await _consignmentService.GetConsignmentRequestByIdAsync(id);
            if (request != null)
            {
                request.Status = "Đã bán";
                await _consignmentService.UpdateConsignmentRequestAsync(request);
            }
            return RedirectToPage();
        }
    }
}
