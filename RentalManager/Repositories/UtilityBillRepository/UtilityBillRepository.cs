using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Mappings;
using RentalManager.Models;

namespace RentalManager.Repositories.UtilityBillRepository
{
    public class UtilityBillRepository : IUtilityBillRepository
    {

        private readonly ApplicationDbContext _context;

        public UtilityBillRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<UtilityBill>?> GetAllAsync()
        {
            return await _context.UnitCharges
                .Include(ub => ub.Property)
                .ToListAsync();
        }


        public async Task<UtilityBill?> GetByIdAsync(int id)
        {
            return await _context.UnitCharges
                .Include(c => c.Property)
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }


        public async Task<List<UtilityBill>?> GetByPropertyIdAsync(int id, bool? isReccurring)
        {
            var query = _context.UnitCharges
                        .Include(u => u.Property)
                        .Where(u => u.PropertyId == id);

            if (isReccurring == true)
                query = query.Where(u => u.isReccuring == isReccurring);

            return await query.ToListAsync();
        }



        public async Task<UtilityBill> AddAsync(UtilityBill bill)
        {
            _context.UnitCharges.Add(bill);
            await _context.SaveChangesAsync();

            return bill;
        }


        public async Task<UtilityBill> UpdateAsync(UtilityBill bill)
        {
            var existingbill = await FindAsync(bill.Id);

            if (existingbill == null) return null;

            var updatedEntity = bill.UpdateEntity(existingbill);

            await _context.SaveChangesAsync();

            return updatedEntity;
        }


        public async Task DeleteAsync(UtilityBill bill)
        {
            _context.UnitCharges.Remove(bill);
            await _context.SaveChangesAsync();
        }


        public async Task<UtilityBill?> FindAsync(int id)
        {
            return await _context.UnitCharges.FindAsync(id);
        }



    }
}
