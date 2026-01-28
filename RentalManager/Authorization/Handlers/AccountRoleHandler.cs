using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Requirements;
using System.Security.Claims;

namespace RentalManager.Authorization.Handlers
{
    public class AccountRoleHandler : AuthorizationHandler<AccountRoleRequirement>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AccountRoleRequirement requirement)
        {
            // SuperAdmin bypass
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

            if (role != null && requirement.AllowedRoles.Contains(role))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
