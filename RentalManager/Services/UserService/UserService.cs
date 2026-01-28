using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Authentication;
using RentalManager.DTOs.UnitType;
using RentalManager.DTOs.User;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.RoleRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.UserRepository;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _repo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly IRoleRepository _rolerepo;
        private readonly ISystemCodeItemRepository _systemcoderepo;
        private readonly ICurrentUser _currentuser;

        public UserService
            (
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IUserRepository repo,
            IPropertyRepository propertyrepo,
            IRoleRepository rolerepo,
            ISystemCodeItemRepository systemcoderepo,
            ICurrentUser currentuser
            )
        {
            _context = context;
            _userManager = userManager;
            _repo = repo;
            _propertyrepo = propertyrepo;
            _rolerepo = rolerepo;
            _systemcoderepo = systemcoderepo;
            _currentuser = currentuser;
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


        public async Task<ApiResponse<READUserDto>> Add(CREATEUserDto dto)
        {
            ApplicationUser? appUser = null;

            try
            {
                // Validate related entities
                var property = await _propertyrepo.FindAsync(_currentuser, dto.PropertyId);
                var role = await _rolerepo.FindAsync(dto.RoleId);
                var gender = await _systemcoderepo.FindAsync(dto.GenderId);
                var status = await _systemcoderepo.FindAsync(dto.UserStatusId);

                if (property == null || role == null || gender == null || status == null)
                    return ApiResponse<READUserDto>.FailResponse("Invalid property, role, gender, or status.");

                // Create Identity user
                appUser = new ApplicationUser
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    UserName = dto.LastName,
                    Email = dto.EmailAddress,
                    AccountId = _currentuser.AccountId
                };

                var identityResult = await _userManager.CreateAsync(appUser, dto.Password);
                if (!identityResult.Succeeded)
                    return ApiResponse<READUserDto>.FailResponse(string.Join(", ", identityResult.Errors.Select(e => e.Description)));

                // Assign role
                var roleResult = await _userManager.AddToRoleAsync(appUser, role.Name);
                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(appUser);
                    return ApiResponse<READUserDto>.FailResponse(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }

                // Create domain user
                var userEntity = dto.ToEntity();
                userEntity.ApplicationUserId = appUser.Id;
                userEntity.RoleId = role.Id;
                userEntity.UserStatusId = status.Id;

                await _repo.AddAsync(userEntity); // Save domain user

                return ApiResponse<READUserDto>.SuccessResponse(userEntity.ToReadDto(), "User created successfully");
            }
            catch (Exception ex)
            {
                if (appUser != null) await _userManager.DeleteAsync(appUser);
                return ApiResponse<READUserDto>.FailResponse($"Error occurred: {ex.InnerException?.Message ?? ex.Message}");
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

                var entity = user.ToEntity();
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


        private User CreateUser(CREATEUserDto user, int appUserId)
        {

            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                MobileNumber = user.MobileNumber,
                ApplicationUserId = appUserId
            };

            return newUser;
        }

    }
}
