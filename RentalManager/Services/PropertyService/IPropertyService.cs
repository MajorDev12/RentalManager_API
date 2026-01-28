using RentalManager.DTOs.Property;
using RentalManager.Models;

namespace RentalManager.Services.PropertyService
{
    public interface IPropertyService
    {
        Task<ApiResponse<READPropertyDto>> Create(CREATEPropertyDto dto, int accountId);
        Task<ApiResponse<List<READPropertyDto>>> GetAll();
        Task<ApiResponse<READPropertyDto>> GetById(int id);
        Task<ApiResponse<READPropertyDto>> Update(int id, UPDATEPropertyDto dto);
        Task<ApiResponse<READPropertyDto>> Delete(int id);
    }
}
