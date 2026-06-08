using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RentalManager.Services;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.DI.Context
{
    public static class ContextServiceExtensions
    {
        public static IServiceCollection AddContextServices(
            this IServiceCollection services)
        {
            // Access to HTTP context (headers, user claims, etc.)
            services.AddHttpContextAccessor();

            // Current user abstraction
            services.AddScoped<ICurrentUser, CurrentUser>();

            // Account/context resolution
            services.AddScoped<IAccountContext, AccountContext>();
            services.AddScoped<IAccountResolver, AccountResolver>();

            return services;
        }
    }
}