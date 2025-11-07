using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Repositories.UnitRepository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ApplicationDbContext _context;
        public UnitRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Unit>?> GetAllAsync()
        {
            return await _context.Units
                .Include(ub => ub.Property)
                .Include(ub => ub.UnitType)
                .Include(ub => ub.Status)
                .OrderBy(ub => ub.Property.Name).ThenBy(ub => ub.Name)
                .ToListAsync();
        }

        public async Task<Unit?> GetByIdAsync(int id)
        {
            return await _context.Units
                .Include(c => c.Property)
                .Include(ub => ub.UnitType)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }

        public async Task<List<Unit>?> GetByPropertyIdAsync(int id)
        {
            return await _context.Units
                .Include(u => u.Property)
                .Include(u => u.UnitType)
                .Include(u => u.Status)
                .Where(u => u.PropertyId == id)
                .ToListAsync();
        }

        public async Task<Unit> AddAsync(Unit unit)
        {
            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            return unit;
        }

        public async Task<Unit> UpdateAsync(Unit unit)
        {
            var existingUnit = await FindAsync(unit.Id);

            if (existingUnit == null) return null;

            var updatedEntity = unit.UpdateEntity(existingUnit);

            await _context.SaveChangesAsync();

            return updatedEntity;
        }

        public async Task<Unit> UpdateStatus(int unitId, int statusId)
        {
            var existingUnit = await FindAsync(unitId);

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

        public async Task<Unit?> FindAsync(int id)
        {
            return await _context.Units.FindAsync(id);
        }


    }
}
