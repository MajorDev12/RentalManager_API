using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.PropertyRepository
{
    public interface IPropertyRepository
    {
        Task<Property> AddProperty(Property property);
        Task<List<Property>?> GetAllAsync(ICurrentUser user);
        Task<Property?> GetByIdAsync(ICurrentUser user, int id);
        Task UpdateAsync(Property property);
        Task DeleteAsync(Property property);
        Task<Property?> FindAsync(ICurrentUser user, int id);
    }
}
