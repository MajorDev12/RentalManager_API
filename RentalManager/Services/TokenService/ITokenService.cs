using RentalManager.DTOs.Authentication;
using RentalManager.Models;
using System.Security.Claims;

namespace RentalManager.Services.TokenService
{
    public interface ITokenService
    {
        Task<AuthResultDto> GenerateTokensAsync(ApplicationUser user, string ipAddress);
        Task<string> CreateAccessTokenAsync(ApplicationUser user);
        ClaimsPrincipal? ValidateToken(string token); // optional helper

        Task<ApiResponse<AuthResultDto>> RefreshTokenAsync(string token, string ip);
    }
}
