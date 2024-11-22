using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.Repositories.Entities;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Cart
{
    public class ConfirmationModel : PageModel
    {
        private readonly IOrderService _orderService;

        public ConfirmationModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Orders Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            Order = await _orderService.GetOrderByIdAsync(orderId);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
