namespace KoiFarmShop.WebApplication.Security
{
    public static class AppPolicies
    {
        public const string ManagerOnly = "ManagerOnly";
        public const string StaffOrManager = "StaffOrManager"; 
        public const string CustomerOnly = "CustomerOnly";
    }
}
