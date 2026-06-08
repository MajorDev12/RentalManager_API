using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.QueryExtensions;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.UtilityBillRepository
{
    public class UtilityBillRepository : IUtilityBillRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public UtilityBillRepository(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }


        public async Task<List<UtilityBill>?> GetAllAsync(ICurrentUser user)
        {
            return await _context.UtilityBills
                .ApplyRoleFilter(user, _context, p => p.PropertyId)
                .Include(p => p.Property)
                .Include(p => p.Utility)
                .Include(x => x.BillingCycle)
                .Include(p => p.Unit)
                .ToListAsync();
        }

        public async Task<List<UtilityLookupDto>?> GetAllLookupsAsync()
        {   
            return await _context.UtilityBills
                .ApplyRoleFilter(_currentUser, _context, p => p.PropertyId)
                .Include(u => u.Utility)
                .Select(p => new UtilityLookupDto
                {
                    Id = p.Id,
                    Name = p.Utility.Item
                })
                .ToListAsync();
        }

        public async Task<(List<UtilityBill>, int)> GetFilteredAsync(UtilityBillQueryFilter filter)
        {
            var query = _context.UtilityBills
                .Include(x => x.Property)
                .Include(x => x.Utility)
                .Include(x => x.BillingCycle)
                .Include(p => p.Unit)
                .AsNoTracking()
                .ApplyRoleFilter(_currentUser, _context, p => p.PropertyId)
                .AsQueryable();

            query = query
                .ApplyUtilitySearch(filter)
                .ApplyUtilityFilters(filter)
                .ApplyUtilitySorting(filter);

            var totalRecords = await query.CountAsync();

            var data = await query
                .ApplyPagination(filter.PageNumber, filter.PageSize)
                .ToListAsync();

            return (data, totalRecords);
        }


        public async Task<UtilityBill?> GetByIdAsync(int id)
        {
            return await _context.UtilityBills
                .Where(u => u.Id == id)
                .Include(u => u.Utility)
                .Include(x => x.BillingCycle)
                .Include(c => c.Property)
                .Include(p => p.Unit)
                .ApplyRoleFilter(_currentUser, _context, p => p.PropertyId)
                .FirstOrDefaultAsync();
        }


        public async Task<List<UtilityBill>?> GetByPropertyIdAsync(int propertyId, bool? isMetered = null)
        {
            IQueryable<UtilityBill> query = _context.UtilityBills
                        .Where(u => u.PropertyId == propertyId)
                        .ApplyRoleFilter(_currentUser, _context, p => p.PropertyId);

            if (isMetered.HasValue)
            {
                query = query.Where(u => u.IsMetered == isMetered.Value);
            }


            return await query
                .Include(u => u.Property)
                .Include(p => p.Utility)
                .Include(p => p.BillingCycle)
                .Include(p => p.Unit)
                .ToListAsync();
        }



        public async Task<List<PropertyUtilityDto>> GetAvailableUtilitiesAsync(
        int propertyId,
        bool? isMetered = null)
        {
            IQueryable<UtilityBill> query = _context.UtilityBills
                .Where(x => x.PropertyId == propertyId)
                .ApplyRoleFilter(_currentUser, _context, p => p.PropertyId);

            if (isMetered.HasValue)
            {
                query = query.Where(x => x.IsMetered == isMetered.Value);
            }

            return await query
                .GroupBy(x => new
                {
                    x.UtilityId,
                    UtilityName = x.Utility.Item
                })
                .Select(g => new PropertyUtilityDto
                {
                    UtilityId = g.Key.UtilityId,
                    UtilityName = g.Key.UtilityName,

                    IsPropertyWide = g.Any(x => x.UnitId == null),

                    HasUnitSpecificConfigurations = g.Any(x => x.UnitId != null)
                })
                .OrderBy(x => x.UtilityName)
                .ToListAsync();
        }


        public async Task<List<UtilityBill>> GetUtilityConfigurationsAsync(
            int propertyId,
            int utilityId)
        {
            return await _context.UtilityBills
                .Where(x => x.PropertyId == propertyId)
                .Where(x => x.UtilityId == utilityId)
                .Include(x => x.Unit)
                .Include(x => x.Utility)
                .Include(x => x.UtilityReadings)
                .ToListAsync();
        }


        public async Task<List<UtilityBill>> GetUtilitiesByUnitAsync(
            int unitId,
            bool? isMetered = null
        )
        {
            var unit = await _context.Units
                .FirstOrDefaultAsync(x => x.Id == unitId);

            if (unit == null)
                throw new Exception("Unit not found");

            IQueryable<UtilityBill> query = _context.UtilityBills
                .Where(x =>
                    x.PropertyId == unit.PropertyId &&
                    (x.UnitId == null || x.UnitId == unitId)
                );

            if (isMetered.HasValue)
            {
                query = query.Where(x => x.IsMetered == isMetered.Value);
            }

            var utilities = await query
                .Include(x => x.Property)
                .Include(x => x.Utility)
                .Include(x => x.BillingCycle)
                .Include(x => x.Unit)
                .ToListAsync();

            // Group by Utility
            var finalUtilities = utilities
                .GroupBy(x => x.UtilityId)
                .Select(g =>
                {
                    // Prefer unit-specific utility
                    return g
                        .OrderByDescending(x => x.UnitId.HasValue)
                        .First();
                })
                .ToList();

            return finalUtilities;
        }

        public async Task<UtilityBill> AddAsync(UtilityBill bill)
        {

            _context.UtilityBills.Add(bill);
            await _context.SaveChangesAsync();

            return bill;
        }

        public async Task<List<UtilityBill>> AddRangeAsync(List<UtilityBill> bills)
        {
            await _context.UtilityBills.AddRangeAsync(bills);
            await _context.SaveChangesAsync();

            return bills;
        }


        public async Task UpdateAsync(UtilityBill bill)
        {
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(UtilityBill bill)
        {
            _context.UtilityBills.Remove(bill);
            await _context.SaveChangesAsync();
        }


        public async Task<UtilityBill?> FindAsync(int id)
        {
            return await _context.UtilityBills
                .Where(u => u.Id == id)
                .ApplyRoleFilter(_currentUser, _context, p => p.PropertyId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> VerifyUtilityByUnit(int unitId, int utilityId)
        {
            return await _context.UtilityBills
                .ApplyRoleFilter(_currentUser, _context, p => p.PropertyId)
                .AnyAsync(u => u.Id == utilityId &&  u.UnitId == unitId);
        }
        

        public async Task<bool> ExistAsync(int utilityId)
        {
            return await _context.UtilityBills
                .ApplyRoleFilter(_currentUser, _context, p => p.PropertyId)
                .AnyAsync(u => u.Id == utilityId);
        }


    }
}
