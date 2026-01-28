using RentalManager.DTOs.Authentication;
using RentalManager.Models;

namespace RentalManager.Services.AuthService
{
    public interface IAuthService
    {
        Task<ApiResponse<bool>> Register(RegisterDto dto);
        Task<ApiResponse<AuthResultDto>> Login(LoginDto dto, string ip);
        Task<ApiResponse<object>> Logout(int userId, string ip);


    }
}
