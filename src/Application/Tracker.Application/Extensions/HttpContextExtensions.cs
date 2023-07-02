using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Tracker.Application.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetUserProfileIdClaimValue(this HttpContext context)
        {
            return GetGuidClaimValue("UserProfileId", context);
        }

        public static Guid GetIdentityIdClaimValue(this HttpContext context)
        {
            return GetGuidClaimValue("IdentityId", context);
        }
        public static string GetClientIdClaimValue(this HttpContext context)
        {
            return GetGuidClaimValue("ClientId", context).ToString();
        }

        private static Guid GetGuidClaimValue(string key, HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            return Guid.Parse(identity?.FindFirst(key)?.Value);
        }
    }
}
