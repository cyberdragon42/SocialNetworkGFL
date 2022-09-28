using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;

namespace BusinessLogic.Helpers
{
    public class ControllerHelper: IControllerHelper
    {
        public string GetIdFromCurrentUser(HttpContext httpContext)
        {
            return httpContext.User.Claims
                    .Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                    .FirstOrDefault()
                    .Value;
        }
    }
}
