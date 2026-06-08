using RentalManager.DTOs.UtilityBill;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.UtilityBillRepository
{
    public interface IUtilityBillRepository
    {
        Task<List<UtilityBill>?> GetAllAsync(ICurrentUser user);
        Task<List<UtilityLookupDto>?> GetAllLookupsAsync();
        Task<(List<UtilityBill>, int)> GetFilteredAsync(UtilityBillQueryFilter filter);
        Task<UtilityBill?> GetByIdAsync(int id);
        Task<List<UtilityBill>?> GetByPropertyIdAsync(int propertyId, bool? isMetered = null);
        Task<List<PropertyUtilityDto>> GetAvailableUtilitiesAsync(int propertyId, bool? isMetered = null);
        Task<List<UtilityBill>> GetUtilityConfigurationsAsync(int propertyId, int utilityId);
        Task<List<UtilityBill>> GetUtilitiesByUnitAsync(int unitId, bool? isMetered = null);
        Task<UtilityBill> AddAsync(UtilityBill bill);
        Task<List<UtilityBill>> AddRangeAsync(List<UtilityBill> bills);
        Task UpdateAsync(UtilityBill bill);
        Task DeleteAsync(UtilityBill bill);
        Task<UtilityBill?> FindAsync(int id);
        Task<bool> VerifyUtilityByUnit(int unitId, int utilityId);
        Task<bool> ExistAsync(int utilityId);
    }
}
