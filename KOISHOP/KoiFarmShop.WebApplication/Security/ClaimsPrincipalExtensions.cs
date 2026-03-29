using System;
using System.Security.Claims;

namespace KoiFarmShop.WebApplication.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetCustomerId(this ClaimsPrincipal user)
        {
            return TryParseClaim(user, AppClaimTypes.CustomerId);
        }

        public static int? GetUserId(this ClaimsPrincipal user)
        {
            return TryParseClaim(user, ClaimTypes.NameIdentifier);
        }

        private static int? TryParseClaim(ClaimsPrincipal user, string claimType)
        {
            string claimValue = user?.FindFirstValue(claimType);
            if (string.IsNullOrWhiteSpace(claimValue))
            {
                return null;
            }

            return int.TryParse(claimValue, out int parsedValue) ? parsedValue : null;
        }
    }
}
