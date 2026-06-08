using RentalManager.Models;

namespace RentalManager.Repositories.MeterReadingRepository
{
    public interface IMeterReadingRepository
    {
        Task GetAllAsync();
        Task<MeterReading?> GetPreviousReadingAsync(int unitId, int utilityId);
        Task<List<MeterReading>?> GetAllPreviousReadingAsync(int propertyId, int utilityId);
        Task<MeterReading> AddReadingAsync(MeterReading reading);
        Task<MeterReading?> GetLastReadingAsync(int UnitId, int UtilityBillId);
    }
}
