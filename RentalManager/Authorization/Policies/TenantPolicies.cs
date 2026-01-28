using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class TenantPolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.Tenant.ReadAll, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord"
                )));

            options.AddPolicy(PolicyNames.Tenant.ReadSelf, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord", "Tenant"
                )));

            options.AddPolicy(PolicyNames.Tenant.Create, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord"
                )));

            options.AddPolicy(PolicyNames.Tenant.Update, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord", "Tenant"
                )));

            options.AddPolicy(PolicyNames.Tenant.Assign, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord"
                )));

            options.AddPolicy(PolicyNames.Tenant.Delete, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner"
                )));
        }
    }
}
