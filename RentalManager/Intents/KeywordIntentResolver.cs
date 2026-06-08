using System.Text.RegularExpressions;

namespace RentalManager.Intents
{
    public class KeywordIntentResolver : IIntentResolver
    {
        public Task<IntentResult> ResolveAsync(string message)
        {
            var normalized = Normalize(message);

            foreach (var entry in KeywordIntentMap.Map)
            {
                foreach (var keyword in entry.Value)
                {
                    if (normalized.Contains(keyword))
                    {
                        return Task.FromResult(
                            new IntentResult(entry.Key, 1.0f, null)
                        );
                    }
                }
            }

            return Task.FromResult(
                new IntentResult(IntentNames.Unknown, 0.0f, null)
            );
        }

        private static string Normalize(string input)
        {
            input = input.ToLowerInvariant();
            input = Regex.Replace(input, @"[^\w\s]", "");
            return input.Trim();
        }
    }
}
