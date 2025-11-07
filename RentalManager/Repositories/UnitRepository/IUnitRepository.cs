using RentalManager.Models;

namespace RentalManager.Repositories.UnitRepository
{
    public interface IUnitRepository
    {
        Task<List<Unit>?> GetAllAsync();
        Task<Unit?> GetByIdAsync(int id);
        Task<List<Unit>?> GetByPropertyIdAsync(int id);
        Task<Unit> AddAsync(Unit unit);
        Task<Unit> UpdateAsync(Unit unit);
        Task<Unit> UpdateStatus(int unitId, int statusId);
        Task DeleteAsync(Unit unit);
        Task<Unit?> FindAsync(int id);
    }
}
