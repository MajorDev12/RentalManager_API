using RentalManager.Models;

namespace RentalManager.Repositories.SystemCodeItemRepository
{
    public interface ISystemCodeItemRepository
    {
        Task<List<SystemCodeItem>?> GetAllAsync();
        Task<SystemCodeItem?> GetByIdAsync(int id);
        Task<List<SystemCodeItem>?> GetByCodeAsync(string codeName);
        Task<SystemCodeItem> AddAsync(SystemCodeItem item);
        Task<SystemCodeItem> UpdateAsync(SystemCodeItem item);
        Task DeleteAsync(SystemCodeItem item);
        Task<SystemCodeItem?> FindAsync(int id);

    }
}
