using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentalManager.Data;
using RentalManager.Models;

namespace RentalManager.Repositories.MeterReadingRepository
{
    public class MeterReadingRepository : IMeterReadingRepository
    {

        private readonly ApplicationDbContext _context;

        public MeterReadingRepository(ApplicationDbContext context) { 
            _context = context;
        }


        public async Task<MeterReading> AddReadingAsync(MeterReading reading)
        {
            _context.AddAsync(reading);
            await _context.SaveChangesAsync();

            return reading;
        }

        public Task GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<MeterReading>?> GetAllPreviousReadingAsync(int propertyId, int utilityId)
        {
            throw new NotImplementedException();
        }

        public async Task<MeterReading?> GetPreviousReadingAsync(int unitId, int utilityId)
        {
            return await _context.MeterReadings
                            .Where(u => u.UtilityBillId == utilityId && u.UtilityBill.UnitId == unitId)
                            .FirstOrDefaultAsync();
        }

        public async Task<MeterReading?> GetLastReadingAsync(int unitId, int utilityBillId)
        {
            return await _context.MeterReadings
                .Where(m => m.UtilityBillId == utilityBillId &&
                            m.UtilityBill.UnitId == unitId)
                .OrderByDescending(m => m.ReadingDate)
                .FirstOrDefaultAsync();
        }
    }
}
