namespace RentalManager.Services.AccountAccessService
{
    public interface IAccountResolver
    {
        Task<int?> ResolveAccountIdByPhoneAsync(string phone);
        Task<int?> ResolveAccountIdByPasswordAsync(string password);
    }
}
