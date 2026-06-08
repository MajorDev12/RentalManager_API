using Microsoft.EntityFrameworkCore;
using RentalManager.BusinessRules.Core;
using RentalManager.Data;
using RentalManager.Models;

namespace RentalManager.BusinessRules.Utilities
{
    public class PropertyUtilityUniqueRule
        : IBusinessRule<UtilityBill>
    {
        private readonly ApplicationDbContext _context;

        public PropertyUtilityUniqueRule(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ValidateAsync(
            UtilityBill entity,
            RuleOperation operation)
        {
            if (operation == RuleOperation.Delete)
                return;

            var exists = await _context.UtilityBills
                .AnyAsync(x =>
                    x.PropertyId == entity.PropertyId &&
                    x.UtilityId == entity.UtilityId &&
                    x.UnitId == entity.UnitId &&
                    x.Id != entity.Id);

            if (exists)
            {
                throw new Exception(
                    entity.UnitId == null
                        ? "Utility already exists for this property."
                        : "Utility already exists for this unit.");
            }
        }
    }
}