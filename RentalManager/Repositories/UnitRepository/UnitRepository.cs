using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.Unit;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.QueryExtensions;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.UnitRepository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ISystemCodeItemRepository _systemCodeItemRepo;
        public UnitRepository(ApplicationDbContext context, ICurrentUser currentUser, ISystemCodeItemRepository systemCodeItemRepo)
        {
            _context = context;
            _currentUser = currentUser;
            _systemCodeItemRepo = systemCodeItemRepo;
        }


        public async Task<List<Unit>?> GetAllAsync()
        {
            return await _context.Units
                .ApplyRoleFilter(_currentUser, _context, x => x.PropertyId)
                .WithDetails()
                .OrderBy(ub => ub.Property.Name).ThenBy(ub => ub.Name)
                .ToListAsync();
        }

        public async Task<List<READUnitLookupDto>?> GetLookupsAsync()
        {
            return await _context.Units
                .ApplyRoleFilter(_currentUser, _context, x => x.PropertyId)
                .Select(u => new READUnitLookupDto
                {
                    Id = u.Id,
                    Name = u.Name,
                })
                .ToListAsync();
        }

        public async Task<(List<Unit>, int)> GetFilteredAsync(UnitQueryFilter filter)
        {
            var query = _context.Units
                .WithDetails()
                .AsNoTracking()
                .ApplyRoleFilter(_currentUser, _context, p => p.PropertyId)
                .AsQueryable();

            query = query
                .ApplyUnitSearch(filter)
                .ApplyUnitFilters(filter)
                .ApplyUnitSorting(filter);

            var totalRecords = await query.CountAsync();

            var data = await query
                .ApplyPagination(filter.PageNumber, filter.PageSize)
                .ToListAsync();

            return (data, totalRecords);
        }



        public async Task<Unit?> GetByIdAsync(int id)
        {
            return await _context.Units
                .Where(u => u.Id == id)
                .ApplyRoleFilter(_currentUser, _context, x => x.PropertyId)
                .WithDetails()
                .FirstOrDefaultAsync();
        }


        public async Task<List<Unit>?> GetByPropertyIdAsync(int id)
        {
            return await _context.Units
                .Where(u => u.PropertyId == id)
                .ApplyRoleFilter(_currentUser, _context, x => x.PropertyId)
                .WithDetails()
                .ToListAsync();
        }


        public async Task<Unit> AddAsync(Unit unit)
        {
            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            return unit;
        }


        public async Task UpdateAsync(Unit unit)
        {

            await _context.SaveChangesAsync();
        }


        public async Task<Unit?> UpdateStatus(int unitId, int statusId)
        {
            var existingUnit = await FindAsync(unitId);
            if (existingUnit == null) return null;

            existingUnit.StatusId = statusId;
            await _context.SaveChangesAsync();

            return existingUnit;
        }


        public async Task DeleteAsync(Unit unit)
        {
            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Unit>?> GetVacantsAsync()
        {
            var status = await _systemCodeItemRepo.GetByCodeAndItemAsync("Vacant", "UNITSTATUS");

            if (status == null)
                return null;

            return await _context.Units
                .Where(u => u.StatusId == status.Id)
                .ApplyRoleFilter(_currentUser, _context, x => x.PropertyId)
                .WithDetails()
                .ToListAsync();
        }


        public async Task<Unit?> FindAsync(int id)
        {
            return await _context.Units
                .Where(u => u.Id == id)
                .ApplyRoleFilter(_currentUser, _context, x => x.PropertyId)
                .WithDetails()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Units
                .ApplyRoleFilter(_currentUser, _context, u => u.PropertyId)
                .AnyAsync(u => u.Id == id);
        }

        public async Task<bool> ExistsInPropertyAsync(int unitId, int propertyId)
        {
            return await _context.Units
                .ApplyRoleFilter(_currentUser, _context, u => u.PropertyId)
                .AnyAsync(u =>
                    u.Id == unitId &&
                    u.PropertyId == propertyId);
        }


    }
}
