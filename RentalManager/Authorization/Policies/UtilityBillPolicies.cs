using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class UtilityBillPolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.UtilityBill.Read, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord"
                )));

            options.AddPolicy(PolicyNames.UtilityBill.Write, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager"
                )));

            options.AddPolicy(PolicyNames.UtilityBill.Update, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager"
                )));

            options.AddPolicy(PolicyNames.UtilityBill.Delete, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager"
                )));
        }
    }
}
