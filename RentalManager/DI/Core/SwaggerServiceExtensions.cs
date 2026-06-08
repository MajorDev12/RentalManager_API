using Microsoft.Extensions.DependencyInjection;

namespace RentalManager.DI.Core
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            return services;
        }
    }
}