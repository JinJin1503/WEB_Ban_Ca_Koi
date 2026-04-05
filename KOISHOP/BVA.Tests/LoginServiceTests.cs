using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces; 
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.Services.Services;
using Moq; 
using System.Threading.Tasks;
using Xunit;

namespace BVA.Tests
{
    public class LoginServiceTests
    {
        [Theory]
        [InlineData(3, true)]  // Đã sai 3 lần -> Lần này vẫn được phép thử (trả về lỗi sai pass, không phải lỗi khóa)
        [InlineData(4, false)] // Đã sai 4 lần -> Tài khoản BỊ KHÓA
        [InlineData(5, false)] // Đã sai 5 lần -> Tài khoản BỊ KHÓA
        public async Task Validate_Login_Lockout_RealCode(int currentFailedAttempts, bool shouldAllowTry)
        {
            // 1. ARRANGE: Chuẩn bị "Database giả"
            var mockRepo = new Mock<IUserRepository>();
            var mockHasher = new Mock<IPasswordHasher>();

            // Tạo một User giả đang có số lần đăng nhập sai bằng với InlineData truyền vào
            var fakeUser = new User
            {
                UserName = "khanhtest",
                PasswordHasher = "hashed_pass",
                FailedAttemptCount = currentFailedAttempts,
                IsLocked = currentFailedAttempts >= 5 // Nếu DB đang lưu >= 5 thì IsLocked là true
            };

            // Dặn Database giả: "Hễ ai gọi hàm GetUserByUserNameAsync tìm 'khanhtest' thì trả về fakeUser này nhé"
            mockRepo.Setup(repo => repo.GetUserByUserNameAsync("khanhtest")).ReturnsAsync(fakeUser);

            // Bơm Database giả vào Service thực tế CỦA ỨNG DỤNG
            var userService = new UserService(mockRepo.Object, mockHasher.Object);

            // 2. ACT: Gọi thẳng vào hàm code thực tế
            // Cố tình truyền sai pass để xem hệ thống có khóa hay không
            var result = await userService.LoginAsync("khanhtest", "wrong_password");

            // 3. ASSERT: Kiểm tra kết quả thực tế từ ứng dụng
            if (shouldAllowTry)
            {
                // Nếu được phép thử (mới sai 4 lần), hàm phải báo lỗi "Sai mật khẩu" (chưa khóa)
                Assert.Contains("Bạn còn", result.errorMessage);
            }
            else
            {
                // Nếu không được thử (đã sai >= 5 lần), hàm phải báo lỗi "bị khóa"
                Assert.Contains("bị khóa", result.errorMessage);
            }
        }
    }
}