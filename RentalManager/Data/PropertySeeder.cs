using Microsoft.EntityFrameworkCore;
using RentalManager.Models;
using RentalManager.Constants;

namespace RentalManager.Data
{
    public static class PropertySeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // =========================
            // 1. Ensure Account exists
            // =========================
            var account = await context.Accounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync();

            if (account == null)
                return; // or throw if you want strict seeding

            // =========================
            // 2. Get PropertyType (SystemCodeItem)
            // =========================
            var propertyType = await context.SystemCodeItems
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x =>
                    x.SystemCode.Code == SystemCodeNames.Code.PropertyType &&
                    x.Item == SystemCodeNames.Item.PropertyType.Apartment);

            if (propertyType == null)
                return;

            // =========================
            // 3. Check if property already exists
            // =========================
            var exists = await context.Properties
                .IgnoreQueryFilters()
                .AnyAsync(p =>
                p.Name == "Sunrise Apartments" &&
                p.AccountId == account.Id);

            if (exists)
                return;

            // =========================
            // 4. Create property
            // =========================
            var property = new Property
            {
                Name = "Sunrise Apartments",
                Country = "Kenya",
                County = "Nakuru",
                Area = "Milimani",
                PhysicalAddress = "Milimani Estate, Nakuru",
                Longitude = 36.0667m,
                Latitude = -0.3031m,
                Floor = 5,
                EmailAddress = "sunrise@property.com",
                MobileNumber = "0712345678",
                Notes = "Seed property for development testing",

                AccountId = account.Id,
                PropertyTypeId = propertyType.Id
            };

            await context.Properties.AddAsync(property);
            await context.SaveChangesAsync();
        }
    }
}