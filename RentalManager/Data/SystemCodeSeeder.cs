using Microsoft.EntityFrameworkCore;
using RentalManager.Models;
using RentalManager.Constants;
using System.Reflection;
using RentalManager.DTOs.SystemCodeItem;
using RentalManager.Helpers.Formatting;

namespace RentalManager.Data
{
    public static class SystemCodeSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var codeType = typeof(SystemCodeNames.Code);
            var itemRootType = typeof(SystemCodeNames.Item);

            // 1. Get all system codes from constants
            var codeFields = codeType
                .GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var codeField in codeFields)
            {
                var codeValue = codeField.GetValue(null)?.ToString();
                if (string.IsNullOrWhiteSpace(codeValue)) continue;

                // Ensure SystemCode exists
                var dbCode = await context.SystemCodes
                    .FirstOrDefaultAsync(c => c.Code == codeValue);

                if (dbCode == null)
                {
                    dbCode = new SystemCode
                    {
                        Code = codeValue
                    };

                    context.SystemCodes.Add(dbCode);
                    await context.SaveChangesAsync();
                }

                // 2. Find matching Item class (by name)
                var itemGroupType = itemRootType
                    .GetNestedType(codeField.Name, BindingFlags.Public);

                if (itemGroupType == null)
                    continue; // No items defined for this code

                var itemFields = itemGroupType
                    .GetFields(BindingFlags.Public | BindingFlags.Static);

                foreach (var itemField in itemFields)
                {
                    var itemValue = itemField.GetValue(null)?.ToString();
                    if (string.IsNullOrWhiteSpace(itemValue)) continue;

                    var exists = await context.SystemCodeItems.AnyAsync(i =>
                        i.SystemCodeId == dbCode.Id &&
                        i.Item == itemValue);

                    if (!exists)
                    {
                        var metadata = GetMetadata(
                            codeValue,
                            itemValue
                        );

                        context.SystemCodeItems.Add(new SystemCodeItem
                        {
                            SystemCodeId = dbCode.Id,

                            Item = itemValue,

                            DisplayName = metadata?.Name ?? TextFormatter.ToDisplayName(itemValue),

                            IconKey = metadata?.IconKey,

                            Color = metadata?.Color,

                            SortOrder = metadata?.SortOrder ?? 0
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }


        private static SystemCodeItemMetadata? GetMetadata(
            string code,
            string item)
        {
            return code switch
            {
                SystemCodeNames.Code.UtilityBill =>
                    SystemCodeMetadata.UtilityBill
                        .GetValueOrDefault(item),

                _ => null
            };
        }

        
    }
}