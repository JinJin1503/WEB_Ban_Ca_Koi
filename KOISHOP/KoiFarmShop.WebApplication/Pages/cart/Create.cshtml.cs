using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;

namespace KoiFarmShop.WebApplication.Pages.Cart
{
    public class CreateModel : PageModel
    {
       

        private readonly ICartService _cartService;
        private readonly IKoiFishService _koiFishService;

        // Constructor nhận vào các dịch vụ
        public CreateModel(ICartService cartService, IKoiFishService koiFishService)
        {
            _cartService = cartService;
            _koiFishService = koiFishService;
        }

        // Thực thể Cart để bind dữ liệu khi gửi form
        [BindProperty]
        public KoiFarmShop.Repositories.Entities.Cart Cart { get; set; }

        [BindProperty]
        public int KoiId { get; set; }

        // Dùng để cung cấp danh sách khách hàng và cá Koi cho select list
        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy danh sách khách hàng từ dịch vụ (giả định rằng dịch vụ này có phương thức GetAllCustomers)
            // Bạn có thể thay đổi tên phương thức để phù hợp với dịch vụ của mình
            ViewData["CustomerId"] = new SelectList(await _cartService.GetCartItemsAsync(1), "CustomerId", "FullName"); // Giả sử CustomerId là duy nhất

            // Lấy danh sách cá Koi có sẵn
            ViewData["KoiFishItems"] = new SelectList(await _koiFishService.GetAllKoisAsync(), "KoiId", "BreedType");

            return Page();
        }

        // Thực hiện thêm Cart mới khi submit form
        public async Task<IActionResult> OnPostAsync()
        {
            NormalizeValidation();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu giỏ hàng không hợp lệ.";
                return RedirectToPage("./Index");
            }

            var selectedKoiId = KoiId;
            if (selectedKoiId <= 0)
            {
                selectedKoiId = Cart?.CartItems?.FirstOrDefault()?.KoiId ?? 0;
            }

            if (Cart == null || Cart.CustomerId <= 0 || selectedKoiId <= 0)
            {
                TempData["ErrorMessage"] = "Thiếu thông tin để tạo giỏ hàng.";
                return RedirectToPage("./Index");
            }

            try
            {
                await _cartService.AddCartItemToCartAsync(Cart.CustomerId, selectedKoiId, 1, 1, 1, 1);
                TempData["SuccessMessage"] = "Thêm giỏ hàng thành công.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Không thể tạo giỏ hàng.";
            }

            return RedirectToPage("./Index");
        }

        private void NormalizeValidation()
        {
            ModelState.Remove("Cart.Customer");
            ModelState.Remove("Cart.CartItems");
        }
    }
}
