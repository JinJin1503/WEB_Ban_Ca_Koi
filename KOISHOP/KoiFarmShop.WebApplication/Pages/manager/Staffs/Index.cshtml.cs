using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.manager.Staffs
{
    [Authorize(Policy = AppPolicies.StaffOrManager)]
    public class IndexModel : PageModel
    {
        private readonly IStaffService _staffService;

        public IndexModel(IStaffService staffService)
        {
            _staffService = staffService;
        }

        public List<Staff> StaffList { get; set; }
        public int PageSize { get; set; } = 10; // Quy định 10 dòng/trang
        public int TotalPages { get; set; }     // Biến lưu tổng số trang
        public int CurrentPage { get; set; }
        public async Task OnGetAsync(int pageIndex = 1)
        {
            CurrentPage = pageIndex;

            // 1. Lấy toàn bộ danh sách (để đếm tổng số lượng)
            var allStaff = await _staffService.GetAllStaffAsync();

            // 2. Tính tổng số trang bằng toán học (Làm tròn lên)
            int totalItems = allStaff.Count();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            // 3. Kỹ thuật cắt dữ liệu: Bỏ qua (Skip) những người ở trang trước, và Lấy (Take) 10 người cho trang này
            StaffList = allStaff.Skip((pageIndex - 1) * PageSize)
                                .Take(PageSize)
                                .ToList();
        }
    }
}
