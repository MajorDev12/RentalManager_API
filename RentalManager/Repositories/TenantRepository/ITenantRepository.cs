using RentalManager.Models;

namespace RentalManager.Repositories.TenantRepository
{
    public interface ITenantRepository
    {
        Task<List<Tenant>?> GetAllAsync();
        Task<Tenant?> GetByIdAsync(int id);
        Task<Tenant> AddAsync(Tenant tenant);
        Task<Tenant> UpdateAsync(Tenant tenant, User user);
        Task DeleteAsync(Tenant tenant);
        Task<Tenant?> FindAsync(int id);
        Task<Tenant?> AssignUnitAsync(Tenant tenant);
    }
}
