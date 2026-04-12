using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq; 
using System;

namespace KoiFarmShop.WebApplication.Pages.manager.Customers
{
    [Authorize(Roles = "Manager,Staff")]
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public IndexModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // Biến chứa dữ liệu truyền sang giao diện
        public IList<Customer> Customers { get; set; } = new List<Customer>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public async Task OnGetAsync(int pageIndex = 1)
        {
            // 1. Lấy toàn bộ danh sách từ database
            var allCustomers = await _customerService.GetAllCustomersAsync();

            if (allCustomers != null && allCustomers.Count > 0)
            {
                // 2. Tính tổng số trang 
                TotalPages = (int)Math.Ceiling(allCustomers.Count / (double)PageSize);

                // 3. Đảm bảo pageIndex hợp lệ (không nhỏ hơn 1, không lớn hơn TotalPages)
                CurrentPage = pageIndex < 1 ? 1 : pageIndex;
                if (CurrentPage > TotalPages)
                {
                    CurrentPage = TotalPages;
                }

                // 4. Lọc dữ liệu đúng trang hiện tại
                Customers = allCustomers
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
        }
    }
}