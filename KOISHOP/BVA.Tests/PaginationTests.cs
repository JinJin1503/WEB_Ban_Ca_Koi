using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.WebApplication.Pages.manager.Staffs;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BVA.Tests
{
    public class PaginationTests
    {
        // ----------------------------------------------------------------
        // TEST BVA THỰC TẾ CHO CHỨC NĂNG PHÂN TRANG CỦA TRANG INDEX
        // ----------------------------------------------------------------
        [Theory]
        [InlineData(0, 10, 0)]   // BVA: Database có 0 người -> 0 trang
        [InlineData(9, 10, 1)]   // BVA: Min - 1 (Database có 9 người -> 1 trang)
        [InlineData(10, 10, 1)]  // BVA: Min/Max (Database có 10 người -> 1 trang, vừa khít)
        [InlineData(11, 10, 2)]  // BVA: Max + 1 (Database có 11 người -> Tràn sang trang 2)
        [InlineData(25, 10, 3)]  // Giá trị bất kỳ: 25 người -> 3 trang
        public async Task Validate_Pagination_RealCode_BVA(int totalStaffsInDatabase, int pageSize, int expectedTotalPages)
        {
            // 1. ARRANGE: Chuẩn bị Môi trường và Dữ liệu giả
            var mockStaffService = new Mock<IStaffService>();

            // Tự động tạo ra một danh sách nhân viên giả có số lượng bằng đúng kịch bản InlineData
            var fakeStaffList = new List<Staff>();
            for (int i = 0; i < totalStaffsInDatabase; i++)
            {
                fakeStaffList.Add(new Staff { StaffId = i }); // Tạo ID giả
            }

            // Dặn Database giả: Cứ gọi hàm GetAllStaffAsync thì ném cái danh sách trên ra
            mockStaffService.Setup(service => service.GetAllStaffAsync())
                            .ReturnsAsync(fakeStaffList);

            // Bơm Service giả vào class IndexModel thực tế của hệ thống
            var indexModel = new IndexModel(mockStaffService.Object)
            {
                PageSize = pageSize
            };

            // 2. ACT: Gọi thẳng hàm tải trang (Giống như thao tác bấm F5 tải lại trang web)
            await indexModel.OnGetAsync(pageIndex: 1);

            // 3. ASSERT: Kiểm tra xem hệ thống phân trang có chính xác không
            // a) Kiểm tra tổng số trang tính toán có khớp với BVA không
            Assert.Equal(expectedTotalPages, indexModel.TotalPages);

            // b) Kiểm tra độ dài mảng dữ liệu (trên trang 1 không được phép vượt quá 10 dòng)
            int expectedStaffsOnPage1 = totalStaffsInDatabase > pageSize ? pageSize : totalStaffsInDatabase;
            Assert.Equal(expectedStaffsOnPage1, indexModel.StaffList.Count);
        }
    }
}