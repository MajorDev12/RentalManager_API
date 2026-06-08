using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Authentication;
using RentalManager.DTOs.Tenant;
using RentalManager.DTOs.UnitType;
using RentalManager.DTOs.User;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.RoleRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.TenantRepository;
using RentalManager.Repositories.UserRepository;
using RentalManager.Services.AccountAccessService;
using static RentalManager.Authorization.Policies.PolicyNames;

namespace RentalManager.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _repo;
        private readonly ITenantRepository _tenantrepo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly IRoleRepository _rolerepo;
        private readonly ISystemCodeItemRepository _systemcoderepo;
        private readonly ICurrentUser _currentuser;

        public UserService
            (
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IUserRepository repo,
            ITenantRepository tenantrepo,
            IPropertyRepository propertyrepo,
            IRoleRepository rolerepo,
            ISystemCodeItemRepository systemcoderepo,
            ICurrentUser currentuser
            )
        {
            _context = context;
            _userManager = userManager;
            _repo = repo;
            _tenantrepo = tenantrepo;
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
                    return new ApiResponse<List<READUserDto>>(null, "Items Not Found.");
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
                    return new ApiResponse<READUserDto>(null, "Items Not Found.");
                }

                var userDtos = user.ToReadDto();

                return new ApiResponse<READUserDto>(userDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUserDto>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READAppUserDto>> GetByApplicationUserId(int id)
        {
            try
            {
                var user = await _repo.GetByApplicationUserIdAsync(id);

                if (user == null)
                {
                    return ApiResponse<READAppUserDto>.FailResponse("Items Not Found.");
                }

                var userDtos = user.ToReadDto();

                return ApiResponse<READAppUserDto>.SuccessResponse(userDtos, "");
            }
            catch (Exception ex)
            {
                return ApiResponse<READAppUserDto>.FailResponse("Error Occurred");
            }
        }


        public async Task<ApiResponse<READUserDto>> Add(CREATEUserDto dto)
        {
            ApplicationUser? appUser = null;

            try
            {
                // Validate related entities
                var property = await _propertyrepo.FindAsync(dto.PropertyId);
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
                userEntity.UserStatusId = status.Id;
                userEntity.AccountId = _currentuser.AccountId;

                await _repo.AddAsync(userEntity); // Save domain user

                return ApiResponse<READUserDto>.SuccessResponse(userEntity.ToReadDto(), "User created successfully");
            }
            catch (Exception ex)
            {
                if (appUser != null) await _userManager.DeleteAsync(appUser);
                return ApiResponse<READUserDto>.FailResponse($"Error occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }




        public async Task<ApiResponse<READUserDto>> Update(int appUserId, UPDATEUserDto userDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var appUser = await _repo.GetByApplicationUserIdAsync(appUserId);

                if (appUser == null || appUser.User == null) return ApiResponse<READUserDto>.FailResponse("User Not Found.");


                // Update Identity user
                await UpdateApplicationUser(appUser, userDto);

                // Update Domain user
                var updatedUser = await UpdateDomainUser(appUser.User, userDto);

                var role = await _rolerepo.GetByIdAsync(appUser.User.Id);

                if (role == null) return ApiResponse<READUserDto>.FailResponse("User Role doesnt exist");

                // Update related table
                await UpdateRoleSpecificUser(appUser.User.Id, userDto, role.Name);

                await transaction.CommitAsync();


                return ApiResponse<READUserDto>.SuccessResponse(
                    updatedUser.ToReadDto(),
                    "Updated successfully."
                );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<READUserDto>.FailResponse(ex.Message);
            }
        }


        public async Task<ApiResponse<READUserDto>> Delete(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(id);

                if (entity == null)
                    return new ApiResponse<READUserDto>("Items Not Found.");

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


        private async Task<User> UpdateDomainUser(User user, UPDATEUserDto dto)
        {
            if (dto.GenderId > 0)
            {
                var gender = await _systemcoderepo.FindAsync(dto.GenderId);
                if (gender == null)
                    throw new Exception("Invalid gender.");

                user.GenderId = dto.GenderId;
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.MobileNumber = dto.MobileNumber;

            await _repo.UpdateAsync(user);
            return user;
        }


        private async Task UpdateApplicationUser(ApplicationUser appUser, UPDATEUserDto dto)
        {
            appUser.Email = dto.EmailAddress;
            appUser.FirstName = dto.FirstName;
            appUser.LastName = dto.LastName;
            appUser.UserName = dto.UserName;

            var result = await _userManager.UpdateAsync(appUser);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

        }



        private async Task UpdateRoleSpecificUser(int userId, UPDATEUserDto dto, string role)
        {
            if (role == "Tenant")
            {
                var tenant = await _tenantrepo.GetByUserIdAsync(userId);
                if (tenant == null)
                    throw new Exception("Tenant record not found.");

                await _tenantrepo.UpdateAsync();
            }
        }


    }
}
