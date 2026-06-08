namespace RentalManager.Services.AccountAccessService
{
    public interface IAccountContext
    {
        int AccountId { get; }

        void SetAccount(int accountId);
    }
}
