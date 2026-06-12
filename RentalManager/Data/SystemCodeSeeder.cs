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
        // Central metadata registry (extensible)
        private static readonly Dictionary<string, Dictionary<string, SystemCodeItemMetadata>> MetadataMap
            = new()
            {
                [SystemCodeNames.Code.UtilityBill] = SystemCodeMetadata.UtilityBill,
                [SystemCodeNames.Code.UnitType] = SystemCodeMetadata.UnitType
            };

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var codeType = typeof(SystemCodeNames.Code);
            var itemRootType = typeof(SystemCodeNames.Item);

            var codeFields = codeType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var codeField in codeFields)
            {
                var codeValue = codeField.GetValue(null)?.ToString();
                if (string.IsNullOrWhiteSpace(codeValue))
                    continue;

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

                // Find matching nested item class
                var itemGroupType = itemRootType
                    .GetNestedType(codeField.Name, BindingFlags.Public);

                if (itemGroupType == null)
                    continue;

                var itemFields = itemGroupType
                    .GetFields(BindingFlags.Public | BindingFlags.Static);

                // Try get metadata dictionary for this code
                MetadataMap.TryGetValue(codeValue, out var groupMetadata);

                foreach (var itemField in itemFields)
                {
                    var itemValue = itemField.GetValue(null)?.ToString();
                    if (string.IsNullOrWhiteSpace(itemValue))
                        continue;

                    var exists = await context.SystemCodeItems.AnyAsync(i =>
                        i.SystemCodeId == dbCode.Id &&
                        i.Item == itemValue);

                    if (exists)
                        continue;

                    // Try get metadata for item
                    SystemCodeItemMetadata? metadata = null;

                    if (groupMetadata != null)
                    {
                        groupMetadata.TryGetValue(itemValue, out metadata);
                    }

                    context.SystemCodeItems.Add(new SystemCodeItem
                    {
                        SystemCodeId = dbCode.Id,
                        Item = itemValue,

                        DisplayName =
                            metadata?.DisplayName
                            ?? TextFormatter.ToDisplayName(itemValue),

                        IconKey = metadata?.IconKey,
                        GroupKey = metadata?.GroupKey,
                        Color = metadata?.Color,

                        SortOrder = metadata?.SortOrder ?? 0
                    });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}