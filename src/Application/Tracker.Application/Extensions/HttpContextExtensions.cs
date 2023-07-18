using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Security.Claims;

namespace Tracker.Application.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetIdentityId(this HttpContext context)
        {
            return GetGuidClaimValue("IdentityId", context);
        }
        public static Guid GetClientId(this HttpContext context)
        {
            return GetGuidClaimValue("ClientId", context);
        }
        public static Guid GetAdminId(this HttpContext context)
        {
            return GetGuidClaimValue("AdminId", context);
        }
        public static string GetUserType(this HttpContext context)
        {
            var key = "UserType";
            var identity = context.User.Identity as ClaimsIdentity;
            return identity?.FindFirst(key)?.Value;
        }

        private static Guid GetGuidClaimValue(string key, HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            return Guid.Parse(identity?.FindFirst(key)?.Value);
        }
        private static ObjectId GetObjectIdClaimValue(string key, HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            return ObjectId.Parse(identity?.FindFirst(key)?.Value);
        }
    }
}
