using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class TenantPolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.Tenant.ReadAll, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Tenant.ReadAll
                )));

            options.AddPolicy(PermissionNames.Tenant.ReadSelf, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Tenant.ReadSelf
                )));

            options.AddPolicy(PermissionNames.Tenant.Create, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Tenant.Create
                )));

            options.AddPolicy(PermissionNames.Tenant.Update, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Tenant.Update
                )));

            options.AddPolicy(PermissionNames.Tenant.Assign, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Tenant.Assign
                )));

            options.AddPolicy(PermissionNames.Tenant.Delete, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Tenant.Delete
                )));
        }
    }
}
