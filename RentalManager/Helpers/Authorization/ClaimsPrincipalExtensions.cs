using System.Security.Claims;

namespace RentalManager.Helpers.Authorization
{
    public static class ClaimsPrincipalExtensions
    {
        public static int UserId(this ClaimsPrincipal user) =>
        int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        public static string Role(this ClaimsPrincipal user) =>
            user.FindFirst(ClaimTypes.Role)!.Value;

        public static int AccountId(this ClaimsPrincipal user) =>
            int.Parse(user.FindFirst("accountId")!.Value);
    }
}
