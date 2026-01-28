using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.UnitRepository
{
    public interface IUnitRepository
    {
        Task<List<Unit>?> GetAllAsync(ICurrentUser user);
        Task<Unit?> GetByIdAsync(ICurrentUser user, int id);
        Task<List<Unit>?> GetByPropertyIdAsync(ICurrentUser user, int id);
        Task<Unit> AddAsync(Unit unit);
        Task UpdateAsync(Unit unit);
        Task<Unit> UpdateStatus(ICurrentUser user, int unitId, int statusId);
        Task DeleteAsync(Unit unit);
        Task<Unit?> FindAsync(ICurrentUser user, int id);
    }
}
