using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Pages.Manager.Consignment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BVA.Tests
{
    public class DetailsConsignmentPageTests
    {
        [Fact]
        public async Task OnGetAsync_WithInvalidId_ReturnsNotFound()
        {
            var mockService = new Mock<IConsignmentRequestService>();
            mockService.Setup(s => s.GetConsignmentRequestByIdAsync(It.IsAny<int>())).ReturnsAsync((ConsignmentRequest)null);
            var pageModel = new DetailsConsignmentModel(mockService.Object);

            var result = await pageModel.OnGetAsync(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnPostApproveAsync_CallsService_And_Redirects()
        {
            var mockService = new Mock<IConsignmentRequestService>();
            var pageModel = new DetailsConsignmentModel(mockService.Object);

            var result = await pageModel.OnPostApproveAsync(1);

            mockService.Verify(s => s.ApproveRequestAsync(1), Times.Once);
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/manager/ManageConsignment", redirectResult.PageName);
        }

        [Fact]
        public async Task OnPostRejectAsync_CallsService_And_Redirects()
        {
   
            var mockService = new Mock<IConsignmentRequestService>();
            var pageModel = new DetailsConsignmentModel(mockService.Object);
            int testId = 2;

      
            var result = await pageModel.OnPostRejectAsync(testId);


            mockService.Verify(s => s.RejectRequestAsync(testId), Times.Once);

            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
           
            Assert.Equal("/manager/ManageConsignment", redirectResult.PageName);
        }

        [Fact]
        public async Task OnPostReceiveKoiAsync_Should_UpdateStatus_To_DaNhanCa()
        {
            var fakeRequest = new ConsignmentRequest { RequestId = 3, Status = "Approved" };
            var mockService = new Mock<IConsignmentRequestService>();
            mockService.Setup(s => s.GetConsignmentRequestByIdAsync(It.IsAny<int>())).ReturnsAsync(fakeRequest);
            var pageModel = new DetailsConsignmentModel(mockService.Object);

            var result = await pageModel.OnPostReceiveKoiAsync(3);

            Assert.Equal("Đã nhận cá", fakeRequest.Status);
            mockService.Verify(s => s.UpdateConsignmentRequestAsync(fakeRequest), Times.Once);
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPostStartCareAsync_Should_UpdateStatus_To_DangChamSoc()
        {
            var fakeRequest = new ConsignmentRequest { RequestId = 4, Status = "Đã nhận cá" };
            var mockService = new Mock<IConsignmentRequestService>();
            mockService.Setup(s => s.GetConsignmentRequestByIdAsync(It.IsAny<int>())).ReturnsAsync(fakeRequest);
            var pageModel = new DetailsConsignmentModel(mockService.Object);

            var result = await pageModel.OnPostStartCareAsync(4);

            Assert.Equal("Đang chăm sóc", fakeRequest.Status);
            mockService.Verify(s => s.UpdateConsignmentRequestAsync(fakeRequest), Times.Once);
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPostSoldAsync_Should_UpdateStatus_To_DaBan()
        {
            var fakeRequest = new ConsignmentRequest { RequestId = 5, Status = "Đã nhận cá" };
            var mockService = new Mock<IConsignmentRequestService>();
            mockService.Setup(s => s.GetConsignmentRequestByIdAsync(It.IsAny<int>())).ReturnsAsync(fakeRequest);
            var pageModel = new DetailsConsignmentModel(mockService.Object);

            var result = await pageModel.OnPostSoldAsync(5);

            Assert.Equal("Đã bán", fakeRequest.Status);
            mockService.Verify(s => s.UpdateConsignmentRequestAsync(fakeRequest), Times.Once);
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}