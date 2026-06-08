using RentalManager.Services.AccountAccessService;

public class JwtAccountResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public JwtAccountResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAccountContext accountContext)
    {
        var claim = context.User?.FindFirst("accountId")?.Value;

        if (int.TryParse(claim, out var accountId))
        {
            accountContext.SetAccount(accountId);
        }

        await _next(context);
    }
}
