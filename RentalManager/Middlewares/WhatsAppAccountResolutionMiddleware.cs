using RentalManager.Services.AccountAccessService;

public class WhatsAppAccountResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public WhatsAppAccountResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IAccountResolver accountResolver,
        IAccountContext accountContext
    )
    {
        // Only apply to WhatsApp webhook
        if (context.Request.Path.StartsWithSegments("/api/webhooks/whatsapp"))
        {
            context.Request.EnableBuffering();

            var form = await context.Request.ReadFormAsync();
            var from = form["From"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(from))
            {
                var normalized = NormalizePhone(from);
                var accountId = await accountResolver.ResolveAccountIdByPhoneAsync(normalized);

                if (accountId.HasValue)
                {
                    accountContext.SetAccount(accountId.Value);
                }
            }

            context.Request.Body.Position = 0;
        }

        await _next(context);
    }

    private string NormalizePhone(string number)
    {
        number = number.Replace("whatsapp:", "").Replace("+", "").Trim();

        if (number.StartsWith("2547"))
            return "0" + number.Substring(3);

        if (number.StartsWith("2541"))
            return "0" + number.Substring(3);

        return number;
    }
}
