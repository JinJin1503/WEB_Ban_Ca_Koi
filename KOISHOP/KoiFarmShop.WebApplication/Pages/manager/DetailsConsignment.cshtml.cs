using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
    [Authorize(Policy = AppPolicies.ManagerOnly)]
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
    }
}
