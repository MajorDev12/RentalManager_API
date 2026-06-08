using RentalManager.DTOs.Unit;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.UnitRepository
{
    public interface IUnitRepository
    {
        Task<List<Unit>?> GetAllAsync();
        Task<List<READUnitLookupDto>?> GetLookupsAsync();
        Task<(List<Unit>, int)> GetFilteredAsync(UnitQueryFilter filter);
        Task<Unit?> GetByIdAsync(int id);
        Task<List<Unit>?> GetByPropertyIdAsync(int id);
        Task<Unit> AddAsync(Unit unit);
        Task<List<Unit>?> GetVacantsAsync();
        Task UpdateAsync(Unit unit);
        Task<Unit?> UpdateStatus(int unitId, int statusId);
        Task DeleteAsync(Unit unit);
        Task<Unit?> FindAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsInPropertyAsync(int unitId, int propertyId);
    }
}
