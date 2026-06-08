namespace RentalManager.Helpers.Formatting
{
    public static class TextFormatter
    {
        public static string ToDisplayName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value
                .Replace("_", " ")
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(w => char.ToUpper(w[0]) + w[1..])
                .Aggregate((a, b) => $"{a} {b}");
        }
    }
}
