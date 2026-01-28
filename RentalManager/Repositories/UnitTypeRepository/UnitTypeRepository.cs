using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.QueryExtensions;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.UnitTypeRepository
{
    public class UnitTypeRepository : IUnitTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public UnitTypeRepository(ApplicationDbContext context)
        {
            _context = context;   
        }


        public async Task<List<UnitType>?> GetAllAsync(ICurrentUser user)
        {
            return await _context.UnitTypes
                .ApplyRoleFilter(user, _context)
                .Include(p => p.Property)
                .ToListAsync();
        }

        public async Task<UnitType?> GetByIdAsync(ICurrentUser user, int id)
        {
            return await _context.UnitTypes
                .ApplyRoleFilter(user, _context)
                .Include(p => p.Property)
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }

        public async Task<List<UnitType>?> GetByPropertyIdAsync(ICurrentUser user, int id)
        {
           return await _context.UnitTypes
                .Where(u => u.PropertyId == id)
                .ApplyRoleFilter(user, _context)
                .Include(u => u.Property)
                .ToListAsync();
        }

        public async Task<UnitType> AddAsync(UnitType type)
        {
            _context.UnitTypes.Add(type);
            await _context.SaveChangesAsync();

            return type;
        }


        public async Task UpdateAsync(UnitType type)
        {
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(UnitType type)
        {
            _context.UnitTypes.Remove(type);
            await _context.SaveChangesAsync();
        }


        public async Task<UnitType?> FindAsync(ICurrentUser user, int id)
        {
            return await _context.UnitTypes
                .Where(u => u.Id == id)
                .ApplyRoleFilter(user, _context)
                .FirstOrDefaultAsync();
        }

    }
}
