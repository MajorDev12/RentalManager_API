using RentalManager.Services.AccountAccessService;

namespace RentalManager.Middlewares
{
    public class AccountAccessMiddleware
    {
        private readonly RequestDelegate _next;

        public AccountAccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IAccountAccessService accountAccess)
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                await _next(context);
                return;
            }

            var accountIdClaim = context.User.FindFirst("accountId")?.Value;

            if (!int.TryParse(accountIdClaim, out var accountId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid account");
                return;
            }

            var result = await accountAccess.CheckAccessAsync(accountId);

            if (!result.Allowed)
            {
                context.Response.StatusCode = StatusCodes.Status402PaymentRequired;
                await context.Response.WriteAsync(result.Reason);
                return;
            }

            await _next(context);
        }
    }

}
