using RentalManager.DTOs.Property;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.PropertyRepository
{
    public interface IPropertyRepository
    {
        Task<Property> AddProperty(Property property);
        Task<List<Property>?> GetAllAsync();
        Task<(List<Property>, int)> GetFilteredAsync(PropertyQueryFilter filter);
        Task<List<READPropertyLookupDto>> GetAllLookupAsync();
        Task<Property?> GetByIdAsync(int id);
        Task UpdateAsync(Property property);
        Task DeleteAsync(Property property);
        Task<Property?> FindAsync(int id);
        Task<bool> ExistAsync(int id);
    }
}
