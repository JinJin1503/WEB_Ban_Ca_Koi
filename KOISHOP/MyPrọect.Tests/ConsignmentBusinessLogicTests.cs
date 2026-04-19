using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Pages.dichvukiban;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Hosting;

namespace BVA.Tests
{
    public class ConsignmentBusinessLogicTests
    {
        // ---------------------------------------------------------------- 
        // Test luồng logic khi Khách hàng gửi đơn Ký gửi Bán cá
        // ----------------------------------------------------------------
        [Theory]
        [InlineData(true)]  // Kịch bản 1: Khách hàng ĐÃ có hồ sơ -> Gửi đơn thành công
        [InlineData(false)] // Kịch bản 2: Khách hàng CHƯA có hồ sơ -> Bị chặn lại và báo lỗi
        public async Task Validate_Customer_Consignment_Submission_Logic(bool hasCustomerProfile)
        {
            // 1. ARRANGE: Chuẩn bị Mock Service (Cắt đứt kết nối với Database thật)
            var mockConsignService = new Mock<IConsignmentRequestService>();
            var mockCustomerService = new Mock<ICustomerService>();

            if (hasCustomerProfile)
            {
                // Dặn hệ thống: Nếu hasCustomerProfile = true, hãy trả về 1 ông khách hàng giả
                mockCustomerService.Setup(s => s.GetCustomerByUserIdAsync(It.IsAny<int>()))
                                   .ReturnsAsync(new Customer { CustomerId = 1, CustomerName = "Nguyen Van A" });
            }
            else
            {
                // Nếu false, trả về null (Không tìm thấy)
                mockCustomerService.Setup(s => s.GetCustomerByUserIdAsync(It.IsAny<int>()))
                                   .ReturnsAsync((Customer)null);
            }
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            var pageModel = new IndexModel(mockConsignService.Object, mockCustomerService.Object, mockEnvironment.Object)
            {
                ConsignmentRequest = new ConsignmentRequest { KoiName = "Koi Test", ConsignmentFee = 1000000 }
            };

            // Giả lập Cookie đăng nhập của User có ID = 1
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }, "mock"));
            pageModel.PageContext = new PageContext { HttpContext = new DefaultHttpContext { User = user } };

            // 2. ACT: Bấm nút Gửi Form
            var result = await pageModel.OnPostAsync();

            // 3. ASSERT: Kiểm tra xem code có chạy đúng thiết kế không
            if (hasCustomerProfile)
            {
                // Phải chuyển trang thành công (Redirect)
                Assert.IsType<RedirectToPageResult>(result);
                // Phải gọi hàm lưu vào Database đúng 1 lần
                mockConsignService.Verify(s => s.AddConsignmentRequestAsync(It.IsAny<ConsignmentRequest>()), Times.Once);
            }
            else
            {
                // Bị đá lại trang cũ (PageResult)
                Assert.IsType<PageResult>(result);
                // Báo lỗi trong ModelState
                Assert.False(pageModel.ModelState.IsValid);
                // Tuyệt đối KHÔNG được lưu vào Database
                mockConsignService.Verify(s => s.AddConsignmentRequestAsync(It.IsAny<ConsignmentRequest>()), Times.Never);
            }
        }
    }
}