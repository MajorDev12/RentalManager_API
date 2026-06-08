namespace RentalManager.Services.AccountAccessService
{
    public class AccountContext : IAccountContext
    {
        public int AccountId {  get; private set; }

        public void SetAccount(int accountId)
        {
            AccountId = accountId;
        }
    }
}
