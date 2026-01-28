using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class PropertyPolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.Property.Read, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord"
                )));

            options.AddPolicy(PolicyNames.Property.Write, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager"
                )));

            options.AddPolicy(PolicyNames.Property.Assign, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner"
                )));
        }
    }
}
