using RentalManager.DTOs.Property;
using RentalManager.Models;

namespace RentalManager.Services.PropertyService
{
    public interface IPropertyService
    {
        Task<ApiResponse<READPropertyDto>> Create(CREATEPropertyDto dto);
        Task<ApiResponse<List<READPropertyDto>>> GetAll();
        Task<ApiResponse<READPropertyDto>> GetById(int id);
        Task<ApiResponse<READPropertyDto>> Update(UPDATEPropertyDto dto);
        Task<ApiResponse<READPropertyDto>> Delete(int id);
    }
}
