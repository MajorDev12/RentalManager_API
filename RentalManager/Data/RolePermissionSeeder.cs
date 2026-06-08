using Microsoft.EntityFrameworkCore;
using RentalManager.Constants;
using RentalManager.Models;

namespace RentalManager.Data
{
    public static class RolePermissionSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var roles = await context.Roles.ToListAsync();
            var permissions = await context.Permissions.ToListAsync();

            var existing = await context.RolePermissions.ToListAsync();

            foreach (var role in roles)
            {
                foreach (var permission in permissions)
                {
                    var exists = existing.Any(rp =>
                        rp.RoleId == role.Id &&
                        rp.PermissionId == permission.Id);

                    if (!exists)
                    {
                        context.RolePermissions.Add(new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = permission.Id,
                            IsAllowed = role.Name == RoleNames.Owner // simple default
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
