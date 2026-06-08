using System.Text.Json;
using RentalManager.AI;

namespace RentalManager.Intents
{
    public class IntentResolver : IIntentResolver
    {
        private readonly KeywordIntentResolver _keywordResolver;
        private readonly IAIClient _aiClient;

        public IntentResolver(IAIClient aiClient, KeywordIntentResolver keywordResolver)
        {
            _aiClient = aiClient;
            _keywordResolver = keywordResolver;
        }

        public async Task<IntentResult> ResolveAsync(string message)
        {
            var keywordResult = await _keywordResolver.ResolveAsync(message);

            if (keywordResult.Intent != IntentNames.Unknown)
                return keywordResult;

            return await _aiClient.CompleteAsync(message);  
        }

    }
}
