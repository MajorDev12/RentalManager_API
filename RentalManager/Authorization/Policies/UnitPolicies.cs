using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class UnitPolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.Unit.Read, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord"
                )));

            options.AddPolicy(PolicyNames.Unit.Write, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager"
                )));

            options.AddPolicy(PolicyNames.Unit.Update, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager"
                )));

            options.AddPolicy(PolicyNames.Unit.Delete, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager"
                )));
        }
    }
}
