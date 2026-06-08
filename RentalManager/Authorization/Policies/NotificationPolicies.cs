using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public class NotificationPolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PermissionNames.Notification.Read, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Notification.Read
                )));

        }
    }
}
