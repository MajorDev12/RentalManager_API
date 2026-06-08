using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Permissions;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class ExpensePolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PermissionNames.Expense.Read, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Expense.Read
                )));

            options.AddPolicy(PermissionNames.Expense.Create, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Expense.Create
                )));

            options.AddPolicy(PermissionNames.Expense.Update, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Expense.Update
                )));

            options.AddPolicy(PermissionNames.Expense.Delete, policy =>
                policy.Requirements.Add(new PermissionRequirement(
                    PermissionNames.Expense.Delete
                )));
        }
    }
}
