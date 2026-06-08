using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RentalManager.Data;
using RentalManager.Models;

namespace RentalManager.DI.Seeding
{
    public static class SeedingExtensions
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // ⚡ 1. Core tenant structure first
            await AccountSeeder.SeedAsync(context);
            await PropertySeeder.SeedAsync(context);

            // ⚡ 2. Identity + authorization foundation
            await RoleSeeder.SeedAsync(context);
            await PermissionSeeder.SeedAsync(context);
            await RolePermissionSeeder.SeedAsync(context);

            // ⚡ 3. System configuration data
            await SystemCodeSeeder.SeedAsync(context);

            // ⚡ 4. Users (depends on Account + Roles)
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            await UserSeeder.SeedAsync(context, userManager, roleManager);
            await UserRoleSeeder.SeedAsync(context);
        }
    }
}