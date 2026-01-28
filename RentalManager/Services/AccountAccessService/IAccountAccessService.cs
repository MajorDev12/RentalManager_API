using RentalManager.Models;

namespace RentalManager.Services.AccountAccessService
{
    public interface IAccountAccessService
    {
        Task<AccountAccessResult> CheckAccessAsync(int accountId);
    }

}
