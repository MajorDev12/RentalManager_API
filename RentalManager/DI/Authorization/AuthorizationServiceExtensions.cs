using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using RentalManager.Authorization;
using RentalManager.Authorization.Handlers;

namespace RentalManager.DI.Authorization
{
    public static class AuthorizationServiceExtensions
    {
        public static IServiceCollection AddAuthorizationServices(
            this IServiceCollection services)
        {
            services.AddAppAuthorization();

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            // Custom authorization response handling
            services.AddSingleton<IAuthorizationMiddlewareResultHandler,
                ApiAuthorizationResultHandler>();

            return services;
        }
    }
}