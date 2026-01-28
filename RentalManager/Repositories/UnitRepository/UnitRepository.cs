using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.QueryExtensions;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.UnitRepository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ApplicationDbContext _context;
        public UnitRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Unit>?> GetAllAsync(ICurrentUser user)
        {
            return await _context.Units
                .ApplyRoleFilter(user, _context)
                .WithDetails()
                .OrderBy(ub => ub.Property.Name).ThenBy(ub => ub.Name)
                .ToListAsync();
        }


        public async Task<Unit?> GetByIdAsync(ICurrentUser user, int id)
        {
            return await _context.Units
                .Where(u => u.Id == id)
                .ApplyRoleFilter(user, _context)
                .WithDetails()
                .FirstOrDefaultAsync();
        }


        public async Task<List<Unit>?> GetByPropertyIdAsync(ICurrentUser user, int id)
        {
            return await _context.Units
                .Where(u => u.PropertyId == id)
                .ApplyRoleFilter(user, _context)
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


        public async Task<Unit> UpdateStatus(ICurrentUser user, int unitId, int statusId)
        {
            var existingUnit = await FindAsync(user, unitId);

            if (existingUnit == null) return null;

            existingUnit.UpdateStatusEntity(statusId);
            await _context.SaveChangesAsync();

            return existingUnit;
        }


        public async Task DeleteAsync(Unit unit)
        {
            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
        }


        public async Task<Unit?> FindAsync(ICurrentUser user, int id)
        {
            return await _context.Units
                .Where(u => u.Id == id)
                .ApplyRoleFilter(user, _context)
                .WithDetails()
                .FirstOrDefaultAsync();
        }


    }
}
