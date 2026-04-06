using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

// LƯU Ý DÒNG NÀY: Phải là Pages.Manager
namespace KoiFarmShop.WebApplication.Pages.Manager
{
    [Authorize(Policy = AppPolicies.StaffOrManager)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // Code trang chủ Dashboard của Admin (Biểu đồ, Doanh thu...)
        }
    }
}