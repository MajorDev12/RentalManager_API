using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Repositories.SystemCodeRepository
{
    public class SystemCodeRepository : ISystemCodeRepository
    {
        private readonly ApplicationDbContext _context;

        public SystemCodeRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }

        public async Task<List<SystemCode>> GetAllAsync()
        {
            return await _context.SystemCodes.ToListAsync();
        }

        public async Task<SystemCode> GetByIdAsync(int id)
        {
            return await _context.SystemCodes.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<SystemCode> AddAsync(SystemCode code)
        {
            _context.SystemCodes.Add(code);
            await _context.SaveChangesAsync();

            return code;
        }

        public async Task<SystemCode> UpdateAsync(SystemCode code)
        {
            var existingCode = await FindAsync(code.Id);

            if (existingCode == null) return null;

            var updatedEntity = code.ToUpdateEntity(existingCode);

            await _context.SaveChangesAsync();

            return updatedEntity;
        }

        public async Task DeleteAsync(SystemCode code)
        {
            _context.SystemCodes.Remove(code);
            await _context.SaveChangesAsync();
        }

        public async Task<SystemCode?> FindAsync(int id)
        {
            return await _context.SystemCodes.FindAsync(id);
        }



    }
}
