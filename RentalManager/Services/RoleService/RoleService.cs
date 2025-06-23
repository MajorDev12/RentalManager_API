using Microsoft.EntityFrameworkCore;
using RentalManager.DTOs.Role;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.RoleRepository;
using System.Data;

namespace RentalManager.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repo;
        private readonly IPropertyRepository _propertyRepo;
        public RoleService(IRoleRepository repo, IPropertyRepository propertyRepo)
        {
            _repo = repo;
            _propertyRepo = propertyRepo;
        }

        public async Task<ApiResponse<List<READRoleDto>>> GetAll()
        {
            try
            {
                var roles = await _repo.GetAllAsync();

                if (roles == null)
                {
                    return new ApiResponse<List<READRoleDto>>("Data Not Found.");
                }

                var roleDtos = roles.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READRoleDto>>(roleDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List< READRoleDto >> ("Error Occurred");
            }
        }

        public async Task<ApiResponse<READRoleDto>> GetById(int id)
        {
            try
            {
                var role = await _repo.GetByIdAsync(id);
                if (role == null)
                {
                    return new ApiResponse<READRoleDto>("No Such Data");
                }

                var roleDto = role.ToReadDto();

                return new ApiResponse<READRoleDto>(roleDto, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READRoleDto>("Error Occurred");
            }
        }

        public async Task<ApiResponse<READRoleDto>> Create(CREATERoleDto AddedRole)
        {
            try
            {
                var role = AddedRole.ToEntity();

                var property = await _propertyRepo.FindAsync(role.PropertyId);

                if (property == null)
                {
                    return new ApiResponse<READRoleDto>("No such Property.");
                }
                var savedRole = await _repo.AddAsync(role);
                var savedDto = savedRole.ToReadDto();

                return new ApiResponse<READRoleDto>(savedDto, "Created Successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READRoleDto>("Error Occurred.");
            }
        }

        public async Task<ApiResponse<READRoleDto>> Update(UPDATERoleDto role)
        {
            try
            {
                var updateRole = role.ToEntity();

                var saved = await _repo.UpdateAsync(updateRole);

                if (saved == null)
                    return new ApiResponse<READRoleDto>(null, "Role Not Found.");

                var savedDto = saved.ToReadDto();
                return new ApiResponse<READRoleDto>(savedDto, "Updated Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READRoleDto>("Error Occurred");
            }
        }

        public async Task<ApiResponse<READRoleDto>> Delete(int id)
        {
            try
            {
                var existing = await _repo.FindAsync(id);

                if (existing == null)
                {
                    return new ApiResponse<READRoleDto>(null, "Role Not Found.");
                }
                var roleDeleted = _repo.DeleteAsync(existing);
                return new ApiResponse<READRoleDto>(null, "Deleted Successfuly");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READRoleDto>("Error Occurred");
            }
        }

    }
}
