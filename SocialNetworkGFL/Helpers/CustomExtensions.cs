using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkGFL.Helpers
{
    public static class CustomExtensions
    {
        public static string GetIdFromCurrentUser(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return null;
            }

            var idClaim = httpContext.User.Claims
                    .Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                    .FirstOrDefault();

            return idClaim?.Value;
        }
    }
}
