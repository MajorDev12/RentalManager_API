using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Models;
using System.Security;

namespace RentalManager.Repositories.TokenRepository
{
    public class TokenRepository : ITokenRepository
    {

        private readonly ApplicationDbContext _context;

        public TokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }




        public async Task AddRefreshTokenAsync(RefreshToken token)
        {
            var activeTokens = await _context.RefreshTokens
                .CountAsync(r => r.UserId == token.UserId && !r.Revoked);

            if (activeTokens >= 5)
                await RevokeAllTokensAsync(token.UserId, "unkown");


            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

        }



        public async Task UpdateRefreshTokenAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }



        public async Task<RefreshToken?> FindRefreshToken(string refreshToken)
        {
            var token = await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

            return token;
        }



        public async Task RevokeAllTokensAsync(int userId, string ip)
        {
            var tokens = await _context.RefreshTokens
                .Where(r => r.UserId == userId && !r.Revoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.Revoked = true;
                token.RevokedOn = DateTime.UtcNow;
                token.RevokedByIp = ip;
            }

            await _context.SaveChangesAsync();
        }


    }
}
