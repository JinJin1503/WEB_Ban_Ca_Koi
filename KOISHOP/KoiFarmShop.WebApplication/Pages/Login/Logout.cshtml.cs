using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.Login
{
    public class LogoutModel : PageModel
    {
        // Sử dụng OnGetAsync để chạy code ngay khi người dùng bấm vào link Đăng xuất
        public async Task<IActionResult> OnGetAsync()
        {
            // 1. Xóa sạch toàn bộ dữ liệu tạm trong Session (như UserName, UserId...)
            HttpContext.Session.Clear();

            // 2. Thu hồi "thẻ bài" (Xóa Cookie xác thực phân quyền của ASP.NET Core)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 3. Đá người dùng văng về trang Đăng nhập (hoặc bạn có thể đổi thành "/Trangchu/Index" tùy ý)
            return RedirectToPage("/Login/Login");
        }
    }
}