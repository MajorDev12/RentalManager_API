using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.UnitTypeRepository
{
    public interface IUnitTypeRepository
    {
        Task<List<UnitType>?> GetAllAsync(ICurrentUser user);
        Task<UnitType?> GetByIdAsync(ICurrentUser user, int id);
        Task<List<UnitType>?> GetByPropertyIdAsync(ICurrentUser user, int id);
        Task<UnitType> AddAsync(UnitType type);
        Task UpdateAsync(UnitType type);
        Task DeleteAsync(UnitType type);
        Task<UnitType?> FindAsync(ICurrentUser user, int id);
    }
}
