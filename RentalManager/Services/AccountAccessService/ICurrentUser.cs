namespace RentalManager.Services.AccountAccessService
{
    public interface ICurrentUser
    {
        int UserId { get; }
        int AccountId { get; }
        string Role { get; }
        bool IsInRole(string role);
    }
}
