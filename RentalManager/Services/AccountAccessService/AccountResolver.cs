using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Models;

namespace RentalManager.Services.AccountAccessService
{
    public class AccountResolver : IAccountResolver
    {
        private readonly ApplicationDbContext _context;

        public AccountResolver(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int?> ResolveAccountIdByPhoneAsync(string phone)
        {
            return await _context.Set<ApplicationUser>()
                .IgnoreQueryFilters()
                .Where(u => u.PhoneNumber == phone)
                .Select(u => u.AccountId)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> ResolveAccountIdByPasswordAsync(string password)
        {
            return await _context.Set<ApplicationUser>()
                .IgnoreQueryFilters()
                .Where(u => u.PasswordHash == password)
                .Select(u => u.AccountId)
                .FirstOrDefaultAsync();
        }
    }

}
