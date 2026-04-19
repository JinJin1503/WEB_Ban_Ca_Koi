using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Pages.Manager.Consignment;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BVA.Tests
{
    public class ManageConsignmentPageTests
    {
        [Fact]
        public async Task OnGetAsync_Should_Populate_Requests_List()
        {
            var fakeRequests = new List<ConsignmentRequest> { new ConsignmentRequest { RequestId = 1, KoiName = "Cá 1" } };
            var mockService = new Mock<IConsignmentRequestService>();
            mockService.Setup(s => s.GetAllConsignmentRequestsAsync()).ReturnsAsync(fakeRequests);
            var pageModel = new ManageConsignmentModel(mockService.Object);

            await pageModel.OnGetAsync();

            Assert.NotNull(pageModel.Requests);
            Assert.Single(pageModel.Requests);
        }

        [Fact]
        public async Task OnPostApproveAsync_Should_Call_Approve_And_Refresh_Page()
        {
            var mockService = new Mock<IConsignmentRequestService>();
            var pageModel = new ManageConsignmentModel(mockService.Object);

            var result = await pageModel.OnPostApproveAsync(5);

            mockService.Verify(s => s.ApproveRequestAsync(5), Times.Once);
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPostRejectAsync_Should_Call_Reject_And_Refresh_Page()
        {
            var mockService = new Mock<IConsignmentRequestService>();
            var pageModel = new ManageConsignmentModel(mockService.Object);

            var result = await pageModel.OnPostRejectAsync(6);

            mockService.Verify(s => s.RejectRequestAsync(6), Times.Once);
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPostReceiveKoiAsync_Should_UpdateStatus_To_DaNhanCa()
        {
            var fakeRequest = new ConsignmentRequest { RequestId = 1, Status = "Approved" };
            var mockService = new Mock<IConsignmentRequestService>();
            mockService.Setup(s => s.GetConsignmentRequestByIdAsync(It.IsAny<int>())).ReturnsAsync(fakeRequest);

            var pageModel = new ManageConsignmentModel(mockService.Object);
            var result = await pageModel.OnPostReceiveKoiAsync(1);

            Assert.Equal("Đã nhận cá", fakeRequest.Status);
            mockService.Verify(s => s.UpdateConsignmentRequestAsync(fakeRequest), Times.Once);
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPostStartCareAsync_Should_UpdateStatus_To_DangChamSoc()
        {
            var fakeRequest = new ConsignmentRequest { RequestId = 2, Status = "Đã nhận cá" };
            var mockService = new Mock<IConsignmentRequestService>();
            mockService.Setup(s => s.GetConsignmentRequestByIdAsync(It.IsAny<int>())).ReturnsAsync(fakeRequest);

            var pageModel = new ManageConsignmentModel(mockService.Object);
            var result = await pageModel.OnPostStartCareAsync(2);

            Assert.Equal("Đang chăm sóc", fakeRequest.Status);
            mockService.Verify(s => s.UpdateConsignmentRequestAsync(fakeRequest), Times.Once);
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPostSoldAsync_Should_UpdateStatus_To_DaBan()
        {
            var fakeRequest = new ConsignmentRequest { RequestId = 3, Status = "Đã nhận cá" };
            var mockService = new Mock<IConsignmentRequestService>();
            mockService.Setup(s => s.GetConsignmentRequestByIdAsync(It.IsAny<int>())).ReturnsAsync(fakeRequest);

            var pageModel = new ManageConsignmentModel(mockService.Object);
            var result = await pageModel.OnPostSoldAsync(3);

            Assert.Equal("Đã bán", fakeRequest.Status);
            mockService.Verify(s => s.UpdateConsignmentRequestAsync(fakeRequest), Times.Once);
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}