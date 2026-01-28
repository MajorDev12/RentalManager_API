using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.QueryExtensions;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.UtilityBillRepository
{
    public class UtilityBillRepository : IUtilityBillRepository
    {

        private readonly ApplicationDbContext _context;

        public UtilityBillRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<UtilityBill>?> GetAllAsync(ICurrentUser user)
        {
            return await _context.UnitCharges
                .ApplyRoleFilter(user, _context)
                .Include(p => p.Property)
                .ToListAsync();
        }


        public async Task<UtilityBill?> GetByIdAsync(ICurrentUser user, int id)
        {
            return await _context.UnitCharges
                .Where(u => u.Id == id)
                .ApplyRoleFilter(user, _context)
                .Include(c => c.Property)
                .FirstOrDefaultAsync();
        }


        public async Task<List<UtilityBill>?> GetByPropertyIdAsync(ICurrentUser user, int id, bool? isReccurring)
        {
            IQueryable<UtilityBill> query = _context.UnitCharges
                        .Where(u => u.PropertyId == id)
                        .ApplyRoleFilter(user, _context);

            if (isReccurring == true)
                query = query.Where(u => u.isReccuring == isReccurring);

            return await query
                .Include(u => u.Property)
                .ToListAsync();
        }



        public async Task<UtilityBill> AddAsync(UtilityBill bill)
        {
            _context.UnitCharges.Add(bill);
            await _context.SaveChangesAsync();

            return bill;
        }


        public async Task UpdateAsync(UtilityBill bill)
        {
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(UtilityBill bill)
        {
            _context.UnitCharges.Remove(bill);
            await _context.SaveChangesAsync();
        }


        public async Task<UtilityBill?> FindAsync(ICurrentUser user, int id)
        {
            return await _context.UnitCharges
                .Where(u => u.Id == id)
                .ApplyRoleFilter(user, _context)
                .FirstOrDefaultAsync();
        }



    }
}
