using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Test1.CustomPolicy
{
    public class CheckIsAllData : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (context.HttpContext.User.IsInRole("Admin"))
                {
                    return;
                }
                Claim claim = context.HttpContext.User.Claims.Where(t => t.Type == "IsAllData" && t.Value == "Yes").FirstOrDefault();
                if (claim == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "SupplementUserInfo" }));
                }
                return;
            }

            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()) return;
        }
    }
}

