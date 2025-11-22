using RentalManager.Models;

namespace RentalManager.Repositories.UtilityBillRepository
{
    public interface IUtilityBillRepository
    {
        Task<List<UtilityBill>?> GetAllAsync();
        Task<UtilityBill?> GetByIdAsync(int id);
        Task<List<UtilityBill>?> GetByPropertyIdAsync(int id, bool? isReccurring = false);
        Task<UtilityBill> AddAsync(UtilityBill bill);
        Task<UtilityBill> UpdateAsync(UtilityBill bill);
        Task DeleteAsync(UtilityBill bill);
        Task<UtilityBill?> FindAsync(int id);
    }
}
