using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class UnitTypePolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.UnitType.Read, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord"
                )));

            options.AddPolicy(PolicyNames.UnitType.Write, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager"
                )));

            options.AddPolicy(PolicyNames.UnitType.Update, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager"
                )));

            options.AddPolicy(PolicyNames.UnitType.Delete, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager"
                )));
        }
    }
}
