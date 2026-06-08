using RentalManager.Authorization;
using RentalManager.Conversation;
using RentalManager.Models;
using RentalManager.WhatsApp;

namespace RentalManager.Intents.Handlers
{
    public class HelpIntentHandler : IIntentHandler
    {
        public string Intent => IntentNames.Help;

        public Task<string> HandleAsync(
            IntentResult intent,
            WhatsAppContext context,
            ConversationState state)
        {
            var message = BuildHelp(context.Role);
            return Task.FromResult(message);
        }


        public string BuildHelp(string role)
        {

            if (!RoleIntentMap.Map.TryGetValue(role, out var intents))
                return "No actions available.";

            var message =
                "Here’s what you can do:\n\n" +
                string.Join("\n", intents.Select(i => $"• {i.Replace("_", " ")}"));

            return message;
        }

    }
}
