using Microsoft.EntityFrameworkCore;
using RentalManager.DTOs.UnitType;
using RentalManager.DTOs.User;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.RoleRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.UserRepository;

namespace RentalManager.Services.UserService
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly IRoleRepository _rolerepo;
        private readonly ISystemCodeItemRepository _systemcoderepo;

        public UserService
            (
            IUserRepository repo,
            IPropertyRepository propertyrepo,
            IRoleRepository rolerepo,
            ISystemCodeItemRepository systemcoderepo
            )
        {
            _repo = repo;
            _propertyrepo = propertyrepo;
            _rolerepo = rolerepo;
            _systemcoderepo = systemcoderepo;
        }

        public async Task<ApiResponse<List<READUserDto>>> GetAll()
        {
            try
            {
                var users = await _repo.GetAllAsync();

                if (users == null || users.Count == 0)
                {
                    return new ApiResponse<List<READUserDto>>(null, "Data Not Found.");
                }

                var userDtos = users.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUserDto>>(userDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUserDto>>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READUserDto>> GetById(int id)
        {
            try
            {
                var user = await _repo.GetByIdAsync(id);

                if (user == null)
                {
                    return new ApiResponse<READUserDto>(null, "Data Not Found.");
                }

                var userDtos = user.ToReadDto();

                return new ApiResponse<READUserDto>(userDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUserDto>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READUserDto>> Add(CREATEUserDto user)
        {
            try
            {
                var property = await _propertyrepo.FindAsync(user.PropertyId);
                var role = await _rolerepo.FindAsync(user.RoleId);
                var gender = await _systemcoderepo.FindAsync(user.GenderId);
                var status = await _systemcoderepo.FindAsync(user.UserStatusId);


                if (status == null || property == null || role == null || gender == null)
                {
                    return new ApiResponse<READUserDto>(null, "One of the items provided does not exist: status, property, role, gender.");
                }


                var entity = user.ToEntity();
                var addedUser = await _repo.AddAsync(entity);

                if (addedUser == null)
                {
                    return new ApiResponse<READUserDto>(null, "Data Not Found.");
                }


                return new ApiResponse<READUserDto>(null, "User Created Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUserDto>($"Error Occurred: {ex.InnerException.Message} ");
            }
        }


        public async Task<ApiResponse<READUserDto>> Update(int id, UPDATEUserDto user)
        {
            try
            {

                var existing = await _repo.FindAsync(id);

                if (existing == null) return new ApiResponse<READUserDto>(null, "No Such Data.");


                var gender = await _systemcoderepo.FindAsync(user.GenderId);
                var status = await _systemcoderepo.FindAsync(user.UserStatusId);


                if (status == null || gender == null)
                {
                    return new ApiResponse<READUserDto>(null, "One of the items provided does not exist: status, property, role, gender.");
                }

                var entity = user.ToEntity(id);
                var updated = await _repo.UpdateAsync(entity);

                if (updated == null)
                    return new ApiResponse<READUserDto>(null, "Data Not Found.");

                return new ApiResponse<READUserDto>(null, "Updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUserDto>("Error Occurred.");
            }
        }


        public async Task<ApiResponse<READUserDto>> Delete(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(id);

                if (entity == null)
                    return new ApiResponse<READUserDto>("Data Not Found.");

                await _repo.DeleteAsync(entity);

                return new ApiResponse<READUserDto>(null, "Deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUserDto>($"Error Occurred: {ex.InnerException.Message}");
            }
        }

    }
}
