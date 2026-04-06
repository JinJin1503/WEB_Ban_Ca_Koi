using KoiFarmShop.WebApplication.Pages.Manager;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Xunit;
using KoiFarmShop.WebApplication.Security; // Thêm dòng này

namespace BVA.Tests
{
    public class ConsignmentAdminTests
    {
        [Fact]
        public void ManageConsignmentPage_Should_Be_Protected_By_Correct_Policy()
        {
            var pageType = typeof(ManageConsignmentModel);
            var authorizeAttribute = pageType.GetCustomAttributes(typeof(AuthorizeAttribute), true)
                                             .FirstOrDefault() as AuthorizeAttribute;

            Assert.NotNull(authorizeAttribute);
            // Sửa từ Roles sang Policy cho khớp với Code của bạn
            Assert.Equal(AppPolicies.ManagerOnly, authorizeAttribute.Policy);
        }

        [Fact]
        public void DetailsConsignmentPage_Should_Be_Protected_By_Correct_Policy()
        {
            var pageType = typeof(DetailsConsignmentModel);
            var authorizeAttribute = pageType.GetCustomAttributes(typeof(AuthorizeAttribute), true)
                                             .FirstOrDefault() as AuthorizeAttribute;

            Assert.NotNull(authorizeAttribute);
            // Sửa từ Roles sang Policy
            Assert.Equal(AppPolicies.ManagerOnly, authorizeAttribute.Policy);
        }
    }
}