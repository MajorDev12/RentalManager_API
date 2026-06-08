using RentalManager.Models;

namespace RentalManager.Repositories.SystemCodeItemRepository
{
    public interface ISystemCodeItemRepository
    {
        Task<List<SystemCodeItem>?> GetAllAsync();
        Task<SystemCodeItem?> GetByIdAsync(int id);
        Task<SystemCodeItem?> GetByCodeAndItemAsync(string item, string? code = "");
        Task<List<SystemCodeItem>?> GetByCodeAsync(string codeName);
        Task<SystemCodeItem> AddAsync(SystemCodeItem item);
        Task<SystemCodeItem> UpdateAsync(SystemCodeItem item);
        Task DeleteAsync(SystemCodeItem item);
        Task<SystemCodeItem?> FindAsync(int id);
        Task<List<SystemCodeItem>> GetByIdsAndCodeAsync(List<int> ids, string codeName);
        Task<bool> ExistsByIdAndCodeAsync(int id, string codeName);
        Task<bool> AllExistByIdsAndCodeAsync(List<int> ids, string codeName);
    }
}
