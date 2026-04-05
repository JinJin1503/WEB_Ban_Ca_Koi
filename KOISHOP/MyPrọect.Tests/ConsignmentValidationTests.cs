using KoiFarmShop.Repositories.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BVA.Tests
{
    public class ConsignmentValidationTests
    {
        // ----------------------------------------------------------------
        // 1. TEST BVA CHO TUỔI CÁ (KoiAge phải từ 0 trở lên)
        // ----------------------------------------------------------------
        [Theory]
        [InlineData(-1, false)]          // BVA: Min - 1 (Dưới biên -> Fail)
        [InlineData(0, true)]            // BVA: Min (Tại biên dưới -> Pass)
        [InlineData(1, true)]            // BVA: Min + 1 (Trên biên dưới -> Pass)
        [InlineData(2147483647, true)]   // BVA: Max (Tại biên trên int -> Pass)
        public void Validate_Consignment_KoiAge_BVA(int age, bool expectedResult)
        {
            // Arrange
            var model = new ConsignmentRequest { KoiAge = age };
            var context = new ValidationContext(model) { MemberName = "KoiAge" };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(model.KoiAge, context, results);

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        // ----------------------------------------------------------------
        // 2. TEST BVA CHO KÍCH THƯỚC CÁ (KoiSize phải từ 0.1 trở lên)
        // ----------------------------------------------------------------
        [Theory]
        [InlineData(0.0, false)] // BVA: Min - 0.1 (Dưới biên -> Fail)
        [InlineData(0.1, true)]  // BVA: Min (Tại biên dưới -> Pass)
        [InlineData(0.2, true)]  // BVA: Min + 0.1 (Trên biên dưới -> Pass)
        public void Validate_Consignment_KoiSize_BVA(double size, bool expectedResult)
        {
            // Arrange
            var model = new ConsignmentRequest { KoiSize = size };
            var context = new ValidationContext(model) { MemberName = "KoiSize" };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(model.KoiSize, context, results);

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        // ----------------------------------------------------------------
        // 3. TEST PHÁT HIỆN LỖ HỔNG (BUG): GIÁ CÁ CHO PHÉP ÂM
        // ----------------------------------------------------------------
        [Theory]
        [InlineData(-500000, true)] // LỖI NGHIÊM TRỌNG: Giá âm nhưng Validator vẫn trả về True (Pass) do Dev thiếu [Range]
        public void Validate_Consignment_Fee_MissingRangeBug(int fee, bool expectedResult)
        {
            // Arrange
            var model = new ConsignmentRequest { ConsignmentFee = fee };
            var context = new ValidationContext(model) { MemberName = "ConsignmentFee" };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(model.ConsignmentFee, context, results);

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
}