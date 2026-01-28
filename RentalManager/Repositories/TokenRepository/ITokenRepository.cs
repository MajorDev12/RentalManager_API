using RentalManager.Models;

namespace RentalManager.Repositories.TokenRepository
{
    public interface ITokenRepository
    {
        Task AddRefreshTokenAsync(RefreshToken token);

        Task UpdateRefreshTokenAsync(RefreshToken token);

        Task<RefreshToken?> FindRefreshToken(string refreshToken);

        Task RevokeAllTokensAsync(int userId, string ip);
    }
}
