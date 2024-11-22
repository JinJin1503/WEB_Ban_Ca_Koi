using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KoiFarmShop.WebApplication.Pages.Contact
{
	public class IndexModel : PageModel
	{
		[BindProperty]
		public string Name { get; set; }

		[BindProperty]
		public string Email { get; set; }

		[BindProperty]
		public string Subject { get; set; }

		[BindProperty]
		public string Message { get; set; }

		public string ErrorMessage { get; set; }
		public string SuccessMessage { get; set; }

		public void OnGet()
		{
			// Khởi tạo trạng thái trang
			ErrorMessage = string.Empty;
			SuccessMessage = string.Empty;
		}

		public IActionResult OnPost()
		{
			if (!ModelState.IsValid)
			{
				ErrorMessage = "Vui lòng điền đầy đủ thông tin.";
				return Page();
			}

			// Xử lý logic gửi tin nhắn
			SuccessMessage = "Your message has been sent. Thank you!";
			return Page();
		}
	}
}
