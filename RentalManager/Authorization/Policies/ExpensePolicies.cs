using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Requirements;

namespace RentalManager.Authorization.Policies
{
    public static class ExpensePolicies
    {
        public static void Register(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyNames.Expense.Read, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord"
                )));

            options.AddPolicy(PolicyNames.Expense.Create, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord"
                )));

            options.AddPolicy(PolicyNames.Expense.Update, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord"
                )));

            options.AddPolicy(PolicyNames.Expense.Delete, policy =>
                policy.Requirements.Add(new AccountRoleRequirement(
                    "Owner", "Manager", "Landlord"
                )));
        }
    }
}
