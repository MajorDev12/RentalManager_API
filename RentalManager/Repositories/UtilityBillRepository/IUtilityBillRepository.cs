using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.UtilityBillRepository
{
    public interface IUtilityBillRepository
    {
        Task<List<UtilityBill>?> GetAllAsync(ICurrentUser user);
        Task<UtilityBill?> GetByIdAsync(ICurrentUser user, int id);
        Task<List<UtilityBill>?> GetByPropertyIdAsync(ICurrentUser user, int id, bool? isReccurring = false);
        Task<UtilityBill> AddAsync(UtilityBill bill);
        Task UpdateAsync(UtilityBill bill);
        Task DeleteAsync(UtilityBill bill);
        Task<UtilityBill?> FindAsync(ICurrentUser user, int id);
    }
}
