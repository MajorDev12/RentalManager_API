using RentalManager.Authorization;
using RentalManager.Conversation;
using RentalManager.WhatsApp;

namespace RentalManager.Intents.Handlers
{
    public class GreetingIntentHandler : IIntentHandler
    {
        public string Intent => IntentNames.Greeting;

        public Task<string> HandleAsync(
            IntentResult intent,
            WhatsAppContext context,
            ConversationState state)
        {
            return Task.FromResult(
                $"Hi {context.UserName} 👋\nType *help* to see what I can do."
            );
        }
    }
}
