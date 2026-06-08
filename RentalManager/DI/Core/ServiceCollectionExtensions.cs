using RentalManager.DI.AI;
using RentalManager.DI.Authentication;
using RentalManager.DI.Authorization;
using RentalManager.DI.BusinessRules;
using RentalManager.DI.Context;
using RentalManager.DI.Database;
using RentalManager.DI.Identity;
using RentalManager.DI.Notifications;
using RentalManager.DI.Repositories;
using RentalManager.DI.Services;
using RentalManager.DI.Validation;

namespace RentalManager.DI.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
        {
            services.AddControllers();

            services
            .AddCorsServices()
            .AddDatabaseServices(configuration)
            .AddIdentityServices()
            .AddAuthenticationServices(configuration, environment)
            .AddAuthorizationServices()
            .AddValidationServices()
            .AddRepositoryServices()
            .AddBusinessServices()
            .AddBusinessRules()
            .AddAIServices()
            .AddNotificationServices()
            .AddContextServices()
            .AddSwaggerServices();

        return services;
        }
    }

}
