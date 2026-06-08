namespace RentalManager.AI
{
    public static class PromptBuilder
    {
        public static string BuildIntentPrompt(
            string message,
            IEnumerable<string> allowedIntents)
        {
            return $$"""
                You are an intent classifier.

                Return ONLY valid JSON in this exact shape:
                    {"intent": "string", "confidence": number, "parameters": { "key": "value" } }
                

                Allowed intents:
                {{string.Join(", ", allowedIntents)}}

                Message:
                "{{message}}"

                Respond with only the intent name.
                """;
        }

        public static string BuildPrompt(string message)
        {
            return $@"
                You are an intent classifier for a WhatsApp rental management system.

                Return ONLY valid JSON in this exact shape:
                {{""intent"": ""string"", ""confidence"": number, ""parameters"": {{ ""key"": ""value"" }} }}

                Supported intents:
                - help
                - view_balance
                - pay_rent
                - view_statement
                - maintenance_request
                - unknown

                Message:
                ""{message}""
                ";
        }

    }
}
