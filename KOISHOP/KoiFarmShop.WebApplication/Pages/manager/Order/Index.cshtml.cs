using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.Repositories.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace KoiFarmShop.WebApplication.Pages.Order
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;

        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public string StatusFilter { get; set; }
        public List<Orders> Orders { get; set; }

        // =========================
        // GET
        // =========================
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

        // =========================
        // APPROVE
        // =========================
        public async Task<IActionResult> OnPostApproveAsync(int orderId)
        {
            try
            {
                await _orderService.UpdateStatus(orderId, "Approved");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await OnGetAsync(null);
                return Page();
            }

            return RedirectToPage();
        }

        // =========================
        // SHIPPING
        // =========================
        public async Task<IActionResult> OnPostShippingAsync(int orderId)
        {
            try
            {
                await _orderService.UpdateStatus(orderId, "Shipping");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await OnGetAsync(null);
                return Page();
            }

            return RedirectToPage();
        }

        // =========================
        // COMPLETED
        // =========================
        public async Task<IActionResult> OnPostCompletedAsync(int orderId)
        {
            try
            {
                await _orderService.UpdateStatus(orderId, "Completed");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await OnGetAsync(null);
                return Page();
            }

            return RedirectToPage();
        }
    }
}