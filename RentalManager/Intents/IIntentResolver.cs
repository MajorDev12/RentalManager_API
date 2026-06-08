namespace RentalManager.Intents
{
    public interface IIntentResolver
    {
        Task<IntentResult> ResolveAsync(string message);
    }

}
