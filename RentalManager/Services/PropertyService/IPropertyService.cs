using RentalManager.DTOs.Commons;
using RentalManager.DTOs.Property;
using RentalManager.Models;

namespace RentalManager.Services.PropertyService
{
    public interface IPropertyService
    {
        Task<ApiResponse<READPropertyDto>> Create(CREATEPropertyDto dto);
        Task<ApiResponse<List<READPropertyDto>?>> GetAll();
        Task<ApiResponse<List<READPropertyLookupDto>?>> GetAllLookups();
        Task<ApiResponse<PagedResponse<List<READPropertyDto>>>> GetFiltered(PropertyQueryFilter filter);
        Task<ApiResponse<READPropertyDto?>> GetById(int id);
        Task<ApiResponse<READPropertyDto>> Update(int id, PATCHPropertyDto dto);
        Task<ApiResponse<READPropertyDto>> Delete(int id);
    }
}
