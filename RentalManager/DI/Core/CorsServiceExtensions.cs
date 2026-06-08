using Microsoft.Extensions.DependencyInjection;

namespace RentalManager.DI.Core
{
    public static class CorsServiceExtensions
    {
        public static IServiceCollection AddCorsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy =>
                        policy.WithOrigins("http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials());
            });

            return services;
        }
    }
}