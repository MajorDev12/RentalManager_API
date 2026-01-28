using Microsoft.AspNetCore.Authorization;

namespace RentalManager.Authorization.Requirements
{
    public class AccountRoleRequirement : IAuthorizationRequirement
    {
        public IReadOnlyList<string> AllowedRoles { get; }

        public AccountRoleRequirement(params string[] roles)
        {
            AllowedRoles = roles;
        }
    }
}
