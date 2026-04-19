using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Manager
{
    [Authorize(Policy = AppPolicies.StaffOrManager)]
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IConsignmentRequestService _consignmentService;

        public IndexModel(ICustomerService customerService, IOrderService orderService, IConsignmentRequestService consignmentService)
        {
            _customerService = customerService;
            _orderService = orderService;
            _consignmentService = consignmentService;
        }

        public int TotalCustomers { get; set; }
        public List<Customer> CustomerList { get; set; } = new List<Customer>();

        // Thống kê đơn thành công
        public int SuccessSaleConsignments { get; set; }
        public int SuccessCareConsignments { get; set; }

        public double OrderRevenue { get; set; }
        public double SaleConsignmentRevenue { get; set; }
        public double CareConsignmentRevenue { get; set; }
        public double TotalRevenue => OrderRevenue + SaleConsignmentRevenue + CareConsignmentRevenue;

        [BindProperty(SupportsGet = true)]
        public int? SelectedMonth { get; set; }

        public async Task OnGetAsync()
        {
            // 1. Lọc Khách hàng
            var allCustomers = await _customerService.GetAllCustomersAsync();
            if (allCustomers != null)
            {
                CustomerList = SelectedMonth.HasValue && SelectedMonth > 0
                    ? allCustomers.Where(c => c.RegistrationDate.Month == SelectedMonth).ToList()
                    : allCustomers.ToList();
                TotalCustomers = CustomerList.Count;
            }

            // 2. Lọc Đơn hàng (Doanh thu bán cá trực tiếp)
            var allOrders = await _orderService.GetAllOrdersAsync();
            if (allOrders != null)
            {
                var filteredOrders = SelectedMonth.HasValue && SelectedMonth > 0
                    ? allOrders.Where(o => o.OrderDate.Month == SelectedMonth && o.Status == "Completed")
                    : allOrders.Where(o => o.Status == "Completed");

                OrderRevenue = filteredOrders.Sum(o => Convert.ToDouble(o.TotalPrice));
            }

            // 3. Lọc Ký gửi (Đếm số đơn thành công & Doanh thu phí)
            var allConsignments = await _consignmentService.GetAllConsignmentRequestsAsync();
            if (allConsignments != null)
            {
                var filteredCons = SelectedMonth.HasValue && SelectedMonth > 0
                    ? allConsignments.Where(c => c.RequestDate.Month == SelectedMonth)
                    : allConsignments;

                // Đếm đơn Ký gửi Bán thành công (Đã bán)
                SuccessSaleConsignments = filteredCons.Count(c => c.ConsignmentType == "Bán" && c.Status == "Đã bán");
                SaleConsignmentRevenue = filteredCons.Where(c => c.ConsignmentType == "Bán" && c.Status == "Đã bán")
                                                     .Sum(c => Convert.ToDouble(c.ConsignmentFee));

                // Đếm đơn Ký gửi Chăm sóc thành công (Đang/Đã chăm sóc)
                SuccessCareConsignments = filteredCons.Count(c => c.ConsignmentType == "Chăm sóc" && c.Status == "Đang chăm sóc");
                CareConsignmentRevenue = filteredCons.Where(c => c.ConsignmentType == "Chăm sóc")
                                                     .Sum(c => Convert.ToDouble(c.ConsignmentFee));
            }
        }
    }
}