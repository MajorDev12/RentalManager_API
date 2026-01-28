using RentalManager.Helpers.Authorization;
using System.Security.Claims;

namespace RentalManager.Services.AccountAccessService
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _http;

        public CurrentUser(IHttpContextAccessor http)
        {
            _http = http;
        }

        private ClaimsPrincipal User =>
            _http.HttpContext?.User
            ?? throw new UnauthorizedAccessException();

        public int UserId => User.UserId();
        public int AccountId => User.AccountId();
        public string Role => User.Role();

        public bool IsInRole(string role) => User.IsInRole(role);
    }
}
