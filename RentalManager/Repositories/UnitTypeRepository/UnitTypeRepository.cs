using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Repositories.UnitTypeRepository
{
    public class UnitTypeRepository : IUnitTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public UnitTypeRepository(ApplicationDbContext context)
        {
            _context = context;   
        }


        public async Task<List<UnitType>?> GetAllAsync()
        {
            return await _context.UnitTypes
                .Include(p => p.Property)
                .ToListAsync();
        }

        public async Task<UnitType?> GetByIdAsync(int id)
        {
            return await _context.UnitTypes
                .Include(p => p.Property)
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }


        public async Task<UnitType> AddAsync(UnitType type)
        {
            _context.UnitTypes.Add(type);
            await _context.SaveChangesAsync();

            return type;
        }

        public async Task<UnitType> UpdateAsync(UnitType type)
        {
            var existingType = await FindAsync(type.Id);

            if (existingType == null) return null;

            var updatedEntity = type.UpdateEntity(existingType);

            await _context.SaveChangesAsync();

            return updatedEntity;
        }


        public async Task DeleteAsync(UnitType type)
        {
            _context.UnitTypes.Remove(type);
            await _context.SaveChangesAsync();
        }


        public async Task<UnitType?> FindAsync(int id)
        {
            return await _context.UnitTypes.FindAsync(id);
        }

        public async Task<List<UnitType>?> GetByPropertyIdAsync(int id)
        {
           return await _context.UnitTypes
                .Include(u => u.Property)
                .Where(u => u.PropertyId == id)
                .ToListAsync();
        }
    }
}
