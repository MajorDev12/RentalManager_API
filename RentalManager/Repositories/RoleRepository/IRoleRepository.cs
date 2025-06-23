using RentalManager.Models;

namespace RentalManager.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(int id);
        Task<Role> AddAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task<Role> DeleteAsync(Role role);
        Task<Role> FindAsync(int id);
    }
}
