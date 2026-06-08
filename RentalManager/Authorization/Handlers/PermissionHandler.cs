using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Handlers
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
        {
            var userPermissions = context.User.FindAll("permissions")
                                              .Select(c => c.Value);

            if (userPermissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
