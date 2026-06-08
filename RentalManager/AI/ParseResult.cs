using RentalManager.Intents;
using System.Text.Json;

namespace RentalManager.AI
{
    public static class ParseResult
    {

        public static IntentResult ToIntent(string raw)
        {
            try
            {
                using var doc = JsonDocument.Parse(raw);
                var root = doc.RootElement;

                return new IntentResult(
                    Intent: root.GetProperty("intent").GetString() ?? "unknown",
                    Confidence: root.GetProperty("confidence").GetSingle(),
                    Parameters: root.TryGetProperty("parameters", out var p)
                        ? JsonSerializer.Deserialize<Dictionary<string, string>>(p)
                        : null
                );
            }
            catch
            {
                // safety net
                return new IntentResult("unknown", 0f, null);
            }

        }

    }
}