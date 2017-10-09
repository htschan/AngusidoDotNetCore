namespace Server.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
                public const string ApiAccessPower = "api_access_power";
                public const string ApiAccessAdmin = "api_access_admin";
            }
        }
    }
}