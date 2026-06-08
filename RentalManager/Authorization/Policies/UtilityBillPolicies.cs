using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class UtilityBillPolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PermissionNames.UtilityBill.Read, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.UtilityBill.Read
                )));

            options.AddPolicy(PermissionNames.UtilityBill.Create, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.UtilityBill.Read
                )));

            options.AddPolicy(PermissionNames.UtilityBill.Update, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.UtilityBill.Read
                )));

            options.AddPolicy(PermissionNames.UtilityBill.Delete, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.UtilityBill.Read
                )));
        }
    }
}
