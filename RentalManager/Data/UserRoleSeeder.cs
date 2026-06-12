using Microsoft.EntityFrameworkCore;
using RentalManager.Models;

namespace RentalManager.Data
{
    public static class UserRoleSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // 🔹 Load all required data upfront (performance)
            List<User>? users = new List<User>();
            try
            {
                 users = await context.Set<ApplicationUser>()
                            .IgnoreQueryFilters()
                            .Include(u => u.User)
                            .Select(u => u.User)
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
            var roles = await context.Roles.IgnoreQueryFilters().ToListAsync();
            var properties = await context.Properties.IgnoreQueryFilters().ToListAsync();
            var units = await context.Units.IgnoreQueryFilters().ToListAsync();

            if (!users.Any() || !roles.Any())
                return;

            var userRolesToAdd = new List<UserRole>();

            foreach (var user in users)
            {
                Console.WriteLine($"Processing user {user.Id}");
                // 🔹 Skip users without role
                if (units == null) 
                { 
                    Console.WriteLine("❌ No RoleId");
                    continue;
                }

                var role = roles.FirstOrDefault(r => r.Name == user.FirstName);

                if (role == null)
                {
                    Console.WriteLine($"❌ Role not found for user {user.FirstName}");
                    continue;
                }

                int? propertyId = null;

                // 🔥 ROLE LOGIC
                switch (role.Name)
                {
                    case "Owner":
                        propertyId = null; // account-wide
                        break;

                    case "Manager":
                    case "Landlord":
                        var property = properties
                            .FirstOrDefault(p => p.AccountId == user.AccountId);

                        propertyId = property?.Id;
                        break;

                    case "Tenant":
                        var tenantProperty = properties
                            .FirstOrDefault(p => p.Id == user.PropertyId);

                        propertyId = tenantProperty?.Id;
                        break;

                    default:
                        continue;
                }

                // 🔹 Prevent duplicates
                var exists = await context.UserRoles.IgnoreQueryFilters().AnyAsync(ur =>
                    ur.UserId == user.Id &&
                    ur.RoleId == role.Id &&
                    ur.AccountId == user.AccountId &&
                    ur.PropertyId == propertyId);

                if (!exists)
                {
                    userRolesToAdd.Add(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id,
                        AccountId = user.AccountId,
                        PropertyId = propertyId,
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            // 🔹 Save once
            if (userRolesToAdd.Any())
            {
                await context.UserRoles.AddRangeAsync(userRolesToAdd);
                await context.SaveChangesAsync();
            }
        }
    }
}