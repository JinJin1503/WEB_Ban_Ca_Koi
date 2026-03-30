using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
    [Authorize(Policy = AppPolicies.ManagerOnly)]
    public class IndexModel : PageModel
    {
        private readonly IStaffService _staffService;

        public IndexModel(IStaffService staffService)
        {
            _staffService = staffService;
        }

        public List<Staff> StaffList { get; set; }

        public async Task OnGetAsync()
        {
            StaffList = await _staffService.GetAllStaffAsync();
        }
    }
}
