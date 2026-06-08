using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RentalManager.Infrastructure
{
    public static class EfModelLogger
    {
        public static void Log(DbContext context, ILogger logger)
        {
            var model = context.Model;

            logger.LogInformation("========== EF CORE MODEL METADATA ==========");

            foreach (var entity in model.GetEntityTypes())
            {
                logger.LogInformation("Entity: {Entity}", entity.ClrType?.Name ?? entity.Name);

                // Global query filter
                if (entity.GetQueryFilter() != null)
                {
                    logger.LogWarning("  ⚠ Global Query Filter is applied");
                }

                foreach (var prop in entity.GetProperties())
                {
                    var columnName = prop.GetColumnName(StoreObjectIdentifier.Table(
                        entity.GetTableName()!, entity.GetSchema()
                    ));

                    logger.LogInformation(
                        "   ├─ Property: {Property} | Column: {Column} | CLR: {ClrType} | Nullable: {Nullable} | Shadow: {Shadow}",
                        prop.Name,
                        columnName,
                        prop.ClrType.Name,
                        prop.IsNullable,
                        prop.IsShadowProperty()
                    );
                }
            }

            logger.LogInformation("============================================");
        }
    }
}
