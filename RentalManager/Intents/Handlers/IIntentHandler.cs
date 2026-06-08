using RentalManager.Conversation;
using RentalManager.WhatsApp;

namespace RentalManager.Intents.Handlers
{
    public interface IIntentHandler
    {
        string Intent { get; }

        Task<string> HandleAsync(
            IntentResult intent,
            WhatsAppContext context,
            ConversationState state
        );
    }
}
