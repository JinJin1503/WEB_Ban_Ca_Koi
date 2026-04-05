using KoiFarmShop.WebApplication.Pages.manager.Staffs; // Trỏ đến file Create của Admin
using KoiFarmShop.WebApplication.Security; // Trỏ đến nơi chứa file AppPolicies.cs
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Xunit;

namespace BVA.Tests
{
    public class AuthorizationTests
    {
        // ----------------------------------------------------------------
        // 2. TEST KIỂM TRA PHÂN QUYỀN (Có chặn đúng đối tượng không?)
        // ----------------------------------------------------------------
        [Fact] // Dùng [Fact] thay vì [Theory] vì test này không cần truyền tham số đầu vào
        public void CreateStaffPage_Should_Be_Protected_By_ManagerOnly_Policy()
        {
            // Arrange: Lấy cái class CreateModel của trang Tạo Nhân viên ra để soi
            var pageType = typeof(CreateModel);

            // Act: Tìm xem trên đầu class này có gắn cái thẻ (Attribute) [Authorize] nào không
            var authorizeAttribute = pageType.GetCustomAttributes(typeof(AuthorizeAttribute), true)
                                             .FirstOrDefault() as AuthorizeAttribute;

            // Assert 1: Đảm bảo class này BẮT BUỘC PHẢI CÓ thẻ [Authorize] (Không được để trống cửa)
            Assert.NotNull(authorizeAttribute);

            // Assert 2: Đảm bảo thẻ đó phải là loại "ổ khóa xịn" dành riêng cho Manager
            Assert.Equal(AppPolicies.ManagerOnly, authorizeAttribute.Policy);
        }
    }
}