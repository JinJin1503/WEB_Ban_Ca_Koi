using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Services;
using KoiFarmShop.WebApplication.Pages.Login;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BVA.Tests
{
    public class LoginValidationTests
    {
        // ----------------------------------------------------------------
        // 1. TEST BVA CHO USERNAME Ở TRANG ĐĂNG NHẬP (Độ dài từ 3 - 40)
        // ----------------------------------------------------------------
        [Theory]
        [InlineData("", false)]      // Bỏ trống (Fail)
        [InlineData("ab", false)]    // Dưới Min (Fail)
        [InlineData("abc", true)]    // Tại Min (Pass)
        [InlineData("abcdefghijabcdefghijabcdefghijabcdefghij", true)] // Tại Max (Pass)
        [InlineData("abcdefghijabcdefghijabcdefghijabcdefghijA", false)] // Vượt Max (Fail)
        public void Validate_Login_UserName_BVA(string username, bool expectedResult)
        {
            var model = new LoginModel(null, null, null) { UserName = username }; // Truyền null vì chưa cần gọi UserService
            var context = new ValidationContext(model) { MemberName = "UserName" };
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateProperty(model.UserName, context, results);

            Assert.Equal(expectedResult, isValid);
        }

        // ----------------------------------------------------------------
        // 2. TEST BVA CHO PASSWORD Ở TRANG ĐĂNG NHẬP (Độ dài từ 8 - 16)
        // ----------------------------------------------------------------
        [Theory]
        [InlineData("", false)]                  // Bỏ trống (Fail)
        [InlineData("1234567", false)]           // Dưới Min (Fail)
        [InlineData("12345678", true)]           // Tại Min (Pass)
        [InlineData("1234567890123456", true)]   // Tại Max (Pass)
        [InlineData("12345678901234567", false)] // Vượt Max (Fail)
        public void Validate_Login_Password_BVA(string password, bool expectedResult)
        {
            var model = new LoginModel(null, null, null) { Password = password };
            var context = new ValidationContext(model) { MemberName = "Password" };
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateProperty(model.Password, context, results);

            Assert.Equal(expectedResult, isValid);
        }       
    }
}