
using OpenAI.Chat;
using RentalManager.Intents;
using System.ClientModel;

namespace RentalManager.AI
{
    public class OpenAIClient : IAIClient
    {
        private readonly ChatClient _client;

        public OpenAIClient(IConfiguration config)
        {
            var apiKey = config["OpenAI:ApiKey"];
            _client = new ChatClient("gpt-4.1-mini", apiKey);
        }

        //public async Task<string> CompleteAsync(string prompt)
        //{
        //    // TODO: replace with real OpenAI call
        //    // This is a stub so your system compiles and flows

        //    return """
        //    {
        //      "intent": "unknown",
        //      "confidence": 0.0,
        //      "parameters": {}
        //    }
        //    """;
        //}


        public async Task<IntentResult> CompleteAsync(string message)
        {
            try
            {
                var prompt = PromptBuilder.BuildPrompt(message);
                var response = await _client.CompleteChatAsync(
                    new ChatMessage[]
                    {
                new SystemChatMessage("You are a WhatsApp assistant."),
                new UserChatMessage(prompt)
                    });

                return ParseResult.ToIntent(response.Value.Content[0].Text);

            }
            catch (ClientResultException ex)
            {
                return new IntentResult(
                   IntentNames.Unknown,
                   0.0f,
                   null
               );
            }
        }

    }
}
