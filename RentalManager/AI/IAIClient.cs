using RentalManager.Intents;

namespace RentalManager.AI
{
    public interface IAIClient
    {
        Task<IntentResult> CompleteAsync(string prompt);
    }
}
