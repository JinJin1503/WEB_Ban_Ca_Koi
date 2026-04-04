using KoiFarmShop.WebApplication.Pages.manager.Staffs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BVA.Tests
{
    public class StaffValidationTests
    {
        // ----------------------------------------------------------------
        // 1. TEST BVA CHO SỐ ĐIỆN THOẠI (Bắt buộc đúng 10 ký tự)
        // ----------------------------------------------------------------
        [Theory]
        [InlineData("012345678", false)]   // BVA: Min - 1 (9 ký tự -> Fail)
        [InlineData("0123456789", true)]   // BVA: Min & Max (Đúng 10 ký tự -> Pass)
        [InlineData("01234567890", false)] // BVA: Max + 1 (11 ký tự -> Fail)
        public void Validate_Staff_Phone_BVA(string phone, bool expectedResult)
        {
            // Arrange: Khởi tạo trực tiếp class InputModel thay vì CreateModel
            var inputModel = new CreateModel.InputModel { Phone = phone };
            var context = new ValidationContext(inputModel) { MemberName = "Phone" };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(inputModel.Phone, context, results);

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        // ----------------------------------------------------------------
        // 2. TEST BVA CHO MỨC LƯƠNG (Giá trị nhỏ nhất là 0, không được âm)
        // ----------------------------------------------------------------
        [Theory]
        [InlineData(-1, false)]     // BVA: Min - 1 (Số âm -> Fail)
        [InlineData(0, true)]       // BVA: Min (Bằng 0 -> Pass - Thực tập sinh không lương)
        [InlineData(1, true)]       // BVA: Min + 1 (Lớn hơn 0 -> Pass)
        [InlineData(5000000, true)] // Giá trị ngẫu nhiên hợp lệ (Pass)
        public void Validate_Staff_Salary_BVA(int salary, bool expectedResult)
        {
            // Arrange: Truyền giá trị vào InputModel
            var inputModel = new CreateModel.InputModel { Salary = salary };
            var context = new ValidationContext(inputModel) { MemberName = "Salary" };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(inputModel.Salary, context, results);

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
}