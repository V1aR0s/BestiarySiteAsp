using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Test1.Models;

namespace Test1.CustomPolicy
{
    public class IsAllDataPolicy : IAuthorizationRequirement
    {}

    public class IsAllDataHandler : AuthorizationHandler<IsAllDataPolicy>
    {

        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IsAllDataHandler(UserManager<User> _userManager)
        {
            this._userManager = _userManager;
        }
        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, IsAllDataPolicy requirement)
        {
            //AuthorizationFilterContext context_;

            if (context.User.Identity.IsAuthenticated)
            {
                var id = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                User user = await _userManager.FindByIdAsync(id);




            }


            context.Succeed(requirement);
            return Task.CompletedTask;
        }


    }
}
