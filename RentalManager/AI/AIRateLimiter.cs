namespace RentalManager.AI
{
    public interface AIRateLimiter
    {
        Task<bool> AllowAsync(string phone);
    }

}
