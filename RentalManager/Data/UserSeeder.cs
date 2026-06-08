using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalManager.Constants;
using RentalManager.Models;

namespace RentalManager.Data
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            var account = await context.Accounts.IgnoreQueryFilters().FirstOrDefaultAsync();
            var accountId = account.Id;

            var property = await context.Properties.IgnoreQueryFilters().FirstOrDefaultAsync();
            var gender = await context.SystemCodeItems
                .Include(i => i.SystemCode)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(i =>
                    i.SystemCode.Code == SystemCodeNames.Code.Gender &&
                    i.Item == SystemCodeNames.Item.Gender.Male);
            var userStatus = await context.SystemCodeItems
                .Include(i => i.SystemCode)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(i =>
                    i.SystemCode.Code == SystemCodeNames.Code.UserStatus &&
                    i.Item == SystemCodeNames.Item.UserStatus.Active);



            if (property == null || gender == null || userStatus == null)
                return;

            // Define seed users
            var seedUsers = new List<(string email, string password, string role)>
            {
                ("owner@demo.com", "Password123!", "Owner"),
                ("manager@demo.com", "Password123!", "Manager"),
                ("tenant@demo.com", "Password123!", "Tenant")
            };

            foreach (var seed in seedUsers)
            {
                var existing = await userManager.FindByEmailAsync(seed.email);
                if (existing != null)
                    continue;

                // 1. Identity user
                var appUser = new ApplicationUser
                {
                    UserName = seed.email,
                    Email = seed.email,
                    AccountId = accountId
                };

                var result = await userManager.CreateAsync(appUser, seed.password);
                if (!result.Succeeded)
                    continue;

                await userManager.AddToRoleAsync(appUser, seed.role);

                // 2. Domain user
                var user = new User
                {
                    FirstName = seed.role,
                    LastName = "User",
                    EmailAddress = seed.email,
                    MobileNumber = "0700000000",
                    AccountId = accountId,
                    ApplicationUserId = appUser.Id,
                    PropertyId = seed.role == "Tenant" ? property.Id : null,
                    GenderId = gender.Id,
                    UserStatusId = userStatus.Id,
                    IsActive = true
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }
    }
}