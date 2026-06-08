using RentalManager.DTOs.Commons;
using RentalManager.DTOs.Unit;
using RentalManager.Models;

namespace RentalManager.Services.UnitService
{
    public interface IUnitService
    {
        Task<ApiResponse<List<READUnitDto>>> GetAll();
        Task<ApiResponse<List<READUnitLookupDto>>> GetLookups();
        Task<ApiResponse<PagedResponse<List<READUnitDto>>>> GetFiltered(UnitQueryFilter filter);
        Task<ApiResponse<READUnitDto>> GetById(int id);
        Task<ApiResponse<List<READUnitDto>>> GetByPropertyId(int id);
        Task<ApiResponse<List<READUnitDto>>> GetVacants();
        Task<ApiResponse<READUnitDto>> Add(CREATEUnitDto unit);
        Task<ApiResponse<READUnitDto>> Update(int id, UPDATEUnitDto unit);
        Task<ApiResponse<READUnitDto>> Patch(int id, PATCHUnitDto dto);
        Task<ApiResponse<READUnitDto>> Delete(int id);
    }
}
