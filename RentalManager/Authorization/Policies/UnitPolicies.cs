using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class UnitPolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PermissionNames.Unit.Read, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Unit.Read
                )));

            options.AddPolicy(PermissionNames.Unit.Create, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Unit.Create
                )));

            options.AddPolicy(PermissionNames.Unit.Update, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Unit.Update
                )));

            options.AddPolicy(PermissionNames.Unit.Delete, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Unit.Delete
                )));
        }
    }
}
