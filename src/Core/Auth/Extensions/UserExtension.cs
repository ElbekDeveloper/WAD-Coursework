using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Core.Auth.Extensions
{
    public static class UserExtension
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User is null)
            {
                return string.Empty;
            }
            return httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}
