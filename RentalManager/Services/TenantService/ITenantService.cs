using RentalManager.DTOs.Tenant;
using RentalManager.Models;

namespace RentalManager.Services
{
    public interface ITenantService
    {
        Task<ApiResponse<List<READTenantDto>>> GetAll();
        Task<ApiResponse<READTenantDto>> GetById(int id);
        Task<ApiResponse<READTenantDto>> Add(CREATETenantDto tenant);
        Task<ApiResponse<READTenantDto>> Update(int id, UPDATETenantDto tenant);
        Task<ApiResponse<READTenantDto>> Delete(int id);
        Task<ApiResponse<READTenantDto>> AssignUnit(ASSIGNUnitDto unitAssigned);
    }
}
