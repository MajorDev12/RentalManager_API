using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalManager.Constants;
using RentalManager.Models;

namespace RentalManager.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var roles = new[]
            {
                RoleNames.SuperAdmin,
                RoleNames.Owner,
                RoleNames.Manager,
                RoleNames.Landlord,
                RoleNames.Caretaker,
                RoleNames.Accountant,
                RoleNames.Tenant
            };

            foreach (var roleName in roles)
            {
                // =========================
                // 1. Seed IdentityRole
                // =========================
                var identityExists = await context.Roles
                    .IgnoreQueryFilters()
                    .AnyAsync(r => r.Name == roleName);

                if (!identityExists)
                {
                    context.Set<IdentityRole<int>>().Add(new IdentityRole<int>
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpperInvariant()
                    });
                }

                // =========================
                // 2. Seed Custom Role table
                // =========================
                var customExists = await context.Set<Role>()
                    .IgnoreQueryFilters()
                    .AnyAsync(r => r.Name == roleName);

                if (!customExists)
                {
                    context.Set<Role>().Add(new Role
                    {
                        Name = roleName,
                        IsEnabled = true
                    });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}