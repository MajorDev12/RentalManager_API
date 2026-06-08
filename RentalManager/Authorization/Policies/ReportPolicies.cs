using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class ReportPolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PermissionNames.Report.Read, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Report.Read
                )));
        }
    }
}
