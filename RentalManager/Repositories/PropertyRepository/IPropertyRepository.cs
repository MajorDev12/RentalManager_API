using RentalManager.Models;

namespace RentalManager.Repositories.PropertyRepository
{
    public interface IPropertyRepository
    {
        Task<Property> AddProperty(Property property);
        Task<List<Property>> GetAllAsync();
        Task<Property> GetByIdAsync(int id);
        Task<Property> UpdateAsync(Property property);
        Task DeleteAsync(Property property);
        Task<Property> FindAsync(int id);
    }
}
