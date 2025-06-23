using RentalManager.DTOs.Role;
using RentalManager.Models;

namespace RentalManager.Services.RoleService
{
    public interface IRoleService
    {
        Task<ApiResponse<List<READRoleDto>>> GetAll();
        Task<ApiResponse<READRoleDto>> GetById(int id);
        Task<ApiResponse<READRoleDto>> Create(CREATERoleDto role);
        Task<ApiResponse<READRoleDto>> Update(int id, UPDATERoleDto role);
        Task<ApiResponse<READRoleDto>> Delete(int id);
    }
}
