using RentalManager.Models;

namespace RentalManager.Repositories.UnitTypeRepository
{
    public interface IUnitTypeRepository
    {
        Task<List<UnitType>?> GetAllAsync();
        Task<UnitType?> GetByIdAsync(int id);
        Task<List<UnitType>?> GetByPropertyIdAsync(int id);
        Task<UnitType> AddAsync(UnitType type);
        Task<UnitType> UpdateAsync(UnitType type);
        Task DeleteAsync(UnitType type);
        Task<UnitType?> FindAsync(int id);
    }
}
