using RentalManager.DTOs.SystemCode;
using RentalManager.Models;

namespace RentalManager.Services.SystemCodeService
{
    public interface ISystemCodeService
    {
        Task<ApiResponse<List<READSystemCodeDto>>> GetAll();
        Task<ApiResponse<READSystemCodeDto>> GetById(int id);
        Task<ApiResponse<READSystemCodeDto>> Add(CREATESystemCodeDto code);
        Task<ApiResponse<READSystemCodeDto>> Update(int id, UPDATESystemCodeDto code);
        Task<ApiResponse<READSystemCodeDto>> Delete(int id);
    }
}
