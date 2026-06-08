using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RentalManager.Data;
using RentalManager.Models;

namespace RentalManager.DI.Identity
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(
            this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                // Password policy
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;

                // User settings
                options.User.RequireUniqueEmail = true;

                // Optional: sign-in settings
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}