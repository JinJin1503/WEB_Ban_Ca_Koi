using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Pages.dichvukiban;
using KoiFarmShop.WebApplication.Pages.Dichvuchamsoc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace KoiFarmShop.Tests
{
    public class ConsignmentTests
    {
        // Hàm Helper giả lập một User đang đăng nhập (vì các page đều yêu cầu Authorize)
        private PageContext GetMockedPageContextWithUser(int userId)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            return new PageContext(new ActionContext(httpContext, new RouteData(), new PageActionDescriptor()));
        }

        [Fact]
        public async Task OnPostAsyncKyGuiChamSoc_Dung()
        {
            // Arrange (Chuẩn bị)
            var mockCareService = new Mock<ICareServiceService>();
            var mockConsignService = new Mock<IConsignmentRequestService>();
            var mockCustomerService = new Mock<ICustomerService>();

            // Giả lập tìm thấy khách hàng ID = 1
            mockCustomerService.Setup(s => s.GetCustomerByUserIdAsync(1))
                .ReturnsAsync(new Customer { CustomerId = 101, UserId = 1 });

            var pageModel = new IndexCareModel(mockCareService.Object, mockConsignService.Object, mockCustomerService.Object)
            {
                PageContext = GetMockedPageContextWithUser(1),
                ConsignmentRequest = new ConsignmentRequest
                {
                    KoiName = "Kohaku",
                    KoiAge = 2,
                    KoiSize = 40,
                    KoiBreed = "Imported"
                }
            };

            // Act (Thực thi)
            var result = await pageModel.OnPostAsync();

            // Assert (Kiểm tra)
            mockConsignService.Verify(s => s.AddConsignmentRequestAsync(It.IsAny<ConsignmentRequest>()), Times.Once);

            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/history_dichvu/History", redirectResult.PageName);
            Assert.Equal("Chăm sóc", pageModel.ConsignmentRequest.ConsignmentType);
        }

        [Fact]
        public async Task OnPostAsyncKyGuiBan_Dung()
        {
            // Arrange
            var mockConsignService = new Mock<IConsignmentRequestService>();
            var mockCustomerService = new Mock<ICustomerService>();

            mockCustomerService.Setup(s => s.GetCustomerByUserIdAsync(2))
                .ReturnsAsync(new Customer { CustomerId = 102, UserId = 2 });

            var pageModel = new IndexModel(mockConsignService.Object, mockCustomerService.Object)
            {
                PageContext = GetMockedPageContextWithUser(2),
                ConsignmentRequest = new ConsignmentRequest
                {
                    KoiName = "Showa",
                    ConsignmentFee = 5000000
                }
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            mockConsignService.Verify(s => s.AddConsignmentRequestAsync(It.IsAny<ConsignmentRequest>()), Times.Once);
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/history_dichvu/History", redirectResult.PageName);
            Assert.Equal("Bán", pageModel.ConsignmentRequest.ConsignmentType); // Phải là luồng Bán 
        }

        [Fact]
        public async Task OnPostAsyncBoTrongThongTin_Trong()
        {
            // Arrange
            var mockConsignService = new Mock<IConsignmentRequestService>();
            var mockCustomerService = new Mock<ICustomerService>();

            var pageModel = new IndexModel(mockConsignService.Object, mockCustomerService.Object)
            {
                PageContext = GetMockedPageContextWithUser(1),
                ConsignmentRequest = new ConsignmentRequest() // Form rỗng
            };

            // Giả lập người dùng không nhập Tên cá (Bỏ trống)
            pageModel.ModelState.AddModelError("ConsignmentRequest.KoiName", "Tên cá là bắt buộc");

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            mockConsignService.Verify(s => s.AddConsignmentRequestAsync(It.IsAny<ConsignmentRequest>()), Times.Never);

            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
        }

        [Fact]
        public async Task OnPostAsyncNhapSaiDinhDang_Sai()
        {
            // Arrange
            var mockCareService = new Mock<ICareServiceService>();
            var mockConsignService = new Mock<IConsignmentRequestService>();
            var mockCustomerService = new Mock<ICustomerService>();

            var pageModel = new IndexCareModel(mockCareService.Object, mockConsignService.Object, mockCustomerService.Object)
            {
                PageContext = GetMockedPageContextWithUser(1),
                ConsignmentRequest = new ConsignmentRequest()
            };

            // Giả lập nhập sai định dạng (Ví dụ tuổi cá nhập chữ "Hai tuổi" thay vì số 2)
            pageModel.ModelState.AddModelError("ConsignmentRequest.KoiAge", "Tuổi cá phải là số nguyên");

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            mockConsignService.Verify(s => s.AddConsignmentRequestAsync(It.IsAny<ConsignmentRequest>()), Times.Never);
            Assert.IsType<PageResult>(result);
            Assert.False(pageModel.ModelState.IsValid);
        }

        [Fact]
        public async Task OnGetAsyncXemLichSu_Dung()
        {
            // Arrange
            var mockConsignService = new Mock<IConsignmentRequestService>();
            var mockCustomerService = new Mock<ICustomerService>();

            // Giả lập khách hàng đang đăng nhập có CustomerId = 101
            mockCustomerService.Setup(s => s.GetCustomerByUserIdAsync(1))
                .ReturnsAsync(new Customer { CustomerId = 101, UserId = 1 });

            // Giả lập DB có 3 đơn: 2 đơn của khách 101, 1 đơn của khách 102
            var fakeDbData = new List<ConsignmentRequest>
            {
                new ConsignmentRequest { RequestId = 1, CustomerId = 101, KoiName = "Ca 1", RequestDate = new DateTime(2023, 1, 1) },
                new ConsignmentRequest { RequestId = 2, CustomerId = 102, KoiName = "Ca cua nguoi khac" },
                new ConsignmentRequest { RequestId = 3, CustomerId = 101, KoiName = "Ca 2", RequestDate = new DateTime(2023, 1, 2) } // Ngày mới hơn
            };
            mockConsignService.Setup(s => s.GetAllConsignmentRequestsAsync()).ReturnsAsync(fakeDbData);

            var historyModel = new WebApplication.Pages.historydichvu.IndexModel(mockConsignService.Object, mockCustomerService.Object)
            {
                PageContext = GetMockedPageContextWithUser(1)
            };

            // Act
            await historyModel.OnGetAsync();

            // Assert
            Assert.NotNull(historyModel.MyRequests);
            Assert.Equal(2, historyModel.MyRequests.Count); // Chỉ lấy 2 đơn của ông 101 

            Assert.Equal("Ca 2", historyModel.MyRequests[0].KoiName);
        }
    }
}