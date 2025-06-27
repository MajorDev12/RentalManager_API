using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Repositories.SystemCodeItemRepository
{
    public class SystemCodeItemRepository : ISystemCodeItemRepository
    {

        private readonly ApplicationDbContext _context;

        public SystemCodeItemRepository(ApplicationDbContext context)
        {
            _context = context;

        }


        public async Task<List<SystemCodeItem>?> GetAllAsync()
        {
            return await _context.SystemCodeItems
                .Include(sc => sc.SystemCode)
                .ToListAsync();
        }


        public async Task<SystemCodeItem?> GetByIdAsync(int id)
        {
            return await _context.SystemCodeItems
                .Include(sc => sc.SystemCode)
                .FirstOrDefaultAsync(e => e.Id == id);
        }


        public async Task<SystemCodeItem> AddAsync(SystemCodeItem item)
        {
            _context.SystemCodeItems.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }


        public async Task<SystemCodeItem> UpdateAsync(SystemCodeItem item)
        {
            var existingItem = await FindAsync(item.Id);

            if (existingItem == null) return null;

            var updatedEntity = item.ToUpdateEntity(existingItem);

            await _context.SaveChangesAsync();

            return updatedEntity;
        }


        public async Task DeleteAsync(SystemCodeItem item)
        {
            _context.SystemCodeItems.Remove(item);
            await _context.SaveChangesAsync();
        }


        public async Task<SystemCodeItem?> FindAsync(int id)
        {
            return await _context.SystemCodeItems.FindAsync(id);
        }

        public async Task<List<SystemCodeItem>?> GetByCodeAsync(string codeName)
        {
            return await _context.SystemCodeItems
                        .Include(cs => cs.SystemCode)
                        .Where(sc => sc.SystemCode.Code == codeName)
                        .ToListAsync();
        }

    }
}
