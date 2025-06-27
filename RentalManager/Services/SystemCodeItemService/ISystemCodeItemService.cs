using RentalManager.DTOs.SystemCode;
using RentalManager.DTOs.SystemCodeItem;
using RentalManager.Models;

namespace RentalManager.Services.SystemCodeItemService
{
    public interface ISystemCodeItemService
    {
        Task<ApiResponse<List<READSystemCodeItemDto>>> GetAll();
        Task<ApiResponse<READSystemCodeItemDto>> GetById(int id);
        Task<ApiResponse<List<READSystemCodeItemDto>>> GetByCode(string codeName);
        Task<ApiResponse<READSystemCodeItemDto>> Add(CREATESystemCodeItemDto item);
        Task<ApiResponse<READSystemCodeItemDto>> Update(int id, UPDATESystemCodeItemDto item);
        Task<ApiResponse<READSystemCodeItemDto>> Delete(int id);
    }
}
