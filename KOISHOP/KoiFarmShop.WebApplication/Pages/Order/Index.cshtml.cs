using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace KoiFarmShop.WebApplication.Pages.Order;

public class IndexModel : PageModel
{
    private readonly IOrderService _orderService;

    public IndexModel(IOrderService orderService)
    {
        _orderService = orderService;
    }
	public string StatusFilter { get; set; }
	public List<Orders> Orders { get; set; }

	public async Task OnGetAsync(string status)
	{
		StatusFilter = status;

		var orders = await _orderService.GetAllOrdersAsync();

		if (!string.IsNullOrEmpty(status))
		{
			orders = orders.Where(o => o.Status == status).ToList();
		}

		Orders = orders;
	}

	public async Task<IActionResult> OnPostApproveAsync(int orderId)
    {
        await _orderService.UpdateStatus(orderId, "Approved");
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostShippingAsync(int orderId)
    {
        await _orderService.UpdateStatus(orderId, "Shipping");
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeliveredAsync(int orderId)
    {
        await _orderService.UpdateStatus(orderId, "Delivered");
        return RedirectToPage();
    }
    public async Task<IActionResult> OnGetTestAsync()
    {
        await _orderService.UpdateStatus(1, "Shipping");
        return Content("Updated!");
    }
}