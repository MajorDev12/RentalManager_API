using RentalManager.DTOs.Unit;
using RentalManager.DTOs.UnitType;
using RentalManager.Models;

namespace RentalManager.Services.UnitService
{
    public interface IUnitService
    {
        Task<ApiResponse<List<READUnitDto>>> GetAll();
        Task<ApiResponse<READUnitDto>> GetById(int id);
        Task<ApiResponse<List<READUnitDto>>> GetByPropertyId(int id);
        Task<ApiResponse<READUnitDto>> Add(CREATEUnitDto unit);
        Task<ApiResponse<READUnitDto>> Update(int id, UPDATEUnitDto unit);
        Task<ApiResponse<READUnitDto>> Delete(int id);
    }
}
