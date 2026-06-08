using RentalManager.Data;
using RentalManager.Models;

namespace RentalManager.Services.AccountAccessService
{
    public class AccountAccessService : IAccountAccessService
    {
        private readonly ApplicationDbContext _context;

        public AccountAccessService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AccountAccessResult> CheckAccessAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);

            if (account == null || account.Id <= 0)
                return Denied("Account not found");

            if (!account.IsActive)
                return Denied("Account disabled");

            if (account.IsTrial)
            {
                if (DateTime.UtcNow > account.TrialEndsAt)
                    return Denied("Trial expired");

                return Allowed();
            }

            if (!account.SubscriptionEndsAt.HasValue ||
                account.SubscriptionEndsAt < DateTime.UtcNow)
                return Denied("Subscription expired");

            return Allowed();
        }

        private static AccountAccessResult Allowed()
            => new() { Allowed = true };

        private static AccountAccessResult Denied(string reason)
            => new() { Allowed = false, Reason = reason };
    }

}
