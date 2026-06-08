using Microsoft.EntityFrameworkCore;
using RentalManager.Authorization.Permissions;
using RentalManager.Models;
using System;

namespace RentalManager.Data
{
    public static class PermissionSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            foreach (var (name, category) in PermissionList.All)
            {
                var exists = await context.Permissions
                    .IgnoreQueryFilters()
                    .AnyAsync(p => p.Name == name);

                if (!exists)
                {
                    context.Permissions.Add(new Permission
                    {
                        Name = name,
                        Category = category
                    });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
