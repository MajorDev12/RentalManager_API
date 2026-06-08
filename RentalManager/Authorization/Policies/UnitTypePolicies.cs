using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class UnitTypePolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.UnitType.Read, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.UnitType.Read
                )));

            options.AddPolicy(PermissionNames.UnitType.Create, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.UnitType.Create
                )));

            options.AddPolicy(PermissionNames.UnitType.Update, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.UnitType.Update
                )));

            options.AddPolicy(PermissionNames.UnitType.Delete, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.UnitType.Delete
                )));
        }
    }
}
