namespace RentalManager.Intents
{
    public static class KeywordIntentMap
    {
        public static readonly Dictionary<string, string[]> Map = new()
        {
            [IntentNames.Help] = new[]
            {
                "help",
                "help me",
                "menu",
                "help menu",
                "what can i do",
                "options"
            },

            [IntentNames.Greeting] = new[]
            {
                "hi",
                "hey",
                "greetings",
                "greeting",
                "hello",
                "niaje",
                "rada",
                "good morning",
                "good afternoon",
                "how are you"
            },

            [IntentNames.ViewBalance] = new[]
            {
                "balance",
                "view balances",
                "view balance",
                "my balance",
                "rent balance",
                "how much do i owe"
            },

            [IntentNames.PayRent] = new[]
            {
                "pay",
                "pay rent",
                "make payment",
                "send money"
            }
        };
    }
}
