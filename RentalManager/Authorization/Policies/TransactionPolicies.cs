using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class TransactionPolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PermissionNames.Transaction.ReadAll, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Transaction.ReadAll
                )));

            options.AddPolicy(PermissionNames.Transaction.ReadSelf, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    PermissionNames.Transaction.ReadSelf
                )));

            options.AddPolicy(PermissionNames.Transaction.Create, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    PermissionNames.Transaction.Create
                )));

            options.AddPolicy(PermissionNames.Transaction.Update, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    PermissionNames.Transaction.Update
                )));

            options.AddPolicy(PermissionNames.Transaction.Assign, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    PermissionNames.Transaction.Assign
                )));

            options.AddPolicy(PermissionNames.Transaction.Delete, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    PermissionNames.Transaction.Delete
                )));
        }
    }
}
