using Microsoft.EntityFrameworkCore;
using RentalManager.Models;

namespace RentalManager.Data
{
    public static class AccountSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Avoid duplicate accounts
            var existing = await context.Accounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Name == "Demo Account");

            if (existing != null)
                return;

            var account = new Account
            {
                Name = "Demo Account",

                IsActive = true,
                IsTrial = true,

                // 14-day trial
                TrialEndsAt = DateTime.UtcNow.AddDays(14),

                SubscriptionEndsAt = null
            };

            context.Accounts.Add(account);
            await context.SaveChangesAsync();
        }
    }
}