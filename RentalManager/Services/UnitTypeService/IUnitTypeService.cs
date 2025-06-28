using RentalManager.DTOs.SystemCode;
using RentalManager.DTOs.UnitType;
using RentalManager.Models;

namespace RentalManager.Services.UnitTypeService
{
    public interface IUnitTypeService
    {
        Task<ApiResponse<List<READUnitTypeDto>>> GetAll();
        Task<ApiResponse<READUnitTypeDto>> GetById(int id);
        Task<ApiResponse<List<READUnitTypeDto>>> GetByPropertyId(int id);
        Task<ApiResponse<READUnitTypeDto>> Add(CREATEUnitTypeDto type);
        Task<ApiResponse<READUnitTypeDto>> Update(int id, UPDATEUnitTypeDto type);
        Task<ApiResponse<READUnitTypeDto>> Delete(int id);
    }
}
