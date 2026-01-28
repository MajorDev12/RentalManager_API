using Microsoft.AspNetCore.Identity;
using RentalManager.Data;
using RentalManager.DTOs.Authentication;
using RentalManager.Models;
using RentalManager.Repositories.RoleRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.TokenRepository;
using RentalManager.Services.TokenService;
using System;

namespace RentalManager.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ITokenRepository _tokenRepository;
        private readonly ISystemCodeItemRepository _systemcodeitemrepo;
        private readonly IRoleRepository _rolerepo;

        public AuthService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            ITokenRepository tokenRepository,
            ISystemCodeItemRepository systemcodeitemrepo,
            IRoleRepository rolerepo)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
            _tokenRepository = tokenRepository;
            _systemcodeitemrepo = systemcodeitemrepo;
            _rolerepo = rolerepo;
        }


        
        public async Task<ApiResponse<bool>> Register(RegisterDto dto)
        {
            var roleString = dto.Role;

            if (!string.Equals(roleString, "owner", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(roleString, "manager", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse<bool>(false, "Role does not exist", false);
            }



            using var transaction = await _context.Database.BeginTransactionAsync();

            ApplicationUser? appUser = null;

            try
            {
                // 1. Create Account
                var newAccount = await CreateAccount();

                // 2. Create Identity user
                appUser = new ApplicationUser
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    UserName = dto.LastName,
                    Email = dto.EmailAddress,
                    AccountId = newAccount.Id,
                };

                var identityResult = await _userManager.CreateAsync(appUser, dto.Password);

                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ApiResponse<bool>(
                        false,
                        string.Join(", ", identityResult.Errors.Select(e => e.Description)),
                        false
                    );
                }

                // 3. Assign role
                var roleResult = await _userManager.AddToRoleAsync(appUser, roleString);
                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(appUser);
                    await transaction.RollbackAsync();

                    return new ApiResponse<bool>(
                        false,
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)),
                        false
                    );
                }

                // 4. Create domain user
                var userStatus = await _systemcodeitemrepo.GetByItemAsync("Active", "USERSTATUS");
                var userRole = await _rolerepo.GetByNameAsync(roleString);

                if (userStatus is null || userRole is null)
                    return null;

                var user = CreateUser(dto, appUser.Id);

                user.UserStatusId = userStatus.Id;
                user.RoleId = userRole.Id;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // 5. Commit
                await transaction.CommitAsync();

                return new ApiResponse<bool>(true, "User Registered Successfully.");
            }
            catch (Exception ex)
            {
                // HARD CLEANUP
                if (appUser != null)
                    await _userManager.DeleteAsync(appUser);

                await transaction.RollbackAsync();

                return new ApiResponse<bool>(
                    false,
                    ex.InnerException?.Message ?? ex.Message,
                    false
                );
            }
        }


        public async Task<ApiResponse<AuthResultDto>> Login(LoginDto dto, string ip)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                    return new ApiResponse<AuthResultDto>(null, "Invalid Credentials.", false);

                // verify password
                if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                    return new ApiResponse<AuthResultDto>(null, "Invalid Credentials.", false);

                var tokens = await _tokenService.GenerateTokensAsync(user, ip);

                var authResult = new AuthResultDto
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken,
                    RefreshTokenExpiry = tokens.RefreshTokenExpiry
                };

                return new ApiResponse<AuthResultDto>(authResult, "Login Successfuly");
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthResultDto>(null, $"Error occurred: {ex.InnerException?.Message ?? ex.Message}", false);
            }
        }


        public async Task<ApiResponse<object>> Logout(int userId, string ip) 
        {
            try
            {
                await _tokenRepository.RevokeAllTokensAsync(userId, ip);

                return new ApiResponse<object>(null, "logout seccessfuly");
            }
            catch(Exception ex)
            {
                return new ApiResponse<object>(null, $"Error occurred: {ex.InnerException?.Message ?? ex.Message}", false);
            }
        }


        private User CreateUser(RegisterDto user, int appUserId)
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


        private async Task<Account> CreateAccount()
        {


            var newAccount = new Account
            {
                Name = "Organization Properties",
                TrialEndsAt = DateTime.UtcNow.AddDays(14),
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            return newAccount;
        }


    }
}
