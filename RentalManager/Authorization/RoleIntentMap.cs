using RentalManager.Intents;

namespace RentalManager.Authorization
{
    public static class RoleIntentMap
    {


        public static readonly Dictionary<string, HashSet<string>> Map =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Owner"] = new()
                {
                IntentNames.Help,
                IntentNames.Greeting,
                IntentNames.ViewBalance,
                },

                ["Manager"] = new()
                {
                IntentNames.Help,
                IntentNames.Greeting,
                IntentNames.ViewBalance,
                },

                ["Landlord"] = new()
                {
                IntentNames.Help,
                "list_properties",
                "view_income"
                },

                ["Tenant"] = new()
                {
                IntentNames.Help,
                "view_balance",
                "pay_rent"
                }
            };
    }
}
