using RentalManager.DTOs.Commons;
using RentalManager.DTOs.UtilityBill;

namespace RentalManager.Services.UtilityBillService
{
    public interface IUtilityBillService
    {
        Task<ApiResponse<List<READUtilityBillDto>>> GetAll();
        Task<ApiResponse<List<UtilityLookupDto>>> GetAllLookups();
        Task<ApiResponse<PagedResponse<List<READUtilityBillDto>>>> GetFiltered(UtilityBillQueryFilter filter);
        Task<ApiResponse<READUtilityBillDto>> GetById(int id);
        Task<ApiResponse<List<READUtilityBillDto>>> GetByPropertyId(int id, bool? isMetered = null);
        Task<ApiResponse<List<READUtilityBillDto>>> GetByUnitId(int unitId, bool? isMetered = null);
        Task<ApiResponse<List<READUtilityBillDto>>> GetByTenantId(int id);
        Task<ApiResponse<READUtilityBillDto>> Add(CREATEUtilityBillDto bill);
        Task<ApiResponse<READUtilityBillDto>> Patch(int id, PATCHUtilityDto updatedUtility);
        Task<ApiResponse<READUtilityBillDto>> Delete(int id);
    }
}
