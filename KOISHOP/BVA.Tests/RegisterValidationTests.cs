using KoiFarmShop.WebApplication.Pages.Register;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BVA.Tests
{
    public class RegisterValidationTests
    {
        // ----------------------------------------------------------------
        // 1. TEST BVA CHO USERNAME (Độ dài từ 3 - 40)
        // ----------------------------------------------------------------
        [Theory]
        [InlineData("", false)]      // BVA: Bỏ trống (Fail)
        [InlineData("ab", false)]    // BVA: Min - 1 (2 ký tự -> Fail)
        [InlineData("abc", true)]    // BVA: Min (3 ký tự -> Pass)
        [InlineData("abcdefghijabcdefghijabcdefghijabcdefghij", true)] // BVA: Max (40 ký tự -> Pass)
        [InlineData("abcdefghijabcdefghijabcdefghijabcdefghijA", false)] // BVA: Max + 1 (41 ký tự -> Fail)
        public void Validate_UserName_BVA_Only(string username, bool expectedResult)
        {
            // Arrange
            var model = new RegisterModel(null, null, null) { UserName = username };
            var context = new ValidationContext(model) { MemberName = "UserName" };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(model.UserName, context, results);

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        // ----------------------------------------------------------------
        // 2. TEST BVA CHO PASSWORD (Độ dài từ 8 - 16)
        // ----------------------------------------------------------------
        [Theory]
        [InlineData("", false)]                  // BVA: Bỏ trống (Fail)
        [InlineData("1234567", false)]           // BVA: Min - 1 (7 ký tự -> Fail)
        [InlineData("12345678", true)]           // BVA: Min (8 ký tự -> Pass)
        [InlineData("1234567890123456", true)]   // BVA: Max (16 ký tự -> Pass)
        [InlineData("12345678901234567", false)] // BVA: Max + 1 (17 ký tự -> Fail)
        public void Validate_Password_BVA_Only(string password, bool expectedResult)
        {
            // Arrange
            var model = new RegisterModel(null, null, null) { Password = password };
            var context = new ValidationContext(model) { MemberName = "Password" };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(model.Password, context, results);

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        // ----------------------------------------------------------------
        // 3. TEST BVA CHO CUSTOMER NAME (Độ dài từ 2 - 50)
        // ----------------------------------------------------------------
        [Theory]
        [InlineData("", false)]      // BVA: Bỏ trống (Fail)
        [InlineData("A", false)]     // BVA: Min - 1 (1 ký tự -> Fail)
        [InlineData("An", true)]     // BVA: Min (2 ký tự -> Pass)
        [InlineData("abcdefghijabcdefghijabcdefghijabcdefghijabcdefghij", true)] // BVA: Max (50 ký tự -> Pass)
        [InlineData("abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijA", false)] // BVA: Max + 1 (51 ký tự -> Fail)
        public void Validate_CustomerName_BVA_Only(string customerName, bool expectedResult)
        {
            // Arrange
            var model = new RegisterModel(null, null, null) { CustomerName = customerName };
            var context = new ValidationContext(model) { MemberName = "CustomerName" };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(model.CustomerName, context, results);

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
}