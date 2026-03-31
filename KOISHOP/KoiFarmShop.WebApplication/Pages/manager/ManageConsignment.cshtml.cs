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
    }
}
