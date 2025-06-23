using RentalManager.Models;

namespace RentalManager.Repositories.SystemCodeRepository
{
    public interface ISystemCodeRepository
    {
        Task<List<SystemCode>> GetAllAsync();
        Task<SystemCode> GetByIdAsync(int id);
        Task<SystemCode> AddAsync(SystemCode code);
        Task<SystemCode> UpdateAsync(SystemCode code);
        Task DeleteAsync(SystemCode code);
        Task<SystemCode?> FindAsync(int id);
    }
}
