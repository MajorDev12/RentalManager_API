namespace RentalManager.Intents
{
    public record IntentResult(
        string Intent,
        float Confidence,
        Dictionary<string, string>? Parameters
    );

}
