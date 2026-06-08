using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class PropertyPolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PermissionNames.Property.Create, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Property.Create
                    )));

            options.AddPolicy(PermissionNames.Property.Read, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Property.Read
                    )));

            options.AddPolicy(PermissionNames.Property.Update, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Property.Update
                    )));

            options.AddPolicy(PermissionNames.Property.Delete, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Property.Delete
                    )));


        }
    }
}
